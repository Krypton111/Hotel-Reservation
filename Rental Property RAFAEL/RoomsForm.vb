Imports Microsoft.Data.SqlClient

Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms

Public Class RoomsForm
    Inherits Form

    '===========================================================
    ' AURELIA GRAND HOTEL COLORS
    '===========================================================

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

    Private ReadOnly SoftGray As Color =
        Color.FromArgb(225, 222, 214)


    '===========================================================
    ' CONTROLS
    '===========================================================

    Private headerPanel As Panel
    Private contentPanel As Panel
    Private formPanel As Panel
    Private gridPanel As Panel

    Private titleLabel As Label
    Private subtitleLabel As Label

    Private roomIDLabel As Label
    Private roomNumberLabel As Label
    Private roomTypeLabel As Label
    Private capacityLabel As Label
    Private rateLabel As Label
    Private floorLabel As Label
    Private statusFieldLabel As Label
    Private descriptionLabel As Label

    Private txtRoomID As TextBox
    Private txtRoomNumber As TextBox
    Private cboRoomType As ComboBox
    Private nudCapacity As NumericUpDown
    Private txtRate As TextBox
    Private nudFloor As NumericUpDown
    Private cboStatus As ComboBox
    Private txtDescription As TextBox

    Private searchLabel As Label
    Private txtSearch As TextBox

    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnClear As Button
    Private btnSearch As Button
    Private btnRefresh As Button
    Private btnClose As Button

    Private dgvRooms As DataGridView

    Private selectedRoomID As Integer = 0


    '===========================================================
    ' CONSTRUCTOR
    '===========================================================

    Public Sub New()

        Me.Text =
            "Rooms | Aurelia Grand Hotel"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(1250, 760)

        Me.MinimumSize =
            New Size(1100, 650)

        Me.BackColor =
            Ivory

        Me.Font =
            New Font(
                "Segoe UI",
                10.0F
            )

        BuildForm()

        LoadRooms()

        ClearFields()

    End Sub


    '===========================================================
    ' BUILD FORM
    '===========================================================

    Private Sub BuildForm()

        ' A single root table reserves separate rows for the page header
        ' and the work area. The work area then reserves a dedicated editor
        ' row above the scrollable room grid.
        Dim rootLayout As New TableLayoutPanel()
        rootLayout.Dock = DockStyle.Fill
        rootLayout.Margin = New Padding(0)
        rootLayout.Padding = New Padding(0)
        rootLayout.ColumnCount = 1
        rootLayout.RowCount = 2
        rootLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        rootLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 105.0F))
        rootLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        headerPanel = New Panel()
        headerPanel.Dock = DockStyle.Fill
        headerPanel.Margin = New Padding(0)
        headerPanel.BackColor = DeepNavy

        titleLabel = New Label()
        titleLabel.Text = "ROOM MANAGEMENT"
        titleLabel.ForeColor = Color.White
        titleLabel.Font = New Font("Segoe UI Semibold", 22.0F)
        titleLabel.AutoSize = True
        titleLabel.Location = New Point(35, 18)
        headerPanel.Controls.Add(titleLabel)

        subtitleLabel = New Label()
        subtitleLabel.Text = "Aurelia Grand Hotel • manage rooms, rates and availability"
        subtitleLabel.ForeColor = Color.FromArgb(215, 207, 190)
        subtitleLabel.Font = New Font("Segoe UI", 10.0F)
        subtitleLabel.AutoSize = True
        subtitleLabel.Location = New Point(38, 62)
        headerPanel.Controls.Add(subtitleLabel)

        rootLayout.Controls.Add(headerPanel, 0, 0)

        contentPanel = New Panel()
        contentPanel.Dock = DockStyle.Fill
        contentPanel.Margin = New Padding(0)
        contentPanel.Padding = New Padding(18, 10, 18, 12)
        contentPanel.BackColor = Ivory

        Dim workLayout As New TableLayoutPanel()
        workLayout.Dock = DockStyle.Fill
        workLayout.Margin = New Padding(0)
        workLayout.Padding = New Padding(0)
        workLayout.ColumnCount = 1
        workLayout.RowCount = 2
        workLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        workLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 300.0F))
        workLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        formPanel = New Panel()
        formPanel.Dock = DockStyle.Fill
        formPanel.Margin = New Padding(0)
        formPanel.Padding = New Padding(5)
        formPanel.BackColor = WarmWhite
        formPanel.BorderStyle = BorderStyle.FixedSingle
        formPanel.MinimumSize = New Size(0, 285)

        gridPanel = New Panel()
        gridPanel.Dock = DockStyle.Fill
        gridPanel.Margin = New Padding(0)
        gridPanel.Padding = New Padding(0, 10, 0, 0)
        gridPanel.BackColor = Ivory

        workLayout.Controls.Add(formPanel, 0, 0)
        workLayout.Controls.Add(gridPanel, 0, 1)
        contentPanel.Controls.Add(workLayout)
        rootLayout.Controls.Add(contentPanel, 0, 1)

        Me.Controls.Clear()
        Me.Controls.Add(rootLayout)
        rootLayout.BringToFront()

        BuildRoomFields()
        BuildGrid()

    End Sub


    '===========================================================
    ' ROOM FIELDS
    '===========================================================

    Private Sub BuildRoomFields()

        ' Four dedicated rows: room details, pricing/status, description,
        ' then search and actions. The layout sizes to the editor panel,
        ' rather than placing controls at fixed x/y positions.
        Dim editorLayout As New TableLayoutPanel()
        editorLayout.Dock = DockStyle.Fill
        editorLayout.Margin = New Padding(0)
        editorLayout.Padding = New Padding(4)
        editorLayout.ColumnCount = 4
        editorLayout.RowCount = 4
        editorLayout.GrowStyle = TableLayoutPanelGrowStyle.FixedSize

        For columnIndex As Integer = 0 To 3
            editorLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        Next

        editorLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 60.0F))
        editorLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 60.0F))
        editorLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 92.0F))
        editorLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        roomIDLabel = CreateLabel("Room ID", Point.Empty)
        txtRoomID = New TextBox()
        txtRoomID.ReadOnly = True
        txtRoomID.BackColor = Color.FromArgb(238, 235, 228)
        editorLayout.Controls.Add(CreateRoomFieldCell(roomIDLabel, txtRoomID), 0, 0)

        roomNumberLabel = CreateLabel("Room Number *", Point.Empty)
        txtRoomNumber = New TextBox()
        txtRoomNumber.MaxLength = 20
        txtRoomNumber.TabIndex = 0
        editorLayout.Controls.Add(CreateRoomFieldCell(roomNumberLabel, txtRoomNumber), 1, 0)

        roomTypeLabel = CreateLabel("Room Type *", Point.Empty)
        cboRoomType = New ComboBox()
        cboRoomType.DropDownStyle = ComboBoxStyle.DropDownList
        cboRoomType.Items.AddRange(New Object() {
            "Standard Room", "Deluxe Room", "Superior Room",
            "Executive Room", "Junior Suite", "Presidential Suite"
        })
        editorLayout.Controls.Add(CreateRoomFieldCell(roomTypeLabel, cboRoomType), 2, 0)

        capacityLabel = CreateLabel("Capacity *", Point.Empty)
        nudCapacity = New NumericUpDown()
        nudCapacity.Minimum = 1
        nudCapacity.Maximum = 20
        nudCapacity.Value = 2
        editorLayout.Controls.Add(CreateRoomFieldCell(capacityLabel, nudCapacity), 3, 0)

        rateLabel = CreateLabel("Rate Per Night *", Point.Empty)
        txtRate = New TextBox()
        editorLayout.Controls.Add(CreateRoomFieldCell(rateLabel, txtRate), 0, 1)

        floorLabel = CreateLabel("Floor *", Point.Empty)
        nudFloor = New NumericUpDown()
        nudFloor.Minimum = 1
        nudFloor.Maximum = 100
        nudFloor.Value = 1
        editorLayout.Controls.Add(CreateRoomFieldCell(floorLabel, nudFloor), 1, 1)

        statusFieldLabel = CreateLabel("Room Status *", Point.Empty)
        cboStatus = New ComboBox()
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList
        cboStatus.Items.AddRange(New Object() {"Available", "Reserved", "Occupied", "Maintenance"})
        editorLayout.Controls.Add(CreateRoomFieldCell(statusFieldLabel, cboStatus), 2, 1)

        Dim descriptionCell As New Panel()
        descriptionCell.Dock = DockStyle.Fill
        descriptionCell.Margin = New Padding(4)
        descriptionLabel = CreateLabel("Description", Point.Empty)
        descriptionLabel.Location = New Point(2, 0)
        descriptionLabel.Size = New Size(150, 21)
        descriptionLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        txtDescription = New TextBox()
        txtDescription.Location = New Point(2, 23)
        txtDescription.Size = New Size(100, 60)
        txtDescription.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtDescription.Multiline = True
        txtDescription.ScrollBars = ScrollBars.Vertical
        txtDescription.MaxLength = 500

        descriptionCell.Controls.Add(txtDescription)
        descriptionCell.Controls.Add(descriptionLabel)
        editorLayout.Controls.Add(descriptionCell, 0, 2)
        editorLayout.SetColumnSpan(descriptionCell, 4)

        Dim searchCell As New Panel()
        searchCell.Dock = DockStyle.Fill
        searchCell.Margin = New Padding(4)
        searchLabel = CreateLabel("Search Rooms", Point.Empty)
        searchLabel.Location = New Point(2, 0)
        searchLabel.Size = New Size(180, 21)

        txtSearch = New TextBox()
        txtSearch.Location = New Point(2, 24)
        txtSearch.Size = New Size(100, 28)
        txtSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtSearch.PlaceholderText = "Room number, type or status..."

        searchCell.Controls.Add(txtSearch)
        searchCell.Controls.Add(searchLabel)
        editorLayout.Controls.Add(searchCell, 0, 3)

        Dim actionsPanel As New FlowLayoutPanel()
        actionsPanel.Dock = DockStyle.Fill
        actionsPanel.Margin = New Padding(2)
        actionsPanel.Padding = New Padding(0, 8, 0, 0)
        actionsPanel.FlowDirection = FlowDirection.LeftToRight
        actionsPanel.WrapContents = True
        actionsPanel.AutoScroll = True

        btnAdd = CreateButton("Add Room", Point.Empty, New Size(104, 36), Color.FromArgb(72, 91, 79))
        AddHandler btnAdd.Click, AddressOf AddButton_Click

        btnUpdate = CreateButton("Update", Point.Empty, New Size(90, 36), Navy)
        AddHandler btnUpdate.Click, AddressOf UpdateButton_Click

        btnDelete = CreateButton("Delete", Point.Empty, New Size(90, 36), Color.FromArgb(116, 70, 65))
        AddHandler btnDelete.Click, AddressOf DeleteButton_Click

        btnClear = CreateButton("Clear", Point.Empty, New Size(78, 36), Color.FromArgb(108, 105, 98))
        AddHandler btnClear.Click, AddressOf ClearButton_Click

        btnSearch = CreateButton("Search", Point.Empty, New Size(82, 36), Color.FromArgb(91, 75, 52))
        AddHandler btnSearch.Click, AddressOf SearchButton_Click

        btnRefresh = CreateButton("Refresh", Point.Empty, New Size(86, 36), Color.FromArgb(69, 82, 91))
        AddHandler btnRefresh.Click, AddressOf RefreshButton_Click

        btnClose = CreateButton("Close", Point.Empty, New Size(78, 36), Color.FromArgb(108, 105, 98))
        AddHandler btnClose.Click, AddressOf CloseButton_Click

        actionsPanel.Controls.AddRange(New Control() {
            btnAdd, btnUpdate, btnDelete, btnClear,
            btnSearch, btnRefresh, btnClose
        })

        editorLayout.Controls.Add(actionsPanel, 1, 3)
        editorLayout.SetColumnSpan(actionsPanel, 3)

        formPanel.Controls.Clear()
        formPanel.Controls.Add(editorLayout)

    End Sub


    Private Function CreateRoomFieldCell(
        fieldLabel As Label,
        inputControl As Control
    ) As Panel

        Dim cell As New Panel()
        cell.Dock = DockStyle.Fill
        cell.Margin = New Padding(4)

        fieldLabel.Location = New Point(2, 0)
        fieldLabel.Size = New Size(150, 21)
        fieldLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        fieldLabel.AutoSize = False

        inputControl.Location = New Point(2, 24)
        inputControl.Size = New Size(100, 28)
        inputControl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        inputControl.Margin = New Padding(0)

        cell.Controls.Add(inputControl)
        cell.Controls.Add(fieldLabel)

        Return cell

    End Function


    '===========================================================
    ' CREATE LABEL
    '===========================================================

    Private Function CreateLabel(
        text As String,
        location As Point
    ) As Label

        Dim label As New Label()

        label.Text =
            text

        label.Location =
            location

        label.AutoSize =
            True

        label.ForeColor =
            Charcoal

        label.Font =
            New Font(
                "Segoe UI Semibold",
                9.5F
            )

        Return label

    End Function


    '===========================================================
    ' CREATE BUTTON
    '===========================================================

    Private Function CreateButton(
        text As String,
        location As Point,
        size As Size,
        color As Color
    ) As Button

        Dim button As New Button()

        button.Text =
            text

        button.Location =
            location

        button.Size =
            size

        button.BackColor =
            color

        button.ForeColor =
            Color.White

        button.FlatStyle =
            FlatStyle.Flat

        button.FlatAppearance.BorderSize =
            0

        button.Font =
            New Font(
                "Segoe UI Semibold",
                9.0F
            )

        button.Cursor =
            Cursors.Hand

        Return button

    End Function


    '===========================================================
    ' LOAD ROOMS
    '===========================================================

    Private Sub LoadRooms()

        Try

            Dim table As New DataTable()

            Dim sql As String =
                "SELECT " &
                "RoomID, " &
                "RoomNumber, " &
                "RoomType, " &
                "Capacity, " &
                "RatePerNight, " &
                "Status, " &
                "FloorNumber, " &
                "Description " &
                "FROM Rooms " &
                "ORDER BY FloorNumber, RoomNumber"

            Using connection =
                DatabaseHelper.GetConnection()

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        adapter.Fill(table)

                    End Using

                End Using

            End Using

            dgvRooms.DataSource =
                table

            FormatGrid()

            If dgvRooms.Rows.Count > 0 Then
                dgvRooms.FirstDisplayedScrollingRowIndex = 0
            End If

        Catch ex As Exception

            MessageBox.Show(
                "Unable to load rooms." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '===========================================================
    ' BUILD GRID
    '===========================================================

    Private Sub BuildGrid()

        dgvRooms = New DataGridView()

        dgvRooms.Dock =
            DockStyle.Fill

        dgvRooms.StandardTab = True

        dgvRooms.BackgroundColor =
            WarmWhite

        dgvRooms.BorderStyle =
            BorderStyle.None

        dgvRooms.AllowUserToAddRows =
            False

        dgvRooms.AllowUserToDeleteRows =
            False

        dgvRooms.ReadOnly =
            True

        dgvRooms.MultiSelect =
            False

        dgvRooms.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvRooms.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvRooms.RowHeadersVisible =
            False

        dgvRooms.EnableHeadersVisualStyles =
            False

        dgvRooms.ColumnHeadersDefaultCellStyle.BackColor =
            Navy

        dgvRooms.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        dgvRooms.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI Semibold",
                10.0F
            )

        dgvRooms.ColumnHeadersHeight =
            42

        dgvRooms.DefaultCellStyle.BackColor =
            WarmWhite

        dgvRooms.DefaultCellStyle.ForeColor =
            Charcoal

        dgvRooms.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(
                231,
                222,
                204
            )

        dgvRooms.DefaultCellStyle.SelectionForeColor =
            Charcoal

        dgvRooms.AlternatingRowsDefaultCellStyle.BackColor =
            Color.FromArgb(
                248,
                246,
                241
            )

        dgvRooms.RowTemplate.Height =
            35

        dgvRooms.ScrollBars =
            ScrollBars.Both

        dgvRooms.AutoSizeRowsMode =
            DataGridViewAutoSizeRowsMode.None

        dgvRooms.AllowUserToResizeRows =
            False

        AddHandler dgvRooms.CellClick,
            AddressOf dgvRooms_CellClick

        gridPanel.Controls.Add(
            dgvRooms
        )

    End Sub


    '===========================================================
    ' FORMAT GRID
    '===========================================================

    Private Sub FormatGrid()

        If dgvRooms.Columns.Count = 0 Then
            Return
        End If


        If dgvRooms.Columns.Contains(
            "RoomID"
        ) Then

            dgvRooms.Columns(
                "RoomID"
            ).HeaderText =
                "Room ID"

        End If


        If dgvRooms.Columns.Contains(
            "RoomNumber"
        ) Then

            dgvRooms.Columns(
                "RoomNumber"
            ).HeaderText =
                "Room Number"

        End If


        If dgvRooms.Columns.Contains(
            "RoomType"
        ) Then

            dgvRooms.Columns(
                "RoomType"
            ).HeaderText =
                "Room Type"

        End If


        If dgvRooms.Columns.Contains(
            "RatePerNight"
        ) Then

            dgvRooms.Columns(
                "RatePerNight"
            ).HeaderText =
                "Rate / Night"

            dgvRooms.Columns(
                "RatePerNight"
            ).DefaultCellStyle.Format =
                "₱#,##0.00"

        End If


        If dgvRooms.Columns.Contains(
            "FloorNumber"
        ) Then

            dgvRooms.Columns(
                "FloorNumber"
            ).HeaderText =
                "Floor"

        End If


        If dgvRooms.Columns.Contains(
            "Description"
        ) Then

            dgvRooms.Columns(
                "Description"
            ).FillWeight =
                180

        End If


        If dgvRooms.Columns.Contains(
            "Status"
        ) Then

            dgvRooms.Columns(
                "Status"
            ).FillWeight =
                90

        End If

    End Sub


    '===========================================================
    ' ADD ROOM
    '===========================================================

    Private Sub AddButton_Click(
        sender As Object,
        e As EventArgs
    )

        If Not ValidateRoomFields() Then
            Return
        End If


        If RoomNumberExists(
            txtRoomNumber.Text.Trim(),
            0
        ) Then

            MessageBox.Show(
                "That room number already exists.",
                "Duplicate Room",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        Try

            Dim sql As String =
                "INSERT INTO Rooms " &
                "(RoomNumber, RoomType, Capacity, BedType, " &
                "RatePerNight, Status, FloorNumber, Description) " &
                "VALUES " &
                "(@RoomNumber, @RoomType, @Capacity, @BedType, " &
                "@RatePerNight, @Status, @FloorNumber, @Description)"

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddRoomParameters(command)

                    command.ExecuteNonQuery()
                    AuditLogger.Log("Changed room data", "A room record was created, updated, or otherwise changed.")

                End Using

            End Using


            MessageBox.Show(
                "Room added successfully.",
                "Room Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadRooms()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to add the room." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '===========================================================
    ' UPDATE ROOM
    '===========================================================

    Private Sub UpdateButton_Click(
        sender As Object,
        e As EventArgs
    )

        If selectedRoomID <= 0 Then

            MessageBox.Show(
                "Please select a room first.",
                "Update Room",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        If Not ValidateRoomFields() Then
            Return
        End If


        If RoomNumberExists(
            txtRoomNumber.Text.Trim(),
            selectedRoomID
        ) Then

            MessageBox.Show(
                "That room number is already assigned to another room.",
                "Duplicate Room",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        Try

            Dim sql As String =
                "UPDATE Rooms SET " &
                "RoomNumber = @RoomNumber, " &
                "RoomType = @RoomType, " &
                "Capacity = @Capacity, " &
                "BedType = @BedType, " &
                "RatePerNight = @RatePerNight, " &
                "Status = @Status, " &
                "FloorNumber = @FloorNumber, " &
                "Description = @Description " &
                "WHERE RoomID = @RoomID"

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddRoomParameters(command)

                    command.Parameters.AddWithValue(
                        "@RoomID",
                        selectedRoomID
                    )

                    command.ExecuteNonQuery()
                    AuditLogger.Log("Changed room data", "A room record was created, updated, or otherwise changed.")

                End Using

            End Using


            MessageBox.Show(
                "Room updated successfully.",
                "Room Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadRooms()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to update the room." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '===========================================================
    ' DELETE ROOM
    '===========================================================

    Private Sub DeleteButton_Click(
        sender As Object,
        e As EventArgs
    )

        If selectedRoomID <= 0 Then

            MessageBox.Show(
                "Please select a room first.",
                "Delete Room",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        If RoomHasReservations(
            selectedRoomID
        ) Then

            MessageBox.Show(
                "This room cannot be deleted because it has reservation records." &
                Environment.NewLine &
                Environment.NewLine &
                "You can change the room status to Maintenance instead.",
                "Cannot Delete Room",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        Dim result As DialogResult =
            MessageBox.Show(
                "Are you sure you want to delete Room " &
                txtRoomNumber.Text &
                "?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )


        If result <> DialogResult.Yes Then
            Return
        End If


        Try

            Dim sql As String =
                "DELETE FROM Rooms " &
                "WHERE RoomID = @RoomID"

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@RoomID",
                        selectedRoomID
                    )

                    command.ExecuteNonQuery()
                    AuditLogger.Log("Changed room data", "A room record was created, updated, or otherwise changed.")

                End Using

            End Using


            MessageBox.Show(
                "Room deleted successfully.",
                "Room Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadRooms()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to delete the room." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '===========================================================
    ' ADD ROOM PARAMETERS
    '===========================================================

    Private Function GetBedTypeForRoomType() As String

        Select Case cboRoomType.Text
            Case "Standard Room"
                Return "Queen Bed"
            Case "Deluxe Room", "Superior Room", "Executive Room"
                Return "King Bed"
            Case "Junior Suite"
                Return "King Bed + Sofa"
            Case "Presidential Suite"
                Return "King Bed + Living Area"
            Case Else
                Return "Queen Bed"
        End Select

    End Function


    Private Sub AddRoomParameters(
        command As SqlCommand
    )

        command.Parameters.AddWithValue(
            "@RoomNumber",
            txtRoomNumber.Text.Trim()
        )

        command.Parameters.AddWithValue(
            "@RoomType",
            cboRoomType.Text
        )

        command.Parameters.AddWithValue(
            "@Capacity",
            Convert.ToInt32(
                nudCapacity.Value
            )
        )

        command.Parameters.AddWithValue(
            "@BedType",
            GetBedTypeForRoomType()
        )


        Dim rate As Decimal

        Decimal.TryParse(
            txtRate.Text.Trim(),
            rate
        )


        command.Parameters.AddWithValue(
            "@RatePerNight",
            rate
        )


        command.Parameters.AddWithValue(
            "@Status",
            cboStatus.Text
        )


        command.Parameters.AddWithValue(
            "@FloorNumber",
            Convert.ToInt32(
                nudFloor.Value
            )
        )


        If String.IsNullOrWhiteSpace(
            txtDescription.Text
        ) Then

            command.Parameters.AddWithValue(
                "@Description",
                DBNull.Value
            )

        Else

            command.Parameters.AddWithValue(
                "@Description",
                txtDescription.Text.Trim()
            )

        End If

    End Sub


    '===========================================================
    ' VALIDATE ROOM
    '===========================================================

    Private Function ValidateRoomFields() As Boolean

        If String.IsNullOrWhiteSpace(
            txtRoomNumber.Text
        ) Then

            MessageBox.Show(
                "Please enter a room number.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtRoomNumber.Focus()

            Return False

        End If


        If cboRoomType.SelectedIndex < 0 Then

            MessageBox.Show(
                "Please select a room type.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboRoomType.Focus()

            Return False

        End If


        If nudCapacity.Value <= 0 Then

            MessageBox.Show(
                "Room capacity must be greater than zero.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            nudCapacity.Focus()

            Return False

        End If


        Dim rate As Decimal


        If Not Decimal.TryParse(
            txtRate.Text.Trim(),
            rate
        ) Then

            MessageBox.Show(
                "Please enter a valid room rate.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtRate.Focus()

            Return False

        End If


        If rate <= 0 Then

            MessageBox.Show(
                "Room rate must be greater than zero.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtRate.Focus()

            Return False

        End If


        If cboStatus.SelectedIndex < 0 Then

            MessageBox.Show(
                "Please select a room status.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboStatus.Focus()

            Return False

        End If


        Return True

    End Function


    '===========================================================
    ' CHECK DUPLICATE ROOM NUMBER
    '===========================================================

    Private Function RoomNumberExists(
        roomNumber As String,
        roomID As Integer
    ) As Boolean

        Try

            Dim sql As String =
                "SELECT COUNT(*) " &
                "FROM Rooms " &
                "WHERE RoomNumber = @RoomNumber " &
                "AND RoomID <> @RoomID"

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@RoomNumber",
                        roomNumber
                    )

                    command.Parameters.AddWithValue(
                        "@RoomID",
                        roomID
                    )

                    Return Convert.ToInt32(
                        command.ExecuteScalar()
                    ) > 0

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Unable to check the room number." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

            Return True

        End Try

    End Function


    '===========================================================
    ' CHECK ROOM RESERVATIONS
    '===========================================================

    Private Function RoomHasReservations(
        roomID As Integer
    ) As Boolean

        Try

            Dim sql As String =
                "SELECT COUNT(*) " &
                "FROM Reservations " &
                "WHERE RoomID = @RoomID"

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@RoomID",
                        roomID
                    )

                    Return Convert.ToInt32(
                        command.ExecuteScalar()
                    ) > 0

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Unable to check room reservations." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

            Return True

        End Try

    End Function


    '===========================================================
    ' SEARCH
    '===========================================================

    Private Sub SearchButton_Click(
        sender As Object,
        e As EventArgs
    )

        SearchRooms()

    End Sub


    Private Sub SearchRooms()

        Dim searchText As String =
            txtSearch.Text.Trim()


        If String.IsNullOrWhiteSpace(
            searchText
        ) Then

            LoadRooms()

            Return

        End If


        Try

            Dim table As New DataTable()


            Dim sql As String =
                "SELECT " &
                "RoomID, " &
                "RoomNumber, " &
                "RoomType, " &
                "Capacity, " &
                "RatePerNight, " &
                "Status, " &
                "FloorNumber, " &
                "Description " &
                "FROM Rooms " &
                "WHERE " &
                "RoomNumber LIKE @Search " &
                "OR RoomType LIKE @Search " &
                "OR Status LIKE @Search " &
                "OR Description LIKE @Search " &
                "ORDER BY FloorNumber, RoomNumber"


            Using connection =
                DatabaseHelper.GetConnection()

                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@Search",
                        "%" &
                        searchText &
                        "%"
                    )


                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        adapter.Fill(table)

                    End Using

                End Using

            End Using


            dgvRooms.DataSource =
                table

            FormatGrid()

            If dgvRooms.Rows.Count > 0 Then
                dgvRooms.FirstDisplayedScrollingRowIndex = 0
            End If


        Catch ex As Exception

            MessageBox.Show(
                "Unable to search rooms." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Search Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '===========================================================
    ' GRID CLICK
    '===========================================================

    Private Sub dgvRooms_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then
            Return
        End If


        Dim row As DataGridViewRow =
            dgvRooms.Rows(e.RowIndex)


        If row.Cells(
            "RoomID"
        ).Value Is Nothing Then

            Return

        End If


        selectedRoomID =
            Convert.ToInt32(
                row.Cells(
                    "RoomID"
                ).Value
            )


        txtRoomID.Text =
            selectedRoomID.ToString()


        txtRoomNumber.Text =
            Convert.ToString(
                row.Cells(
                    "RoomNumber"
                ).Value
            )


        cboRoomType.Text =
            Convert.ToString(
                row.Cells(
                    "RoomType"
                ).Value
            )


        nudCapacity.Value =
            Convert.ToDecimal(
                row.Cells(
                    "Capacity"
                ).Value
            )


        txtRate.Text =
            Convert.ToDecimal(
                row.Cells(
                    "RatePerNight"
                ).Value
            ).ToString(
                "0.00"
            )


        cboStatus.Text =
            Convert.ToString(
                row.Cells(
                    "Status"
                ).Value
            )


        nudFloor.Value =
            Convert.ToDecimal(
                row.Cells(
                    "FloorNumber"
                ).Value
            )


        If row.Cells(
            "Description"
        ).Value Is Nothing OrElse
           IsDBNull(
               row.Cells(
                   "Description"
               ).Value
           ) Then

            txtDescription.Text = ""

        Else

            txtDescription.Text =
                Convert.ToString(
                    row.Cells(
                        "Description"
                    ).Value
                )

        End If

    End Sub


    '===========================================================
    ' CLEAR
    '===========================================================

    Private Sub ClearButton_Click(
        sender As Object,
        e As EventArgs
    )

        ClearFields()

    End Sub


    Private Sub ClearFields()

        selectedRoomID =
            0

        txtRoomID.Text =
            ""

        txtRoomNumber.Text =
            ""

        txtDescription.Text =
            ""

        txtRate.Text =
            ""

        nudCapacity.Value =
            2

        nudFloor.Value =
            1


        If cboRoomType.Items.Count > 0 Then

            cboRoomType.SelectedIndex =
                0

        End If


        If cboStatus.Items.Count > 0 Then

            cboStatus.SelectedIndex =
                0

        End If


        If txtSearch IsNot Nothing Then

            txtSearch.Text =
                ""

        End If


        If dgvRooms IsNot Nothing Then

            dgvRooms.ClearSelection()

        End If


        ' Do not force focus/scroll during initial form construction.

    End Sub


    '===========================================================
    ' REFRESH
    '===========================================================

    Private Sub RefreshButton_Click(
        sender As Object,
        e As EventArgs
    )

        LoadRooms()

        ClearFields()

    End Sub


    '===========================================================
    ' CLOSE
    '===========================================================

    Private Sub CloseButton_Click(
        sender As Object,
        e As EventArgs
    )

        Me.Close()

    End Sub

End Class
