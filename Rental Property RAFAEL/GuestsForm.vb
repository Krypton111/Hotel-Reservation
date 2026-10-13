Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class GuestsForm
    Inherits Form

    '==========================================================
    ' CONTROLS
    '==========================================================

    Private txtGuestID As TextBox
    Private txtFirstName As TextBox
    Private txtLastName As TextBox
    Private txtFullName As TextBox
    Private txtContactNumber As TextBox
    Private txtEmail As TextBox
    Private txtAddress As TextBox
    Private txtCity As TextBox
    Private txtCountry As TextBox
    Private txtSearch As TextBox

    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnArchive As Button
    Private btnViewArchived As Button
    Private viewingArchived As Boolean = False
    Private btnClear As Button
    Private btnSearch As Button
    Private btnClose As Button

    Private dgvGuests As DataGridView

    Private selectedGuestID As Integer = 0

    '==========================================================
    ' COLORS
    '==========================================================

    Private ReadOnly DeepNavy As Color =
        Color.FromArgb(20, 27, 38)

    Private ReadOnly Navy As Color =
        Color.FromArgb(31, 42, 56)

    Private ReadOnly Champagne As Color =
        Color.FromArgb(198, 166, 110)

    Private ReadOnly Ivory As Color =
        Color.FromArgb(247, 244, 237)

    Private ReadOnly WarmWhite As Color =
        Color.FromArgb(253, 251, 247)

    Private ReadOnly Charcoal As Color =
        Color.FromArgb(45, 45, 43)

    Private ReadOnly MutedGray As Color =
        Color.FromArgb(125, 125, 120)

    '==========================================================
    ' CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildForm()

        LoadGuests()

        ClearFields()

    End Sub

    '==========================================================
    ' BUILD FORM
    '==========================================================

    Private Sub BuildForm()

        Me.Text =
            "Aurelia Grand Hotel - Guests"

        Me.StartPosition =
            FormStartPosition.CenterParent

        Me.Size =
            New Size(1200, 760)

        Me.MinimumSize =
            New Size(1050, 650)

        Me.BackColor =
            Ivory

        Me.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        '======================================================
        ' HEADER
        '======================================================

        Dim headerPanel As New Panel()

        headerPanel.Dock =
            DockStyle.Top

        headerPanel.Height =
            90

        headerPanel.BackColor =
            DeepNavy

        Me.Controls.Add(
            headerPanel
        )

        Dim lblTitle As New Label()

        lblTitle.Text =
            "GUEST MANAGEMENT"

        lblTitle.Location =
            New Point(30, 18)

        lblTitle.Size =
            New Size(500, 35)

        lblTitle.Font =
            New Font(
                "Georgia",
                22,
                FontStyle.Regular
            )

        lblTitle.ForeColor =
            Color.White

        headerPanel.Controls.Add(
            lblTitle
        )

        Dim lblSubtitle As New Label()

        lblSubtitle.Text =
            "Manage guest profiles and contact information."

        lblSubtitle.Location =
            New Point(32, 56)

        lblSubtitle.Size =
            New Size(600, 22)

        lblSubtitle.Font =
            New Font(
                "Segoe UI",
                9.0F
            )

        lblSubtitle.ForeColor =
            Color.FromArgb(
                205,
                201,
                192
            )

        headerPanel.Controls.Add(
            lblSubtitle
        )

        '======================================================
        ' MAIN PANEL
        '======================================================

        Dim mainPanel As New Panel()

        mainPanel.Dock =
            DockStyle.Fill

        mainPanel.Padding =
            New Padding(
                25,
                20,
                25,
                20
            )

        mainPanel.BackColor =
            Ivory

        Me.Controls.Add(
            mainPanel
        )

        '======================================================
        ' GUEST INFORMATION PANEL
        '======================================================

        Dim formPanel As New Panel()

        formPanel.Dock =
            DockStyle.Top

        formPanel.Height =
            240

        formPanel.BackColor =
            WarmWhite

        formPanel.BorderStyle =
            BorderStyle.FixedSingle

        mainPanel.Controls.Add(
            formPanel
        )

        CreateSectionTitle(
            formPanel,
            "GUEST INFORMATION",
            20,
            15
        )

        txtGuestID =
            CreateTextBox(
                formPanel,
                "Guest ID",
                20,
                50,
                130,
                True
            )

        txtFirstName =
            CreateTextBox(
                formPanel,
                "First Name",
                170,
                50,
                200,
                False
            )

        txtLastName =
            CreateTextBox(
                formPanel,
                "Last Name",
                390,
                50,
                200,
                False
            )

        txtFullName =
            CreateTextBox(
                formPanel,
                "Full Name",
                610,
                50,
                250,
                True
            )

        txtContactNumber =
            CreateTextBox(
                formPanel,
                "Contact Number",
                880,
                50,
                220,
                False
            )

        txtEmail =
            CreateTextBox(
                formPanel,
                "Email Address",
                20,
                115,
                300,
                False
            )

        txtAddress =
            CreateTextBox(
                formPanel,
                "Address",
                340,
                115,
                360,
                False
            )

        txtCity =
            CreateTextBox(
                formPanel,
                "City",
                720,
                115,
                180,
                False
            )

        txtCountry =
            CreateTextBox(
                formPanel,
                "Country",
                920,
                115,
                180,
                False
            )

        ' Keep form input lengths within the SQL Server column definitions.
        txtFirstName.MaxLength = 80
        txtLastName.MaxLength = 80
        txtContactNumber.MaxLength = 30
        txtEmail.MaxLength = 150
        txtAddress.MaxLength = 250
        txtCity.MaxLength = 100
        txtCountry.MaxLength = 100

        '======================================================
        ' BUTTON PANEL
        '======================================================

        Dim buttonPanel As New Panel()

        buttonPanel.Dock =
            DockStyle.Top

        buttonPanel.Height =
            65

        buttonPanel.BackColor =
            Ivory

        mainPanel.Controls.Add(
            buttonPanel
        )

        btnAdd =
            CreateActionButton(
                "ADD GUEST",
                0,
                Champagne,
                Color.White
            )

        buttonPanel.Controls.Add(
            btnAdd
        )

        btnUpdate =
            CreateActionButton(
                "UPDATE",
                155,
                Navy,
                Color.White
            )

        buttonPanel.Controls.Add(
            btnUpdate
        )

        btnArchive =
            CreateActionButton(
                "ARCHIVE",
                260,
                Color.FromArgb(
                    120,
                    65,
                    65
                ),
                Color.White
            )

        buttonPanel.Controls.Add(
            btnArchive
        )

        btnClear =
            CreateActionButton(
                "CLEAR",
                365,
                Color.FromArgb(
                    90,
                    90,
                    85
                ),
                Color.White
            )

        buttonPanel.Controls.Add(
            btnClear
        )

        btnClose =
            CreateActionButton(
                "CLOSE",
                470,
                DeepNavy,
                Color.White
            )

        buttonPanel.Controls.Add(
            btnClose
        )

        btnViewArchived = CreateActionButton(
            "SHOW ARCHIVED",
            625,
            Color.FromArgb(84, 88, 96),
            Color.White
        )
        btnViewArchived.Size = New Size(160, 38)
        buttonPanel.Controls.Add(btnViewArchived)

        '======================================================
        ' SEARCH PANEL
        '======================================================

        Dim searchPanel As New Panel()

        searchPanel.Dock =
            DockStyle.Top

        searchPanel.Height =
            55

        searchPanel.BackColor =
            Ivory

        mainPanel.Controls.Add(
            searchPanel
        )

        Dim lblSearch As New Label()

        lblSearch.Text =
            "SEARCH"

        lblSearch.Location =
            New Point(0, 15)

        lblSearch.Size =
            New Size(70, 25)

        lblSearch.Font =
            New Font(
                "Segoe UI Semibold",
                9.0F,
                FontStyle.Bold
            )

        lblSearch.ForeColor =
            Charcoal

        searchPanel.Controls.Add(
            lblSearch
        )

        txtSearch =
            New TextBox()

        txtSearch.Location =
            New Point(75, 10)

        txtSearch.Size =
            New Size(430, 30)

        txtSearch.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        txtSearch.PlaceholderText =
            "Search guest name, email or contact number"

        searchPanel.Controls.Add(
            txtSearch
        )

        btnSearch =
            CreateActionButton(
                "SEARCH",
                520,
                Navy,
                Color.White
            )

        btnSearch.Location =
            New Point(520, 7)

        searchPanel.Controls.Add(
            btnSearch
        )

        '======================================================
        ' DATA GRID
        '======================================================

        dgvGuests =
            New DataGridView()

        dgvGuests.Dock =
            DockStyle.Fill

        dgvGuests.BackgroundColor =
            WarmWhite

        dgvGuests.BorderStyle =
            BorderStyle.FixedSingle

        dgvGuests.AllowUserToAddRows =
            False

        dgvGuests.AllowUserToDeleteRows =
            False

        dgvGuests.ReadOnly =
            True

        dgvGuests.MultiSelect =
            False

        dgvGuests.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvGuests.RowHeadersVisible =
            False

        ' Keep both scrollbars available so every guest row and column can be reached.
        dgvGuests.ScrollBars = ScrollBars.Both
        dgvGuests.ColumnHeadersVisible = True
        dgvGuests.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvGuests.RowTemplate.Height = 28
        dgvGuests.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None

        dgvGuests.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvGuests.EnableHeadersVisualStyles =
            False

        dgvGuests.ColumnHeadersDefaultCellStyle.BackColor =
            DeepNavy

        dgvGuests.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        dgvGuests.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI Semibold",
                8.5F,
                FontStyle.Bold
            )

        dgvGuests.DefaultCellStyle.Font =
            New Font(
                "Segoe UI",
                8.5F
            )

        dgvGuests.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(
                235,
                226,
                207
            )

        dgvGuests.DefaultCellStyle.SelectionForeColor =
            Charcoal

        mainPanel.Controls.Add(dgvGuests)

        ' The list heading has its own docked panel above the table.
        Dim guestListHeader As New Panel()
        guestListHeader.Dock = DockStyle.Top
        guestListHeader.Height = 38
        guestListHeader.BackColor = Ivory

        Dim lblGuestList As New Label()
        lblGuestList.Text = "GUEST LIST"
        lblGuestList.Dock = DockStyle.Fill
        lblGuestList.TextAlign = ContentAlignment.MiddleLeft
        lblGuestList.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        lblGuestList.ForeColor = DeepNavy
        guestListHeader.Controls.Add(lblGuestList)
        mainPanel.Controls.Add(guestListHeader)

        ' WinForms docking is z-order-sensitive. Put the fill grid behind
        ' the top-docked panels so it occupies only the remaining area.
        mainPanel.Controls.SetChildIndex(dgvGuests, mainPanel.Controls.Count - 1)
        mainPanel.Controls.SetChildIndex(guestListHeader, 0)
        mainPanel.Controls.SetChildIndex(searchPanel, 0)
        mainPanel.Controls.SetChildIndex(buttonPanel, 0)
        mainPanel.Controls.SetChildIndex(formPanel, 0)

        '======================================================
        ' EVENTS
        '======================================================

        AddHandler txtFirstName.TextChanged,
            AddressOf NameChanged

        AddHandler txtLastName.TextChanged,
            AddressOf NameChanged

        AddHandler btnAdd.Click,
            AddressOf AddGuest

        AddHandler btnUpdate.Click,
            AddressOf UpdateGuest

        AddHandler btnArchive.Click,
            AddressOf ArchiveGuest

        AddHandler btnViewArchived.Click,
            AddressOf ToggleArchivedGuests

        AddHandler btnClear.Click,
            AddressOf ClearButton_Click

        AddHandler btnSearch.Click,
            AddressOf SearchGuests

        AddHandler txtSearch.KeyDown,
            AddressOf SearchTextBox_KeyDown

        AddHandler btnClose.Click,
            AddressOf CloseForm

        AddHandler dgvGuests.CellClick,
            AddressOf GuestGrid_CellClick

    End Sub

    '==========================================================
    ' SECTION TITLE
    '==========================================================

    Private Sub CreateSectionTitle(
        parent As Control,
        text As String,
        x As Integer,
        y As Integer
    )

        Dim label As New Label()

        label.Text =
            text

        label.Location =
            New Point(
                x,
                y
            )

        label.Size =
            New Size(
                350,
                25
            )

        label.Font =
            New Font(
                "Segoe UI Semibold",
                9.0F,
                FontStyle.Bold
            )

        label.ForeColor =
            Charcoal

        parent.Controls.Add(
            label
        )

        Dim line As New Panel()

        line.Location =
            New Point(
                x,
                y + 27
            )

        line.Size =
            New Size(
                55,
                2
            )

        line.BackColor =
            Champagne

        parent.Controls.Add(
            line
        )

    End Sub

    '==========================================================
    ' TEXTBOX CREATOR
    '==========================================================

    Private Function CreateTextBox(
        parent As Control,
        labelText As String,
        x As Integer,
        y As Integer,
        width As Integer,
        readOnlyValue As Boolean
    ) As TextBox

        Dim label As New Label()

        label.Text =
            labelText

        label.Location =
            New Point(
                x,
                y
            )

        label.Size =
            New Size(
                width,
                20
            )

        label.Font =
            New Font(
                "Segoe UI",
                8.0F
            )

        label.ForeColor =
            MutedGray

        parent.Controls.Add(
            label
        )

        Dim box As New TextBox()

        box.Location =
            New Point(
                x,
                y + 20
            )

        box.Size =
            New Size(
                width,
                28
            )

        box.Font =
            New Font(
                "Segoe UI",
                9.0F
            )

        box.BorderStyle =
            BorderStyle.FixedSingle

        box.ReadOnly =
            readOnlyValue

        If readOnlyValue Then

            box.BackColor =
                Color.FromArgb(
                    239,
                    236,
                    228
                )

        Else

            box.BackColor =
                Color.White

        End If

        parent.Controls.Add(
            box
        )

        Return box

    End Function

    '==========================================================
    ' BUTTON CREATOR
    '==========================================================

    Private Function CreateActionButton(
        text As String,
        x As Integer,
        backColor As Color,
        foreColor As Color
    ) As Button

        Dim button As New Button()

        button.Text =
            text

        button.Location =
            New Point(
                x,
                10
            )

        button.Size =
            New Size(
                145,
                38
            )

        button.FlatStyle =
            FlatStyle.Flat

        button.FlatAppearance.BorderSize =
            0

        button.BackColor =
            backColor

        button.ForeColor =
            foreColor

        button.Font =
            New Font(
                "Segoe UI Semibold",
                8.5F,
                FontStyle.Bold
            )

        button.Cursor =
            Cursors.Hand

        Return button

    End Function

    '==========================================================
    ' NAME CHANGE
    '==========================================================

    Private Sub NameChanged(
        sender As Object,
        e As EventArgs
    )

        Dim firstName As String =
            txtFirstName.Text.Trim()

        Dim lastName As String =
            txtLastName.Text.Trim()

        If firstName = "" AndAlso
           lastName = "" Then

            txtFullName.Clear()

        Else

            txtFullName.Text =
                (firstName & " " &
                 lastName).Trim()

        End If

    End Sub

    '==========================================================
    ' LOAD GUESTS
    '==========================================================

    Private Sub LoadGuests()

        Try

            Using connection As SqlConnection =
                DatabaseHelper.GetConnection()

                connection.Open()

                Dim sql As String =
                    "SELECT " &
                    "GuestID, " &
                    "FirstName, " &
                    "LastName, " &
                    "FullName, " &
                    "ContactNumber, " &
                    "Email, " &
                    "Address, " &
                    "City, " &
                    "Country " &
                    "FROM Guests " &
                    "WHERE IsArchived = " & If(viewingArchived, "1", "0") & " " &
                    "ORDER BY GuestID DESC"

                Using adapter As New SqlDataAdapter(
                    sql,
                    connection
                )

                    Dim table As New DataTable()

                    adapter.Fill(
                        table
                    )

                    dgvGuests.DataSource =
                        table

                End Using

            End Using

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '==========================================================
    ' VALIDATE
    '==========================================================

    Private Function ValidateFields() As Boolean

        If String.IsNullOrWhiteSpace(
            txtFirstName.Text
        ) Then

            MessageBox.Show(
                "Please enter the guest's first name.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtFirstName.Focus()

            Return False

        End If

        If String.IsNullOrWhiteSpace(
            txtLastName.Text
        ) Then

            MessageBox.Show(
                "Please enter the guest's last name.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtLastName.Focus()

            Return False

        End If

        If String.IsNullOrWhiteSpace(
            txtContactNumber.Text
        ) Then

            MessageBox.Show(
                "Please enter the guest's contact number.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtContactNumber.Focus()

            Return False

        End If

        If String.IsNullOrWhiteSpace(
            txtEmail.Text
        ) Then

            MessageBox.Show(
                "Please enter the guest's email address.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtEmail.Focus()

            Return False

        End If

        If Not txtEmail.Text.Contains("@") Then

            MessageBox.Show(
                "Please enter a valid email address.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtEmail.Focus()

            Return False

        End If

        Return True

    End Function

    '==========================================================
    ' ADD GUEST
    '==========================================================

    Private Sub AddGuest(
        sender As Object,
        e As EventArgs
    )

        If viewingArchived Then
            MessageBox.Show("Switch to active guests before adding a guest.", "Archive View", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If Not ValidateFields() Then

            Return

        End If

        Try

            Using connection As SqlConnection =
                DatabaseHelper.GetConnection()

                Dim sql As String =
                    "INSERT INTO Guests " &
                    "(" &
                    "FirstName, " &
                    "LastName, " &
                    "ContactNumber, " &
                    "Email, " &
                    "Address, " &
                    "City, " &
                    "Country" &
                    ") " &
                    "VALUES " &
                    "(" &
                    "@FirstName, " &
                    "@LastName, " &
                    "@ContactNumber, " &
                    "@Email, " &
                    "@Address, " &
                    "@City, " &
                    "@Country" &
                    ")"

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddGuestParameters(
                        command
                    )

                    connection.Open()

                    If command.ExecuteNonQuery() <> 1 Then
                        Throw New InvalidOperationException("The guest record was not found or was not changed.")
                    End If
                    AuditLogger.Log("Changed guest data", "A guest record was created, updated, or otherwise changed.")
                    

                End Using

            End Using

            MessageBox.Show(
                "Guest successfully added.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            LoadGuests()

            ClearFields()

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '==========================================================
    ' ADD PARAMETERS
    '==========================================================

    Private Sub AddGuestParameters(
        command As SqlCommand
    )

        command.Parameters.Add(
            "@FirstName",
            SqlDbType.NVarChar,
            100
        ).Value =
            txtFirstName.Text.Trim()

        command.Parameters.Add(
            "@LastName",
            SqlDbType.NVarChar,
            100
        ).Value =
            txtLastName.Text.Trim()

        command.Parameters.Add(
            "@ContactNumber",
            SqlDbType.NVarChar,
            30
        ).Value =
            txtContactNumber.Text.Trim()

        command.Parameters.Add(
            "@Email",
            SqlDbType.NVarChar,
            150
        ).Value =
            txtEmail.Text.Trim()

        If String.IsNullOrWhiteSpace(
            txtAddress.Text
        ) Then

            command.Parameters.Add(
                "@Address",
                SqlDbType.NVarChar,
                300
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@Address",
                SqlDbType.NVarChar,
                300
            ).Value =
                txtAddress.Text.Trim()

        End If

        If String.IsNullOrWhiteSpace(
            txtCity.Text
        ) Then

            command.Parameters.Add(
                "@City",
                SqlDbType.NVarChar,
                100
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@City",
                SqlDbType.NVarChar,
                100
            ).Value =
                txtCity.Text.Trim()

        End If

        command.Parameters.Add(
            "@Country",
            SqlDbType.NVarChar,
            100
        ).Value =
            If(
                String.IsNullOrWhiteSpace(txtCountry.Text),
                "Philippines",
                txtCountry.Text.Trim()
            )

    End Sub

    '==========================================================
    ' UPDATE GUEST
    '==========================================================

    Private Sub UpdateGuest(
        sender As Object,
        e As EventArgs
    )

        If viewingArchived Then
            MessageBox.Show("Restore this guest before editing it.", "Archive View", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If selectedGuestID <= 0 Then

            MessageBox.Show(
                "Please select a guest from the table first.",
                "Update Guest",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If

        If Not ValidateFields() Then

            Return

        End If

        Try

            Using connection As SqlConnection =
                DatabaseHelper.GetConnection()

                Dim sql As String =
                    "UPDATE Guests SET " &
                    "FirstName = @FirstName, " &
                    "LastName = @LastName, " &
                    "ContactNumber = @ContactNumber, " &
                    "Email = @Email, " &
                    "Address = @Address, " &
                    "City = @City, " &
                    "Country = @Country " &
                    "WHERE GuestID = @GuestID"

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddGuestParameters(
                        command
                    )

                    command.Parameters.Add(
                        "@GuestID",
                        SqlDbType.Int
                    ).Value =
                        selectedGuestID

                    connection.Open()

                    command.ExecuteNonQuery()
                    AuditLogger.Log("Changed guest data", "A guest record was created, updated, or otherwise changed.")
                    

                End Using

            End Using

            MessageBox.Show(
                "Guest information successfully updated.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            LoadGuests()

            ClearFields()

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '==========================================================
    ' ARCHIVE GUEST
    '==========================================================

    Private Sub ArchiveGuest(
        sender As Object,
        e As EventArgs
    )

        If selectedGuestID <= 0 Then
            MessageBox.Show(
                "Please select a guest from the table first.",
                If(viewingArchived, "Restore Guest", "Archive Guest"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )
            Return
        End If

        If Not viewingArchived AndAlso GuestHasReservations(selectedGuestID) Then
            MessageBox.Show(
                "Archive this guest's reservations first. This prevents active reservations from being left linked to an archived guest.",
                "Guest Has Reservations",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )
            Return
        End If

        Dim actionText As String = If(viewingArchived, "restore", "archive")
        Dim result As DialogResult = MessageBox.Show(
            "Are you sure you want to " & actionText & " guest " & txtFullName.Text.Trim() & "?" &
            Environment.NewLine & Environment.NewLine &
            "Archived records are retained and can be restored later.",
            "Confirm " & If(viewingArchived, "Restore", "Archive"),
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

        If result <> DialogResult.Yes Then Return

        Try
            Using connection As SqlConnection = DatabaseHelper.GetConnection()
                connection.Open()

                Dim sql As String =
                    "UPDATE Guests SET IsArchived = @IsArchived, " &
                    "ArchivedAt = CASE WHEN @IsArchived = 1 THEN SYSDATETIME() ELSE NULL END " &
                    "WHERE GuestID = @GuestID"

                Using command As New SqlCommand(sql, connection)
                    command.Parameters.Add("@IsArchived", SqlDbType.Bit).Value = Not viewingArchived
                    command.Parameters.Add("@GuestID", SqlDbType.Int).Value = selectedGuestID

                    If command.ExecuteNonQuery() <> 1 Then
                        Throw New InvalidOperationException("The guest record was not found.")
                    End If
                End Using
            End Using

            AuditLogger.Log(
                If(viewingArchived, "Restored guest", "Archived guest"),
                If(viewingArchived, "Restored a guest record.", "Archived a guest record without deleting linked reservations or payments.")
            )

            MessageBox.Show(
                If(viewingArchived, "Guest restored successfully.", "Guest archived successfully."),
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            LoadGuests()
            ClearFields()

        Catch ex As Exception
            ShowDatabaseError(ex)
        End Try

    End Sub

    Private Sub ToggleArchivedGuests(sender As Object, e As EventArgs)

        viewingArchived = Not viewingArchived
        btnViewArchived.Text = If(viewingArchived, "SHOW ACTIVE", "SHOW ARCHIVED")
        btnArchive.Text = If(viewingArchived, "RESTORE", "ARCHIVE")
        btnAdd.Enabled = Not viewingArchived
        btnUpdate.Enabled = Not viewingArchived

        txtSearch.Clear()
        LoadGuests()
        ClearFields()

    End Sub

    '==========================================================
    ' CHECK RESERVATIONS
    '==========================================================

    Private Function GuestHasReservations(
        guestID As Integer
    ) As Boolean

        Try

            Using connection As SqlConnection =
                DatabaseHelper.GetConnection()

                Using command As New SqlCommand(
                    "SELECT COUNT(*) " &
                    "FROM Reservations " &
                    "WHERE GuestID = @GuestID AND IsArchived = 0",
                    connection
                )

                    command.Parameters.Add(
                        "@GuestID",
                        SqlDbType.Int
                    ).Value =
                        guestID

                    connection.Open()

                    Return Convert.ToInt32(
                        command.ExecuteScalar()
                    ) > 0

                End Using

            End Using

        Catch ex As Exception

            ShowDatabaseError(ex)

            Return True

        End Try

    End Function

    '==========================================================
    ' SEARCH GUESTS
    '==========================================================

    Private Sub SearchGuests(
        sender As Object,
        e As EventArgs
    )

        Dim searchText As String =
            txtSearch.Text.Trim()

        If searchText = "" Then

            LoadGuests()

            Return

        End If

        Try

            Using connection As SqlConnection =
                DatabaseHelper.GetConnection()

                Dim sql As String =
                    "SELECT " &
                    "GuestID, " &
                    "FirstName, " &
                    "LastName, " &
                    "FullName, " &
                    "ContactNumber, " &
                    "Email, " &
                    "Address, " &
                    "City, " &
                    "Country " &
                    "FROM Guests " &
                    "WHERE IsArchived = " & If(viewingArchived, "1", "0") & " " &
                    "AND (FullName LIKE @Search " &
                    "OR FirstName LIKE @Search " &
                    "OR LastName LIKE @Search " &
                    "OR ContactNumber LIKE @Search " &
                    "OR Email LIKE @Search) " &
                    "ORDER BY GuestID DESC"

                Using adapter As New SqlDataAdapter(
                    sql,
                    connection
                )

                    adapter.SelectCommand.Parameters.Add(
                        "@Search",
                        SqlDbType.NVarChar,
                        200
                    ).Value =
                        "%" &
                        searchText &
                        "%"

                    Dim table As New DataTable()

                    adapter.Fill(
                        table
                    )

                    dgvGuests.DataSource =
                        table

                End Using

            End Using

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '==========================================================
    ' SEARCH ENTER
    '==========================================================

    Private Sub SearchTextBox_KeyDown(
        sender As Object,
        e As KeyEventArgs
    )

        If e.KeyCode =
            Keys.Enter Then

            SearchGuests(
                sender,
                EventArgs.Empty
            )

            e.SuppressKeyPress =
                True

        End If

    End Sub

    '==========================================================
    ' GRID CLICK
    '==========================================================

    Private Sub GuestGrid_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then

            Return

        End If

        Dim row As DataGridViewRow =
            dgvGuests.Rows(
                e.RowIndex
            )

        selectedGuestID =
            Convert.ToInt32(
                row.Cells(
                    "GuestID"
                ).Value
            )

        txtGuestID.Text =
            selectedGuestID.ToString()

        txtFirstName.Text =
            GetCellString(
                row,
                "FirstName"
            )

        txtLastName.Text =
            GetCellString(
                row,
                "LastName"
            )

        txtFullName.Text =
            GetCellString(
                row,
                "FullName"
            )

        txtContactNumber.Text =
            GetCellString(
                row,
                "ContactNumber"
            )

        txtEmail.Text =
            GetCellString(
                row,
                "Email"
            )

        txtAddress.Text =
            GetCellString(
                row,
                "Address"
            )

        txtCity.Text =
            GetCellString(
                row,
                "City"
            )

        txtCountry.Text =
            GetCellString(
                row,
                "Country"
            )

    End Sub

    '==========================================================
    ' GET CELL STRING
    '==========================================================

    Private Function GetCellString(
        row As DataGridViewRow,
        columnName As String
    ) As String

        If row.Cells(
            columnName
        ).Value Is Nothing Then

            Return ""

        End If

        If IsDBNull(
            row.Cells(
                columnName
            ).Value
        ) Then

            Return ""

        End If

        Return row.Cells(
            columnName
        ).Value.ToString()

    End Function

    '==========================================================
    ' CLEAR FIELDS
    '==========================================================

    Private Sub ClearFields()

        selectedGuestID =
            0

        txtGuestID.Clear()

        txtFirstName.Clear()

        txtLastName.Clear()

        txtFullName.Clear()

        txtContactNumber.Clear()

        txtEmail.Clear()

        txtAddress.Clear()

        txtCity.Clear()

        txtCountry.Clear()

        dgvGuests.ClearSelection()

    End Sub

    '==========================================================
    ' CLEAR BUTTON
    '==========================================================

    Private Sub ClearButton_Click(
        sender As Object,
        e As EventArgs
    )

        ClearFields()

    End Sub

    '==========================================================
    ' CLOSE
    '==========================================================

    Private Sub CloseForm(
        sender As Object,
        e As EventArgs
    )

        Me.Close()

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
            "Aurelia Grand Hotel",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

    End Sub

End Class