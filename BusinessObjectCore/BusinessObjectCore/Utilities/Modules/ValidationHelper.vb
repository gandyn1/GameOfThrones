Imports System.Text.RegularExpressions

Namespace Utilities

    Public Class ValidationHelper

        Public Const REGEX_EMAIL_ADDRESS As String = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])"

        Public Shared Function isEmailAddress(ByVal input As String) As Boolean

            Dim check As Boolean = Regex.Match(input, REGEX_EMAIL_ADDRESS).Success

            If check Then check = input.Replace("@", "").Length = input.Length - 1

            Return check

        End Function

        Public Shared Function isValidSQLDate(ByVal Target As Date) As Boolean

            Dim low As Date = "1/1/1753"
            Dim high As Date = "12/31/9999"

            Return Target >= low AndAlso Target <= high

        End Function

    End Class

End Namespace

