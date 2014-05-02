Imports System.Reflection
Imports System.IO

Namespace Utilities

    Public Class AssemblyHelper

        Public Shared Function AssemblyPath() As String

            Return AssemblyPath(Reflection.Assembly.GetExecutingAssembly())

        End Function


        Public Shared Function AssemblyPath(ByVal Assembly As Assembly) As String

            Return Path.GetDirectoryName(Assembly.Location)

        End Function

    End Class

End Namespace

