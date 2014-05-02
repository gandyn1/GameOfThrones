Imports BusinessObjectCore
Imports BusinessObjectCore.Utilities
Imports System.Data.Objects
Imports System.Reflection
Imports System.Text

Public MustInherit Class EntityBusinessObject(Of TKey, TEntityModel As Class, TObjectContext As ObjectContext, TBusinessObject As {EntityBusinessObject(Of TKey, TEntityModel, TObjectContext, TBusinessObject), New})
    Inherits BaseBusinessObject(Of TKey, TBusinessObject)

    Protected ReadOnly Property Context As TObjectContext
        Get
            If _Context Is Nothing Then
                Utility.Logger.Log("ConnectionString Set To: {0}", EntityConnectionString)
                _Context = Activator.CreateInstance(GetType(TObjectContext), EntityConnectionString)
            ElseIf Not ConnectionStringHelper.areEqual(_Context.Connection.ConnectionString, EntityConnectionString) Then
                Utility.Logger.Log("ConnectionString Set To: {0}", EntityConnectionString)
                _Context = Activator.CreateInstance(GetType(TObjectContext), EntityConnectionString)
            End If

            Return _Context
        End Get
    End Property
    Private Shared _Context As TObjectContext


    Public EntityObject As TEntityModel = Nothing

    Private _ReferencedObjects As Func(Of TEntityModel, Object()) = Function(o) {}
    Protected Friend ReadOnly Property ReferencedObjects As Object()
        Get
           SetContext()
            Return _ReferencedObjects(EntityObject)
        End Get
    End Property

    Private _getObjectSet As Func(Of TObjectContext, ObjectSet(Of TEntityModel))
    Protected Friend ReadOnly Property ObjectSet As ObjectSet(Of TEntityModel)
        Get
            SetContext()
            Return _getObjectSet(Context)
        End Get
    End Property

    Private Sub SetContext()
        
    End Sub

    Sub New(ByVal getObjectSet As Func(Of TObjectContext, ObjectSet(Of TEntityModel)),
            Optional ByVal getReferencedObjects As Func(Of TEntityModel, Object()) = Nothing)

        If getObjectSet Is Nothing Then
            Throw New Exception("getObjectSet can not be nothing.")
        End If

        _getObjectSet = getObjectSet

        If Not getReferencedObjects = Nothing Then
            _ReferencedObjects = getReferencedObjects
        End If

    End Sub

    Protected Friend Overloads Function GetKey(ByVal entity As TEntityModel) As TKey
        Dim obj As Object = entity
        Dim key As TKey

        Try
            key = obj.EntityKey.EntityKeyValues(0).Value
        Catch ex As Exception
            Throw New Exception(String.Format("Unable to set key of type {0}.  Try overriding ""Protected Friend Function GetKey(ByVal entity As TEntityModel) As TKey""", GetType(TKey)))
        End Try

        Return key
    End Function

    Public Overrides ReadOnly Property Key As TKey
        Get
            Return GetKey(EntityObject)
        End Get
    End Property

    Public Overrides Function IsNew() As Boolean
        Return CType(EntityObject, Object).EntityKey() Is Nothing
    End Function

    Private Sub Me_OnDelete() Handles Me.OnDelete
        ObjectSet.DeleteObject(EntityObject)        
        SaveEntity()
    End Sub

    Private Sub SaveEntity()
        Utilities.EntityFrameworkHelper.SaveSingleEntity(_ReferencedObjects(EntityObject).Union({EntityObject}).ToArray, Context)
    End Sub

    Public Overloads Overrides Function GetList() As System.Collections.Generic.IEnumerable(Of TBusinessObject)
        Dim listBO As New List(Of TBusinessObject)

        For Each entity In ObjectSet
            Dim bo As TBusinessObject = GetInstance()
            bo.EntityObject = entity
            bo.SetProperties()
            listBO.Add(bo)
        Next

        Return listBO
    End Function

    Protected Overrides Function GetSingle(ByVal Key As TKey) As TBusinessObject
        Dim entity = ObjectSet.ToList.Where(Function(o) GetKey(o).Equals(Key)).SingleOrDefault
        Dim bo As TBusinessObject = Nothing

        If Not IsNothing(entity) Then
            bo = GetInstance()
            bo.EntityObject = entity
            bo.SetProperties()
        End If

        Return bo
    End Function

    Protected MustOverride Sub SetProperties()
    'Protected Friend Overrides Sub Refresh()
    '    Context.Refresh(RefreshMode.StoreWins, EntityObject)
    '    SetProperties()
    'End Sub

    Private Sub Me_OnInsert() Handles Me.OnInsert        
        ObjectSet.AddObject(EntityObject)
        SaveEntity()
    End Sub

    Private Sub Me_OnUpdate() Handles Me.OnUpdate
        SaveEntity()
    End Sub

    Private Sub Me_OnAfterSave() Handles Me.OnAfterSave
        SaveEntity()
    End Sub

    Protected Friend Function ConvertEntityModel(ByVal EntityModel As TEntityModel) As TBusinessObject
        Return ConvertEntityModel(Me, EntityModel)
    End Function

    Public Shared Function ConvertEntityModel(ByVal businessObject As IBusinessObject, ByVal EntityModel As TEntityModel) As TBusinessObject

        Dim bo = BusinessObjectHelper.GetInstance(Of TBusinessObject)(businessObject)
        bo.EntityObject = EntityModel
        bo.SetProperties()
        Return bo

    End Function

    Public Overrides Function GetNew() As TBusinessObject
        If CanCreate() Then
            Dim bo = Me.GetInstance
            bo.EntityObject = Context.CreateObject(Of TEntityModel)()
            bo.SetProperties()
            Return bo
        Else
            Return Nothing
        End If
    End Function

End Class
