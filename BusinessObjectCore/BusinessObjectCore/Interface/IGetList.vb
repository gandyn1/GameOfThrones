Namespace Base
    Public Interface IGetList(Of TBusinessObject)
        Inherits IGetList

        Shadows Function GetList() As IEnumerable(Of TBusinessObject)

    End Interface

    Public Interface IGetList

        Function GetList() As Object

    End Interface
End Namespace
