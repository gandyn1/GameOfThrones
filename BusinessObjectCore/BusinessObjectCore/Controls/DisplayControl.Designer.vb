Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class DisplayControl
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
            Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
            Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
            Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
            Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.LayoutControlItemSave = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItemCancel = New DevExpress.XtraLayout.LayoutControlItem()
            Me.pnlMain = New DevExpress.XtraEditors.PanelControl()
            Me.pnlSaveAndCancel = New DevExpress.XtraEditors.PanelControl()
            Me.pnlDivider = New DevExpress.XtraEditors.PanelControl()
            Me.pnlBorderInner = New DevExpress.XtraEditors.PanelControl()
            Me.pnlBorderOuter = New DevExpress.XtraEditors.PanelControl()
            Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
            CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.LayoutControl1.SuspendLayout()
            CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemSave, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemCancel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.pnlSaveAndCancel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlSaveAndCancel.SuspendLayout()
            CType(Me.pnlDivider, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.pnlBorderInner, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlBorderInner.SuspendLayout()
            CType(Me.pnlBorderOuter, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlBorderOuter.SuspendLayout()
            CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnSave
            '
            Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.btnSave.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.btnSave.Appearance.Options.UseFont = True
            Me.btnSave.AutoWidthInLayoutControl = True
            Me.btnSave.Location = New System.Drawing.Point(43, 12)
            Me.btnSave.MinimumSize = New System.Drawing.Size(100, 0)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(100, 22)
            Me.btnSave.StyleController = Me.LayoutControl1
            Me.btnSave.TabIndex = 13
            Me.btnSave.Text = "Save"
            '
            'LayoutControl1
            '
            Me.LayoutControl1.AutoScroll = False
            Me.LayoutControl1.Controls.Add(Me.btnCancel)
            Me.LayoutControl1.Controls.Add(Me.btnSave)
            Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LayoutControl1.Location = New System.Drawing.Point(0, 2)
            Me.LayoutControl1.Name = "LayoutControl1"
            Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(992, 221, 250, 350)
            Me.LayoutControl1.Root = Me.LayoutControlGroup1
            Me.LayoutControl1.Size = New System.Drawing.Size(350, 40)
            Me.LayoutControl1.TabIndex = 16
            Me.LayoutControl1.Text = "LayoutControl1"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.btnCancel.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.btnCancel.Appearance.Options.UseFont = True
            Me.btnCancel.AutoWidthInLayoutControl = True
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(208, 12)
            Me.btnCancel.MinimumSize = New System.Drawing.Size(100, 0)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(100, 22)
            Me.btnCancel.StyleController = Me.LayoutControl1
            Me.btnCancel.TabIndex = 14
            Me.btnCancel.Text = "Cancel"
            '
            'LayoutControlGroup1
            '
            Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
            Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
            Me.LayoutControlGroup1.GroupBordersVisible = False
            Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItemSave, Me.LayoutControlItemCancel})
            Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
            Me.LayoutControlGroup1.Size = New System.Drawing.Size(350, 46)
            Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
            Me.LayoutControlGroup1.TextVisible = False
            '
            'LayoutControlItemSave
            '
            Me.LayoutControlItemSave.Control = Me.btnSave
            Me.LayoutControlItemSave.ControlAlignment = System.Drawing.ContentAlignment.MiddleCenter
            Me.LayoutControlItemSave.CustomizationFormText = "LayoutControlItem1"
            Me.LayoutControlItemSave.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlItemSave.Name = "LayoutControlItem1"
            Me.LayoutControlItemSave.Size = New System.Drawing.Size(166, 26)
            Me.LayoutControlItemSave.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment
            Me.LayoutControlItemSave.Text = "LayoutControlItem1"
            Me.LayoutControlItemSave.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItemSave.TextToControlDistance = 0
            Me.LayoutControlItemSave.TextVisible = False
            '
            'LayoutControlItemCancel
            '
            Me.LayoutControlItemCancel.Control = Me.btnCancel
            Me.LayoutControlItemCancel.ControlAlignment = System.Drawing.ContentAlignment.MiddleCenter
            Me.LayoutControlItemCancel.CustomizationFormText = "LayoutControlItem2"
            Me.LayoutControlItemCancel.Location = New System.Drawing.Point(166, 0)
            Me.LayoutControlItemCancel.Name = "LayoutControlItem2"
            Me.LayoutControlItemCancel.Size = New System.Drawing.Size(164, 26)
            Me.LayoutControlItemCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment
            Me.LayoutControlItemCancel.Text = "LayoutControlItem2"
            Me.LayoutControlItemCancel.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItemCancel.TextToControlDistance = 0
            Me.LayoutControlItemCancel.TextVisible = False
            '
            'pnlMain
            '
            Me.pnlMain.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
            Me.pnlMain.Appearance.Options.UseBackColor = True
            Me.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlMain.Location = New System.Drawing.Point(2, 2)
            Me.pnlMain.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat
            Me.pnlMain.LookAndFeel.UseDefaultLookAndFeel = False
            Me.pnlMain.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.pnlMain.Name = "pnlMain"
            Me.pnlMain.Size = New System.Drawing.Size(350, 57)
            Me.pnlMain.TabIndex = 17
            '
            'pnlSaveAndCancel
            '
            Me.pnlSaveAndCancel.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
            Me.pnlSaveAndCancel.Appearance.Options.UseBackColor = True
            Me.pnlSaveAndCancel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.pnlSaveAndCancel.Controls.Add(Me.LayoutControl1)
            Me.pnlSaveAndCancel.Controls.Add(Me.pnlDivider)
            Me.pnlSaveAndCancel.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlSaveAndCancel.Location = New System.Drawing.Point(2, 53)
            Me.pnlSaveAndCancel.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
            Me.pnlSaveAndCancel.Name = "pnlSaveAndCancel"
            Me.pnlSaveAndCancel.Size = New System.Drawing.Size(350, 42)
            Me.pnlSaveAndCancel.TabIndex = 16
            '
            'pnlDivider
            '
            Me.pnlDivider.Appearance.BackColor = System.Drawing.Color.LightGray
            Me.pnlDivider.Appearance.Options.UseBackColor = True
            Me.pnlDivider.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.pnlDivider.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlDivider.Location = New System.Drawing.Point(0, 0)
            Me.pnlDivider.Name = "pnlDivider"
            Me.pnlDivider.Size = New System.Drawing.Size(350, 2)
            Me.pnlDivider.TabIndex = 15
            '
            'pnlBorderInner
            '
            Me.pnlBorderInner.Appearance.BackColor = System.Drawing.Color.PowderBlue
            Me.pnlBorderInner.Appearance.Options.UseBackColor = True
            Me.pnlBorderInner.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.pnlBorderInner.Controls.Add(Me.pnlSaveAndCancel)
            Me.pnlBorderInner.Controls.Add(Me.pnlMain)
            Me.pnlBorderInner.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlBorderInner.Location = New System.Drawing.Point(1, 1)
            Me.pnlBorderInner.Margin = New System.Windows.Forms.Padding(0)
            Me.pnlBorderInner.Name = "pnlBorderInner"
            Me.pnlBorderInner.Padding = New System.Windows.Forms.Padding(2)
            Me.pnlBorderInner.Size = New System.Drawing.Size(354, 97)
            Me.pnlBorderInner.TabIndex = 0
            '
            'pnlBorderOuter
            '
            Me.pnlBorderOuter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.pnlBorderOuter.Controls.Add(Me.pnlBorderInner)
            Me.pnlBorderOuter.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlBorderOuter.Location = New System.Drawing.Point(0, 0)
            Me.pnlBorderOuter.Name = "pnlBorderOuter"
            Me.pnlBorderOuter.Padding = New System.Windows.Forms.Padding(1)
            Me.pnlBorderOuter.Size = New System.Drawing.Size(356, 99)
            Me.pnlBorderOuter.TabIndex = 1
            '
            'PanelControl1
            '
            Me.PanelControl1.Location = New System.Drawing.Point(111, 12)
            Me.PanelControl1.Name = "PanelControl1"
            Me.PanelControl1.Size = New System.Drawing.Size(100, 22)
            Me.PanelControl1.TabIndex = 0
            '
            'DisplayControl
            '
            Me.AcceptButton = Me.btnSave
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(356, 99)
            Me.ControlBox = False
            Me.Controls.Add(Me.pnlBorderOuter)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "DisplayControl"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.Text = "SaveAndCancel"
            CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.LayoutControl1.ResumeLayout(False)
            CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemSave, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemCancel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.pnlSaveAndCancel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlSaveAndCancel.ResumeLayout(False)
            CType(Me.pnlDivider, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.pnlBorderInner, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlBorderInner.ResumeLayout(False)
            CType(Me.pnlBorderOuter, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlBorderOuter.ResumeLayout(False)
            CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents pnlMain As DevExpress.XtraEditors.PanelControl
        Friend WithEvents pnlSaveAndCancel As DevExpress.XtraEditors.PanelControl
        Friend WithEvents pnlBorderInner As DevExpress.XtraEditors.PanelControl
        Friend WithEvents pnlDivider As DevExpress.XtraEditors.PanelControl
        Friend WithEvents pnlBorderOuter As DevExpress.XtraEditors.PanelControl
        Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
        Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
        Friend WithEvents LayoutControlItemCancel As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlItemSave As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    End Class

End Namespace

