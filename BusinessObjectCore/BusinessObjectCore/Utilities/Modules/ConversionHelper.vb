Imports System.ComponentModel

Namespace Utilities

    Public Class ConversionHelper

        Public Class DBNullConversions

            Public Shared Function ToNothing(ByVal obj As Object)
                If IsDBNull(obj) Then
                    Return Nothing
                Else
                    Return obj
                End If
            End Function

        End Class

        Public Class NothingConversions

            Public Shared Function ToDBNull(ByVal obj As Object) As DBNull
                If IsNothing(obj) OrElse
                TypeOf (obj) Is Date AndAlso Not ValidationHelper.isValidSQLDate(obj) Then
                    Return DBNull.Value
                Else
                    Return obj
                End If
            End Function

            Public Shared Function ToEmptyString(ByVal obj As Object) As Object
                If IsNothing(obj) OrElse
                 TypeOf (obj) Is Date AndAlso Not ValidationHelper.isValidSQLDate(obj) Then
                    Return String.Empty
                Else
                    Return obj
                End If
            End Function

        End Class

        Public Class BooleanConversions

            Public Shared Function ToBoolean(ByRef YesNo As String) As Boolean

                Return Not IsNothing(YesNo) AndAlso Not IsDBNull(YesNo) AndAlso YesNo.ToUpper = "Y"

            End Function

            Public Shared Function ToYesNo(ByRef value As Boolean) As String
                If value Then
                    Return "Y"
                Else
                    Return "N"
                End If
            End Function

        End Class

        Public Class DateConversions

            Public Shared Function ToMacolaDate(ByVal value As Object) As Date

                If TypeOf value Is Date Then
                    Return value
                ElseIf TypeOf value Is DBNull Then
                    Return Nothing
                Else
                    Return IntegerToDate(value)
                End If

            End Function

            Public Shared Function IntegerToDate(ByRef value As Integer) As Date
                Dim ReturnValue As Date

                Try
                    ReturnValue = Date.ParseExact(value.ToString, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                Catch ex As Exception
                    ReturnValue = Date.MinValue
                End Try

                Return ReturnValue
            End Function

            Public Shared Function DateToInteger(ByRef value As Date) As Integer
                Dim ReturnValue As Integer

                ReturnValue = value.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)

                Return ReturnValue
            End Function

            Public Shared Function TimeToInteger(ByRef value As Date) As Integer
                Dim ReturnValue As Integer

                If IsNothing(value) Then
                    ReturnValue = 0
                Else
                    Dim Hour As String = value.Hour
                    Dim Minute As String = value.Minute
                    If Minute.Length = 1 Then
                        Minute = "0" & Minute
                    End If
                    Dim Second As String = value.Second
                    If Second.Length = 1 Then
                        Second = "0" & Second
                    End If

                    ReturnValue = Hour & Minute & Second
                End If

                Return ReturnValue
            End Function

        End Class

        Public Class ListConversions

            Public Shared Function ToBaseList(Of BaseType)(ByVal target As IList) As List(Of BaseType)

                Dim BaseClassList As New List(Of BaseType)

                For Each obj In target
                    BaseClassList.Add(obj)
                Next

                Return BaseClassList

            End Function

            Public Shared Function ToBindingList(Of T)(ByVal target As List(Of T)) As BindingList(Of T)

                Dim value = New BindingList(Of T)

                For Each o In target
                    value.Add(o)
                Next

                Return value

            End Function

        End Class

    End Class

End Namespace
