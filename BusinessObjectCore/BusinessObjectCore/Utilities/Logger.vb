Imports System.IO
Imports System.ComponentModel
Imports System.Reflection

Namespace Utilities
    Public Class Logger

#Region "Settings"

        Public Enum FormatAs
            Text
            Sql
        End Enum

        <Flags()>
        Public Enum LogToFlags As Integer
            FileAppend = 1
            FileOverwrite = 2
            Msgbox = 4
        End Enum
        ''' <summary>
        ''' Where to log.
        ''' Default Value: FileAppend
        ''' </summary>
        Public Property LogTo As LogToFlags = LogToFlags.FileAppend

        <Flags()>
        Public Enum LogWhenFlags
            Never = 0
            Debug = 1
            Release = 2
            Always = Debug Or Release
        End Enum
        ''' <summary>
        ''' When to log.    
        ''' Default Value: Never 
        ''' </summary>
        Public Property LogWhen As LogWhenFlags = LogWhenFlags.Debug

        ''' <summary>
        ''' If set to false, data will be logged when the program ends.     
        ''' If an unhandled exception occurs, nothing will be logged.    
        ''' Default Value: False
        ''' </summary>
        Public Property LogRealTime As Boolean = False

        Private ReadOnly Property LoggingData As Boolean
            Get
                Dim isDebugMode As Boolean = False

#If DEBUG Then
                isDebugMode = True
#End If
                Return ((LogWhen And LogWhenFlags.Debug) = LogWhenFlags.Debug AndAlso isDebugMode) OrElse
                       ((LogWhen And LogWhenFlags.Release) = LogWhenFlags.Release AndAlso Not isDebugMode)
            End Get
        End Property

        Private _FilePath As String
        Public ReadOnly Property FilePath As String
            Get
                Return _FilePath
            End Get
        End Property


#End Region

        Public Sub New(ByVal FilePath As String)

            _FilePath = FilePath

        End Sub

        Private Shared _CallingAssemblyLogCount As IDictionary(Of Assembly, Integer) = New Dictionary(Of Assembly, Integer)

        Public Function TimedLog(ByVal format As String, ByVal ParamArray args() As Object) As TimedLog

            Return TimedLog(String.Format(format, args))

        End Function

        Public Function TimedLog(ByVal task As String) As TimedLog

            Log(String.Format("Started {0}.", task))

            Return New TimedLog(Me, task)

        End Function

#Region "Log Unhandled Exceptions"

        ''' <summary>
        ''' Logs UnHandled exceptions when the debugger is not attached, and exits the program
        ''' </summary>
        Public Sub LogUnhandledExceptionAndExit()

            LogUnhandledExceptions(Sub(ex As Exception)
                                       MsgBox(String.Format("We're sorry.  An error has occurred in {0}." & vbNewLine & "The application will close immediately.", _
                                                            Application.ProductName), vbCritical, "Error")
                                       Application.Exit()
                                   End Sub)

        End Sub

        ''' <summary>
        ''' Logs UnHandled exceptions when the debugger is not attached
        ''' </summary>
        Public Sub LogUnhandledExceptions()

            LogUnhandledExceptions(Nothing)

        End Sub

        ''' <summary>
        ''' Logs UnHandled exceptions when the debugger is not attached
        ''' </summary>
        Public Sub LogUnhandledExceptions(ByVal callBack As Action(Of Exception))

            AddHandler Application.ThreadException, Sub(sender As Object, t As Threading.ThreadExceptionEventArgs)

                                                        LogAlways(t.Exception)

                                                        If callBack IsNot Nothing Then callBack.Invoke(t.Exception)

                                                    End Sub

        End Sub

#End Region

#Region "Log Overloads"

#Region "Log"

        Public Sub Log(ByVal target As String)

            LogObjectAs(False, target, FormatAs.Text)

        End Sub

        Public Sub Log(ByVal target As Object)

            LogObjectAs(False, target, FormatAs.Text)

        End Sub

        Public Sub Log(ByVal target As Object, ByVal formatAs As FormatAs)

            LogObjectAs(False, target, formatAs)

        End Sub

#End Region

#Region "Log Always"

        Public Sub LogAlways(ByVal target As String)

            LogObjectAs(True, target, FormatAs.Text)

        End Sub

        Public Sub LogAlways(ByVal target As Object)

            LogObjectAs(True, target, FormatAs.Text)

        End Sub

        Public Sub LogAlways(ByVal target As Object, ByVal formatAs As FormatAs)

            LogObjectAs(True, target, formatAs)

        End Sub

#End Region

#End Region

        Private Sub LogObjectAs(ByVal alwaysLogThis As Boolean, ByVal target As Object, ByVal formatAs As FormatAs)

            'Log Exception
            If TypeOf target Is Exception Then
                With Utilities.ExceptionHelper.getExceptionInfo(target)
                    Log(alwaysLogThis, String.Format("Entry Assembly: {0}", .EntryAssembly))
                    Log(alwaysLogThis, String.Format("Executing Assembly: {0}", .ExecutingAssembly))

                    If Not .StackLines.FirstOrDefault Is Nothing Then
                        Log(alwaysLogThis, String.Format("Stack 1: {0}", .StackLines(0)))
                    End If
                    For i As Integer = 2 To .StackLines.Count
                        Log(alwaysLogThis, String.Format("      {0}: {1}", i, .StackLines(i - 1)))
                    Next
                End With
            Else
                'Log Sql query
                If formatAs = Logger.FormatAs.Sql Then
                    Log(alwaysLogThis, Utilities.SqlHelper.FormatQuery(target))
                End If

                'Log Text
                If formatAs = Logger.FormatAs.Text Then
                    Log(alwaysLogThis, target.ToString)
                End If
            End If

        End Sub

        Private LinesToLogRealTime As New List(Of String)
        Private Sub Log(ByVal alwaysLogThis As Boolean, ByVal msg As String)

            If LoggingData OrElse alwaysLogThis Then

                'On first call to log, delete the file
                If _CallingAssemblyLogCount.Count = 0 AndAlso (LogTo And LogToFlags.FileOverwrite) = LogToFlags.FileOverwrite Then
                    If File.Exists(FilePath) Then
                        File.Delete(FilePath)
                    End If
                End If

                Dim CallingAssembly = Assembly.GetCallingAssembly()
                If Not _CallingAssemblyLogCount.ContainsKey(CallingAssembly) Then _CallingAssemblyLogCount.Add(CallingAssembly, 0)

                _CallingAssemblyLogCount(CallingAssembly) += 1

                'Log to File
                If (LogTo And LogToFlags.FileAppend) = LogToFlags.FileAppend OrElse
                    (LogTo And LogToFlags.FileOverwrite) = LogToFlags.FileOverwrite OrElse
                    alwaysLogThis Then

                    LinesToLogRealTime.Add(String.Format("[{2,-20}:    {3, 10}][{0}]      {1}", Now, msg, CallingAssembly.GetName.Name, _CallingAssemblyLogCount(CallingAssembly)))

                    If LogRealTime Then
                        If Not File.Exists(FilePath) Then File.Create(FilePath).Dispose()
                        Using s = File.AppendText(FilePath)
                            s.WriteLine(LinesToLogRealTime.Last)
                            LinesToLogRealTime.RemoveAt(LinesToLogRealTime.Count - 1)
                        End Using
                    End If

                End If

                'Log to Msgbox
                If (LogTo And LogToFlags.Msgbox) = LogToFlags.Msgbox Then
                    MsgBox(msg)
                End If

            End If

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()

            If LinesToLogRealTime.Count > 0 Then
                If Not File.Exists(FilePath) Then File.Create(FilePath).Dispose()
                Using s = File.AppendText(FilePath)
                    For Each line In LinesToLogRealTime
                        s.WriteLine(line)
                    Next
                End Using
            End If

        End Sub

    End Class

#Region "Timed Log"

    Public Class TimedLog
        Implements IDisposable

        Private _logger As Logger
        Private _startTime As Date
        Private _message As String

        Private Sub New()

        End Sub

        Sub New(ByVal logger As Logger, ByVal message As String)
            _logger = logger
            _message = message
            _startTime = Now
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            _logger.Log(String.Format("Finished {0} in {1} ms.", _message, Now.Subtract(_startTime).TotalMilliseconds.ToString("N0")))
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

#End Region
End Namespace



