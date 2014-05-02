Namespace Base
    Public Interface IUpdatable
        Inherits IBusinessObject

        Event OnSave()
        Event OnDelete()

        Sub Save()
        Sub Delete()

        Function CanSave() As Boolean
        Function CanDelete() As Boolean

    End Interface
End Namespace
