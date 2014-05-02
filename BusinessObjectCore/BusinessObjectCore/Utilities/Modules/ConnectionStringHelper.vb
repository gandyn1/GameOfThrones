Imports System.Data.SqlClient
Imports System.Data.EntityClient
Imports System.Text.RegularExpressions

Namespace Utilities

    Public Class ConnectionStringHelper

        Public Shared Function getConnectionString(ByVal ServerName As String, ByVal DatabaseName As String)
            Return String.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=5", ServerName, DatabaseName)
        End Function

        Public Shared Function getEntityConnectionString(ByVal connectionString As String) As String
            Dim con As New SqlConnectionStringBuilder(connectionString)
            Return getEntityConnectionString(con.InitialCatalog, con.DataSource, con.UserID, con.Password)
        End Function

        Public Shared Function getEntityConnectionString(ByVal Database As String, ByVal Server As String) As String
            Return getEntityConnectionString(Database, Server, Nothing, Nothing)
        End Function

        Public Shared Function getEntityConnectionString(ByVal Database As String, ByVal Server As String,
                                                  ByVal UserName As String, ByVal Password As String) As String

            Dim connectionString As String = New System.Data.EntityClient.EntityConnectionStringBuilder() _
         With {.Metadata = "res://*", _
               .Provider = "System.Data.SqlClient", _
               .ProviderConnectionString = New System.Data.SqlClient.SqlConnectionStringBuilder() _
                 With {.InitialCatalog = Database, _
                       .DataSource = Server, _
                       .IntegratedSecurity = UserName.ToString = "", _
                       .UserID = UserName, _
                       .Password = Password}.ConnectionString}.ConnectionString
            Return connectionString

        End Function

        Public Shared Function areEqual(ByVal connectionString1 As String, ByVal connectionString2 As String)

            Return getInitialCatalog(connectionString1) = getInitialCatalog(connectionString2) AndAlso
                   getDataSource(connectionString1) = getDataSource(connectionString2)

        End Function

        Public Shared Function getInitialCatalog(ByVal connectionString As String) As String

            With New Regex("Initial Catalog=([^;]*)")
                Return .Match(connectionString).Groups(1).Value
            End With

        End Function

        Public Shared Function getDataSource(ByVal connectionString As String) As String

            With New Regex("Data Source=([^;]*)")
                Return .Match(connectionString).Groups(1).Value
            End With

        End Function

    End Class

End Namespace


