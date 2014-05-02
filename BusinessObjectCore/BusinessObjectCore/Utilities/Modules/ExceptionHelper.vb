Imports System.Reflection

Namespace Utilities

    Public Class ExceptionHelper

        Public Class ExceptionInfo

            Public ReadOnly Property EntryAssemblyName As String
                Get
                    Return EntryAssembly.FullName.Split(",").First()
                End Get
            End Property

            Public ReadOnly Property ExecutingAssemblyName As String
                Get
                    Return ExecutingAssembly.FullName.Split(",").First()
                End Get
            End Property

            Public Property EntryAssembly As Assembly
            Public Property ExecutingAssembly As Assembly

            Public StackLines As New List(Of String)
            Public ReadOnly Property Stack As String
                Get
                    Return String.Join(vbCrLf, StackLines.ToArray)
                End Get
            End Property

            Public Sub New(ByVal ex As Exception)

                Dim line As String

                EntryAssembly = System.Reflection.Assembly.GetEntryAssembly
                ExecutingAssembly = System.Reflection.Assembly.GetExecutingAssembly

                While ex IsNot Nothing
                    Dim curStackTrace As New System.Diagnostics.StackTrace(ex, True)
                    Dim stackFrames() As System.Diagnostics.StackFrame = curStackTrace.GetFrames
                    If stackFrames Is Nothing Then
                        ex = ex.InnerException
                        Continue While
                    End If
                    For Each curFrame As System.Diagnostics.StackFrame In stackFrames
                        line = curFrame.GetFileLineNumber & " at " & curFrame.GetMethod.DeclaringType.Name & "." & curFrame.GetMethod.Name
                        Dim szSignature As String = ""
                        Dim Parameters() As System.Reflection.ParameterInfo = curFrame.GetMethod.GetParameters
                        For Each curParameter As System.Reflection.ParameterInfo In Parameters
                            If szSignature <> "" Then szSignature &= ", "
                            szSignature &= curParameter.ParameterType.Name & " " & curParameter.Name
                        Next
                        line &= "(" & szSignature & ")"
                        StackLines.Add(line)
                    Next

                    ex = ex.InnerException
                End While
            End Sub

            Public Overrides Function ToString() As String

                Return String.Join(vbCrLf, {"Entry Assembly: " & EntryAssemblyName, "Executing Assembly: " & ExecutingAssemblyName, vbCrLf, Stack})

            End Function

        End Class

        Public Shared Function getExceptionInfo(ByVal exception As Exception) As ExceptionInfo

            Return New ExceptionInfo(exception)

        End Function

    End Class

End Namespace

