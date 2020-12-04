Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Kofax.Capture.AdminModule.InteropServices

''' <summary>
''' Logging workflow controller: link between Ascent Capture application and the workflow agent setup
''' </summary>
<Guid("A4106092-1C2D-4444-AE0F-5F81D384F530"),
ClassInterface(ClassInterfaceType.AutoDispatch),
ProgId("LoggingWFAgentConfiguration.LoggingWFAgentCtrl.ProgId"),
CLSCompliant(False)>
Public Class LoggingWFAgentCtrl
	Inherits UserControl

	''' <summary>
	''' Menu root for the context menu item
	''' </summary>
	Private Const cm_strBatchClassMenu As String = "BatchClass"

	''' <summary>
	''' Represents text value when menu is clicked
	''' </summary>
	Private Const cm_strLoggingWorflowBatchClassMenu As String = "Logging Workflow Menu"

	''' <summary>
	''' Administration app instance
	''' </summary>
	Private m_oApp As AdminApplication

	''' <summary>
	''' Configuration form where we can config LoggingWFAgent workflow
	''' </summary>
	Private m_frmWorkflowAgentSetup As New LoggingWFAgentConfigForm

	''' <summary>
	''' KC uses this setter to inject Administration module instance to the workflow
	''' </summary>
	Public WriteOnly Property Application() As AdminApplication
		Set(ByVal Value As AdminApplication)
			Try
				' Store the administration app instance
				m_oApp = Value

				' Pass the Administration app instance to the configuration form
				m_frmWorkflowAgentSetup.AdminApp = m_oApp

				' Add a new menu item to batch class menu
				m_oApp.AddMenu(cm_strLoggingWorflowBatchClassMenu, My.Resources.LoggingMenuItemText, cm_strBatchClassMenu)
			Catch ex As Exception
				MsgBox(String.Format(My.Resources.AppErrorMessage, ex.Message))
			End Try
		End Set
	End Property

	''' <summary>
	''' Receive each action event
	''' Look for menu selection
	''' </summary>
	''' <param name="nActionNumber">number assigned to event</param>
	''' <param name="vArgument">currently NOT used</param>
	''' <param name="pnCancel">Response to the event (Only DocumentClosing and FieldExiting process the response)</param>
	''' <returns>returned is currently ignored</returns>
	Public Function ActionEvent(ByVal nActionNumber As Integer, ByRef vArgument As Object, ByRef pnCancel As Integer) As Integer

		' Set the return values
		pnCancel = 0
		ActionEvent = 0

		' Menu clicked event
		If nActionNumber = KfxOcxEvent.KfxOcxEventMenuClicked Then

			' Check if the new menu item is clicked then show the configuration window
			If vArgument = cm_strLoggingWorflowBatchClassMenu Then
				m_frmWorkflowAgentSetup.ShowDialog(Me)
			End If
		End If
	End Function
End Class
