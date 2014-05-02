Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Namespace UI
    Public Class DisplayControl

        Private _ParentForm As Form
        Private _ctlClicked As Control
        Private _mousePosition As Point

        ''' <summary>
        ''' Raised when the save button has been clicked.
        ''' </summary>    
        Public Event OnSave()

        ''' <summary>
        ''' Raised when the cancel button has been clicked or the form has been closed.
        ''' </summary> 
        Public Event OnCancel()

#Region "Properties"

        Public Property BorderColor As Color
            Get
                Return pnlBorderInner.BackColor
            End Get
            Set(ByVal value As Color)
                pnlBorderInner.BackColor = value
            End Set
        End Property

        Public Property ButtonSaveVisible As Boolean
            Get
                Return btnSave.Visible
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    LayoutControlItemSave.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Else
                    LayoutControlItemSave.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                End If
            End Set
        End Property
        Public ReadOnly Property ButtonSave As SimpleButton
            Get
                Return btnSave
            End Get
        End Property

        Public Property ButtonCancelVisible As Boolean
            Get
                Return btnCancel.Visible
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    LayoutControlItemCancel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Else
                    LayoutControlItemCancel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                End If
            End Set
        End Property
        Public ReadOnly Property ButtonCancel As SimpleButton
            Get
                Return btnCancel
            End Get
        End Property

#End Region

        Private Sub New(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control, ByVal MousePosition As Point,
                        ByVal buttonSaveVisible As Boolean, ByVal buttonCancelVisible As Boolean)

            ' This call is required by the designer.
            InitializeComponent()

            If TypeOf ctlToDisplay Is Form Then
                ctlToDisplay = ctlToDisplay.Controls(0)
            End If

            ' Add any initialization after the InitializeComponent() call.
            _ParentForm = ctlToDisplay.FindForm
            _ctlClicked = ctlClicked

            If _ParentForm Is Nothing Then
                _ParentForm = New Form
            End If

            Me.Width = ctlToDisplay.Width + Me.Width - Me.ClientSize.Width
            Me.Height = Me.Height - pnlMain.Height + ctlToDisplay.Height

            Me.pnlMain.Width = ctlToDisplay.Width
            Me.pnlMain.Height = ctlToDisplay.Height
            pnlMain.Controls.Add(ctlToDisplay)

            DialogResult = DialogResult.None

            Me.ButtonSaveVisible = buttonSaveVisible
            Me.ButtonCancelVisible = buttonCancelVisible
            _mousePosition = MousePosition

            SetSaveCancelButtons()

            AddHandler btnCancel.VisibleChanged, AddressOf SetSaveCancelButtons
            AddHandler btnSave.VisibleChanged, AddressOf SetSaveCancelButtons

            '---------------------------------------------------------------------------------------------------------
            'Code to close the popup when the user clicks off
            '---------------------------------------------------------------------------------------------------------
            AddHandler Me.Shown, Sub()

                                     AddHandler _ctlClicked.FindForm.Activated, Sub()

                                                                                    For Each ctl As Control In pnlMain.Controls
                                                                                        If TypeOf ctl Is GridControl Then
                                                                                            Dim grid As GridControl = ctl
                                                                                            Dim view As GridView = grid.FocusedView
                                                                                            view.HideCustomization()
                                                                                            view.HideEditor()
                                                                                        End If
                                                                                    Next

                                                                                    Me.Hide()
                                             
                                                                                End Sub

                                 End Sub
            '---------------------------------------------------------------------------------------------------------


        End Sub

#Region "Display"

        Public Shared Function DisplayWithButtons(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control) As DisplayControl

            Return DisplayWithButtons(ctlToDisplay, ctlClicked, Nothing, Nothing)

        End Function

        Public Shared Function DisplayWithButtons(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control, ByVal MousePosition As Point) As DisplayControl

            Return DisplayWithButtons(ctlToDisplay, ctlClicked, MousePosition, Nothing)

        End Function

        Public Shared Function DisplayWithButtons(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control, ByVal SaveCallback As Action) As DisplayControl

            Return DisplayWithButtons(ctlToDisplay, ctlClicked, Nothing, SaveCallback)

        End Function

        Public Shared Function DisplayWithButtons(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control, ByVal MousePosition As Point, ByVal SaveCallback As Action) As DisplayControl

            Dim save As New DisplayControl(ctlToDisplay, ctlClicked, MousePosition, True, True)

            AddHandler save.OnSave, Sub()
                                        If Not SaveCallback Is Nothing Then SaveCallback.Invoke()
                                    End Sub

            save.Show()

            Return save

        End Function

        Public Shared Function Display(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control) As DisplayControl

            Return Display(ctlToDisplay, ctlClicked, Nothing, Nothing)

        End Function

        Public Shared Function Display(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control, ByVal MousePosition As Point) As DisplayControl

            Return Display(ctlToDisplay, ctlClicked, MousePosition, Nothing)

        End Function

        Public Shared Function Display(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control, ByVal ClosedCallback As Action) As DisplayControl

            Return Display(ctlToDisplay, ctlClicked, Nothing, ClosedCallback)

        End Function

        Public Shared Function Display(ByVal ctlToDisplay As Control, ByVal ctlClicked As Control, ByVal MousePosition As Point, ByVal ClosedCallback As Action) As DisplayControl

            Dim save As New DisplayControl(ctlToDisplay, ctlClicked, MousePosition, False, False)

            save.Show()

            AddHandler save.VisibleChanged, Sub()
                                                If Not ClosedCallback Is Nothing Then ClosedCallback.Invoke()
                                            End Sub

            Return save

        End Function


#End Region

#Region "Save And Cancel Buttons"

        Private changeInHeight As Integer
        Private Sub SetSaveCancelButtons()

            If Not ButtonSave.Visible AndAlso Not ButtonCancel.Visible AndAlso changeInHeight = 0 Then
                changeInHeight = pnlSaveAndCancel.Height - 5
                Me.Height -= changeInHeight
                pnlMain.BringToFront()
            Else
                Me.Height += changeInHeight
                pnlMain.SendToBack()
                changeInHeight = 0
            End If

            btnSave.Width = btnSave.CalcBestSize.Width + 10
            btnCancel.Width = btnCancel.CalcBestSize.Width + 10

            If _mousePosition = Nothing Then
                Utilities.ControlsHelper.SnapToControl(Me, _ctlClicked)
            Else
                Utilities.ControlsHelper.SnapToControl(Me, _ctlClicked, _mousePosition)
            End If

        End Sub

        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click            
            Me.Hide()
            _ParentForm.Show()
            _ParentForm.Hide()
            RaiseEvent OnSave()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.Hide()
            RaiseEvent OnCancel()
        End Sub

        Public Event DisplayControlClosing(ByRef cancel As Boolean)

        Private Sub SaveAndCancel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            _ParentForm.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            _ParentForm.Height = 0
            _ParentForm.Width = 0
            _ParentForm.Show()
            _ParentForm.Hide()
        End Sub

#End Region

        Private Sub SetBorder()

            pnlDivider.BackColor = pnlBorderInner.BackColor
            pnlDivider.Height = 1

            pnlBorderInner.Padding = New Padding(2)
            pnlBorderOuter.Padding = New Padding(1)
            pnlBorderOuter.BackColor = Color.DarkGray

        End Sub

    End Class
End Namespace

