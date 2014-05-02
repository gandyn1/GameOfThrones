Imports BusinessObjectCore.Utilities
Imports System.Data.SqlClient

Namespace Utilities
    Public Class Connection

        Private _ConnectionString As String

        Public Sub New(ByVal connectionString As String)
            _ConnectionString = connectionString
        End Sub

        Public Sub ExecuteNonQuery(ByVal CommandText As String, ByVal ParamArray params() As SqlParameter)
            ExecuteNonQuery(_ConnectionString, CommandText, params)
        End Sub

        Public Shared Sub ExecuteNonQuery(ByVal connectionString As String, ByVal CommandText As String, ByVal ParamArray params() As SqlParameter)

            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Using Command As SqlCommand = connection.CreateCommand
                    Command.CommandText = CommandText
                    SetNothingToDBNull(params)
                    Command.Parameters.AddRange(params)
                    Command.ExecuteNonQuery()
                End Using
            End Using

        End Sub

        Public Function ExecuteDataTable(ByVal CommandText As String, ByVal ParamArray params() As SqlParameter) As DataTable
            Return ExecuteDataTable(_ConnectionString, CommandText, params)
        End Function

        Public Shared Function ExecuteDataTable(ByVal connectionString As String, ByVal CommandText As String, ByVal ParamArray params() As SqlParameter) As DataTable

            Dim dataTable As New DataTable

            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Using Command As SqlCommand = connection.CreateCommand
                    Command.CommandText = CommandText
                    SetNothingToDBNull(params)
                    Command.Parameters.AddRange(params)

                    Dim SqlDataAdapter As New SqlDataAdapter(Command)

                    SqlDataAdapter.Fill(dataTable)
                End Using
            End Using

            Return dataTable

        End Function

        Public Function ExecuteScaler(ByVal CommandText As String, ByVal ParamArray params() As SqlParameter)
            Return ExecuteScaler(_ConnectionString, CommandText, params)
        End Function

        Public Shared Function ExecuteScaler(ByVal connectionString As String, ByVal CommandText As String, ByVal ParamArray params() As SqlParameter)
            Dim obj As Object = Nothing

            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Using Command As SqlCommand = connection.CreateCommand
                    Command.CommandText = CommandText
                    SetNothingToDBNull(params)
                    Command.Parameters.AddRange(params)
                    obj = Command.ExecuteScalar()
                End Using
            End Using

            Return ConversionHelper.DBNullConversions.ToNothing(obj)

        End Function

        Private Shared Sub SetNothingToDBNull(ByRef params() As SqlParameter)

            For i As Integer = 0 To params.Count - 1
                params(i).Value = ConversionHelper.NothingConversions.ToDBNull(params(i).Value)
            Next

        End Sub



    End Class
End Namespace


