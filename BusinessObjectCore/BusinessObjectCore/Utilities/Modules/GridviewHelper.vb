Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Runtime.CompilerServices
Imports DevExpress.XtraEditors.Repository
Imports System.Globalization
Imports DevExpress.XtraGrid.Views.Base
Imports System.Linq.Expressions
Imports DevExpress.Data
Imports DevExpress.XtraGrid
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo


Namespace Utilities

    Public Class Columns

        Private _Columns As List(Of GridColumn)
        Private _view As GridView
        Private _control As GridControl

        Sub New(ByVal Columns() As GridColumn)
            _Columns = Columns.ToList
            _view = _Columns.First.View
            _control = _view.GridControl
        End Sub

        Public Function AlsoFor(ByVal ParamArray columns() As GridColumn) As Columns
            _Columns.AddRange(columns)
            Return Me
        End Function

        Public Function ExceptFor(ByVal ParamArray columns() As GridColumn) As Columns
            For Each col In columns
                _Columns.Remove(col)
            Next
            Return Me
        End Function

        Public Function ForEach(ByVal action As Action(Of GridColumn))

            _Columns.ForEach(action)

            Return Me

        End Function

#Region "Column Helpers"

        Public Function ShowInColumnChooser(ByVal value As Boolean) As Columns

            For Each col As GridColumn In _Columns
                col.OptionsColumn.ShowInCustomizationForm = value
            Next

            Return Me
        End Function

        Public Function SetAutoFilterCondition(ByVal value As AutoFilterCondition) As Columns

            For Each col As GridColumn In _Columns
                col.OptionsFilter.AutoFilterCondition = value
            Next

            Return Me

        End Function

        ''' <summary>
        ''' Sets both Focus and Editable Properties
        ''' </summary>
        Public Function SetEnable(ByVal value As Boolean) As Columns

            For Each col As GridColumn In _Columns
                col.OptionsColumn.AllowEdit = value
                col.OptionsColumn.AllowFocus = value
            Next

            Return Me

        End Function

        Public Function SetMaxLength(ByVal max As Integer) As Columns

            Dim textedit As New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
            textedit.MaxLength = max

            For Each col As GridColumn In _Columns
                col.ColumnEdit = textedit
            Next

            Return Me

        End Function

        Public Function MoveBefore(ByVal column As GridColumn) As Columns

            For Each col As GridColumn In _Columns
                For Each gridColumns As GridColumn In _view.Columns
                    If gridColumns.VisibleIndex >= column.VisibleIndex Then gridColumns.VisibleIndex += 1
                Next
                col.VisibleIndex = column.VisibleIndex
            Next

            Return Me

        End Function

        Public Function MoveAfter(ByVal column As GridColumn) As Columns

            For i As Integer = _Columns.Count - 1 To 0 Step -1
                For Each gridColumns As GridColumn In _view.Columns
                    If gridColumns.VisibleIndex > column.VisibleIndex Then gridColumns.VisibleIndex += 1
                Next
                _Columns(i).VisibleIndex = column.VisibleIndex + 1
            Next

            Return Me

        End Function

        ''' <summary>
        ''' Sets visible index equal to the order passed in
        ''' </summary>
        Public Function SetVisibleIndex() As Columns

            For Each col As GridColumn In _view.Columns
                col.VisibleIndex = -1
            Next

            For i As Integer = 0 To _Columns.Count - 1
                _Columns(i).VisibleIndex = i
            Next

            Return Me

        End Function

        Public Function AbbreviateCaption() As Columns

            Return AbbreviateCaption(True)

        End Function

        Public Function AbbreviateCaption(ByVal onlyWhenCaptionIsBlank As Boolean) As Columns

            For Each col As GridColumn In _Columns
                Dim abbreviation = StringHelper.GetAbbreviation(StringHelper.GetWords(col.FieldName))

                If Not onlyWhenCaptionIsBlank OrElse col.Caption = "" Then
                    col.Caption = abbreviation
                End If
            Next

            Return Me

        End Function

        Public Function AddSummaryForGroup(summaryType As SummaryItemType, withFooterSummary As Boolean) As Columns

            For Each col As GridColumn In _Columns
                _view.GroupSummary.Add(summaryType, col.FieldName, Nothing, "(" & col.Caption & ": {0:" & col.DisplayFormat.FormatString & "})")
                If withFooterSummary Then _view.GroupSummary.Add(summaryType, col.FieldName, col, "{0:" & col.DisplayFormat.FormatString & "}")
            Next

            Return Me

        End Function

        Public Function AddSummaryForColumn(summaryType As SummaryItemType) As Columns

            Return AddSummaryForColumn(summaryType, True)

        End Function

        Public Function AddSummaryForColumn(ByVal summaryType As SummaryItemType, ByVal showSummaryType As Boolean) As Columns

            For Each col As GridColumn In _Columns
                col.SummaryItem.SummaryType = summaryType
                col.SummaryItem.DisplayFormat = IIf(showSummaryType, [Enum].GetName(summaryType.GetType, summaryType) & ": ", "") & "{0:" & col.DisplayFormat.FormatString & "}"
            Next

            Return Me

        End Function

        Public Function AddToolTipForCell(ByVal toolTipMessage As Action(Of toolTipForCellArgs(Of Object))) As Columns

            Return AddToolTipForCell(Of Object)(toolTipMessage)

        End Function

        Public Class toolTipForCellArgs(Of TRow)
            Public Property Row As TRow
            Public Property Column As GridColumn
            Public Property CellValue As Object
            Public Property Message As String            
        End Class
        Public Function AddToolTipForCell(Of TRow)(ByVal toolTipMessage As Action(Of toolTipForCellArgs(Of TRow))) As Columns

            If _control.ToolTipController Is Nothing Then _control.ToolTipController = New ToolTipController

            AddHandler _control.ToolTipController.GetActiveObjectInfo, Sub(obj As Object, args As ToolTipControllerGetActiveObjectInfoEventArgs)
                                                                           Dim toolTip As ToolTipControlInfo = Nothing
                                                                           Dim view As GridView = _control.GetViewAt(args.ControlMousePosition)
                                                                           If view Is Nothing Then Return
                                                                           Dim hitInfo As GridHitInfo = view.CalcHitInfo(args.ControlMousePosition)

                                                                           If hitInfo.RowHandle >= 0 Then

                                                                               Dim value As TRow = view.GetRow(hitInfo.RowHandle)

                                                                               If _Columns.Contains(hitInfo.Column) Then

                                                                                   Dim toolTipForCellArgs = New toolTipForCellArgs(Of TRow) With {.Row = value, .Column = hitInfo.Column, .CellValue = view.GetRowCellValue(hitInfo.RowHandle, hitInfo.Column)}

                                                                                   toolTipMessage.Invoke(toolTipForCellArgs)

                                                                                   If toolTipForCellArgs.Message <> "" Then
                                                                                       toolTip = New ToolTipControlInfo(hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString(), toolTipForCellArgs.Message, hitInfo.Column.Caption)
                                                                                   End If
                                                                               End If

                                                                               If toolTip IsNot Nothing Then
                                                                                   args.Info = toolTip
                                                                               End If

                                                                           End If
                                                                       End Sub

            Return Me

        End Function

#Region "Formatting"

        Public Function SetFormatAsMoney() As Columns

            For Each col As GridColumn In _Columns
                col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                col.DisplayFormat.FormatString = "C2"
            Next

            Return Me

        End Function

        Public Function SetFormatAsDecimal() As Columns

            For Each col As GridColumn In _Columns
                col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                col.DisplayFormat.FormatString = "N2"
            Next

            Return Me

        End Function

        Public Function SetFormatAsInteger() As Columns

            For Each col As GridColumn In _Columns
                col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                col.DisplayFormat.FormatString = "N0"
            Next

            Return Me

        End Function

        Public Function SetFormatAsPercentage() As Columns

            For Each col As GridColumn In _Columns
                col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                col.DisplayFormat.FormatString = "p"
            Next

            Return Me

        End Function

        Public Function SetFormatAsDate() As Columns

            Return SetFormatAsDate(False)

        End Function

        Private _dateEdit As New RepositoryItemDateEdit()
        Public Function SetFormatAsDate(ByVal showCentury As Boolean) As Columns

            _dateEdit.NullDate = DateTime.MinValue
            _dateEdit.NullText = String.Empty

            Dim datePattern = CultureInfo.CurrentCulture.DateTimeFormat().ShortDatePattern

            If Not showCentury Then datePattern = datePattern.Replace("yyyy", "yy")

            For Each col As GridColumn In _Columns
                col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                col.DisplayFormat.FormatString = datePattern
                col.ColumnEdit = _dateEdit
            Next

            Return Me

        End Function

        Private _MemoEdit As New RepositoryItemMemoExEdit
        Public Function SetFormatAsMemo() As Columns

            With _MemoEdit
                .ShowIcon = False
                .ScrollBars = ScrollBars.Both
                .PopupFormMinSize = New Point(400, 200)
            End With

            For Each col As GridColumn In _Columns
                col.ColumnEdit = _MemoEdit
            Next

            Return Me

        End Function

        Public Function SetFormatBasedOnType() As Columns

            For Each col As GridColumn In _Columns

                If TypeHelper.IsDate(col.ColumnType) Then GridviewHelper.Columns(col).SetFormatAsDate()

                If TypeHelper.IsDecimal(col.ColumnType) Then GridviewHelper.Columns(col).SetFormatAsDecimal()

                If TypeHelper.IsInteger(col.ColumnType) Then GridviewHelper.Columns(col).SetFormatAsInteger()

            Next

            Return Me

        End Function

#End Region

#End Region

    End Class

    Public Class GridviewHelper

#Region "Columns"

        Public Shared Function Columns(ByVal ParamArray target() As GridColumn) As Columns

            Return New Columns(target)

        End Function

        Public Shared Function Columns(ByVal view As GridView) As Columns

            Return Columns(view.Columns)

        End Function

        Public Shared Function Columns(ByVal target As GridColumnCollection) As Columns

            Dim cols As New List(Of GridColumn)

            For Each col In target
                cols.Add(col)
            Next

            Return New Columns(cols.ToArray)

        End Function


#End Region

        Public Shared Sub ExpandAllRows(ByVal view As GridView)

            view.BeginUpdate()

            For i As Integer = 0 To view.DataRowCount - 1
                ExpandAllRows(view, i)
            Next i

            view.EndUpdate()

        End Sub

        Public Class PropertyGridColumn(Of TModel)

            Private _view As GridView

            Public Sub New(ByVal view As GridView)
                _view = view
            End Sub

            Public Function ForProperty(Of TProperty)(ByVal [property] As Expression(Of Func(Of TModel, TProperty))) As GridColumn

                Return _view.Columns(ReflectionHelper.GetPropertyName(Of TModel, TProperty)([property]))

            End Function

        End Class

        Public Shared Function GetGridColumn(Of TModel)(ByVal view As GridView) As PropertyGridColumn(Of TModel)

            Return New PropertyGridColumn(Of TModel)(view)

        End Function

        Public Shared Sub ExpandAllRows(ByVal view As GridView, ByVal masterRowHandle As Integer)

            Dim relationCount As Integer
            Dim childView As ColumnView

            ' Get the number of master-detail relationships. 
            relationCount = view.GetRelationCount(masterRowHandle)

            ' Iterate through relationships. 
            Dim index As Integer
            For index = relationCount - 1 To 0 Step -1

                ' Open the detail View for the current relationship. 
                view.ExpandMasterRow(masterRowHandle, index)

                ' Get the detail View. 
                childView = view.GetDetailView(masterRowHandle, index)

                If TypeOf childView Is GridView Then
                    Dim childRowCount As Integer

                    ' Get the number of rows in the detail View. 
                    childRowCount = CType(childView, GridView).DataRowCount

                    ' Expand child rows recursively. 
                    Dim handle As Integer
                    For handle = 0 To childRowCount - 1
                        ExpandAllRows(childView, handle)
                    Next
                End If

            Next

        End Sub

        Public Shared Sub CollapsAllMasterRows(ByVal view As GridView)

            For i As Integer = 0 To view.DataRowCount - 1
                view.CollapseMasterRow(i)
            Next i

        End Sub


    End Class

End Namespace

