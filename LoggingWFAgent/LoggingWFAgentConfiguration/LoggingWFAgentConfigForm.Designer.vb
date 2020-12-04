<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoggingWFAgentConfigForm
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.txtServiceURL = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.cmdCancel = New System.Windows.Forms.Button()
		Me.cmdOK = New System.Windows.Forms.Button()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.numDocumentThreshold = New System.Windows.Forms.NumericUpDown()
		CType(Me.numDocumentThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'txtServiceURL
		'
		Me.txtServiceURL.Location = New System.Drawing.Point(214, 28)
		Me.txtServiceURL.Name = "txtServiceURL"
		Me.txtServiceURL.Size = New System.Drawing.Size(430, 20)
		Me.txtServiceURL.TabIndex = 0
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(21, 31)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(68, 13)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "Service URL"
		'
		'cmdCancel
		'
		Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.cmdCancel.Location = New System.Drawing.Point(472, 105)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.UseVisualStyleBackColor = True
		'
		'cmdOK
		'
		Me.cmdOK.Location = New System.Drawing.Point(569, 105)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(75, 23)
		Me.cmdOK.TabIndex = 3
		Me.cmdOK.Text = "OK"
		Me.cmdOK.UseVisualStyleBackColor = True
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(21, 65)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(187, 13)
		Me.Label2.TabIndex = 4
		Me.Label2.Text = "Number of documents logging at once"
		'
		'numDocumentThreshold
		'
		Me.numDocumentThreshold.Location = New System.Drawing.Point(214, 63)
		Me.numDocumentThreshold.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.numDocumentThreshold.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.numDocumentThreshold.Name = "numDocumentThreshold"
		Me.numDocumentThreshold.Size = New System.Drawing.Size(84, 20)
		Me.numDocumentThreshold.TabIndex = 5
		Me.numDocumentThreshold.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'LoggingWFAgentConfigForm
		'
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(673, 161)
		Me.Controls.Add(Me.numDocumentThreshold)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.txtServiceURL)
		Me.MaximizeBox = False
		Me.Name = "LoggingWFAgentConfigForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Logging WF Agent Settings"
		CType(Me.numDocumentThreshold, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents txtServiceURL As Windows.Forms.TextBox
	Friend WithEvents Label1 As Windows.Forms.Label
	Friend WithEvents cmdCancel As Windows.Forms.Button
	Friend WithEvents cmdOK As Windows.Forms.Button
	Friend WithEvents Label2 As Windows.Forms.Label
	Friend WithEvents numDocumentThreshold As Windows.Forms.NumericUpDown
End Class
