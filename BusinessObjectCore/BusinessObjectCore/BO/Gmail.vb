Namespace BO
    Public Class Gmail
        Inherits Email

        Public Sub New(ByVal email As String, ByVal password As String)
            MyBase.New(email, password, "smtp.gmail.com", "Imap.gmail.com", 587, 993)
        End Sub

    End Class
End Namespace

