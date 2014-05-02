Imports System.Data.Objects
Imports System.Data.SqlClient


Namespace Utilities

    Public Class EntityFrameworkHelper

        Public Shared Sub SaveSingleEntity(ByVal entities() As Object, ByVal context As ObjectContext)

            Dim objs = context.ObjectStateManager.GetObjectStateEntries(EntityState.Added Or EntityState.Deleted Or EntityState.Modified)

            Dim entityStates(objs.Count) As System.Data.EntityState
            Dim skip(objs.Count) As Boolean

            For i As Integer = 0 To objs.Count - 1
                entityStates(i) = objs(i).State
                If Not entities.Contains(objs(i).Entity) Then
                    objs(i).ChangeState(EntityState.Unchanged)
                Else
                    skip(i) = True
                End If
            Next

            context.SaveChanges()
    
            For i As Integer = 0 To objs.Count - 1
                If Not skip(i) Then
                    objs(i).ChangeState(entityStates(i))
                End If
            Next

        End Sub

    End Class

End Namespace

