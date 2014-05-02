Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid

Namespace Utilities

    Public Class ControlsHelper

        Public Const WINDOWS_FORM_TITLE_BAR_HEIGHT As Integer = 38

        Public Shared Sub SnapToControl(ByVal ctlToPosition As Control, ByVal ctlClicked As Control)

            SnapToControl(ctlToPosition, ctlClicked, Nothing)

        End Sub

       
        Public Shared Function FindFocused(ByVal ctl As Control) As Control

            If TypeOf ctl Is GridControl Then
                Dim grid As GridControl = ctl
                Dim view As GridView = grid.FocusedView
                If view.CustomizationForm IsNot Nothing AndAlso view.CustomizationForm.Visible Then
                    Return view.CustomizationForm
                ElseIf view.ActiveEditor IsNot Nothing AndAlso view.ActiveEditor.Visible Then
                    Return view.ActiveEditor
                End If
            Else
                If ctl.Focused Then
                    Return ctl
                End If
            End If

            For Each c In ctl.Controls
                Dim focused = FindFocused(c)
                If focused IsNot Nothing Then
                    Return focused
                End If
            Next

            Return Nothing

        End Function

        Public Shared Function ContainsFocus(ByVal ctl As Control) As Boolean

            Return FindFocused(ctl) IsNot Nothing

        End Function

        Public Shared Sub SnapToControl(ByVal ctlToPosition As Control, ByVal ctlClicked As Control, ByVal MousePosition As Point)

            Dim form As Form = ctlClicked.FindForm
            Dim location As Point = ctlClicked.Location
            Dim ctlClickedHeight, ctlClickedWidth As Integer
            Dim ctlClickedScreenLocation As Point
            Dim ctlClickedRelativeToForm As Point
            Dim ctlToPositionIsAForm As Boolean = ctlToPosition.Parent Is Nothing

            If TypeOf ctlClicked Is GridControl Then
                'Move edit next to the clicked cell
                Try
                    If Not MousePosition = Nothing Then
                        Dim grid As GridControl = ctlClicked
                        Dim Point As Point = grid.PointToClient(MousePosition)
                        Dim Gridhitinfo As GridHitInfo = grid.FocusedView.CalcHitInfo(Point)
                        Dim GridViewInfo As GridViewInfo = CType(grid.FocusedView.GetViewInfo(), GridViewInfo)
                        Dim rectangle = GridViewInfo.GetGridCellInfo(Gridhitinfo.RowHandle, Gridhitinfo.Column).Bounds
                        location = New Point(location.X + rectangle.Left, location.Y + rectangle.Bottom)
                        ctlClickedHeight = rectangle.Height
                        ctlClickedWidth = rectangle.Width
                        ctlClickedScreenLocation = grid.PointToScreen(rectangle.Location)
                    Else
                        Dim grid As GridControl = ctlClicked
                        Dim view = DirectCast(grid.FocusedView, DevExpress.XtraGrid.Views.Grid.GridView)
                        Dim GridViewInfo As GridViewInfo = CType(grid.FocusedView.GetViewInfo(), GridViewInfo)
                        Dim rectangle = GridViewInfo.GetGridCellInfo(view.FocusedRowHandle, view.FocusedColumn).Bounds
                        location = New Point(location.X + rectangle.Left, location.Y + rectangle.Bottom)
                        ctlClickedHeight = rectangle.Height
                        ctlClickedWidth = rectangle.Width
                        ctlClickedScreenLocation = grid.PointToScreen(rectangle.Location)
                    End If
                Catch ex As Exception
                    Throw New Exception("SnapToControl uses the view's FocusedRowHandle() and FocusedColumn() to calculate cell bounds. Please try handling one of the view's click events.")
                End Try
            Else
                'move edit under the clicked control            
                location = New Point(location.X, location.Y + ctlClicked.Height)
                ctlClickedHeight = ctlClicked.Height
                ctlClickedWidth = ctlClicked.Width
                ctlClickedScreenLocation = ctlClicked.Parent.PointToScreen(ctlClicked.Location)
            End If

            ctlClickedRelativeToForm = form.PointToClient(ctlClickedScreenLocation)

            '------------------------------------------------------------------------------------------------------------------------------------------------------
            'Reposition control if it is off the screen
            '------------------------------------------------------------------------------------------------------------------------------------------------------
            Dim ctlToPositionMoveUp As Boolean
            Dim x As Integer = location.X
            Dim y As Integer = location.Y

            If ctlToPositionIsAForm Then
                'Check the screen to see if we should move up
                ctlToPositionMoveUp = My.Computer.Screen.WorkingArea.Height < ctlToPosition.Height + ctlClickedScreenLocation.Y + ctlClickedHeight
            Else
                'otherwise check the form to see if we should move up
                ctlToPositionMoveUp = form.Height < ctlToPosition.Height + ctlClickedRelativeToForm.Y + ctlClickedHeight + WINDOWS_FORM_TITLE_BAR_HEIGHT
            End If

            If ctlToPositionMoveUp Then
                y -= ctlToPosition.Height + ctlClickedHeight
            End If

            'Check form to see if we should move left.
            If form.Width < ctlToPosition.Width + ctlClickedRelativeToForm.X + 14 Then

                'Move edit control to the left
                x -= (ctlToPosition.Width + ctlClickedRelativeToForm.X + 14) - form.Width

                ' try to center the control under the clicked control
                Dim ctlToPositionSreenLocation As Point = ctlClicked.Parent.PointToScreen(New Point(x, y))
                Dim middleOfControlPostion = ctlToPositionSreenLocation.X + (ctlToPosition.Width / 2)
                Dim middleOfControlClicked = ctlClickedScreenLocation.X + (ctlClickedWidth / 2)
                If middleOfControlPostion > middleOfControlClicked Then
                    'best fit
                    x -= middleOfControlPostion - middleOfControlClicked
                Else
                    'move off the edge
                    x -= 10
                End If
            End If

            'update the location to reflect the changes made on x and y
            location = New Point(x, y)
            '------------------------------------------------------------------------------------------------------------------------------------------------------
            '------------------------------------------------------------------------------------------------------------------------------------------------------

            'If the control to position is not contained by another control then we must convert the location
            'to screen coordinates
            If ctlToPositionIsAForm Then
                location = ctlClicked.Parent.PointToScreen(location)
            End If

            ctlToPosition.Location = location

        End Sub

        Public Shared Sub CenterInParentVertically(ByVal ParamArray ctls() As Control)

            If ctls.Count > 0 Then

                Dim height = ctls.Max(Function(o) o.Bottom) - ctls.Min(Function(o) o.Top)
                Dim topControl = ctls.OrderBy(Function(o) o.Top).First
                Dim difference = ((topControl.Parent.ClientRectangle.Height - height) / 2) - topControl.Top

                For Each ctl In ctls
                    ctl.Top += difference
                Next

            End If

        End Sub

        Public Shared Sub CenterInParentHorizontally(ByVal ParamArray ctls() As Control)

            If ctls.Count > 0 Then

                Dim width = ctls.Max(Function(o) o.Right) - ctls.Min(Function(o) o.Left)
                Dim topControl = ctls.OrderBy(Function(o) o.Left).First
                Dim difference = ((topControl.Parent.ClientRectangle.Width - width) / 2) - topControl.Left

                For Each ctl In ctls
                    ctl.Left += difference
                Next

            End If

        End Sub

        Public Shared Sub CenterInParent(ByVal ParamArray ctls() As Control)

            CenterInParentHorizontally(ctls)
            CenterInParentVertically(ctls)

        End Sub

        Public Shared Sub CenterVisibleChildControls(ByVal ctl As Control)

            Dim visibleControls As New List(Of Control)

            For Each c As Control In ctl.Controls
                If c.Visible Then
                    visibleControls.Add(c)
                End If
            Next

            CenterInParent(visibleControls.ToArray)

        End Sub

    End Class



End Namespace
