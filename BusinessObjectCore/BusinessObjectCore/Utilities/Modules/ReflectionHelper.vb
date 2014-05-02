Imports System.Linq.Expressions
Imports System.Reflection

Namespace Utilities

    Public Class ReflectionHelper

        Public Shared Function GetPropertyName(Of TModel, TProperty)(ByVal [property] As Expression(Of Func(Of TModel, TProperty))) As String
            Dim memberExpression As MemberExpression = DirectCast([property].Body, MemberExpression)
            Return memberExpression.Member.Name
        End Function

    End Class

End Namespace


