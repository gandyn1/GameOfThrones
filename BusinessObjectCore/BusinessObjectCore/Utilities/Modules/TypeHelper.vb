Namespace Utilities

    Public Class TypeHelper

        Public Shared IntegerTypes() As Type = New Type() {GetType(Byte), GetType(Integer), GetType(Long), _
                                                           GetType(SByte), GetType(Short), GetType(UInteger), _
                                                           GetType(ULong), GetType(UShort)}

        Public Shared DecimalTypes() As Type = New Type() {GetType(Decimal), GetType(Double), GetType(Single)}

        Public Shared DateTypes() As Type = New Type() {GetType(Date)}

        Public Shared StringTypes() As Type = New Type() {GetType(String), GetType(Char)}

        Public Shared Function IsInteger(type As Type) As Boolean

            Return IntegerTypes.Contains(type)

        End Function

        Public Shared Function IsDecimal(type As Type) As Boolean

            Return DecimalTypes.Contains(type)

        End Function

        Public Shared Function IsNumeric(type As Type) As Boolean

            Return IsInteger(type) OrElse IsDecimal(type)

        End Function

        Public Shared Function IsDate(type As Type) As Boolean

            Return DateTypes.Contains(type)

        End Function

        Public Shared Function IsString(type As Type) As Boolean

            Return StringTypes.Contains(type)

        End Function

    End Class

End Namespace

