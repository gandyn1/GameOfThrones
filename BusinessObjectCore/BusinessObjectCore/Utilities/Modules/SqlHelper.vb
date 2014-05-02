Imports System.Data.SqlClient

Namespace Utilities

    Public Class SqlHelper

        Private Class CQueryFormatter
            '--------------------------------------------------------------------------------
            'Name: FormatVBSQLVarToQuery
            'Abtract: take the quotes off
            '--------------------------------------------------------------------------------
            Public Shared Function FormatVBSQLVarToQuery(ByVal v_strValue As String) As String
                Dim strReturn As String = ""
                Dim astrBuffer() As String
                Dim intIndexOfFirstQuoteOccurrence As Integer

                'Is this string empty?
                If v_strValue <> "" Then
                    astrBuffer = Split(v_strValue, vbCrLf)
                    intIndexOfFirstQuoteOccurrence = InStr(astrBuffer(0), """") - 1
                    If intIndexOfFirstQuoteOccurrence = -1 Then intIndexOfFirstQuoteOccurrence = 0

                    'Remove Dim blah as string
                    astrBuffer(0) = astrBuffer(0).Substring(intIndexOfFirstQuoteOccurrence, astrBuffer(0).Length - intIndexOfFirstQuoteOccurrence)

                    'Remove Quotes
                    For intIndex As Integer = 0 To astrBuffer.Length - 1
                        astrBuffer(intIndex) = astrBuffer(intIndex).Trim()
                        astrBuffer(intIndex) = astrBuffer(intIndex).TrimStart("""")
                        astrBuffer(intIndex) = astrBuffer(intIndex).Trim()
                        astrBuffer(intIndex) = astrBuffer(intIndex).TrimEnd("_")
                        astrBuffer(intIndex) = astrBuffer(intIndex).Trim()
                        astrBuffer(intIndex) = astrBuffer(intIndex).TrimEnd("&")
                        astrBuffer(intIndex) = astrBuffer(intIndex).Trim()
                        astrBuffer(intIndex) = astrBuffer(intIndex).TrimEnd("""")
                        astrBuffer(intIndex) = astrBuffer(intIndex).Trim()

                        'Add to return
                        strReturn += astrBuffer(intIndex) & vbCrLf
                    Next

                    'Trim last new line
                    strReturn = strReturn.TrimEnd(vbCrLf)
                End If

                'Return
                Return strReturn
            End Function

            '--------------------------------------------------------------------------------
            'Name: CondensedSQLFormat
            'Abtract: Format the Keywords and Paragraphs in a condensed structure
            '--------------------------------------------------------------------------------
            Public Shared Function CondensedSQLFormat(ByVal v_strValue As String, Optional ByVal v_blnVBVar As Boolean = False) As String

                Dim strReturn As String = ""
                Dim lstSQLKeyWords As New List(Of String)
                Dim lstSQLKeyWordParagraphs As New List(Of String)
                Dim lstBuffer As List(Of String) = Nothing

                'Break down String
                Try
                    BreakDownSQLKeyWords(v_strValue, lstSQLKeyWords, lstSQLKeyWordParagraphs)
                Catch ex As Exception
                    Debug.WriteLine("Error on CQueryFormatter.BreakDownSQLKeyWords()")
                    Return v_strValue
                End Try

                'Create Script
                For intIndex As Integer = 0 To lstSQLKeyWords.Count - 1
                    strReturn += lstSQLKeyWords.Item(intIndex)
                    If lstSQLKeyWordParagraphs.Item(intIndex).Trim <> "" AndAlso lstSQLKeyWordParagraphs.Item(intIndex).Trim.Replace("(", "") = "" Then
                        lstSQLKeyWords.Item(intIndex + 1) = lstSQLKeyWordParagraphs.Item(intIndex) & lstSQLKeyWords.Item(intIndex + 1)
                    Else
                        Select Case lstSQLKeyWords.Item(intIndex).Replace("(", "").Replace(")", "").Trim
                            Case "SELECT"
                                lstBuffer = SplitSQLLineOnCommas(lstSQLKeyWordParagraphs.Item(intIndex))
                                lstBuffer = JoinListOnDelimiter(lstBuffer, ", ", 150)
                            Case "WHERE"
                                lstBuffer = SplitSQLLineOnAnds(lstSQLKeyWordParagraphs.Item(intIndex))
                                lstBuffer = JoinListOnDelimiter(lstBuffer, " AND ", 150)
                            Case Else
                                lstBuffer = New List(Of String)
                                lstBuffer.Add(lstSQLKeyWordParagraphs.Item(intIndex))
                        End Select
                        For intParagraphIndex As Integer = 0 To lstBuffer.Count - 1
                            If intParagraphIndex <> 0 Then strReturn += vbTab
                            strReturn += lstBuffer(intParagraphIndex)
                            If intParagraphIndex <> lstBuffer.Count - 1 Then strReturn += vbCrLf
                        Next
                    End If
                    'Newline
                    If intIndex <> lstSQLKeyWords.Count - 1 Then strReturn += vbCrLf
                Next

                'Format for a visual basic variable?
                If v_blnVBVar Then
                    strReturn = PrepareSQLQueryForVariable(strReturn)
                End If

                'Return
                Return strReturn
            End Function


            '--------------------------------------------------------------------------------
            'Name: PrepareSQLQueryForVariable
            'Abtract: wrap in quotes for 
            '--------------------------------------------------------------------------------
            Private Shared Function PrepareSQLQueryForVariable(ByVal v_strValue As String)
                Dim strReturn As String = ""
                Dim astrBuffer() As String = Split(v_strValue, vbCrLf)
                For intIndex As Integer = 0 To astrBuffer.Length - 1
                    strReturn += """" & astrBuffer(intIndex) & """"
                    If intIndex <> astrBuffer.Length - 1 Then strReturn += " & _" & vbCrLf
                Next
                Return strReturn
            End Function


            '--------------------------------------------------------------------------------
            'Name: SplitSQLLineOnCommas
            'Abtract: split up on commans except ignore commas used in function ex. Isnull(blah,''),
            '--------------------------------------------------------------------------------
            Private Shared Function SplitSQLLineOnCommas(ByVal v_strValue As String) As List(Of String)
                Dim intParenthesesCount As Integer = 0
                Dim lstSplitOnCommas As New List(Of String)
                Dim chrCurrentCharacter As Char = ""
                Dim strCurrentWord As String = ""

                'Iterate character by character
                For intCharacterIndex As Integer = 0 To v_strValue.Length - 1
                    chrCurrentCharacter = v_strValue(intCharacterIndex)
                    Select Case chrCurrentCharacter
                        Case "(" : intParenthesesCount += 1
                        Case ")" : intParenthesesCount -= 1
                        Case ","
                            If intParenthesesCount = 0 Then
                                lstSplitOnCommas.Add(strCurrentWord)
                                strCurrentWord = ""
                            Else
                                strCurrentWord += chrCurrentCharacter
                            End If
                    End Select
                    If chrCurrentCharacter <> "," Then strCurrentWord += chrCurrentCharacter
                Next

                'add last word
                lstSplitOnCommas.Add(strCurrentWord)

                'Return
                Return lstSplitOnCommas
            End Function


            '--------------------------------------------------------------------------------
            'Name: SplitSQLLineOnAnds
            'Abtract: split up on Ands
            '--------------------------------------------------------------------------------
            Private Shared Function SplitSQLLineOnAnds(ByVal v_strValue As String) As List(Of String)
                Dim lstSplitOnAnds As New List(Of String)
                Dim astrValue() As String

                'Split and convert to a list
                astrValue = Split(v_strValue, " AND ")
                For intIndex As Integer = 0 To astrValue.Length - 1
                    lstSplitOnAnds.Add(astrValue(intIndex))
                Next

                'Return
                Return lstSplitOnAnds
            End Function


            '--------------------------------------------------------------------------------
            'Name: JoinListOnDelimiter
            'Abtract: asdf  bssdgb  asdgadg = asdf, bssdgb, asdgadg
            '--------------------------------------------------------------------------------
            Private Shared Function JoinListOnDelimiter(ByVal v_lstValue As List(Of String), ByVal v_strDelimeter As String _
                                                 , ByVal v_intMaxLength As Integer) As List(Of String)
                Dim strCurrentValue As String = ""
                Dim strCurrentLine As String = ""
                Dim lstReturn As New List(Of String)

                For intListIndex As Integer = 0 To v_lstValue.Count - 1
                    strCurrentValue = v_lstValue(intListIndex)

                    'Check Length
                    If strCurrentLine.Length + v_strDelimeter.Length + strCurrentValue.Length <= v_intMaxLength Then
                        strCurrentLine += v_strDelimeter + strCurrentValue
                    Else
                        If strCurrentLine <> "" Then
                            If lstReturn.Count = 0 Then strCurrentLine = strCurrentLine.Remove(0, v_strDelimeter.Length)
                            lstReturn.Add(strCurrentLine)
                        End If
                        strCurrentLine = v_strDelimeter + strCurrentValue
                    End If
                Next

                If lstReturn.Count = 0 Then strCurrentLine = strCurrentLine.Remove(0, v_strDelimeter.Length)
                lstReturn.Add(strCurrentLine)

                'Return
                Return lstReturn
            End Function


            '--------------------------------------------------------------------------------
            'Name: BreakDownSQLKeyWords
            'Abtract: Break up string into Sql key words (Select,From,ect)
            '--------------------------------------------------------------------------------
            Private Shared Sub BreakDownSQLKeyWords(ByRef v_strValue As String, ByRef lstSQLKeyWords As List(Of String), ByRef lstSQLKeyWordParagraphs As List(Of String))
                Dim strKeyWords As String
                Dim strBuffer As String = ""
                Dim astrBuffer() As String
                Dim aSQLKeyWordParagraphs() As String
                Dim aSQLKeyWordParagraphsBuffer() As String
                Dim astrSQLKeyWords() As String = {"LEFT OUTER JOIN", "RIGHT OUTER JOIN", "FULL OUTER JOIN", "NATURAL JOIN", "CROSS JOIN", "GROUP BY", "UNION ALL", "INNER JOIN" _
                                                   , "LEFT JOIN", "RIGHT JOIN", "FULL JOIN", "ORDER BY", "CREATE TABLE", "CREATE DB", "JOIN", "SELECT", "INSERT", "INTO", "FROM", "WHERE", "HAVING", "UNION"}
                'Prepare to analyze
                v_strValue = Replace(v_strValue, vbCrLf, " ")
                strKeyWords = v_strValue

                'Replace each keyword with newline
                For Each strKeyWord As String In astrSQLKeyWords
                    v_strValue = Replace(v_strValue, " " & strKeyWord & " ", " " & vbCrLf & " ", 1, -1, vbTextCompare)
                    v_strValue = Replace(v_strValue, " " & strKeyWord, " " & vbCrLf, 1, -1, vbTextCompare)
                    v_strValue = Replace(v_strValue, strKeyWord & " ", vbCrLf & " ", 1, -1, vbTextCompare)
                    v_strValue = Replace(v_strValue, "(" & strKeyWord, "(" & vbCrLf, 1, -1, vbTextCompare)
                    v_strValue = Replace(v_strValue, strKeyWord & ")", vbCrLf & ")", 1, -1, vbTextCompare)
                    v_strValue = Replace(v_strValue, "(" & strKeyWord & " ", "(" & vbCrLf & " ", 1, -1, vbTextCompare)
                    v_strValue = Replace(v_strValue, " " & strKeyWord & ")", " " & vbCrLf & ")", 1, -1, vbTextCompare)
                    v_strValue = Replace(v_strValue, "(" & strKeyWord & ")", "(" & vbCrLf & ")", 1, -1, vbTextCompare)
                Next
                aSQLKeyWordParagraphs = Split(v_strValue, vbCrLf)

                'Remove Each of the Paragraphs found to leave only key words
                aSQLKeyWordParagraphsBuffer = aSQLKeyWordParagraphs.Clone
                For intIndex As Integer = 0 To aSQLKeyWordParagraphsBuffer.Length - 1
                    If aSQLKeyWordParagraphsBuffer(intIndex) <> "" Then
                        'Update Buffer
                        strKeyWords = strKeyWords.Replace(aSQLKeyWordParagraphsBuffer(intIndex), "")
                        'Update rest of array
                        For intIndex2 As Integer = intIndex + 1 To aSQLKeyWordParagraphsBuffer.Length - 1
                            aSQLKeyWordParagraphsBuffer(intIndex2) = aSQLKeyWordParagraphsBuffer(intIndex2).Replace(aSQLKeyWordParagraphsBuffer(intIndex), "")
                        Next
                    End If
                Next

                'Add to Collections
                strKeyWords = strKeyWords.Replace(" ", "")
                For intIndex As Integer = 0 To astrSQLKeyWords.Length - 1
                    strKeyWords = Replace(strKeyWords, astrSQLKeyWords(intIndex).Replace(" ", ""), intIndex & "|", 1, -1, vbTextCompare)
                Next
                astrBuffer = strKeyWords.TrimEnd("|").Split("|")
                For Each intValue As Integer In astrBuffer
                    lstSQLKeyWords.Add(astrSQLKeyWords(intValue))
                Next
                For Each strValue As String In aSQLKeyWordParagraphs
                    If strValue <> "" Then
                        lstSQLKeyWordParagraphs.Add(strValue)
                    End If
                Next
            End Sub
        End Class

        Public Shared Function FormatQuery(ByVal query As String) As String
            Return CQueryFormatter.CondensedSQLFormat(query, False)
        End Function

        Public Shared Function ReplaceParameters(ByVal query As String, ByVal ParamArray params() As SqlParameter)

            Return ReplaceParameters(query, False, params)
            
        End Function

        Public Shared Function ReplaceParameters(ByVal query As String, ByVal formatAsText As Boolean, ByVal ParamArray params() As SqlParameter)

            For Each param In params
                If formatAsText Then
                    query = query.Replace(param.ParameterName, "'" & param.Value & "'")
                Else
                    query = query.Replace(param.ParameterName, param.Value)
                End If
            Next

            Return query

        End Function


        <Obsolete("Use ReplaceParameters or ReplaceParametersFormatAsText instead.")>
        Public Shared Function FormatQuery(ByVal query As String, ByVal ParamArray params() As SqlParameter) As String
            Return FormatQuery(ReplaceParametersWithValues(query, params))
        End Function

        <Obsolete("Use ReplaceParameters or ReplaceParametersFormatAsText instead.")>
        Public Shared Function ReplaceParametersWithValues(ByVal query As String, ByVal ParamArray params() As SqlParameter)

            For Each param In params
                query = query.Replace(param.ParameterName, "'" & param.Value & "'")
            Next

            Return query

        End Function

    End Class
End Namespace

