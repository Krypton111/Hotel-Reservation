Imports System.Data
Imports System.Drawing
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class UserManagementForm
    Inherits Form

    Private ReadOnly DeepNavy As Color = Color.FromArgb(20, 27, 38)
    Private ReadOnly Navy As Color = Color.FromArgb(31, 42, 56)
    Private ReadOnly Champagne As Color = Color.FromArgb(198, 166, 110)
    Private ReadOnly Ivory As Color = Color.FromArgb(247, 244, 237)
    Private ReadOnly WarmWhite As Color = Color.FromArgb(253, 251, 247)
    Private ReadOnly Charcoal As Color = Color.FromArgb(45, 45, 43)
    Private ReadOnly MutedGray As Color = Color.FromArgb(125, 125, 120)

    Private txtUserID As TextBox
    Private txtUsername As TextBox
    Private txtFullName As TextBox
    Private txtPassword As TextBox
    Private cmbRole As ComboBox
    Private chkActive As CheckBox
    Private txtUserSearch As TextBox
    Private dgvUsers As DataGridView
    Private dgvActivity As DataGridView
    Private cmbActivityUser As ComboBox
    Private cmbPeriod As ComboBox
    Private dtFrom As DateTimePicker
    Private dtTo As DateTimePicker
    Private lblActivityCount As Label

    Public Sub New()
        If Not LoginForm.LoggedInRole.Equals("Admin", StringComparison.OrdinalIgnoreCase) Then
            Throw New UnauthorizedAccessException("User Management is available to Admin accounts only.")
        End If

        BuildForm()
        AuditLogger.Log("Opened User Management", "Viewed user accounts and activity log.")
        LoadUsers()
        LoadActivityUsers()
        ApplyPeriod()
        LoadActivity()
    End Sub

    Private Sub BuildForm()
        Text = "User Management - Aurelia Grand Hotel"
        BackColor = Ivory
        Font = New Font("Segoe UI", 9.5F)
        MinimumSize = New Size(950, 650)
        Size = New Size(1150, 780)
        FormBorderStyle = FormBorderStyle.None
        TopLevel = False
        Dock = DockStyle.Fill

        Dim root As New TableLayoutPanel With {
            .Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 3,
            .BackColor = Ivory, .Padding = New Padding(22)
        }
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 65))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 48))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 52))
        Controls.Add(root)

        Dim header As New Panel With {.Dock = DockStyle.Fill, .BackColor = Ivory}
        Dim title As New Label With {
            .Text = "User Management", .Font = New Font("Georgia", 21, FontStyle.Bold),
            .ForeColor = Charcoal, .Location = New Point(0, 0), .Size = New Size(500, 34)
        }
        Dim subtitle As New Label With {
            .Text = "Manage accounts and review staff activity by date.",
            .ForeColor = MutedGray, .Location = New Point(2, 36), .AutoSize = True
        }
        header.Controls.Add(title)
        header.Controls.Add(subtitle)
        root.Controls.Add(header, 0, 0)

        Dim upper As New SplitContainer With {
            .Dock = DockStyle.Fill, .Orientation = Orientation.Vertical,
            .SplitterDistance = 320, .BackColor = Ivory, .Panel1MinSize = 290, .Panel2MinSize = 400
        }
        root.Controls.Add(upper, 0, 1)

        Dim editor As New Panel With {.Dock = DockStyle.Fill, .BackColor = WarmWhite, .Padding = New Padding(16)}
        upper.Panel1.Controls.Add(editor)
        AddLabel(editor, "ACCOUNT DETAILS", 16, 12, 270, 25, True)
        AddLabel(editor, "User ID", 16, 48, 270, 20, False)
        txtUserID = AddTextBox(editor, 16, 70, 270, 28, True)
        AddLabel(editor, "Username", 16, 108, 270, 20, False)
        txtUsername = AddTextBox(editor, 16, 130, 270, 28)
        AddLabel(editor, "Full name", 16, 168, 270, 20, False)
        txtFullName = AddTextBox(editor, 16, 190, 270, 28)
        AddLabel(editor, "New password (leave blank to keep current)", 16, 228, 280, 20, False)
        txtPassword = AddTextBox(editor, 16, 250, 270, 28)
        txtPassword.UseSystemPasswordChar = True
        AddLabel(editor, "Role", 16, 288, 270, 20, False)
        cmbRole = New ComboBox With {.Location = New Point(16, 310), .Size = New Size(270, 29), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbRole.Items.AddRange(New Object() {"Admin", "Manager", "Receptionist", "Staff"})
        cmbRole.SelectedIndex = 3
        editor.Controls.Add(cmbRole)
        chkActive = New CheckBox With {.Text = "Account is active", .Location = New Point(16, 348), .AutoSize = True, .Checked = True, .ForeColor = Charcoal}
        editor.Controls.Add(chkActive)

        Dim btnSave As Button = AddButton(editor, "Save Changes", 16, 386, 130, 38, Navy)
        Dim btnNew As Button = AddButton(editor, "New Account", 156, 386, 130, 38, Color.FromArgb(122, 139, 114))
        Dim btnReset As Button = AddButton(editor, "Clear", 16, 432, 130, 34, Color.FromArgb(125, 125, 120))
        AddHandler btnSave.Click, AddressOf SaveUser
        AddHandler btnNew.Click, Sub() ClearEditor()
        AddHandler btnReset.Click, Sub() ClearEditor()

        Dim accounts As New Panel With {.Dock = DockStyle.Fill, .BackColor = WarmWhite, .Padding = New Padding(14)}
        upper.Panel2.Controls.Add(accounts)
        AddLabel(accounts, "ALL SYSTEM ACCOUNTS", 14, 10, 280, 24, True)
        txtUserSearch = New TextBox With {.Location = New Point(14, 39), .Size = New Size(260, 28), .PlaceholderText = "Search username, name, or role"}
        accounts.Controls.Add(txtUserSearch)
        Dim btnRefresh As Button = AddButton(accounts, "Refresh", 282, 37, 90, 31, Navy)
        AddHandler btnRefresh.Click, Sub() LoadUsers()
        dgvUsers = MakeGrid()
        dgvUsers.Location = New Point(14, 78)
        dgvUsers.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvUsers.Size = New Size(500, 300)
        accounts.Controls.Add(dgvUsers)
        AddHandler dgvUsers.CellClick, AddressOf UserSelected
        AddHandler txtUserSearch.TextChanged, AddressOf FilterUsers

        Dim activityPanel As New Panel With {.Dock = DockStyle.Fill, .BackColor = WarmWhite, .Padding = New Padding(14)}
        root.Controls.Add(activityPanel, 0, 2)
        AddLabel(activityPanel, "USER ACTIVITY HISTORY", 14, 8, 300, 25, True)
        cmbPeriod = New ComboBox With {.Location = New Point(14, 38), .Size = New Size(145, 28), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbPeriod.Items.AddRange(New Object() {"Today", "Yesterday", "Last 7 days", "Last 30 days", "This month", "Custom range"})
        cmbPeriod.SelectedIndex = 0
        activityPanel.Controls.Add(cmbPeriod)
        dtFrom = New DateTimePicker With {.Location = New Point(168, 38), .Size = New Size(135, 28), .Format = DateTimePickerFormat.Short, .Value = Date.Today}
        dtTo = New DateTimePicker With {.Location = New Point(311, 38), .Size = New Size(135, 28), .Format = DateTimePickerFormat.Short, .Value = Date.Today}
        activityPanel.Controls.Add(dtFrom)
        activityPanel.Controls.Add(dtTo)
        cmbActivityUser = New ComboBox With {.Location = New Point(455, 38), .Size = New Size(190, 28), .DropDownStyle = ComboBoxStyle.DropDownList, .Anchor = AnchorStyles.Top Or AnchorStyles.Left}
        activityPanel.Controls.Add(cmbActivityUser)
        Dim btnFilter As Button = AddButton(activityPanel, "Apply", 653, 36, 80, 31, Navy)
        btnFilter.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Dim btnToday As Button = AddButton(activityPanel, "Today", 741, 36, 78, 31, Color.FromArgb(122, 139, 114))
        btnToday.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblActivityCount = New Label With {.Text = "0 activities", .Location = New Point(825, 41), .Size = New Size(150, 22), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .ForeColor = MutedGray, .TextAlign = ContentAlignment.MiddleRight}
        activityPanel.Controls.Add(lblActivityCount)
        dgvActivity = MakeGrid()
        dgvActivity.Location = New Point(14, 76)
        dgvActivity.Size = New Size(900, 180)
        dgvActivity.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        activityPanel.Controls.Add(dgvActivity)
        AddHandler cmbPeriod.SelectedIndexChanged, Sub() ApplyPeriod()
        AddHandler btnFilter.Click, Sub() LoadActivity()
        AddHandler btnToday.Click, Sub()
                                       cmbPeriod.SelectedItem = "Today"
                                       ApplyPeriod()
                                       LoadActivity()
                                   End Sub
        AddHandler dtFrom.ValueChanged, Sub()
                                            If cmbPeriod.SelectedItem IsNot Nothing AndAlso cmbPeriod.SelectedItem.ToString() <> "Custom range" Then cmbPeriod.SelectedItem = "Custom range"
                                        End Sub
        AddHandler dtTo.ValueChanged, Sub()
                                          If cmbPeriod.SelectedItem IsNot Nothing AndAlso cmbPeriod.SelectedItem.ToString() <> "Custom range" Then cmbPeriod.SelectedItem = "Custom range"
                                      End Sub

        AddHandler Me.SizeChanged, Sub()
                                       If dgvUsers IsNot Nothing Then dgvUsers.Size = New Size(Math.Max(250, accounts.ClientSize.Width - 28), Math.Max(150, accounts.ClientSize.Height - 92))
                                       If dgvActivity IsNot Nothing Then dgvActivity.Size = New Size(Math.Max(400, activityPanel.ClientSize.Width - 28), Math.Max(100, activityPanel.ClientSize.Height - 90))
                                       If lblActivityCount IsNot Nothing Then lblActivityCount.Left = activityPanel.ClientSize.Width - lblActivityCount.Width - 14
                                   End Sub
    End Sub

    Private Sub AddLabel(parent As Control, text As String, x As Integer, y As Integer, width As Integer, height As Integer, bold As Boolean)
        parent.Controls.Add(New Label With {.Text = text, .Location = New Point(x, y), .Size = New Size(width, height), .ForeColor = If(bold, Navy, Charcoal), .Font = New Font("Segoe UI", If(bold, 10.0F, 9.0F), If(bold, FontStyle.Bold, FontStyle.Regular))})
    End Sub

    Private Function AddTextBox(parent As Control, x As Integer, y As Integer, width As Integer, height As Integer, Optional readOnlyValue As Boolean = False) As TextBox
        Dim box As New TextBox With {.Location = New Point(x, y), .Size = New Size(width, height), .ReadOnly = readOnlyValue, .BorderStyle = BorderStyle.FixedSingle}
        parent.Controls.Add(box)
        Return box
    End Function

    Private Function AddButton(parent As Control, text As String, x As Integer, y As Integer, width As Integer, height As Integer, color As Color) As Button
        Dim button As New Button With {.Text = text, .Location = New Point(x, y), .Size = New Size(width, height), .BackColor = color, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .UseVisualStyleBackColor = False}
        button.FlatAppearance.BorderSize = 0
        parent.Controls.Add(button)
        Return button
    End Function

    Private Function MakeGrid() As DataGridView
        Return New DataGridView With {
            .BackgroundColor = Color.White, .BorderStyle = BorderStyle.None,
            .AllowUserToAddRows = False, .AllowUserToDeleteRows = False, .ReadOnly = True,
            .MultiSelect = False, .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .RowHeadersVisible = False, .EnableHeadersVisualStyles = False,
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {.BackColor = Navy, .ForeColor = Color.White, .Font = New Font("Segoe UI", 9, FontStyle.Bold)},
            .DefaultCellStyle = New DataGridViewCellStyle With {.SelectionBackColor = Color.FromArgb(218, 192, 143), .SelectionForeColor = Charcoal},
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {.BackColor = Color.FromArgb(249, 247, 242)}
        }
    End Function

    Private Sub LoadUsers()
        Try
            Using connection = DatabaseHelper.GetConnection()
                connection.Open()
                Using adapter As New SqlDataAdapter("SELECT UserID, Username, FullName, RoleName, IsActive FROM Users ORDER BY UserID", connection)
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    dgvUsers.DataSource = table
                End Using
            End Using
            If dgvUsers.Columns.Contains("UserID") Then dgvUsers.Columns("UserID").HeaderText = "ID"
            If dgvUsers.Columns.Contains("FullName") Then dgvUsers.Columns("FullName").HeaderText = "Full name"
            If dgvUsers.Columns.Contains("RoleName") Then dgvUsers.Columns("RoleName").HeaderText = "Role"
            If dgvUsers.Columns.Contains("IsActive") Then dgvUsers.Columns("IsActive").HeaderText = "Active"
            FilterUsers(Nothing, EventArgs.Empty)
        Catch ex As Exception
            ShowError("Unable to load accounts. Confirm the Users table contains UserID, Username, FullName, RoleName, PasswordHash, and IsActive.", ex)
        End Try
    End Sub

    Private Sub FilterUsers(sender As Object, e As EventArgs)
        Dim table = TryCast(dgvUsers.DataSource, DataTable)
        If table Is Nothing Then Return
        Dim value = txtUserSearch.Text.Trim().Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]")
        table.DefaultView.RowFilter = If(value = "", "", "Username LIKE '%" & value & "%' OR FullName LIKE '%" & value & "%' OR RoleName LIKE '%" & value & "%'")
    End Sub

    Private Sub UserSelected(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row = dgvUsers.Rows(e.RowIndex)
        txtUserID.Text = Convert.ToString(row.Cells("UserID").Value)
        txtUsername.Text = Convert.ToString(row.Cells("Username").Value)
        txtFullName.Text = Convert.ToString(row.Cells("FullName").Value)
        cmbRole.SelectedItem = Convert.ToString(row.Cells("RoleName").Value)
        chkActive.Checked = Convert.ToBoolean(row.Cells("IsActive").Value)
        txtPassword.Clear()
    End Sub

    Private Sub SaveUser(sender As Object, e As EventArgs)
        Dim creating As Boolean = String.IsNullOrWhiteSpace(txtUserID.Text)
        If String.IsNullOrWhiteSpace(txtUsername.Text) OrElse String.IsNullOrWhiteSpace(txtFullName.Text) OrElse cmbRole.SelectedIndex < 0 Then
            MessageBox.Show("Username, full name, and role are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If creating AndAlso String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Enter an initial password for the new account.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPassword.Focus()
            Return
        End If
        Dim id As Integer = If(creating, 0, Convert.ToInt32(txtUserID.Text))
        If Not creating AndAlso id = LoginForm.LoggedInUserID AndAlso Not chkActive.Checked Then
            MessageBox.Show("You cannot deactivate the account currently in use.", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            Using connection = DatabaseHelper.GetConnection()
                connection.Open()
                Dim sql As String
                If creating Then
                    sql = "INSERT INTO Users (Username, FullName, PasswordHash, RoleName, IsActive) VALUES (@Username, @FullName, @PasswordHash, @RoleName, @IsActive)"
                Else
                    sql = "UPDATE Users SET Username=@Username, FullName=@FullName, RoleName=@RoleName, IsActive=@IsActive"
                    If Not String.IsNullOrWhiteSpace(txtPassword.Text) Then sql &= ", PasswordHash=@PasswordHash"
                    sql &= " WHERE UserID=@UserID"
                End If
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = txtUsername.Text.Trim()
                    command.Parameters.Add("@FullName", SqlDbType.NVarChar, 150).Value = txtFullName.Text.Trim()
                    command.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = cmbRole.Text
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = chkActive.Checked
                    If creating OrElse Not String.IsNullOrWhiteSpace(txtPassword.Text) Then command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 64).Value = HashPassword(txtPassword.Text)
                    If Not creating Then command.Parameters.Add("@UserID", SqlDbType.Int).Value = id
                    command.ExecuteNonQuery()
                End Using
            End Using
            If creating Then
                AuditLogger.Log("Created user account", "Username=" & txtUsername.Text.Trim() & "; role=" & cmbRole.Text & "; active=" & chkActive.Checked.ToString())
            Else
                AuditLogger.Log("Updated user account", "UserID=" & id.ToString() & "; username=" & txtUsername.Text.Trim() & "; role=" & cmbRole.Text & "; active=" & chkActive.Checked.ToString())
                If Not String.IsNullOrWhiteSpace(txtPassword.Text) Then AuditLogger.Log("Reset user password", "Password changed for UserID=" & id.ToString() & ". Password value was not recorded.")
            End If
            MessageBox.Show(If(creating, "Account created successfully.", "Account updated successfully."), "User Management", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadUsers()
            ClearEditor()
        Catch ex As Exception
            ShowError("Unable to update this account. The username may already be in use.", ex)
        End Try
    End Sub

    Private Sub LoadActivityUsers()
        Try
            Using connection = DatabaseHelper.GetConnection()
                connection.Open()
                Using adapter As New SqlDataAdapter("SELECT UserID, Username, FullName FROM Users ORDER BY Username", connection)
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    Dim allRow = table.NewRow()
                    allRow("UserID") = 0
                    allRow("Username") = "(All users)"
                    allRow("FullName") = ""
                    table.Rows.InsertAt(allRow, 0)
                    cmbActivityUser.DataSource = table
                    cmbActivityUser.DisplayMember = "Username"
                    cmbActivityUser.ValueMember = "UserID"
                End Using
            End Using
        Catch ex As Exception
            ' User list error is surfaced when account loading runs.
        End Try
    End Sub

    Private Sub ApplyPeriod()
        If cmbPeriod.SelectedItem Is Nothing OrElse dtFrom Is Nothing OrElse dtTo Is Nothing Then Return
        Dim today = Date.Today
        Select Case cmbPeriod.SelectedItem.ToString()
            Case "Today"
                dtFrom.Value = today : dtTo.Value = today
            Case "Yesterday"
                dtFrom.Value = today.AddDays(-1) : dtTo.Value = today.AddDays(-1)
            Case "Last 7 days"
                dtFrom.Value = today.AddDays(-6) : dtTo.Value = today
            Case "Last 30 days"
                dtFrom.Value = today.AddDays(-29) : dtTo.Value = today
            Case "This month"
                dtFrom.Value = New Date(today.Year, today.Month, 1) : dtTo.Value = today
        End Select
    End Sub

    Private Sub LoadActivity()
        Try
            Using connection = DatabaseHelper.GetConnection()
                connection.Open()
                Dim sql As String =
                    "IF OBJECT_ID(N'dbo.UserActivityLog', N'U') IS NULL " &
                    "BEGIN " &
                    "CREATE TABLE dbo.UserActivityLog (ActivityID INT IDENTITY(1,1) PRIMARY KEY, UserID INT NULL, Username NVARCHAR(100) NOT NULL, FullName NVARCHAR(150) NULL, RoleName NVARCHAR(50) NULL, ActionName NVARCHAR(100) NOT NULL, Details NVARCHAR(1000) NULL, ActivityDate DATETIME2(0) NOT NULL CONSTRAINT DF_UserActivityLog_Date DEFAULT SYSDATETIME()); " &
                    "CREATE INDEX IX_UserActivityLog_Date ON dbo.UserActivityLog(ActivityDate); END"
                Using ensure As New SqlCommand(sql, connection)
                    ensure.ExecuteNonQuery()
                End Using
                Dim query = "SELECT ActivityDate, Username, FullName, RoleName, ActionName, Details FROM dbo.UserActivityLog WHERE ActivityDate >= @FromDate AND ActivityDate < @ToDate"
                If cmbActivityUser.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbActivityUser.SelectedValue) AndAlso Convert.ToInt32(cmbActivityUser.SelectedValue) > 0 Then query &= " AND UserID=@UserID"
                query &= " ORDER BY ActivityDate DESC"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.Add("@FromDate", SqlDbType.DateTime2).Value = dtFrom.Value.Date
                    command.Parameters.Add("@ToDate", SqlDbType.DateTime2).Value = dtTo.Value.Date.AddDays(1)
                    If cmbActivityUser.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbActivityUser.SelectedValue) AndAlso Convert.ToInt32(cmbActivityUser.SelectedValue) > 0 Then command.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt32(cmbActivityUser.SelectedValue)
                    Using adapter As New SqlDataAdapter(command)
                        Dim table As New DataTable()
                        adapter.Fill(table)
                        dgvActivity.DataSource = table
                    End Using
                End Using
            End Using
            If dgvActivity.Columns.Contains("ActivityDate") Then dgvActivity.Columns("ActivityDate").HeaderText = "Date and time"
            If dgvActivity.Columns.Contains("Username") Then dgvActivity.Columns("Username").HeaderText = "Username"
            If dgvActivity.Columns.Contains("FullName") Then dgvActivity.Columns("FullName").HeaderText = "Full name"
            If dgvActivity.Columns.Contains("RoleName") Then dgvActivity.Columns("RoleName").HeaderText = "Role"
            If dgvActivity.Columns.Contains("ActionName") Then dgvActivity.Columns("ActionName").HeaderText = "Action"
            If dgvActivity.Columns.Contains("Details") Then dgvActivity.Columns("Details").HeaderText = "Details"
            lblActivityCount.Text = dgvActivity.Rows.Count.ToString() & " activities"
            AuditLogger.Log("Viewed activity history", "Date range " & dtFrom.Value.ToString("yyyy-MM-dd") & " to " & dtTo.Value.ToString("yyyy-MM-dd"))
        Catch ex As Exception
            ShowError("Unable to load the activity history.", ex)
        End Try
    End Sub

    Private Sub ClearEditor()
        txtUserID.Clear()
        txtUsername.Clear()
        txtFullName.Clear()
        txtPassword.Clear()
        cmbRole.SelectedItem = "Staff"
        chkActive.Checked = True
        dgvUsers.ClearSelection()
    End Sub

    Private Function HashPassword(value As String) As String
        Using sha = SHA256.Create()
            Dim bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value))
            Dim builder As New StringBuilder()
            For Each item In bytes
                builder.Append(item.ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function

    Private Sub ShowError(message As String, ex As Exception)
        MessageBox.Show(message & Environment.NewLine & Environment.NewLine & ex.Message, "User Management", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
End Class
