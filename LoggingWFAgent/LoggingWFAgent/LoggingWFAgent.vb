Imports Kofax.Capture.DBLiteOpt
Imports Kofax.Capture.SDK.Workflow
Imports LoggingWFAgent.LoggingServiceReference
Imports System.Runtime.InteropServices
Imports System.ServiceModel

''' <summary>
''' Logging workflow which collects batch data and sends them to a service
''' </summary>
<Guid("01DE7B57-7532-4917-9D22-1FDD4EAC4C08"),
ClassInterface(ClassInterfaceType.None),
ProgId("LoggingWFAgent.LoggingWFAgent.ProgId"),
CLSCompliant(False)>
Public Class LoggingWFAgent
	Implements IACWorkflowAgent

	''' <summary>
	''' Required parameter for extracting root elements
	''' </summary>
	Private Const cm_nNoHint As Integer = 0

	''' <summary>
	''' Custom storage name for service URL
	''' </summary>
	Private Const cm_strSERVICE_URL_STORAGE_NAME As String = "Example.LoggingWFAgent.ServiceUrl"

	''' <summary>
	''' Custom storage name for document threshold
	''' </summary>
	Private Const cm_strLOGGING_THRESHOLD_STORAGE_NAME As String = "Example.LoggingWFAgent.DocumentThreshold"

	''' <summary>
	''' Default document threshold
	''' </summary>
	Private Const cm_nDEFAULT_DOCUMENT_THRESHOLD As Integer = 200

	''' <summary>
	''' Workflow entry point
	''' </summary>
	''' <param name="oWorkflowData">Workflow data</param>
	Public Sub ProcessWorkflow(ByRef oWorkflowData As IACWorkflowData) Implements IACWorkflowAgent.ProcessWorkflow

		' Extract setup root element
		Dim oRootSetupElement = oWorkflowData.ExtractSetupACDataElement(cm_nNoHint)

		' Extract runtime root element
		Dim oRuntimeRootSetupElement = oWorkflowData.ExtractRuntimeACDataElement(cm_nNoHint)

		' Extract batch element
		Dim oBatchElement = oRuntimeRootSetupElement.FindChildElementByName("Batch")

		' Extract document collection
		Dim oDocumentsCollection = FindElementsByPath(oBatchElement, "Documents/Document")

		' Extract loose pages
		Dim oLoosePagesCollection = FindElementsByPath(oBatchElement, "Pages/Page")

		' Extract batch class element
		Dim oBatchClassElement = FindElementsByPath(oRootSetupElement, "BatchClasses/BatchClass").Item(1)

		' Get service URL from batch class storage
		Dim strServiceURL = GetCustomStorageValue(oBatchClassElement, cm_strSERVICE_URL_STORAGE_NAME)

		' Get number of documents logging at once from batch class storage
		Dim strDocumentThreshold = GetCustomStorageValue(oBatchClassElement, cm_strLOGGING_THRESHOLD_STORAGE_NAME)
		Dim nDocumentThreshold As Integer
		If Not Integer.TryParse(strDocumentThreshold, nDocumentThreshold) Then
			nDocumentThreshold = cm_nDEFAULT_DOCUMENT_THRESHOLD
		End If

		Try
			Dim oServiceClient As New LoggingServiceClient(New BasicHttpBinding(), New EndpointAddress(strServiceURL))

			' Log batch information
			Dim oBatchInfo As New BatchInfo With {
				.CurrentModuleName = oWorkflowData.CurrentModule.Name,
				.BatchClassName = oBatchClassElement.AttributeValue("Name"),
				.BatchName = oBatchElement.AttributeValue("Name"),
				.CreationDate = oBatchElement.AttributeValue("CreationDateTime"),
				.NumberOfLoosePages = oLoosePagesCollection.Count
			}
			Dim nBatchId = oServiceClient.LogBatchInfo(oBatchInfo)

			' A list for storing document log records
			Dim oDocumentInfoList As New List(Of DocumentInfo)

			' Iterate over all documents
			For Each oDocumentElement As ACDataElement In oDocumentsCollection

				Dim oFieldInfoList As New List(Of FieldInfo)
				Dim oIndexFieldsCollection = FindElementsByPath(oDocumentElement, "IndexFields/IndexField")

				' Iterate over all index fields in each document
				For Each oIndexField As ACDataElement In oIndexFieldsCollection

					' Add a new field to the field list
					oFieldInfoList.Add(New FieldInfo With {
						.IndexFieldName = oIndexField.AttributeValue("Name"),
						.IndexFieldValue = oIndexField.AttributeValue("Value")
					})
				Next

				' Add a new document record to the document list
				oDocumentInfoList.Add(New DocumentInfo With {
					.FormTypeName = oDocumentElement.AttributeValue("FormTypeName"),
					.FieldInfoList = oFieldInfoList.ToArray()
				})

				'Check if number of documents reaches the threshold then call the service to log the documents
				If oDocumentInfoList.Count >= nDocumentThreshold Then

					' Call the service to log current document list
					oServiceClient.LogDocumentInfo(oDocumentInfoList.ToArray(), nBatchId)

					' Clear all logged documents from the list
					oDocumentInfoList.Clear()
				End If
			Next

			' Log the rest of documents
			oServiceClient.LogDocumentInfo(oDocumentInfoList.ToArray(), nBatchId)

		Catch ex As Exception
			MsgBox(String.Format(My.Resources.LoggingErrorMessage, ex.Message))
		End Try
	End Sub

	''' <summary>
	''' Find all elements which are decendents of <paramref name="oParentElement"/> using given <paramref name="strPath"/> to them
	''' </summary>
	''' <param name="oParentElement">parent element</param>
	''' <param name="strPath">path to target elements from <paramref name="oParentElement"/></param>
	''' <returns> a collection of elements</returns>
	Private Function FindElementsByPath(ByRef oParentElement As ACDataElement, strPath As String) As ACDataElementCollection
		Dim nIndexOfLastSeperator = strPath.LastIndexOf("/")
		Dim oImmediateParent = FindElementByPath(oParentElement, strPath.Substring(0, nIndexOfLastSeperator))
		If oImmediateParent Is Nothing Then
			FindElementsByPath = Nothing
			Exit Function
		End If
		FindElementsByPath = oImmediateParent.FindChildElementsByName(strPath.Substring(nIndexOfLastSeperator + 1))
	End Function

	''' <summary>
	''' Finding an element by traversing down from <paramref name="oParentElement"/> with a given <paramref name="strPath"/>
	''' </summary>
	''' <param name="oParentElement">parent element</param>
	''' <param name="strPath">path to target element from <paramref name="oParentElement"/></param>
	''' <returns>target element</returns>
	Private Function FindElementByPath(ByRef oParentElement As ACDataElement, strPath As String) As ACDataElement
		If String.IsNullOrEmpty(strPath) Then
			Return oParentElement
		End If

		Dim oElement = oParentElement
		For Each strName In strPath.Split("/")
			oElement = oElement.FindChildElementByName(strName)
			If oElement Is Nothing Then
				Exit For
			End If
		Next
		FindElementByPath = oElement
	End Function

	''' <summary>
	''' Get corresponding value from batch class storage by a key
	''' </summary>
	''' <param name="oBatchClassElement">Batch class element</param>
	''' <param name="strKey">a storage key</param>
	''' <returns>corresponding value</returns>
	Private Function GetCustomStorageValue(ByRef oBatchClassElement As ACDataElement, strKey As String) As String
		Try
			' Find the Batch Class custom storage strings
			Dim oCustomStorageStringsElement = oBatchClassElement.FindChildElementByName("BatchClassCustomStorageStrings")

			' Find the Batch Class custom storage string with "Name" attribute of strKey parameter
			Dim oCustomStorageStringElement =
				oCustomStorageStringsElement.FindChildElementByAttribute("BatchClassCustomStorageString", "Name", strKey)

			' Read the service URL from the "Value" attribute
			Return oCustomStorageStringElement.AttributeValue("Value")
		Catch

			' If an error occurs or no storage string was stored, use the an empty string
			Return String.Empty
		End Try
	End Function

End Class
