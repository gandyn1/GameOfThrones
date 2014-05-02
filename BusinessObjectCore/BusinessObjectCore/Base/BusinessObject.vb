Imports BusinessObjectCore.Utilities

Namespace Base

    Public MustInherit Class Criteria(Of TKey)
        Public Property Key As TKey
    End Class

    Public MustInherit Class BusinessObject(Of TKey, TCriteria As Criteria(Of TKey), TBusinessObject As {BusinessObject(Of TKey, TCriteria, TBusinessObject), New})
        Inherits BaseBusinessObject(Of TKey, TBusinessObject)

        Protected Friend MustOverride Overloads Function GetList(ByVal Criteria As TCriteria) As IEnumerable(Of TBusinessObject)

        Public Overloads Shared Function GetList(ByVal BusinessObject As IBusinessObject, ByVal Criteria As TCriteria) As IEnumerable(Of TBusinessObject)
            Return GetList(BusinessObject.ActiveNameSpace, BusinessObject.ConnectionString, Criteria)
        End Function
        Public Overloads Shared Function GetList(ByVal ErpNameSpace As String, ByVal connectionString As String, ByVal Criteria As TCriteria) As IEnumerable(Of TBusinessObject)
            Dim list As IEnumerable(Of TBusinessObject) = Nothing
            If Criteria Is Nothing Then
                Criteria = Activator.CreateInstance(Of TCriteria)()
            End If
            Using Utility.Logger.TimedLog("{0}.GetList()", GetType(TBusinessObject).Name)
                list = BusinessObjectHelper.GetInstance(Of TBusinessObject)(ErpNameSpace, connectionString).GetList(Criteria)
            End Using
            Return list
        End Function

        Public Overloads Overrides Function GetList() As IEnumerable(Of TBusinessObject)
            Dim criteria As TCriteria = Nothing
            Return GetList(criteria)
        End Function

        Protected Overloads Overrides Function GetSingle(ByVal Key As TKey) As TBusinessObject
            Dim Criteria As TCriteria = Activator.CreateInstance(Of TCriteria)()
            Criteria.Key = Key
            Return GetList(Criteria).SingleOrDefault
        End Function

    End Class

End Namespace

