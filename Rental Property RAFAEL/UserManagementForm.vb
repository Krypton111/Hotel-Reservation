Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class UserManagementForm
    Inherits Form

    '==========================================================
    ' COLORS - CASA BREW COFFEE CAFE
    '==========================================================

    Private ReadOnly CoffeeDark As Color =
        Color.FromArgb(62, 39, 35)

    Private ReadOnly Coffee As Color =
        Color.FromArgb(111, 78, 55)

    Private ReadOnly Caramel As Color =
        Color.FromArgb(166, 123, 91)

    Private ReadOnly Latte As Color =
        Color.FromArgb(200, 162, 122)

    Private ReadOnly Cream As Color =
        Color.FromArgb(245, 237, 227)

    Private ReadOnly CardColor As Color =
        Color.FromArgb(255, 249, 243)

    Private ReadOnly TextDark As Color =
        Color.FromArgb(59, 48, 42)

    Private ReadOnly Sage As Color =
        Color.FromArgb(122, 139, 114)


    '==========================================================
    ' CONTROLS
    '==========================================================

    Private lblTitle As Label
    Private lblSubtitle As Label

    Private pnlEditor As Panel
    Private pnlTable As Panel

    Private lblUserID As Label
    Private txtUserID As TextBox

    Private lblUsername As Label
    Private txtUsername As TextBox

    Private lblFullName As Label
    Private txtFullName As TextBox

    Private lblPassword As Label
    Private txtPassword As TextBox

    Private lblRole As Label
    Private cmbRole As ComboBox

    Private lblStatus As Label
    Private cmbStatus As ComboBox

    Private lblSearch As Label
    Private txtSearch As TextBox

    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnResetPassword As Button
    Private btnClear As Button
    Private btnRefresh As Button

    Private dgvUsers As DataGridView

    Private lblRecordCount As Label


    '==========================================================
    ' CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildForm()

        LoadUsers()

        ClearFields()

    End Sub


    '==========================================================
    ' BUILD FORM
    '==========================================================

    Private Sub BuildForm()

        Me.Text = "User Management - Casa Brew"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(1000, 650)

        Me.MinimumSize =
            New Size(900, 600)

        Me.BackColor =
            Cream

        Me.Font =
            New Font("Segoe UI", 9.5F)


        '======================================================
        ' TITLE
        '======================================================

        lblTitle = New Label()

        lblTitle.Text =
            "User Management"

        lblTitle.Location =
            New Point(25, 20)

        lblTitle.Size =
            New Size(500, 40)

        lblTitle.Font =
            New Font(
                "Georgia",
                20,
                FontStyle.Bold
            )

        lblTitle.ForeColor =
            TextDark

        Me.Controls.Add(lblTitle)


        '======================================================
        ' SUBTITLE
        '======================================================

        lblSubtitle = New Label()

        lblSubtitle.Text =
            "Manage system accounts, roles, passwords, and access."

        lblSubtitle.Location =
            New Point(27, 58)

        lblSubtitle.Size =
            New Size(650, 30)

        lblSubtitle.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        lblSubtitle.ForeColor =
            Coffee

        Me.Controls.Add(lblSubtitle)


        '======================================================
        ' EDITOR PANEL
        '======================================================

        pnlEditor = New Panel()

        pnlEditor.Location =
            New Point(25, 95)

        pnlEditor.Size =
            New Size(330, 450)

        pnlEditor.BackColor =
            CardColor

        Me.Controls.Add(pnlEditor)


        '======================================================
        ' USER ID
        '======================================================

        lblUserID =
            CreateLabel(
                "User ID",
                New Point(20, 18)
            )

        pnlEditor.Controls.Add(lblUserID)


        txtUserID =
            CreateTextBox(
                New Point(20, 42),
                New Size(285, 30)
            )

        txtUserID.ReadOnly = True

        txtUserID.BackColor =
            Color.FromArgb(
                235,
                228,
                221
            )

        pnlEditor.Controls.Add(txtUserID)


        '======================================================
        ' USERNAME
        '======================================================

        lblUsername =
            CreateLabel(
                "Username",
                New Point(20, 82)
            )

        pnlEditor.Controls.Add(lblUsername)


        txtUsername =
            CreateTextBox(
                New Point(20, 106),
                New Size(285, 30)
            )

        pnlEditor.Controls.Add(txtUsername)


        '======================================================
        ' FULL NAME
        '======================================================

        lblFullName =
            CreateLabel(
                "Full Name",
                New Point(20, 146)
            )

        pnlEditor.Controls.Add(lblFullName)


        txtFullName =
            CreateTextBox(
                New Point(20, 170),
                New Size(285, 30)
            )

        pnlEditor.Controls.Add(txtFullName)


        '======================================================
        ' PASSWORD
        '======================================================

        lblPassword =
            CreateLabel(
                "Password",
                New Point(20, 210)
            )

        pnlEditor.Controls.Add(lblPassword)


        txtPassword =
            CreateTextBox(
                New Point(20, 234),
                New Size(285, 30)
            )

        txtPassword.UseSystemPasswordChar =
            True

        pnlEditor.Controls.Add(txtPassword)


        '======================================================
        ' ROLE
        '======================================================

        lblRole =
            CreateLabel(
                "Role",
                New Point(20, 274)
            )

        pnlEditor.Controls.Add(lblRole)


        cmbRole = New ComboBox()

        cmbRole.Location =
            New Point(20, 298)

        cmbRole.Size =
            New Size(285, 30)

        cmbRole.DropDownStyle =
            ComboBoxStyle.DropDownList

        cmbRole.Items.Add("Admin")
        cmbRole.Items.Add("Property Manager")
        cmbRole.Items.Add("Receptionist")
        cmbRole.Items.Add("Staff")

        cmbRole.SelectedIndex = 3

        pnlEditor.Controls.Add(cmbRole)


        '======================================================
        ' STATUS
        '======================================================

        lblStatus =
            CreateLabel(
                "Account Status",
                New Point(20, 338)
            )

        pnlEditor.Controls.Add(lblStatus)


        cmbStatus = New ComboBox()

        cmbStatus.Location =
            New Point(20, 362)

        cmbStatus.Size =
            New Size(285, 30)

        cmbStatus.DropDownStyle =
            ComboBoxStyle.DropDownList

        cmbStatus.Items.Add("Active")
        cmbStatus.Items.Add("Inactive")

        cmbStatus.SelectedIndex = 0

        pnlEditor.Controls.Add(cmbStatus)


        '======================================================
        ' TABLE PANEL
        '======================================================

        pnlTable = New Panel()

        pnlTable.Location =
            New Point(375, 95)

        pnlTable.Size =
            New Size(580, 450)

        pnlTable.BackColor =
            CardColor

        Me.Controls.Add(pnlTable)


        '======================================================
        ' SEARCH
        '======================================================

        lblSearch =
            CreateLabel(
                "Search Users",
                New Point(20, 18)
            )

        pnlTable.Controls.Add(lblSearch)


        txtSearch =
            CreateTextBox(
                New Point(20, 42),
                New Size(270, 30)
            )

        txtSearch.PlaceholderText =
            "Username, name, or role..."

        pnlTable.Controls.Add(txtSearch)


        '======================================================
        ' REFRESH
        '======================================================

        btnRefresh =
            CreateActionButton(
                "Refresh",
                New Point(305, 40),
                New Size(105, 32),
                Coffee
            )

        pnlTable.Controls.Add(btnRefresh)


        '======================================================
        ' RECORD COUNT
        '======================================================

        lblRecordCount = New Label()

        lblRecordCount.Location =
            New Point(425, 47)

        lblRecordCount.Size =
            New Size(130, 20)

        lblRecordCount.TextAlign =
            ContentAlignment.MiddleRight

        lblRecordCount.ForeColor =
            Coffee

        pnlTable.Controls.Add(lblRecordCount)


        '======================================================
        ' DATA GRID
        '======================================================

        dgvUsers = New DataGridView()

        dgvUsers.Location =
            New Point(20, 85)

        dgvUsers.Size =
            New Size(535, 345)

        dgvUsers.BackgroundColor =
            Color.White

        dgvUsers.BorderStyle =
            BorderStyle.None

        dgvUsers.AllowUserToAddRows =
            False

        dgvUsers.AllowUserToDeleteRows =
            False

        dgvUsers.ReadOnly =
            True

        dgvUsers.MultiSelect =
            False

        dgvUsers.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvUsers.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvUsers.RowHeadersVisible =
            False

        dgvUsers.EnableHeadersVisualStyles =
            False

        dgvUsers.ColumnHeadersDefaultCellStyle.BackColor =
            Coffee

        dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        dgvUsers.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            )

        dgvUsers.ColumnHeadersHeight =
            38

        dgvUsers.DefaultCellStyle.BackColor =
            Color.White

        dgvUsers.DefaultCellStyle.ForeColor =
            TextDark

        dgvUsers.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(
                225,
                210,
                198
            )

        dgvUsers.DefaultCellStyle.SelectionForeColor =
            TextDark

        dgvUsers.AlternatingRowsDefaultCellStyle.BackColor =
            Color.FromArgb(
                249,
                244,
                239
            )

        dgvUsers.RowTemplate.Height =
            32

        pnlTable.Controls.Add(dgvUsers)


        '======================================================
        ' BOTTOM BUTTONS
        '======================================================

        btnAdd =
            CreateActionButton(
                "Add User",
                New Point(25, 560),
                New Size(120, 40),
                Sage
            )

        Me.Controls.Add(btnAdd)


        btnUpdate =
            CreateActionButton(
                "Update",
                New Point(155, 560),
                New Size(120, 40),
                Coffee
            )

        Me.Controls.Add(btnUpdate)


        btnDelete =
            CreateActionButton(
                "Deactivate",
                New Point(285, 560),
                New Size(120, 40),
                Color.FromArgb(
                    145,
                    91,
                    70
                )
            )

        Me.Controls.Add(btnDelete)


        btnResetPassword =
            CreateActionButton(
                "Reset Password",
                New Point(415, 560),
                New Size(140, 40),
                Caramel
            )

        Me.Controls.Add(btnResetPassword)


        btnClear =
            CreateActionButton(
                "Clear",
                New Point(565, 560),
                New Size(100, 40),
                Latte
            )

        Me.Controls.Add(btnClear)


        '======================================================
        ' EVENT HANDLERS
        '
        ' IMPORTANT:
        ' All controls are created BEFORE AddHandler is used.
        '======================================================

        AddHandler btnAdd.Click,
            AddressOf AddUser

        AddHandler btnUpdate.Click,
            AddressOf UpdateUser

        AddHandler btnDelete.Click,
            AddressOf DeleteUser

        AddHandler btnResetPassword.Click,
            AddressOf ResetPassword

        AddHandler btnClear.Click,
            AddressOf ClearFieldsButton

        AddHandler btnRefresh.Click,
            AddressOf RefreshUsers

        AddHandler txtSearch.TextChanged,
            AddressOf SearchUsers

        AddHandler dgvUsers.CellClick,
            AddressOf UserRowSelected

    End Sub


    '==========================================================
    ' CREATE LABEL
    '==========================================================

    Private Function CreateLabel(
        text As String,
        location As Point
    ) As Label

        Dim label As New Label()

        label.Text =
            text

        label.Location =
            location

        label.Size =
            New Size(285, 22)

        label.Font =
            New Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            )

        label.ForeColor =
            TextDark

        Return label

    End Function


    '==========================================================
    ' CREATE TEXTBOX
    '==========================================================

    Private Function CreateTextBox(
        location As Point,
        textBoxSize As Size
    ) As TextBox

        Dim textBox As New TextBox()

        textBox.Location =
            location

        textBox.Size =
            textBoxSize

        textBox.BorderStyle =
            BorderStyle.FixedSingle

        textBox.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        Return textBox

    End Function


    '==========================================================
    ' CREATE BUTTON
    '==========================================================

    Private Function CreateActionButton(
        text As String,
        location As Point,
        buttonSize As Size,
        buttonColor As Color
    ) As Button

        Dim button As New Button()

        button.Text =
            text

        button.Location =
            location

        button.Size =
            buttonSize

        button.BackColor =
            buttonColor

        button.ForeColor =
            Color.White

        button.FlatStyle =
            FlatStyle.Flat

        button.FlatAppearance.BorderSize =
            0

        button.Font =
            New Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            )

        button.Cursor =
            Cursors.Hand

        button.UseVisualStyleBackColor =
            False

        Return button

    End Function


    '==========================================================
    ' LOAD USERS
    '==========================================================

    Private Sub LoadUsers()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                Dim sql As String =
                    "SELECT " &
                    "UserID, " &
                    "Username, " &
                    "FullName, " &
                    "Role, " &
                    "IsActive " &
                    "FROM Users " &
                    "ORDER BY UserID"

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvUsers.DataSource =
                            table

                    End Using

                End Using

            End Using

            FormatUserGrid()

            UpdateRecordCount()

        Catch ex As Exception

            MessageBox.Show(
                "Unable to load users." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "User Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' FORMAT GRID
    '==========================================================

    Private Sub FormatUserGrid()

        If dgvUsers.Columns.Count = 0 Then
            Return
        End If


        If dgvUsers.Columns.Contains(
            "UserID"
        ) Then

            dgvUsers.Columns(
                "UserID"
            ).HeaderText =
                "ID"

        End If


        If dgvUsers.Columns.Contains(
            "Username"
        ) Then

            dgvUsers.Columns(
                "Username"
            ).HeaderText =
                "Username"

        End If


        If dgvUsers.Columns.Contains(
            "FullName"
        ) Then

            dgvUsers.Columns(
                "FullName"
            ).HeaderText =
                "Full Name"

        End If


        If dgvUsers.Columns.Contains(
            "Role"
        ) Then

            dgvUsers.Columns(
                "Role"
            ).HeaderText =
                "Role"

        End If


        If dgvUsers.Columns.Contains(
            "IsActive"
        ) Then

            dgvUsers.Columns(
                "IsActive"
            ).HeaderText =
                "Active"

        End If

    End Sub


    '==========================================================
    ' SELECT USER
    '==========================================================

    Private Sub UserRowSelected(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then
            Return
        End If

        Dim row As DataGridViewRow =
            dgvUsers.Rows(e.RowIndex)


        If row.Cells("UserID").Value IsNot Nothing Then

            txtUserID.Text =
                row.Cells("UserID").Value.ToString()

        End If


        If row.Cells("Username").Value IsNot Nothing Then

            txtUsername.Text =
                row.Cells("Username").Value.ToString()

        End If


        If row.Cells("FullName").Value IsNot Nothing Then

            txtFullName.Text =
                row.Cells("FullName").Value.ToString()

        End If


        If row.Cells("Role").Value IsNot Nothing Then

            cmbRole.Text =
                row.Cells("Role").Value.ToString()

        End If


        If row.Cells("IsActive").Value IsNot Nothing Then

            Dim active As Boolean =
                Convert.ToBoolean(
                    row.Cells("IsActive").Value
                )

            If active Then

                cmbStatus.SelectedItem =
                    "Active"

            Else

                cmbStatus.SelectedItem =
                    "Inactive"

            End If

        End If


        'Password is intentionally not loaded.
        txtPassword.Clear()

    End Sub


    '==========================================================
    ' ADD USER
    '==========================================================

    Private Sub AddUser(
        sender As Object,
        e As EventArgs
    )

        If Not ValidateFields(True) Then
            Return
        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "INSERT INTO Users " &
                    "(Username, FullName, Password, Role, IsActive) " &
                    "VALUES " &
                    "(@Username, @FullName, @Password, @Role, @IsActive)"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@Username",
                        txtUsername.Text.Trim()
                    )

                    command.Parameters.AddWithValue(
                        "@FullName",
                        txtFullName.Text.Trim()
                    )

                    command.Parameters.AddWithValue(
                        "@Password",
                        txtPassword.Text
                    )

                    command.Parameters.AddWithValue(
                        "@Role",
                        cmbRole.Text
                    )

                    command.Parameters.AddWithValue(
                        "@IsActive",
                        cmbStatus.SelectedItem.ToString() =
                        "Active"
                    )

                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "User account created successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadUsers()

            ClearFields()


        Catch ex As SqlException

            If ex.Number = 2627 OrElse
               ex.Number = 2601 Then

                MessageBox.Show(
                    "That username already exists." &
                    Environment.NewLine &
                    "Please choose another username.",
                    "Duplicate Username",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

            Else

                ShowDatabaseError(ex)

            End If


        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub


    '==========================================================
    ' UPDATE USER
    '==========================================================

    Private Sub UpdateUser(
        sender As Object,
        e As EventArgs
    )

        If String.IsNullOrWhiteSpace(
            txtUserID.Text
        ) Then

            MessageBox.Show(
                "Select a user first.",
                "Update User",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        If Not ValidateFields(False) Then
            Return
        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "UPDATE Users SET " &
                    "Username = @Username, " &
                    "FullName = @FullName, " &
                    "Role = @Role, " &
                    "IsActive = @IsActive " &
                    "WHERE UserID = @UserID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@Username",
                        txtUsername.Text.Trim()
                    )

                    command.Parameters.AddWithValue(
                        "@FullName",
                        txtFullName.Text.Trim()
                    )

                    command.Parameters.AddWithValue(
                        "@Role",
                        cmbRole.Text
                    )

                    command.Parameters.AddWithValue(
                        "@IsActive",
                        cmbStatus.SelectedItem.ToString() =
                        "Active"
                    )

                    command.Parameters.AddWithValue(
                        "@UserID",
                        Convert.ToInt32(
                            txtUserID.Text
                        )
                    )

                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "User information updated successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadUsers()

            ClearFields()


        Catch ex As SqlException

            If ex.Number = 2627 OrElse
               ex.Number = 2601 Then

                MessageBox.Show(
                    "That username is already being used.",
                    "Duplicate Username",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

            Else

                ShowDatabaseError(ex)

            End If


        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub


    '==========================================================
    ' DEACTIVATE USER
    '==========================================================

    Private Sub DeleteUser(
        sender As Object,
        e As EventArgs
    )

        If String.IsNullOrWhiteSpace(
            txtUserID.Text
        ) Then

            MessageBox.Show(
                "Select a user first.",
                "Deactivate User",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        Dim selectedID As Integer =
            Convert.ToInt32(
                txtUserID.Text
            )


        '======================================================
        ' PREVENT DEACTIVATING CURRENT ACCOUNT
        '======================================================

        If selectedID =
           LoginForm.LoggedInUserID Then

            MessageBox.Show(
                "You cannot deactivate the account currently being used.",
                "User Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        Dim answer As DialogResult =
            MessageBox.Show(
                "Deactivate this user account?" &
                Environment.NewLine &
                Environment.NewLine &
                txtUsername.Text,
                "Confirm Deactivation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )


        If answer = DialogResult.No Then
            Return
        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "UPDATE Users " &
                    "SET IsActive = 0 " &
                    "WHERE UserID = @UserID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@UserID",
                        selectedID
                    )

                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "User account has been deactivated.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadUsers()

            ClearFields()


        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub


    '==========================================================
    ' RESET PASSWORD
    '==========================================================

    Private Sub ResetPassword(
        sender As Object,
        e As EventArgs
    )

        If String.IsNullOrWhiteSpace(
            txtUserID.Text
        ) Then

            MessageBox.Show(
                "Select a user first.",
                "Reset Password",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        If String.IsNullOrWhiteSpace(
            txtPassword.Text
        ) Then

            MessageBox.Show(
                "Enter the new password in the Password field.",
                "Reset Password",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtPassword.Focus()

            Return

        End If


        Dim answer As DialogResult =
            MessageBox.Show(
                "Reset the password for " &
                txtUsername.Text &
                "?",
                "Confirm Password Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )


        If answer = DialogResult.No Then
            Return
        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "UPDATE Users " &
                    "SET Password = @Password " &
                    "WHERE UserID = @UserID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@Password",
                        txtPassword.Text
                    )

                    command.Parameters.AddWithValue(
                        "@UserID",
                        Convert.ToInt32(
                            txtUserID.Text
                        )
                    )

                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Password has been reset successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            txtPassword.Clear()


        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub


    '==========================================================
    ' VALIDATE FIELDS
    '==========================================================

    Private Function ValidateFields(
        includePassword As Boolean
    ) As Boolean


        If String.IsNullOrWhiteSpace(
            txtUsername.Text
        ) Then

            MessageBox.Show(
                "Please enter a username.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtUsername.Focus()

            Return False

        End If


        If String.IsNullOrWhiteSpace(
            txtFullName.Text
        ) Then

            MessageBox.Show(
                "Please enter the user's full name.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtFullName.Focus()

            Return False

        End If


        If includePassword AndAlso
           String.IsNullOrWhiteSpace(
               txtPassword.Text
           ) Then

            MessageBox.Show(
                "Please enter a password.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtPassword.Focus()

            Return False

        End If


        If cmbRole.SelectedIndex < 0 Then

            MessageBox.Show(
                "Please select a role.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cmbRole.Focus()

            Return False

        End If


        If cmbStatus.SelectedIndex < 0 Then

            MessageBox.Show(
                "Please select an account status.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cmbStatus.Focus()

            Return False

        End If


        Return True

    End Function


    '==========================================================
    ' SEARCH USERS
    '==========================================================

    Private Sub SearchUsers(
        sender As Object,
        e As EventArgs
    )

        If dgvUsers.DataSource Is Nothing Then
            Return
        End If


        Dim table As DataTable =
            TryCast(
                dgvUsers.DataSource,
                DataTable
            )


        If table Is Nothing Then
            Return
        End If


        Dim searchText As String =
            txtSearch.Text.Trim()


        ' Escape characters that have special meaning
        ' in DataView.RowFilter.
        searchText =
            searchText.Replace(
                "'",
                "''"
            )

        searchText =
            searchText.Replace(
                "[",
                "[[]"
            )

        searchText =
            searchText.Replace(
                "%",
                "[%]"
            )

        searchText =
            searchText.Replace(
                "*",
                "[*]"
            )


        If searchText = "" Then

            table.DefaultView.RowFilter = ""

        Else

            table.DefaultView.RowFilter =
                "Convert(UserID, 'System.String') LIKE '%" &
                searchText &
                "%' OR " &
                "Username LIKE '%" &
                searchText &
                "%' OR " &
                "FullName LIKE '%" &
                searchText &
                "%' OR " &
                "Role LIKE '%" &
                searchText &
                "%'"

        End If


        UpdateRecordCount()

    End Sub


    '==========================================================
    ' UPDATE RECORD COUNT
    '==========================================================

    Private Sub UpdateRecordCount()

        If dgvUsers.DataSource Is Nothing Then

            lblRecordCount.Text =
                "0 users"

            Return

        End If


        lblRecordCount.Text =
            dgvUsers.Rows.Count.ToString() &
            " users"

    End Sub


    '==========================================================
    ' REFRESH USERS
    '==========================================================

    Private Sub RefreshUsers(
        sender As Object,
        e As EventArgs
    )

        txtSearch.Clear()

        LoadUsers()

        ClearFields()

    End Sub


    '==========================================================
    ' CLEAR BUTTON
    '==========================================================

    Private Sub ClearFieldsButton(
        sender As Object,
        e As EventArgs
    )

        ClearFields()

    End Sub


    '==========================================================
    ' CLEAR FIELDS
    '==========================================================

    Private Sub ClearFields()

        txtUserID.Clear()

        txtUsername.Clear()

        txtFullName.Clear()

        txtPassword.Clear()

        If cmbRole.Items.Count > 0 Then
            cmbRole.SelectedIndex = 3
        End If

        If cmbStatus.Items.Count > 0 Then
            cmbStatus.SelectedIndex = 0
        End If

        dgvUsers.ClearSelection()

    End Sub


    '==========================================================
    ' DATABASE ERROR
    '==========================================================

    Private Sub ShowDatabaseError(
        ex As Exception
    )

        MessageBox.Show(
            "A database error occurred." &
            Environment.NewLine &
            Environment.NewLine &
            ex.Message,
            "Database Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

    End Sub

End Class