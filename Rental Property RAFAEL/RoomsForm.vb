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

        '-------------------------------------------------------
        ' HEADER
        '-------------------------------------------------------

        headerPanel = New Panel()

        headerPanel.Dock =
            DockStyle.Top

        headerPanel.Height =
            105

        headerPanel.BackColor =
            DeepNavy

        Me.Controls.Add(
            headerPanel
        )


        titleLabel = New Label()

        titleLabel.Text =
            "ROOM MANAGEMENT"

        titleLabel.ForeColor =
            Color.White

        titleLabel.Font =
            New Font(
                "Segoe UI Semibold",
                22.0F
            )

        titleLabel.AutoSize =
            True

        titleLabel.Location =
            New Point(35, 18)

        headerPanel.Controls.Add(
            titleLabel
        )


        subtitleLabel = New Label()

        subtitleLabel.Text =
            "Aurelia Grand Hotel • manage rooms, rates and availability"

        subtitleLabel.ForeColor =
            Color.FromArgb(
                215,
                207,
                190
            )

        subtitleLabel.Font =
            New Font(
                "Segoe UI",
                10.0F
            )

        subtitleLabel.AutoSize =
            True

        subtitleLabel.Location =
            New Point(38, 62)

        headerPanel.Controls.Add(
            subtitleLabel
        )


        '-------------------------------------------------------
        ' CONTENT
        '-------------------------------------------------------

        contentPanel = New Panel()

        contentPanel.Dock =
            DockStyle.Fill

        contentPanel.BackColor =
            Ivory

        contentPanel.Padding =
            New Padding(25)

        Me.Controls.Add(
            contentPanel
        )


        '-------------------------------------------------------
        ' ROOM EDITOR IS CREATED AFTER THE GRID BELOW
        '-------------------------------------------------------

        '-------------------------------------------------------
        ' GRID PANEL
        ' Add the fill-docked grid FIRST. WinForms docking follows
        ' control z-order; adding it after the top panel can cause
        ' the grid to occupy/cover the room editor area.
        '-------------------------------------------------------

        gridPanel = New Panel()

        gridPanel.Dock =
            DockStyle.Fill

        gridPanel.BackColor =
            Ivory

        gridPanel.Padding =
            New Padding(
                0,
                15,
                0,
                0
            )

        contentPanel.Controls.Add(
            gridPanel
        )

        BuildGrid()


        '-------------------------------------------------------
        ' ROOM EDITOR
        ' Add the top-docked editor LAST so it reserves its own
        ' area above the grid instead of being overlapped.
        '-------------------------------------------------------

        formPanel = New Panel()

        formPanel.Dock =
            DockStyle.Top

        formPanel.Height =
            285

        formPanel.BackColor =
            WarmWhite

        formPanel.BorderStyle =
            BorderStyle.FixedSingle

        formPanel.AutoScroll = False
        formPanel.Padding = New Padding(4)

        contentPanel.Controls.Add(
            formPanel
        )

        contentPanel.Controls.SetChildIndex(formPanel, 0)
        formPanel.BringToFront()
        formPanel.Visible = True

        BuildRoomFields()

    End Sub


    '===========================================================
    ' ROOM FIELDS
    '===========================================================

    Private Sub BuildRoomFields()

        ' Use a table-based layout instead of fixed pixel coordinates.
        ' Every input gets its own cell, so later controls cannot cover it.
        Dim editorLayout As New TableLayoutPanel()

        editorLayout.Dock = DockStyle.Fill
        editorLayout.Margin = New Padding(0)
        editorLayout.Padding = New Padding(8)
        editorLayout.ColumnCount = 4
        editorLayout.RowCount = 4
        editorLayout.GrowStyle = TableLayoutPanelGrowStyle.FixedSize

        For i As Integer = 0 To 3
            editorLayout.ColumnStyles.Add(
                New ColumnStyle(SizeType.Percent, 25.0F)
            )
        Next

        editorLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 64.0F))
        editorLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 64.0F))
        editorLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        editorLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F))

        ' Room ID
        roomIDLabel = CreateLabel("Room ID", Point.Empty)
        txtRoomID = New TextBox()
        txtRoomID.ReadOnly = True
        txtRoomID.BackColor = Color.FromArgb(238, 235, 228)
        txtRoomID.Dock = DockStyle.Fill
        editorLayout.Controls.Add(CreateRoomFieldCell(roomIDLabel, txtRoomID), 0, 0)

        ' Room number
        roomNumberLabel = CreateLabel("Room Number *", Point.Empty)
        txtRoomNumber = New TextBox()
        txtRoomNumber.MaxLength = 20
        txtRoomNumber.Dock = DockStyle.Fill
        txtRoomNumber.TabIndex = 0
        editorLayout.Controls.Add(CreateRoomFieldCell(roomNumberLabel, txtRoomNumber), 1, 0)

        ' Room type
        roomTypeLabel = CreateLabel("Room Type *", Point.Empty)
        cboRoomType = New ComboBox()
        cboRoomType.DropDownStyle = ComboBoxStyle.DropDownList
        cboRoomType.Dock = DockStyle.Fill
        cboRoomType.Items.AddRange(New Object() {
            "Standard Room",
            "Deluxe Room",
            "Superior Room",
            "Executive Room",
            "Junior Suite",
            "Presidential Suite"
        })
        editorLayout.Controls.Add(CreateRoomFieldCell(roomTypeLabel, cboRoomType), 2, 0)

        ' Capacity
        capacityLabel = CreateLabel("Capacity *", Point.Empty)
        nudCapacity = New NumericUpDown()
        nudCapacity.Minimum = 1
        nudCapacity.Maximum = 20
        nudCapacity.Value = 2
        nudCapacity.Dock = DockStyle.Fill
        editorLayout.Controls.Add(CreateRoomFieldCell(capacityLabel, nudCapacity), 3, 0)

        ' Rate per night
        rateLabel = CreateLabel("Rate Per Night *", Point.Empty)
        txtRate = New TextBox()
        txtRate.Dock = DockStyle.Fill
        editorLayout.Controls.Add(CreateRoomFieldCell(rateLabel, txtRate), 0, 1)

        ' Floor number
        floorLabel = CreateLabel("Floor *", Point.Empty)
        nudFloor = New NumericUpDown()
        nudFloor.Minimum = 1
        nudFloor.Maximum = 100
        nudFloor.Value = 1
        nudFloor.Dock = DockStyle.Fill
        editorLayout.Controls.Add(CreateRoomFieldCell(floorLabel, nudFloor), 1, 1)

        ' Room status
        statusFieldLabel = CreateLabel("Room Status *", Point.Empty)
        cboStatus = New ComboBox()
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList
        cboStatus.Dock = DockStyle.Fill
        cboStatus.Items.AddRange(New Object() {
            "Available",
            "Reserved",
            "Occupied",
            "Maintenance"
        })
        editorLayout.Controls.Add(CreateRoomFieldCell(statusFieldLabel, cboStatus), 2, 1)

        ' Description spans the full editor width.
        Dim descriptionCell As New TableLayoutPanel()
        descriptionCell.Dock = DockStyle.Fill
        descriptionCell.Margin = New Padding(4)
        descriptionCell.Padding = New Padding(3, 0, 3, 2)
        descriptionCell.ColumnCount = 1
        descriptionCell.RowCount = 2
        descriptionCell.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        descriptionCell.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        descriptionCell.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        descriptionLabel = CreateLabel("Description", Point.Empty)
        descriptionLabel.Dock = DockStyle.Fill

        txtDescription = New TextBox()
        txtDescription.Dock = DockStyle.Fill
        txtDescription.Multiline = True
        txtDescription.ScrollBars = ScrollBars.Vertical
        txtDescription.MaxLength = 500

        descriptionCell.Controls.Add(descriptionLabel, 0, 0)
        descriptionCell.Controls.Add(txtDescription, 0, 1)
        editorLayout.Controls.Add(descriptionCell, 0, 2)
        editorLayout.SetColumnSpan(descriptionCell, 4)

        ' Search cell
        Dim searchCell As New TableLayoutPanel()
        searchCell.Dock = DockStyle.Fill
        searchCell.Margin = New Padding(4)
        searchCell.Padding = New Padding(3, 0, 3, 2)
        searchCell.ColumnCount = 1
        searchCell.RowCount = 2
        searchCell.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        searchCell.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        searchCell.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        searchLabel = CreateLabel("Search Rooms", Point.Empty)
        searchLabel.Dock = DockStyle.Fill

        txtSearch = New TextBox()
        txtSearch.Dock = DockStyle.Fill
        txtSearch.PlaceholderText = "Room number, type or status..."

        searchCell.Controls.Add(searchLabel, 0, 0)
        searchCell.Controls.Add(txtSearch, 0, 1)
        editorLayout.Controls.Add(searchCell, 0, 3)

        ' Actions occupy the remaining three columns and wrap on smaller windows.
        Dim actionsPanel As New FlowLayoutPanel()
        actionsPanel.Dock = DockStyle.Fill
        actionsPanel.Margin = New Padding(2)
        actionsPanel.Padding = New Padding(0, 7, 0, 0)
        actionsPanel.FlowDirection = FlowDirection.LeftToRight
        actionsPanel.WrapContents = True
        actionsPanel.AutoScroll = True

        btnAdd = CreateButton("Add Room", Point.Empty, New Size(110, 38), Color.FromArgb(72, 91, 79))
        AddHandler btnAdd.Click, AddressOf AddButton_Click

        btnUpdate = CreateButton("Update", Point.Empty, New Size(100, 38), Navy)
        AddHandler btnUpdate.Click, AddressOf UpdateButton_Click

        btnDelete = CreateButton("Delete", Point.Empty, New Size(100, 38), Color.FromArgb(116, 70, 65))
        AddHandler btnDelete.Click, AddressOf DeleteButton_Click

        btnClear = CreateButton("Clear", Point.Empty, New Size(90, 38), Color.FromArgb(108, 105, 98))
        AddHandler btnClear.Click, AddressOf ClearButton_Click

        btnSearch = CreateButton("Search", Point.Empty, New Size(95, 38), Color.FromArgb(91, 75, 52))
        AddHandler btnSearch.Click, AddressOf SearchButton_Click

        btnRefresh = CreateButton("Refresh", Point.Empty, New Size(95, 38), Color.FromArgb(69, 82, 91))
        AddHandler btnRefresh.Click, AddressOf RefreshButton_Click

        btnClose = CreateButton("Close", Point.Empty, New Size(90, 38), Color.FromArgb(108, 105, 98))
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
    ) As TableLayoutPanel

        Dim cell As New TableLayoutPanel()
        cell.Dock = DockStyle.Fill
        cell.Margin = New Padding(4)
        cell.Padding = New Padding(3, 0, 3, 2)
        cell.ColumnCount = 1
        cell.RowCount = 2
        cell.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        cell.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        cell.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        fieldLabel.Dock = DockStyle.Fill
        fieldLabel.Margin = New Padding(0)
        inputControl.Dock = DockStyle.Fill
        inputControl.Margin = New Padding(0, 1, 0, 0)

        cell.Controls.Add(fieldLabel, 0, 0)
        cell.Controls.Add(inputControl, 0, 1)

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
                "(RoomNumber, RoomType, Capacity, " &
                "RatePerNight, Status, FloorNumber, Description) " &
                "VALUES " &
                "(@RoomNumber, @RoomType, @Capacity, " &
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
