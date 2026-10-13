Imports System.Data
Imports System.Drawing
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class LoginForm
    Inherits Form

    ' ============================================================
    ' COLORS - AURELIA GRAND HOTEL
    ' ============================================================

    Private ReadOnly DeepNavy As Color =
        Color.FromArgb(20, 27, 38)

    Private ReadOnly Navy As Color =
        Color.FromArgb(31, 42, 56)

    Private ReadOnly Champagne As Color =
        Color.FromArgb(198, 166, 110)

    Private ReadOnly SoftGold As Color =
        Color.FromArgb(218, 192, 143)

    Private ReadOnly Ivory As Color =
        Color.FromArgb(247, 244, 237)

    Private ReadOnly WarmWhite As Color =
        Color.FromArgb(253, 251, 247)

    Private ReadOnly Charcoal As Color =
        Color.FromArgb(45, 45, 43)

    Private ReadOnly MutedGray As Color =
        Color.FromArgb(125, 125, 120)


    ' ============================================================
    ' CONTROLS
    ' ============================================================

    Private lblHotelName As Label
    Private lblHotelSubtitle As Label
    Private lblWelcome As Label

    Private lblUsername As Label
    Private lblPassword As Label

    Private txtUsername As TextBox
    Private txtPassword As TextBox

    Private btnLogin As Button
    Private btnExit As Button

    Private chkShowPassword As CheckBox

    Private pnlLogin As Panel


    ' ============================================================
    ' LOGIN INFORMATION
    ' ============================================================

    Public Shared LoggedInUserID As Integer = 0

    Public Shared LoggedInUsername As String = ""

    Public Shared LoggedInFullName As String = ""

    Public Shared LoggedInRole As String = ""


    ' ============================================================
    ' CONSTRUCTOR
    ' ============================================================

    Public Sub New()

        InitializeForm()

    End Sub


    ' ============================================================
    ' INITIALIZE FORM
    ' ============================================================

    Private Sub InitializeForm()

        ' --------------------------------------------------------
        ' FORM SETTINGS
        ' --------------------------------------------------------

        Me.Text =
            "Aurelia Grand Hotel - Login"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(620, 720)

        Me.MinimumSize =
            New Size(620, 720)

        Me.FormBorderStyle =
            FormBorderStyle.FixedSingle

        Me.MaximizeBox = False

        Me.MinimizeBox = False

        Me.BackColor =
            DeepNavy

        Me.Font =
            New Font(
                "Segoe UI",
                10.0F,
                FontStyle.Regular
            )


        ' --------------------------------------------------------
        ' HOTEL NAME
        ' --------------------------------------------------------

        lblHotelName =
            New Label With {
                .Text = "AURELIA",
                .Location = New Point(0, 55),
                .Size = New Size(620, 55),
                .TextAlign =
                    ContentAlignment.MiddleCenter,
                .Font =
                    New Font(
                        "Georgia",
                        30,
                        FontStyle.Bold
                    ),
                .ForeColor =
                    Champagne
            }

        Me.Controls.Add(lblHotelName)


        ' --------------------------------------------------------
        ' HOTEL SUBTITLE
        ' --------------------------------------------------------

        lblHotelSubtitle =
            New Label With {
                .Text = "GRAND HOTEL",
                .Location = New Point(0, 108),
                .Size = New Size(620, 30),
                .TextAlign =
                    ContentAlignment.MiddleCenter,
                .Font =
                    New Font(
                        "Georgia",
                        13,
                        FontStyle.Regular
                    ),
                .ForeColor =
                    SoftGold
            }

        Me.Controls.Add(lblHotelSubtitle)


        ' --------------------------------------------------------
        ' DECORATIVE LINE
        ' --------------------------------------------------------

        Dim separator As New Panel()

        separator.Location =
            New Point(205, 150)

        separator.Size =
            New Size(210, 1)

        separator.BackColor =
            Champagne

        Me.Controls.Add(separator)


        ' --------------------------------------------------------
        ' WELCOME TEXT
        ' --------------------------------------------------------

        lblWelcome =
            New Label With {
                .Text = "Welcome back",
                .Location = New Point(0, 180),
                .Size = New Size(620, 35),
                .TextAlign =
                    ContentAlignment.MiddleCenter,
                .Font =
                    New Font(
                        "Georgia",
                        16,
                        FontStyle.Italic
                    ),
                .ForeColor =
                    Ivory
            }

        Me.Controls.Add(lblWelcome)


        ' --------------------------------------------------------
        ' LOGIN PANEL
        ' --------------------------------------------------------

        pnlLogin =
            New Panel()

        pnlLogin.Location =
            New Point(105, 235)

        pnlLogin.Size =
            New Size(410, 350)

        pnlLogin.BackColor =
            WarmWhite

        Me.Controls.Add(pnlLogin)


        ' --------------------------------------------------------
        ' USERNAME LABEL
        ' --------------------------------------------------------

        lblUsername =
            New Label With {
                .Text = "USERNAME",
                .Location = New Point(35, 35),
                .AutoSize = True,
                .Font =
                    New Font(
                        "Segoe UI Semibold",
                        9.5F,
                        FontStyle.Bold
                    ),
                .ForeColor =
                    Charcoal
            }

        pnlLogin.Controls.Add(lblUsername)


        ' --------------------------------------------------------
        ' USERNAME TEXTBOX
        ' --------------------------------------------------------

        txtUsername =
            New TextBox With {
                .Location = New Point(35, 62),
                .Size = New Size(340, 35),
                .Font =
                    New Font(
                        "Segoe UI",
                        10.5F
                    ),
                .BorderStyle =
                    BorderStyle.FixedSingle
            }

        pnlLogin.Controls.Add(txtUsername)


        ' --------------------------------------------------------
        ' PASSWORD LABEL
        ' --------------------------------------------------------

        lblPassword =
            New Label With {
                .Text = "PASSWORD",
                .Location = New Point(35, 120),
                .AutoSize = True,
                .Font =
                    New Font(
                        "Segoe UI Semibold",
                        9.5F,
                        FontStyle.Bold
                    ),
                .ForeColor =
                    Charcoal
            }

        pnlLogin.Controls.Add(lblPassword)


        ' --------------------------------------------------------
        ' PASSWORD TEXTBOX
        ' --------------------------------------------------------

        txtPassword =
            New TextBox With {
                .Location = New Point(35, 147),
                .Size = New Size(340, 35),
                .Font =
                    New Font(
                        "Segoe UI",
                        10.5F
                    ),
                .UseSystemPasswordChar = True,
                .BorderStyle =
                    BorderStyle.FixedSingle
            }

        pnlLogin.Controls.Add(txtPassword)


        ' --------------------------------------------------------
        ' SHOW PASSWORD
        ' --------------------------------------------------------

        chkShowPassword =
            New CheckBox With {
                .Text = "Show password",
                .Location = New Point(35, 192),
                .AutoSize = True,
                .Font =
                    New Font(
                        "Segoe UI",
                        9.0F
                    ),
                .ForeColor =
                    MutedGray
            }

        AddHandler chkShowPassword.CheckedChanged,
            AddressOf ShowPasswordChanged

        pnlLogin.Controls.Add(chkShowPassword)


        ' --------------------------------------------------------
        ' LOGIN BUTTON
        ' --------------------------------------------------------

        btnLogin =
            New Button With {
                .Text = "SIGN IN",
                .Location = New Point(35, 235),
                .Size = New Size(165, 45),
                .BackColor =
                    Champagne,
                .ForeColor =
                    DeepNavy,
                .FlatStyle =
                    FlatStyle.Flat,
                .Font =
                    New Font(
                        "Segoe UI Semibold",
                        10.0F,
                        FontStyle.Bold
                    ),
                .UseVisualStyleBackColor = False,
                .Cursor =
                    Cursors.Hand
            }

        btnLogin.FlatAppearance.BorderSize = 0

        AddHandler btnLogin.Click,
            AddressOf LoginButtonClicked

        pnlLogin.Controls.Add(btnLogin)


        ' --------------------------------------------------------
        ' EXIT BUTTON
        ' --------------------------------------------------------

        btnExit =
            New Button With {
                .Text = "EXIT APPLICATION",
                .Location = New Point(210, 235),
                .Size = New Size(165, 45),
                .BackColor =
                    DeepNavy,
                .ForeColor =
                    Ivory,
                .FlatStyle =
                    FlatStyle.Flat,
                .Font =
                    New Font(
                        "Segoe UI Semibold",
                        10.0F,
                        FontStyle.Bold
                    ),
                .UseVisualStyleBackColor = False,
                .Cursor =
                    Cursors.Hand
            }

        btnExit.FlatAppearance.BorderSize = 0

        AddHandler btnExit.Click,
            AddressOf ExitButtonClicked

        pnlLogin.Controls.Add(btnExit)


        ' --------------------------------------------------------
        ' ENTER KEY
        ' --------------------------------------------------------

        Me.AcceptButton =
            btnLogin


        ' --------------------------------------------------------
        ' INITIAL FOCUS
        ' --------------------------------------------------------

        Me.ActiveControl =
            txtUsername

    End Sub


    ' ============================================================
    ' SHOW / HIDE PASSWORD
    ' ============================================================

    Private Sub ShowPasswordChanged(
        sender As Object,
        e As EventArgs
    )

        txtPassword.UseSystemPasswordChar =
            Not chkShowPassword.Checked

    End Sub


    ' ============================================================
    ' LOGIN
    ' ============================================================

    Private Sub LoginButtonClicked(
        sender As Object,
        e As EventArgs
    )

        Dim username As String =
            txtUsername.Text.Trim()

        Dim password As String =
            txtPassword.Text


        ' --------------------------------------------------------
        ' VALIDATION
        ' --------------------------------------------------------

        If String.IsNullOrWhiteSpace(username) Then

            MessageBox.Show(
                "Please enter your username.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtUsername.Focus()

            Return

        End If


        If String.IsNullOrWhiteSpace(password) Then

            MessageBox.Show(
                "Please enter your password.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtPassword.Focus()

            Return

        End If


        Try

            Dim passwordHash As String =
                HashPassword(password)


            Dim sql As String =
                "SELECT " &
                "UserID, " &
                "Username, " &
                "FullName, " &
                "RoleName " &
                "FROM Users " &
                "WHERE Username = @Username " &
                "AND PasswordHash = @PasswordHash " &
                "AND IsActive = 1"


            Using conn As SqlConnection =
                DatabaseHelper.GetConnection()

                Using cmd As New SqlCommand(
                    sql,
                    conn
                )

                    cmd.Parameters.Add(
                        "@Username",
                        SqlDbType.NVarChar,
                        50
                    ).Value =
                        username


                    cmd.Parameters.Add(
                        "@PasswordHash",
                        SqlDbType.NVarChar,
                        64
                    ).Value =
                        passwordHash


                    conn.Open()


                    Using reader As SqlDataReader =
                        cmd.ExecuteReader()

                        If reader.Read() Then

                            LoggedInUserID =
                                Convert.ToInt32(
                                    reader("UserID")
                                )


                            LoggedInUsername =
                                reader(
                                    "Username"
                                ).ToString()


                            LoggedInFullName =
                                reader(
                                    "FullName"
                                ).ToString()


                            LoggedInRole =
                                reader(
                                    "RoleName"
                                ).ToString()


                            MessageBox.Show(
                                "Welcome to Aurelia Grand Hotel, " &
                                LoggedInFullName &
                                "." &
                                Environment.NewLine &
                                Environment.NewLine &
                                "Role: " &
                                LoggedInRole,
                                "Welcome",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            )


                            Me.DialogResult =
                                DialogResult.OK

                            Me.Close()

                        Else

                            MessageBox.Show(
                                "The username or password you entered is incorrect.",
                                "Sign In Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            )


                            txtPassword.Clear()

                            txtPassword.Focus()

                        End If

                    End Using

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Unable to connect to the Aurelia Grand Hotel database." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    ' ============================================================
    ' SHA-256 PASSWORD HASH
    ' ============================================================

    Private Function HashPassword(
        password As String
    ) As String

        Using sha256 As SHA256 =
            SHA256.Create()

            Dim bytes As Byte() =
                Encoding.UTF8.GetBytes(
                    password
                )


            Dim hash As Byte() =
                sha256.ComputeHash(
                    bytes
                )


            Dim builder As New StringBuilder()


            For Each b As Byte In hash

                builder.Append(
                    b.ToString("x2")
                )

            Next


            Return builder.ToString()

        End Using

    End Function


    ' ============================================================
    ' EXIT
    ' ============================================================

    Private Sub ExitButtonClicked(
        sender As Object,
        e As EventArgs
    )

        Dim result As DialogResult =
            MessageBox.Show(
                "Are you sure you want to exit Aurelia Grand Hotel?",
                "Exit Application",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )

        If result = DialogResult.Yes Then
            Application.Exit()
        End If

    End Sub

End Class