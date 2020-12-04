Imports System.Runtime.InteropServices
Imports Kofax.Capture.AdminModule.InteropServices

''' <summary>
''' Configuration form where logging workflow can be be configured
''' </summary>
<Guid("0E9C73E1-F2D9-4F53-B284-426CC4C06645"),
ClassInterface(ClassInterfaceType.None),
ProgId("LoggingWFAgentConfiguration.LoggingWFAgentConfigForm")>
Public Class LoggingWFAgentConfigForm

	''' <summary>
	''' Custom storage name for service URL
	''' </summary>
	Private Const cm_strSERVICE_URL_STORAGE_NAME As String = "Example.LoggingWFAgent.ServiceUrl"

	''' <summary>
	''' Custom storage name for document threshold
	''' </summary>
	Private Const cm_strDOCUMENT_THRESHOLD_STORAGE_NAME As String = "Example.LoggingWFAgent.DocumentThreshold"

	''' <summary>
	''' Default document threshold
	''' </summary>
	Private Const cm_nDEFAULT_DOCUMENT_THRESHOLD As Integer = 200

	''' <summary>
	''' Administration app property
	''' </summary>
	''' <returns>an instance of administration module</returns>
	Public Property AdminApp() As AdminApplication

	''' <summary>
	''' Cancel button's click handler
	''' </summary>
	''' <param name="sender">event sender</param>
	''' <param name="e">event arguments</param>
	Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
		Close()
	End Sub

	''' <summary>
	''' OK button's click handler
	''' </summary>
	''' <param name="sender">event sender</param>
	''' <param name="e">event arguments</param>
	Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click

		' Save values, which user inputed, to batch class storage
		AdminApp.ActiveBatchClass.CustomStorageString(cm_strSERVICE_URL_STORAGE_NAME) = txtServiceURL.Text
		AdminApp.ActiveBatchClass.CustomStorageString(cm_strDOCUMENT_THRESHOLD_STORAGE_NAME) = numDocumentThreshold.Value

		' Close the form
		Close()
	End Sub

	''' <summary>
	''' Form load handler
	''' </summary>
	''' <param name="sender">event sender</param>
	''' <param name="e">event arguments</param>
	Private Sub LoggingWFAgentConfigForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		Try
			' Fill service URL TextBox with value got from batch class storage
			txtServiceURL.Text = AdminApp.ActiveBatchClass.CustomStorageString(cm_strSERVICE_URL_STORAGE_NAME)
		Catch
			' If the value can not be got then use default value
			txtServiceURL.Text = String.Empty
		End Try

		Try
			numDocumentThreshold.Value = CInt(AdminApp.ActiveBatchClass.CustomStorageString(cm_strDOCUMENT_THRESHOLD_STORAGE_NAME))
		Catch
			numDocumentThreshold.Value = cm_nDEFAULT_DOCUMENT_THRESHOLD
		End Try
	End Sub
End Class