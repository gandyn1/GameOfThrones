Imports System.Text.RegularExpressions

Namespace Utilities
    Public Class StringHelper

        Public Shared Function ContainsUpperCase(ByVal target As String) As Boolean

            Return target.ToCharArray.Where(Function(o) Char.IsUpper(o)).Count > 0

        End Function

        Public Shared Function ContainsLowerCase(ByVal target As String) As Boolean

            Return target.ToCharArray.Where(Function(o) Char.IsLower(o)).Count > 0

        End Function

        Public Shared Function GetWords(ByVal target As String) As String()

            Dim words As New List(Of String)

            For Each Match In Regex.Matches(target, "[A-Za-z][a-z]*|[0-9][0-9,]*")
                words.Add(Match.Value)
            Next

            Return words.ToArray

        End Function

        Public Shared Function GetAbbreviation(ByVal target() As String) As String

            Dim joinedText = String.Join(" ", target)
            Dim abbreviation As String = GetAbbreviation(joinedText)

            If joinedText = abbreviation Then
                abbreviation = String.Join(" ", target.Select(Function(o) GetAbbreviation(o)).ToArray)
            End If

            Return abbreviation

        End Function

        Public Shared Function GetAbbreviation(ByVal target As String) As String

            target = target

            Select Case target.ToLower
                Case "order", "ordered"
                    Return "Ord"
                Case "invoice"
                    Return "Inv"
                Case "sequence"
                    Return "Seq"
                Case "date"
                    Return "Dt"
                Case "weight"
                    Return "Wt"
                Case "volume"
                    Return "Vol"
                Case "description"
                    Return "Desc"
                Case "estimate", "estimated"
                    Return "Est"
                Case "actual"
                    Return "Act"
                Case "unit of measure"
                    Return "UOM"
                Case "quantity"
                    Return "Qty"
                Case "customer"
                    Return "Cus"
                Case "number"
                    Return "No"
                Case "vendor"
                    Return "Vend"
                Case "comment", "comments"
                    Return "Cmt"
                Case "location"
                    Return "Loc"
                Case "extended", "extension", "extend"
                    Return "Ext"
                Case Else
                    Return target
            End Select

        End Function

    End Class
End Namespace

