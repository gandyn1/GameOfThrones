Imports System.Net.Mail

Namespace BO
    Public Class Email

        Public Property EmailAddress As String
        Protected Property Password As String
        Protected Property SmtpServer As String
        Protected Property ImapServer As String
        Protected Property SmtpPort As Integer
        Protected Property ImapPort As Integer

        Public Sub New(ByVal email As String, ByVal password As String, _
                        ByVal smtpServer As String, ByVal imapServer As String, _
                        ByVal smtpPort As Integer, ByVal imapPort As Integer)

            Me.EmailAddress = email
            Me.Password = password
            Me.SmtpServer = smtpServer
            Me.ImapServer = imapServer
            Me.SmtpPort = smtpPort
            Me.ImapPort = imapPort

            Message = New MailMessage
            Message.From = New MailAddress(Me.EmailAddress)

        End Sub

        Public Property Message As MailMessage = Nothing

        Public Function Send() As Boolean

            Dim check As Boolean
            Dim server As New SmtpClient(SmtpServer)
            server.Port = SmtpPort
            server.Credentials = New System.Net.NetworkCredential(EmailAddress, Password)
            server.EnableSsl = True

            Try
                server.Send(Message)
                check = True
            Catch ex As Exception
                Utilities.Utility.Logger.LogAlways(ex)
            End Try

            Return check

        End Function

        'Private Shared Sub FetchEmails()

        '    Using cl As New ImapClient("")
        '        cl.Port = 993
        '        cl.Ssl = True
        '        cl.UserName = "Ngandy@leahyconsulting.com"
        '        cl.Password = "Shawnee18"
        '        cl.ServerName = "Imap.gmail.com"
        '        cl.Authenticate()

        '        'Select Folder
        '        Dim folder As ImapFolder = cl.SelectFolder("[Gmail]/All Mail")
        '        Dim list = cl.ExecuteSearch("UNSEEN UNDELETED")

        '        For i As Integer = 0 To list.MailIndexList.Count - 1
        '            Dim msg = cl.GetMessage(list.MailIndexList(i))
        '            MsgBox("")
        '        Next

        '    End Using

        'End Sub

    End Class
End Namespace

