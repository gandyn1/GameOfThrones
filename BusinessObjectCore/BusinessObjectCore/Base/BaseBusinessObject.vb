Imports BusinessObjectCore.Utilities

Namespace Base
    Public Enum CrudFlag As Integer
        None = 0
        Create = 1
        Save = 2
        Delete = 4
        All = Create Or Save Or Delete
    End Enum

    ''' <summary>
    ''' Base Business Ojbect that implements IBusinessOjbect, IUpdatable, IGetSingle, IGetList.
    ''' </summary>
    Public MustInherit Class BaseBusinessObject(Of TKey, TBusinessObject As BaseBusinessObject(Of TKey, TBusinessObject))
        Implements IUpdatable, 
                   IGetSingle(Of TBusinessObject, TKey), 
                   IGetList(Of TBusinessObject), 
                   IGetNew(Of TBusinessObject)

        Protected Friend Property ConnectionString As String Implements IBusinessObject.ConnectionString
        Protected Friend Property ErpNameSpace As String Implements IBusinessObject.ActiveNameSpace

        Protected Friend Property CrudFlags As CrudFlag = CrudFlag.All

        Public Overridable Function CanCreate() As Boolean Implements IGetNew.CanCreate
            Return (CrudFlags And CrudFlag.Create) = CrudFlag.Create
        End Function

        Public Overridable Function CanDelete() As Boolean Implements IUpdatable.CanDelete
            Return (CrudFlags And CrudFlag.Delete) = CrudFlag.Delete AndAlso Not IsNew()
        End Function

        Public Overridable Function CanSave() As Boolean Implements IUpdatable.CanSave
            Return (CrudFlags And CrudFlag.Save) = CrudFlag.Save
        End Function

#Region "Get Business Object"

        Public MustOverride ReadOnly Property Key As TKey

        Public Overridable Function GetNew() As TBusinessObject Implements IGetNew(Of TBusinessObject).GetNew
            Return Nothing
        End Function
        Private Function GetNewUntyped() As Object Implements IGetNew.GetNew
            Return GetNew()
        End Function

        Protected MustOverride Function GetSingle(ByVal Key As TKey) As TBusinessObject Implements IGetSingle(Of TBusinessObject, TKey).GetSingle
        Private Function GetSingleUntyped(ByVal Key As Object) As Object Implements IGetSingle.GetSingle
            Return GetSingle(Key)
        End Function

        ''' <summary>
        '''Retrieve a list of objects filtered down by the criteria pass in
        ''' </summary>
        Public MustOverride Function GetList() As IEnumerable(Of TBusinessObject) Implements IGetList(Of TBusinessObject).GetList
        Private Function GetListUntyped() As Object Implements IGetList.GetList
            Return GetList()
        End Function

#End Region

#Region "Save Business Object"

        Public Event OnBeforeInsert()
        Public Event OnBeforeUpdate()
        Public Event OnBeforeDelete()
        Public Event OnBeforeSave()

        Public Event OnInsert()
        Public Event OnUpdate()
        Public Event OnDelete() Implements IUpdatable.OnDelete
        Public Event OnSave() Implements IUpdatable.OnSave

        Public Event OnAfterInsert()
        Public Event OnAfterUpdate()
        Public Event OnAfterDelete()
        Public Event OnAfterSave()

        ''' <summary>
        ''' Invoked on DatabaseUpdate and DatabaseInsert
        ''' </summary>
        ''' <remarks></remarks>  
        Public Sub Delete() Implements IUpdatable.Delete
            If CanDelete() Then
                RaiseEvent OnBeforeDelete()
                RaiseEvent OnDelete()
                RaiseEvent OnAfterDelete()
            End If
        End Sub

        Public Sub Save() Implements IUpdatable.Save

            If CanSave() Then
                RaiseEvent OnBeforeSave()
                If IsNew() Then
                    RaiseEvent OnBeforeInsert()
                    RaiseEvent OnSave()
                    RaiseEvent OnInsert()
                    RaiseEvent OnAfterInsert()
                Else
                    RaiseEvent OnBeforeUpdate()
                    RaiseEvent OnSave()
                    RaiseEvent OnUpdate()
                    RaiseEvent OnAfterUpdate()
                End If
                RaiseEvent OnAfterSave()
            End If

        End Sub

        ''' <summary>
        ''' Used when saving to decide if Create or Update should be Invoked.
        ''' </summary>
        Public Overridable Function IsNew() As Boolean
            Return Key Is Nothing
        End Function

#End Region

#Region "Shared Methods"

        Public Shared Function GetSingle(ByVal BusinessObject As IBusinessObject, ByVal key As TKey) As TBusinessObject
            Return GetSingle(BusinessObject.ActiveNameSpace, BusinessObject.ConnectionString, key)
        End Function
        Public Shared Function GetSingle(ByVal ActiveNameSpace As String, ByVal connectionString As String, ByVal key As TKey) As TBusinessObject
            Return BusinessObjectHelper.GetInstance(Of TBusinessObject)(ActiveNameSpace, connectionString).GetSingle(key)
        End Function

        Public Shared Function GetList(ByVal BusinessObject As IBusinessObject) As IEnumerable(Of TBusinessObject)
            Return GetList(BusinessObject.ActiveNameSpace, BusinessObject.ConnectionString)
        End Function
        Public Shared Function GetList(ByVal ErpNameSpace As String, ByVal connectionString As String) As IEnumerable(Of TBusinessObject)
            Dim list As IEnumerable(Of TBusinessObject) = Nothing
            Using Utility.Logger.TimedLog("{0}.GetList()", GetType(TBusinessObject).Name)
                list = BusinessObjectHelper.GetInstance(Of TBusinessObject)(ErpNameSpace, connectionString).GetList()
            End Using
            Return list
        End Function

        Public Shared Sub Delete(ByVal BusinessObject As IBusinessObject, ByVal Key As TKey)
            Delete(BusinessObject.ActiveNameSpace, BusinessObject.ConnectionString, Key)
        End Sub
        Public Shared Sub Delete(ByVal ErpNameSpace As String, ByVal connectionString As String, ByVal Key As TKey)
            GetSingle(ErpNameSpace, connectionString, Key).Delete()
        End Sub

        Public Shared Function GetNew(ByVal BusinessObject As IBusinessObject) As TBusinessObject
            Return GetNew(BusinessObject.ActiveNameSpace, BusinessObject.ConnectionString)
        End Function
        Public Shared Function GetNew(ByVal ActiveNameSpace As String, ByVal connectionString As String) As TBusinessObject
            Return BusinessObjectHelper.GetInstance(Of TBusinessObject)(ActiveNameSpace, connectionString).GetNew()
        End Function

#End Region

#Region "Helper Methods"

        Protected Friend Function GetInstance() As TBusinessObject
            Return BusinessObjectHelper.GetInstance(Of TBusinessObject)(Me)
        End Function

        Private Shared _connections As New Dictionary(Of String, Connection)
        Public ReadOnly Property Connection As Connection
            Get
                If IsNothing(ConnectionString) Then Return Nothing

                If Not _connections.ContainsKey(ConnectionString) Then _connections.Add(ConnectionString, New Connection(ConnectionString))

                Return _connections(ConnectionString)
            End Get
        End Property

        Protected Friend ReadOnly Property EntityConnectionString As String
            Get
                Return Utilities.ConnectionStringHelper.getEntityConnectionString(ConnectionString)
            End Get
        End Property

#End Region

    End Class

End Namespace


