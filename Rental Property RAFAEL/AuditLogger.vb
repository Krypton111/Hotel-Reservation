Imports Microsoft.Data.SqlClient

Public Module AuditLogger

    Private ReadOnly SyncRoot As New Object()
    Private _tableReady As Boolean = False

    Private Sub EnsureTable(connection As SqlConnection)
        If _tableReady Then Return

        SyncLock SyncRoot
            If _tableReady Then Return

            Dim sql As String =
                "IF OBJECT_ID(N'dbo.UserActivityLog', N'U') IS NULL " &
                "BEGIN " &
                "CREATE TABLE dbo.UserActivityLog (" &
                "ActivityID INT IDENTITY(1,1) NOT NULL PRIMARY KEY, " &
                "UserID INT NULL, Username NVARCHAR(100) NOT NULL, " &
                "FullName NVARCHAR(150) NULL, RoleName NVARCHAR(50) NULL, " &
                "ActionName NVARCHAR(100) NOT NULL, Details NVARCHAR(1000) NULL, " &
                "ActivityDate DATETIME2(0) NOT NULL CONSTRAINT DF_UserActivityLog_Date DEFAULT SYSDATETIME()); " &
                "CREATE INDEX IX_UserActivityLog_Date ON dbo.UserActivityLog(ActivityDate); " &
                "END"

            Using command As New SqlCommand(sql, connection)
                command.ExecuteNonQuery()
            End Using

            _tableReady = True
        End SyncLock
    End Sub

    Public Sub Log(actionName As String, Optional details As String = "")
        Try
            Using connection = DatabaseHelper.GetConnection()
                connection.Open()
                EnsureTable(connection)

                Dim sql As String =
                    "INSERT INTO dbo.UserActivityLog " &
                    "(UserID, Username, FullName, RoleName, ActionName, Details) " &
                    "VALUES (@UserID, @Username, @FullName, @RoleName, @ActionName, @Details)"

                Using command As New SqlCommand(sql, connection)
                    If LoginForm.LoggedInUserID > 0 Then
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = LoginForm.LoggedInUserID
                    Else
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = DBNull.Value
                    End If
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value =
                        If(String.IsNullOrWhiteSpace(LoginForm.LoggedInUsername), "SYSTEM", LoginForm.LoggedInUsername)
                    command.Parameters.Add("@FullName", SqlDbType.NVarChar, 150).Value =
                        If(String.IsNullOrWhiteSpace(LoginForm.LoggedInFullName), CType(DBNull.Value, Object), LoginForm.LoggedInFullName)
                    command.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value =
                        If(String.IsNullOrWhiteSpace(LoginForm.LoggedInRole), CType(DBNull.Value, Object), LoginForm.LoggedInRole)
                    command.Parameters.Add("@ActionName", SqlDbType.NVarChar, 100).Value = actionName
                    command.Parameters.Add("@Details", SqlDbType.NVarChar, 1000).Value =
                        If(String.IsNullOrWhiteSpace(details), CType(DBNull.Value, Object), details)
                    command.ExecuteNonQuery()
                End Using
            End Using
        Catch
            ' Auditing must never crash the main workflow if the database is unavailable.
        End Try
    End Sub
End Module
