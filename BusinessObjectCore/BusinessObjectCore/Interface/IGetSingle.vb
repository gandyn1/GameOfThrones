Namespace Base
    Public Interface IGetSingle(Of TBusinessObject, TKey)
        Inherits IGetSingle

        Shadows Function GetSingle(ByVal Key As TKey) As TBusinessObject

    End Interface

    Public Interface IGetSingle
        Inherits IBusinessObject

        Function GetSingle(ByVal Key As Object) As Object

    End Interface
End Namespace

