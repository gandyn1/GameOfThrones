Imports System.Reflection
Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Data.Objects

Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Namespace Utilities

    Public Class Utility

        Public Const NAMESPACE_ES As String = "PulseErp.MacolaES"
        Public Const NAMESPACE_PG As String = "PulseErp.MacolaProgression"

#Region "Shared Logger"

        Private Shared _Logger As Logger = Nothing
        Public Shared Property Logger As Logger
            Get
                If (_Logger Is Nothing) Then
                    _Logger = New Logger(String.Format("{0}\Logger.txt", Application.StartupPath))
                End If

                Return _Logger
            End Get
            Set(ByVal value As Logger)
                _Logger = value
            End Set
        End Property

#End Region

#Region "Leahy No-Reply Email"

        Public Shared Function LeahyEmail(ByVal ex As Exception, ByVal ParamArray SendTo() As String) As Gmail

            Dim info = ExceptionHelper.getExceptionInfo(ex)

            Return LeahyEmail(String.Format("Exception occurred from within {0}", info.EntryAssemblyName), info.ToString, SendTo)

        End Function

        Public Shared Function LeahyEmail(ByVal subject As String, ByVal body As String, ByVal ParamArray SendTo() As String) As Gmail

            Dim email = LeahyEmail()
            With email

                For Each send In SendTo
                    .Message.To.Add(send)
                Next

                .Message.Subject = subject
                .Message.Body = body

            End With

            Return email

        End Function

        Public Shared Function LeahyEmail() As Gmail

            Return New Gmail("no-reply@leahyconsulting.com", "mlCONSULTING1234")

        End Function

#End Region

        Public Class Display

        End Class

    End Class

End Namespace






