Namespace Base

    Public Interface IGetNew(Of TBusinessObject)
        Inherits IGetNew

        Shadows Function GetNew() As TBusinessObject

    End Interface

    Public Interface IGetNew

        Function GetNew() As Object
        Function CanCreate() As Boolean

    End Interface

End Namespace
