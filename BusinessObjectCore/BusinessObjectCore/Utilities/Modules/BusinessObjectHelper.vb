Namespace Utilities

    Public Class BusinessObjectHelper

        Public Shared Sub SaveAll(ByVal list As IEnumerable(Of IUpdatable))
            For Each value In list
                value.Save()
            Next
        End Sub

        Public Shared Function GetInstance(Of T As IBusinessObject)(ByVal copyPropertiesFrom As IBusinessObject) As T

            Return GetInstance(Of T)(copyPropertiesFrom.ActiveNameSpace, copyPropertiesFrom.ConnectionString)

        End Function

        Public Shared Function GetInstance(Of T As IBusinessObject)(ByVal erpNamespace As String, ByVal connectionString As String) As T
            Dim obj As T = Nothing

            If GetType(T).IsAbstract Then
                obj = Activator.CreateInstance(System.Type.GetType(erpNamespace & "." & GetType(T).Name), True)
            Else
                obj = Activator.CreateInstance(GetType(T), True)
            End If

            obj.ConnectionString = connectionString
            obj.ActiveNameSpace = erpNamespace
            Return obj
        End Function


    End Class

End Namespace


