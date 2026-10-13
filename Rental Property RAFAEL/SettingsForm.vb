Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class SettingsForm
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

    Private pnlAccount As Panel
    Private pnlSystem As Panel
    Private pnlDatabase As Panel

    Private lblAccountTitle As Label
    Private lblSystemTitle As Label
    Private lblDatabaseTitle As Label

    Private lblUsername As Label
    Private txtUsername As TextBox

    Private lblFullName As Label
    Private txtFullName As TextBox

    Private lblRole As Label
    Private txtRole As TextBox

    Private lblCurrentPassword As Label
    Private txtCurrentPassword As TextBox

    Private lblNewPassword As Label
    Private txtNewPassword As TextBox

    Private lblConfirmPassword As Label
    Private txtConfirmPassword As TextBox

    Private btnUpdateProfile As Button
    Private btnChangePassword As Button

    Private lblSystemName As Label
    Private txtSystemName As TextBox

    Private lblSystemDescription As Label
    Private txtSystemDescription As TextBox

    Private btnSaveSystem As Button
    Private btnResetSystem As Button

    Private lblDatabaseStatus As Label
    Private lblDatabaseName As Label
    Private lblDatabaseServer As Label

    Private btnTestDatabase As Button

    '==========================================================
    ' CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildForm()

        LoadCurrentUser()

        LoadSystemInformation()

    End Sub

    '==========================================================
    ' BUILD FORM
    '==========================================================

    Private Sub BuildForm()

        Me.Text =
            "Settings - Casa Brew"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(1000, 650)

        Me.MinimumSize =
            New Size(900, 600)

        Me.BackColor =
            Cream

        Me.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        '======================================================
        ' TITLE
        '======================================================

        lblTitle =
            New Label()

        lblTitle.Text =
            "Settings"

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

        lblSubtitle =
            New Label()

        lblSubtitle.Text =
            "Manage your account and application settings."

        lblSubtitle.Location =
            New Point(27, 58)

        lblSubtitle.Size =
            New Size(700, 30)

        lblSubtitle.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        lblSubtitle.ForeColor =
            Coffee

        Me.Controls.Add(lblSubtitle)

        '======================================================
        ' ACCOUNT PANEL
        '======================================================

        pnlAccount =
            New Panel()

        pnlAccount.Location =
            New Point(25, 100)

        pnlAccount.Size =
            New Size(450, 445)

        pnlAccount.BackColor =
            CardColor

        Me.Controls.Add(pnlAccount)

        '======================================================
        ' ACCOUNT TITLE
        '======================================================

        lblAccountTitle =
            New Label()

        lblAccountTitle.Text =
            "Account Information"

        lblAccountTitle.Location =
            New Point(20, 18)

        lblAccountTitle.Size =
            New Size(350, 30)

        lblAccountTitle.Font =
            New Font(
                "Georgia",
                15,
                FontStyle.Bold
            )

        lblAccountTitle.ForeColor =
            TextDark

        pnlAccount.Controls.Add(
            lblAccountTitle
        )

        '======================================================
        ' USERNAME
        '======================================================

        lblUsername =
            CreateLabel(
                "Username",
                New Point(20, 60)
            )

        pnlAccount.Controls.Add(
            lblUsername
        )

        txtUsername =
            CreateTextBox(
                New Point(20, 84),
                New Size(390, 30)
            )

        pnlAccount.Controls.Add(
            txtUsername
        )

        '======================================================
        ' FULL NAME
        '======================================================

        lblFullName =
            CreateLabel(
                "Full Name",
                New Point(20, 124)
            )

        pnlAccount.Controls.Add(
            lblFullName
        )

        txtFullName =
            CreateTextBox(
                New Point(20, 148),
                New Size(390, 30)
            )

        pnlAccount.Controls.Add(
            txtFullName
        )

        '======================================================
        ' ROLE
        '======================================================

        lblRole =
            CreateLabel(
                "Role",
                New Point(20, 188)
            )

        pnlAccount.Controls.Add(
            lblRole
        )

        txtRole =
            CreateTextBox(
                New Point(20, 212),
                New Size(390, 30)
            )

        txtRole.ReadOnly =
            True

        txtRole.BackColor =
            Color.FromArgb(
                235,
                228,
                221
            )

        pnlAccount.Controls.Add(
            txtRole
        )

        '======================================================
        ' UPDATE PROFILE BUTTON
        '======================================================

        btnUpdateProfile =
            CreateActionButton(
                "Save Profile",
                New Point(20, 255),
                New Size(150, 40),
                Sage
            )

        AddHandler btnUpdateProfile.Click,
            AddressOf UpdateProfile

        pnlAccount.Controls.Add(
            btnUpdateProfile
        )

        '======================================================
        ' PASSWORD SECTION
        '======================================================

        Dim lblPasswordTitle As New Label()

        lblPasswordTitle.Text =
            "Change Password"

        lblPasswordTitle.Location =
            New Point(20, 315)

        lblPasswordTitle.Size =
            New Size(300, 28)

        lblPasswordTitle.Font =
            New Font(
                "Georgia",
                13,
                FontStyle.Bold
            )

        lblPasswordTitle.ForeColor =
            TextDark

        pnlAccount.Controls.Add(
            lblPasswordTitle
        )

        '======================================================
        ' CURRENT PASSWORD
        '======================================================

        lblCurrentPassword =
            CreateLabel(
                "Current Password",
                New Point(20, 350)
            )

        pnlAccount.Controls.Add(
            lblCurrentPassword
        )

        txtCurrentPassword =
            CreateTextBox(
                New Point(20, 374),
                New Size(120, 30)
            )

        txtCurrentPassword.UseSystemPasswordChar =
            True

        pnlAccount.Controls.Add(
            txtCurrentPassword
        )

        '======================================================
        ' NEW PASSWORD
        '======================================================

        lblNewPassword =
            CreateLabel(
                "New Password",
                New Point(155, 350)
            )

        pnlAccount.Controls.Add(
            lblNewPassword
        )

        txtNewPassword =
            CreateTextBox(
                New Point(155, 374),
                New Size(120, 30)
            )

        txtNewPassword.UseSystemPasswordChar =
            True

        pnlAccount.Controls.Add(
            txtNewPassword
        )

        '======================================================
        ' CONFIRM PASSWORD
        '======================================================

        lblConfirmPassword =
            CreateLabel(
                "Confirm",
                New Point(290, 350)
            )

        pnlAccount.Controls.Add(
            lblConfirmPassword
        )

        txtConfirmPassword =
            CreateTextBox(
                New Point(290, 374),
                New Size(120, 30)
            )

        txtConfirmPassword.UseSystemPasswordChar =
            True

        pnlAccount.Controls.Add(
            txtConfirmPassword
        )

        '======================================================
        ' CHANGE PASSWORD BUTTON
        '======================================================

        btnChangePassword =
            CreateActionButton(
                "Change Password",
                New Point(280, 255),
                New Size(130, 40),
                Caramel
            )

        AddHandler btnChangePassword.Click,
            AddressOf ChangePassword

        pnlAccount.Controls.Add(
            btnChangePassword
        )

        '======================================================
        ' SYSTEM PANEL
        '======================================================

        pnlSystem =
            New Panel()

        pnlSystem.Location =
            New Point(500, 100)

        pnlSystem.Size =
            New Size(455, 250)

        pnlSystem.BackColor =
            CardColor

        Me.Controls.Add(pnlSystem)

        '======================================================
        ' SYSTEM TITLE
        '======================================================

        lblSystemTitle =
            New Label()

        lblSystemTitle.Text =
            "System Information"

        lblSystemTitle.Location =
            New Point(20, 18)

        lblSystemTitle.Size =
            New Size(350, 30)

        lblSystemTitle.Font =
            New Font(
                "Georgia",
                15,
                FontStyle.Bold
            )

        lblSystemTitle.ForeColor =
            TextDark

        pnlSystem.Controls.Add(
            lblSystemTitle
        )

        '======================================================
        ' SYSTEM NAME
        '======================================================

        lblSystemName =
            CreateLabel(
                "System Name",
                New Point(20, 60)
            )

        pnlSystem.Controls.Add(
            lblSystemName
        )

        txtSystemName =
            CreateTextBox(
                New Point(20, 84),
                New Size(395, 30)
            )

        pnlSystem.Controls.Add(
            txtSystemName
        )

        '======================================================
        ' SYSTEM DESCRIPTION
        '======================================================

        lblSystemDescription =
            CreateLabel(
                "Description",
                New Point(20, 124)
            )

        pnlSystem.Controls.Add(
            lblSystemDescription
        )

        txtSystemDescription =
            CreateTextBox(
                New Point(20, 148),
                New Size(395, 30)
            )

        pnlSystem.Controls.Add(
            txtSystemDescription
        )

        '======================================================
        ' SYSTEM BUTTONS
        '======================================================

        btnSaveSystem =
            CreateActionButton(
                "Save",
                New Point(20, 195),
                New Size(110, 38),
                Sage
            )

        AddHandler btnSaveSystem.Click,
            AddressOf SaveSystemInformation

        pnlSystem.Controls.Add(
            btnSaveSystem
        )

        btnResetSystem =
            CreateActionButton(
                "Reset",
                New Point(140, 195),
                New Size(110, 38),
                Latte
            )

        AddHandler btnResetSystem.Click,
            AddressOf ResetSystemInformation

        pnlSystem.Controls.Add(
            btnResetSystem
        )

        '======================================================
        ' DATABASE PANEL
        '======================================================

        pnlDatabase =
            New Panel()

        pnlDatabase.Location =
            New Point(500, 370)

        pnlDatabase.Size =
            New Size(455, 175)

        pnlDatabase.BackColor =
            CardColor

        Me.Controls.Add(pnlDatabase)

        '======================================================
        ' DATABASE TITLE
        '======================================================

        lblDatabaseTitle =
            New Label()

        lblDatabaseTitle.Text =
            "Database Connection"

        lblDatabaseTitle.Location =
            New Point(20, 18)

        lblDatabaseTitle.Size =
            New Size(350, 30)

        lblDatabaseTitle.Font =
            New Font(
                "Georgia",
                15,
                FontStyle.Bold
            )

        lblDatabaseTitle.ForeColor =
            TextDark

        pnlDatabase.Controls.Add(
            lblDatabaseTitle
        )

        '======================================================
        ' DATABASE NAME
        '======================================================

        lblDatabaseName =
            New Label()

        lblDatabaseName.Location =
            New Point(20, 60)

        lblDatabaseName.Size =
            New Size(395, 25)

        lblDatabaseName.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        lblDatabaseName.ForeColor =
            TextDark

        pnlDatabase.Controls.Add(
            lblDatabaseName
        )

        '======================================================
        ' DATABASE SERVER
        '======================================================

        lblDatabaseServer =
            New Label()

        lblDatabaseServer.Location =
            New Point(20, 88)

        lblDatabaseServer.Size =
            New Size(395, 25)

        lblDatabaseServer.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        lblDatabaseServer.ForeColor =
            TextDark

        pnlDatabase.Controls.Add(
            lblDatabaseServer
        )

        '======================================================
        ' DATABASE STATUS
        '======================================================

        lblDatabaseStatus =
            New Label()

        lblDatabaseStatus.Location =
            New Point(20, 116)

        lblDatabaseStatus.Size =
            New Size(230, 25)

        lblDatabaseStatus.Font =
            New Font(
                "Segoe UI",
                9.5F,
                FontStyle.Bold
            )

        lblDatabaseStatus.ForeColor =
            Sage

        pnlDatabase.Controls.Add(
            lblDatabaseStatus
        )

        '======================================================
        ' TEST DATABASE
        '======================================================

        btnTestDatabase =
            CreateActionButton(
                "Test Connection",
                New Point(260, 110),
                New Size(155, 38),
                Coffee
            )

        AddHandler btnTestDatabase.Click,
            AddressOf TestDatabaseConnection

        pnlDatabase.Controls.Add(
            btnTestDatabase
        )

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
            New Size(125, 22)

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
    ' LOAD CURRENT USER
    '==========================================================

    Private Sub LoadCurrentUser()

        txtUsername.Text =
            LoginForm.LoggedInUsername

        txtFullName.Text =
            LoginForm.LoggedInFullName

        txtRole.Text =
            LoginForm.LoggedInRole

    End Sub

    '==========================================================
    ' UPDATE PROFILE
    '==========================================================

    Private Sub UpdateProfile(
        sender As Object,
        e As EventArgs
    )

        If String.IsNullOrWhiteSpace(
            txtUsername.Text
        ) Then

            MessageBox.Show(
                "Username cannot be empty.",
                "Settings",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtUsername.Focus()

            Return

        End If

        If String.IsNullOrWhiteSpace(
            txtFullName.Text
        ) Then

            MessageBox.Show(
                "Full name cannot be empty.",
                "Settings",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtFullName.Focus()

            Return

        End If

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                Dim sql As String =
                    "UPDATE Users SET " &
                    "Username = @Username, " &
                    "FullName = @FullName " &
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
                        "@UserID",
                        LoginForm.LoggedInUserID
                    )

                    command.ExecuteNonQuery()

                End Using

            End Using

            LoginForm.LoggedInUsername =
                txtUsername.Text.Trim()

            LoginForm.LoggedInFullName =
                txtFullName.Text.Trim()

            MessageBox.Show(
                "Your profile has been updated successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

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
    ' CHANGE PASSWORD
    '==========================================================

    Private Sub ChangePassword(
        sender As Object,
        e As EventArgs
    )

        If String.IsNullOrWhiteSpace(
            txtCurrentPassword.Text
        ) Then

            MessageBox.Show(
                "Please enter your current password.",
                "Change Password",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtCurrentPassword.Focus()

            Return

        End If

        If String.IsNullOrWhiteSpace(
            txtNewPassword.Text
        ) Then

            MessageBox.Show(
                "Please enter a new password.",
                "Change Password",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtNewPassword.Focus()

            Return

        End If

        If txtNewPassword.Text <>
           txtConfirmPassword.Text Then

            MessageBox.Show(
                "The new passwords do not match.",
                "Change Password",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtConfirmPassword.Focus()

            Return

        End If

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                '--------------------------------------------------
                ' VERIFY CURRENT PASSWORD
                '--------------------------------------------------

                Dim checkSql As String =
                    "SELECT COUNT(*) " &
                    "FROM Users " &
                    "WHERE UserID = @UserID " &
                    "AND Password = @Password " &
                    "AND IsActive = 1"

                Using checkCommand As New SqlCommand(
                    checkSql,
                    connection
                )

                    checkCommand.Parameters.AddWithValue(
                        "@UserID",
                        LoginForm.LoggedInUserID
                    )

                    checkCommand.Parameters.AddWithValue(
                        "@Password",
                        txtCurrentPassword.Text
                    )

                    Dim result As Integer =
                        Convert.ToInt32(
                            checkCommand.ExecuteScalar()
                        )

                    If result = 0 Then

                        MessageBox.Show(
                            "The current password is incorrect.",
                            "Change Password",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        txtCurrentPassword.Clear()

                        txtCurrentPassword.Focus()

                        Return

                    End If

                End Using

                '--------------------------------------------------
                ' UPDATE PASSWORD
                '--------------------------------------------------

                Dim updateSql As String =
                    "UPDATE Users " &
                    "SET Password = @NewPassword " &
                    "WHERE UserID = @UserID"

                Using updateCommand As New SqlCommand(
                    updateSql,
                    connection
                )

                    updateCommand.Parameters.AddWithValue(
                        "@NewPassword",
                        txtNewPassword.Text
                    )

                    updateCommand.Parameters.AddWithValue(
                        "@UserID",
                        LoginForm.LoggedInUserID
                    )

                    updateCommand.ExecuteNonQuery()

                End Using

            End Using

            MessageBox.Show(
                "Your password has been changed successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            txtCurrentPassword.Clear()

            txtNewPassword.Clear()

            txtConfirmPassword.Clear()

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '==========================================================
    ' LOAD SYSTEM INFORMATION
    '==========================================================

    Private Sub LoadSystemInformation()

        txtSystemName.Text =
            "Casa Brew"

        txtSystemDescription.Text =
            "Rental Property Management System"

        lblDatabaseName.Text =
            "Database: RentalPropertyDB"

        lblDatabaseServer.Text =
            "Server: Local SQL Server"

        lblDatabaseStatus.Text =
            "Status: Checking..."

        TestDatabaseConnection(
            Nothing,
            EventArgs.Empty
        )

    End Sub

    '==========================================================
    ' SAVE SYSTEM INFORMATION
    '==========================================================

    Private Sub SaveSystemInformation(
        sender As Object,
        e As EventArgs
    )

        If String.IsNullOrWhiteSpace(
            txtSystemName.Text
        ) Then

            MessageBox.Show(
                "System name cannot be empty.",
                "Settings",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If

        MessageBox.Show(
            "System information saved for this session.",
            "Casa Brew",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        )

    End Sub

    '==========================================================
    ' RESET SYSTEM INFORMATION
    '==========================================================

    Private Sub ResetSystemInformation(
        sender As Object,
        e As EventArgs
    )

        txtSystemName.Text =
            "Casa Brew"

        txtSystemDescription.Text =
            "Rental Property Management System"

    End Sub

    '==========================================================
    ' TEST DATABASE CONNECTION
    '==========================================================

    Private Sub TestDatabaseConnection(
        sender As Object,
        e As EventArgs
    )

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                lblDatabaseStatus.Text =
                    "Status: Connected"

                lblDatabaseStatus.ForeColor =
                    Sage

            End Using

        Catch ex As Exception

            lblDatabaseStatus.Text =
                "Status: Connection Failed"

            lblDatabaseStatus.ForeColor =
                Color.FromArgb(
                    145,
                    91,
                    70
                )

        End Try

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