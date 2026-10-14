Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Text
Imports Microsoft.Data.SqlClient
Imports System.Windows.Forms

Public Class ReportsForm
    Inherits Form

    '===========================================================
    ' COLORS - AURELIA GRAND HOTEL
    '===========================================================

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

    '===========================================================
    ' FORM CONTROLS
    '===========================================================

    Private headerPanel As Panel
    Private contentPanel As Panel

    Private titleLabel As Label
    Private subtitleLabel As Label

    Private summaryPanel As Panel

    Private totalRoomsLabel As Label
    Private availableRoomsLabel As Label
    Private occupiedRoomsLabel As Label
    Private reservedRoomsLabel As Label
    Private totalReservationsLabel As Label
    Private totalRevenueLabel As Label

    Private reportTypeLabel As Label
    Private reportTypeComboBox As ComboBox

    Private refreshButton As Button
    Private exportButton As Button
    Private closeButton As Button

    Private reportGrid As DataGridView

    Private statusLabel As Label

    '===========================================================
    ' CONSTRUCTOR
    '===========================================================

    Public Sub New()

        Me.Text = "Reports | Aurelia Grand Hotel"
        Me.StartPosition = FormStartPosition.CenterScreen

        Me.Size =
            New Size(1280, 760)

        Me.MinimumSize =
            New Size(1100, 650)

        Me.BackColor = Ivory

        Me.Font =
            New Font("Segoe UI", 10.0F)

        BuildInterface()

        AddHandler Me.Resize,
            AddressOf ReportsForm_Resize

        LoadSummary()
        LoadReport()

    End Sub

    '===========================================================
    ' BUILD INTERFACE
    '===========================================================

    Private Sub BuildInterface()

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

        Me.Controls.Add(headerPanel)


        titleLabel = New Label()

        titleLabel.Text =
            "REPORTS & ANALYTICS"

        titleLabel.ForeColor =
            Color.White

        titleLabel.Font =
            New Font(
                "Segoe UI Semibold",
                22.0F
            )

        titleLabel.AutoSize = True

        titleLabel.Location =
            New Point(35, 18)

        headerPanel.Controls.Add(titleLabel)


        subtitleLabel = New Label()

        subtitleLabel.Text =
            "Aurelia Grand Hotel • reservations • occupancy • guests • payments"

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

        subtitleLabel.AutoSize = True

        subtitleLabel.Location =
            New Point(38, 62)

        headerPanel.Controls.Add(subtitleLabel)


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

        Me.Controls.Add(contentPanel)


        '-------------------------------------------------------
        ' SUMMARY
        '-------------------------------------------------------

        summaryPanel = New Panel()

        summaryPanel.Dock =
            DockStyle.Top

        summaryPanel.Height =
            140

        summaryPanel.BackColor =
            Ivory

        contentPanel.Controls.Add(summaryPanel)


        CreateSummaryCard(
            summaryPanel,
            "TOTAL ROOMS",
            "0",
            Navy,
            0,
            AddressOf SetTotalRoomsLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "AVAILABLE",
            "0",
            Color.FromArgb(64, 83, 78),
            195,
            AddressOf SetAvailableRoomsLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "OCCUPIED",
            "0",
            Color.FromArgb(88, 72, 58),
            390,
            AddressOf SetOccupiedRoomsLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "RESERVED",
            "0",
            Color.FromArgb(103, 86, 61),
            585,
            AddressOf SetReservedRoomsLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "RESERVATIONS",
            "0",
            Color.FromArgb(73, 67, 59),
            780,
            AddressOf SetTotalReservationsLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "REVENUE",
            "₱0.00",
            Color.FromArgb(116, 91, 54),
            975,
            AddressOf SetTotalRevenueLabel
        )


        '-------------------------------------------------------
        ' TOOLBAR
        '-------------------------------------------------------

        Dim toolbarPanel As New Panel()

        toolbarPanel.Dock =
            DockStyle.Top

        toolbarPanel.Height =
            65

        toolbarPanel.BackColor =
            Ivory

        contentPanel.Controls.Add(toolbarPanel)


        reportTypeLabel = New Label()

        reportTypeLabel.Text =
            "REPORT:"

        reportTypeLabel.Font =
            New Font(
                "Segoe UI Semibold",
                10.0F
            )

        reportTypeLabel.ForeColor =
            Charcoal

        reportTypeLabel.AutoSize = True

        reportTypeLabel.Location =
            New Point(5, 22)

        toolbarPanel.Controls.Add(reportTypeLabel)


        reportTypeComboBox = New ComboBox()

        reportTypeComboBox.DropDownStyle =
            ComboBoxStyle.DropDownList

        reportTypeComboBox.Font =
            New Font(
                "Segoe UI",
                10.0F
            )

        reportTypeComboBox.Width =
            250

        reportTypeComboBox.Location =
            New Point(75, 17)

        reportTypeComboBox.Items.Add(
            "Reservation Report"
        )

        reportTypeComboBox.Items.Add(
            "Guest Report"
        )

        reportTypeComboBox.Items.Add(
            "Payment Report"
        )

        reportTypeComboBox.Items.Add(
            "Room Report"
        )

        reportTypeComboBox.Items.Add(
            "Occupancy Report"
        )

        reportTypeComboBox.Items.Add(
            "Today's Check-Ins"
        )

        reportTypeComboBox.Items.Add(
            "Today's Check-Outs"
        )

        reportTypeComboBox.Items.Add(
            "Revenue Report"
        )

        reportTypeComboBox.SelectedIndex =
            0

        AddHandler reportTypeComboBox.SelectedIndexChanged,
            AddressOf ReportTypeComboBox_SelectedIndexChanged

        toolbarPanel.Controls.Add(
            reportTypeComboBox
        )


        refreshButton = CreateButton(
            "↻  Refresh",
            New Point(345, 15),
            New Size(120, 38),
            Navy
        )

        AddHandler refreshButton.Click,
            AddressOf RefreshButton_Click

        toolbarPanel.Controls.Add(
            refreshButton
        )


        exportButton = CreateButton(
            "▣  Export CSV",
            New Point(475, 15),
            New Size(135, 38),
            Color.FromArgb(91, 75, 52)
        )

        AddHandler exportButton.Click,
            AddressOf ExportButton_Click

        toolbarPanel.Controls.Add(
            exportButton
        )


        closeButton = CreateButton(
            "Close",
            New Point(620, 15),
            New Size(100, 38),
            Color.FromArgb(108, 105, 98)
        )

        AddHandler closeButton.Click,
            AddressOf CloseButton_Click

        toolbarPanel.Controls.Add(
            closeButton
        )


        '-------------------------------------------------------
        ' DATA GRID
        '-------------------------------------------------------

        reportGrid = New DataGridView()

        reportGrid.Dock =
            DockStyle.Fill

        reportGrid.BackgroundColor =
            WarmWhite

        reportGrid.BorderStyle =
            BorderStyle.None

        reportGrid.AllowUserToAddRows =
            False

        reportGrid.AllowUserToDeleteRows =
            False

        reportGrid.ReadOnly =
            True

        reportGrid.MultiSelect =
            False

        reportGrid.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        'Keep the grid inside the space left after the summary, toolbar,
        'and status bar. Let the user scroll horizontally for wide reports.
        reportGrid.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.None

        reportGrid.AutoSizeRowsMode =
            DataGridViewAutoSizeRowsMode.None

        reportGrid.ScrollBars =
            ScrollBars.Both

        reportGrid.RowHeadersVisible =
            False

        reportGrid.EnableHeadersVisualStyles =
            False

        reportGrid.ColumnHeadersDefaultCellStyle.BackColor =
            Navy

        reportGrid.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        reportGrid.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI Semibold",
                10.0F
            )

        reportGrid.ColumnHeadersHeight =
            42

        reportGrid.DefaultCellStyle.BackColor =
            WarmWhite

        reportGrid.DefaultCellStyle.ForeColor =
            Charcoal

        reportGrid.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(
                231,
                222,
                204
            )

        reportGrid.DefaultCellStyle.SelectionForeColor =
            Charcoal

        reportGrid.AlternatingRowsDefaultCellStyle.BackColor =
            Color.FromArgb(
                248,
                246,
                241
            )

        reportGrid.RowTemplate.Height =
            32

        reportGrid.ColumnHeadersHeightSizeMode =
            DataGridViewColumnHeadersHeightSizeMode.DisableResizing

        contentPanel.Controls.Add(
            reportGrid
        )

        'Docked controls are laid out according to z-order in WinForms.
        'Explicitly establish the intended vertical stack so the grid cannot
        'cover the report selector or summary cards.
        statusLabel?.BringToFront()
        reportGrid.BringToFront()
        toolbarPanel.BringToFront()
        summaryPanel.BringToFront()

        AddHandler reportGrid.DataBindingComplete,
            Sub(sender As Object, e As DataGridViewBindingCompleteEventArgs)
                FormatGrid()
            End Sub


        '-------------------------------------------------------
        ' STATUS
        '-------------------------------------------------------

        statusLabel = New Label()

        statusLabel.Dock =
            DockStyle.Bottom

        statusLabel.Height =
            30

        statusLabel.Text =
            "Ready"

        statusLabel.TextAlign =
            ContentAlignment.MiddleLeft

        statusLabel.ForeColor =
            MutedGray

        statusLabel.Font =
            New Font(
                "Segoe UI",
                9.0F
            )

        contentPanel.Controls.Add(
            statusLabel
        )

    End Sub

    '===========================================================
    ' SUMMARY CARDS
    '===========================================================

    Private Sub CreateSummaryCard(
        parent As Control,
        title As String,
        value As String,
        cardColor As Color,
        x As Integer,
        valueSetter As Action(Of Label)
    )

        Dim card As New Panel()

        card.Size =
            New Size(175, 110)

        card.Location =
            New Point(x, 10)

        card.BackColor =
            cardColor

        parent.Controls.Add(card)


        Dim titleLabel As New Label()

        titleLabel.Text =
            title

        titleLabel.ForeColor =
            Color.FromArgb(
                232,
                224,
                211
            )

        titleLabel.Font =
            New Font(
                "Segoe UI Semibold",
                8.5F
            )

        titleLabel.AutoSize = False

        titleLabel.TextAlign =
            ContentAlignment.MiddleLeft

        titleLabel.Location =
            New Point(15, 12)

        titleLabel.Size =
            New Size(145, 25)

        card.Controls.Add(titleLabel)


        Dim valueLabel As New Label()

        valueLabel.Text =
            value

        valueLabel.ForeColor =
            Color.White

        valueLabel.Font =
            New Font(
                "Segoe UI Semibold",
                18.0F
            )

        valueLabel.AutoSize = False

        valueLabel.TextAlign =
            ContentAlignment.MiddleLeft

        valueLabel.Location =
            New Point(15, 42)

        valueLabel.Size =
            New Size(145, 50)

        card.Controls.Add(valueLabel)

        valueSetter(valueLabel)

    End Sub


    Private Sub SetTotalRoomsLabel(
        label As Label
    )
        totalRoomsLabel = label
    End Sub


    Private Sub SetAvailableRoomsLabel(
        label As Label
    )
        availableRoomsLabel = label
    End Sub


    Private Sub SetOccupiedRoomsLabel(
        label As Label
    )
        occupiedRoomsLabel = label
    End Sub


    Private Sub SetReservedRoomsLabel(
        label As Label
    )
        reservedRoomsLabel = label
    End Sub


    Private Sub SetTotalReservationsLabel(
        label As Label
    )
        totalReservationsLabel = label
    End Sub


    Private Sub SetTotalRevenueLabel(
        label As Label
    )
        totalRevenueLabel = label
    End Sub

    '===========================================================
    ' BUTTON CREATION
    '===========================================================

    Private Function CreateButton(
        buttonText As String,
        location As Point,
        buttonSize As Size,
        buttonColor As Color
    ) As Button

        Dim btn As New Button()

        btn.Text =
            buttonText

        btn.Location =
            location

        btn.Size =
            buttonSize

        btn.BackColor =
            buttonColor

        btn.ForeColor =
            Color.White

        btn.FlatStyle =
            FlatStyle.Flat

        btn.FlatAppearance.BorderSize =
            0

        btn.Font =
            New Font(
                "Segoe UI Semibold",
                9.0F
            )

        btn.Cursor =
            Cursors.Hand

        Return btn

    End Function

    '===========================================================
    ' LOAD SUMMARY
    '===========================================================

    Private Sub LoadSummary()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                '------------------------------------------------
                ' TOTAL ROOMS
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) FROM dbo.Rooms",
                    connection
                )

                    totalRoomsLabel.Text =
                        Convert.ToInt32(
                            command.ExecuteScalar()
                        ).ToString()

                End Using


                '------------------------------------------------
                ' AVAILABLE ROOMS
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) FROM dbo.Rooms " &
                    "WHERE Status = 'Available'",
                    connection
                )

                    availableRoomsLabel.Text =
                        Convert.ToInt32(
                            command.ExecuteScalar()
                        ).ToString()

                End Using


                '------------------------------------------------
                ' OCCUPIED ROOMS
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) FROM dbo.Rooms " &
                    "WHERE Status = 'Occupied'",
                    connection
                )

                    occupiedRoomsLabel.Text =
                        Convert.ToInt32(
                            command.ExecuteScalar()
                        ).ToString()

                End Using


                '------------------------------------------------
                ' RESERVED ROOMS
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) FROM dbo.Rooms " &
                    "WHERE Status = 'Reserved'",
                    connection
                )

                    reservedRoomsLabel.Text =
                        Convert.ToInt32(
                            command.ExecuteScalar()
                        ).ToString()

                End Using


                '------------------------------------------------
                ' TOTAL RESERVATIONS
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) " &
                    "FROM dbo.Reservations " &
                    "WHERE ReservationStatus <> 'Cancelled'",
                    connection
                )

                    totalReservationsLabel.Text =
                        Convert.ToInt32(
                            command.ExecuteScalar()
                        ).ToString()

                End Using


                '------------------------------------------------
                ' TOTAL PAYMENTS / REVENUE
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT ISNULL(SUM(PaymentAmount), 0) " &
                    "FROM dbo.Payments",
                    connection
                )

                    Dim revenue As Decimal =
                        Convert.ToDecimal(
                            command.ExecuteScalar()
                        )

                    totalRevenueLabel.Text =
                        "₱" &
                        revenue.ToString("N2")

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Unable to load report summary." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Reports Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    '===========================================================
    ' LOAD REPORT
    '===========================================================

    Private Sub LoadReport()

        If reportTypeComboBox Is Nothing Then
            Return
        End If

        Select Case reportTypeComboBox.SelectedIndex

            Case 0
                LoadReservationReport()

            Case 1
                LoadGuestReport()

            Case 2
                LoadPaymentReport()

            Case 3
                LoadRoomReport()

            Case 4
                LoadOccupancyReport()

            Case 5
                LoadTodaysCheckIns()

            Case 6
                LoadTodaysCheckOuts()

            Case 7
                LoadRevenueReport()

        End Select

    End Sub

    '===========================================================
    ' RESERVATION REPORT
    '===========================================================

    Private Sub LoadReservationReport()

        Try

            'Query the base tables directly so this report does not depend on a
            'stale vw_ReservationReport definition from an older database version.
            Dim query As String =
                "SELECT " &
                "r.ReservationID AS [Reservation ID], " &
                "g.FullName AS [Guest Name], " &
                "g.ContactNumber AS [Contact Number], " &
                "g.Email AS [Email], " &
                "rm.RoomNumber AS [Room Number], " &
                "rm.RoomType AS [Room Type], " &
                "r.CheckInDate AS [Check-In], " &
                "r.CheckOutDate AS [Check-Out], " &
                "r.NumberOfGuests AS [Guests], " &
                "r.NumberOfNights AS [Nights], " &
                "r.RoomRate AS [Rate/Night], " &
                "r.TotalAmount AS [Total Amount], " &
                "r.ReservationStatus AS [Reservation Status], " &
                "r.PaymentStatus AS [Payment Status] " &
                "FROM dbo.Reservations r " &
                "INNER JOIN dbo.Guests g ON g.GuestID = r.GuestID " &
                "INNER JOIN dbo.Rooms rm ON rm.RoomID = r.RoomID " &
                "WHERE ISNULL(r.IsArchived, 0) = 0 " &
                "AND ISNULL(g.IsArchived, 0) = 0 " &
                "AND ISNULL(rm.IsArchived, 0) = 0 " &
                "ORDER BY r.CheckInDate DESC"

            LoadGrid(query)

            statusLabel.Text =
                "Showing reservation history and reservation status."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' GUEST REPORT
    '===========================================================

    Private Sub LoadGuestReport()

        Try

            Dim query As String =
                "SELECT " &
                "GuestID AS [Guest ID], " &
                "FullName AS [Guest Name], " &
                "ContactNumber AS [Contact Number], " &
                "Email AS [Email], " &
                "Address AS [Address], " &
                "City AS [City], " &
                "Country AS [Country], " &
                "ReservationCount AS [Reservations], " &
                "TotalReservationValue AS [Reservation Value] " &
                "FROM dbo.vw_GuestReport " &
                "ORDER BY FullName"

            LoadGrid(query)

            statusLabel.Text =
                "Showing registered guests and reservation history."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' PAYMENT REPORT
    '===========================================================

    Private Sub LoadPaymentReport()

        Try

            Dim query As String =
                "SELECT " &
                "PaymentID AS [Payment ID], " &
                "PaymentDate AS [Payment Date], " &
                "GuestName AS [Guest Name], " &
                "RoomNumber AS [Room Number], " &
                "RoomType AS [Room Type], " &
                "ReservationID AS [Reservation ID], " &
                "ReservationTotal AS [Reservation Total], " &
                "PaymentAmount AS [Payment Amount], " &
                "PaymentMethod AS [Payment Method], " &
                "PaymentStatus AS [Payment Status], " &
                "ReferenceNumber AS [Reference Number] " &
                "FROM dbo.vw_PaymentReport " &
                "ORDER BY PaymentDate DESC"

            LoadGrid(query)

            statusLabel.Text =
                "Showing hotel payments and transaction information."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' ROOM REPORT
    '===========================================================

    Private Sub LoadRoomReport()

        Try

            Dim query As String =
                "SELECT " &
                "RoomID AS [Room ID], " &
                "RoomNumber AS [Room Number], " &
                "RoomType AS [Room Type], " &
                "Capacity AS [Capacity], " &
                "RatePerNight AS [Rate/Night], " &
                "Status AS [Status], " &
                "Floor AS [Floor], " &
                "Description AS [Description] " &
                "FROM dbo.vw_RoomReport " &
                "ORDER BY Floor, RoomNumber"

            LoadGrid(query)

            statusLabel.Text =
                "Showing all hotel rooms and current room status."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' OCCUPANCY REPORT
    '===========================================================

    Private Sub LoadOccupancyReport()

        Try

            Dim query As String =
                "SELECT " &
                "TotalRooms AS [Total Rooms], " &
                "AvailableRooms AS [Available], " &
                "OccupiedRooms AS [Occupied], " &
                "ReservedRooms AS [Reserved], " &
                "MaintenanceRooms AS [Maintenance] " &
                "FROM dbo.vw_RoomSummary"

            LoadGrid(query)

            statusLabel.Text =
                "Showing current room occupancy summary."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' TODAY'S CHECK-INS
    '===========================================================

    Private Sub LoadTodaysCheckIns()

        Try

            Dim query As String =
                "SELECT " &
                "ReservationID AS [Reservation ID], " &
                "GuestName AS [Guest Name], " &
                "RoomNumber AS [Room Number], " &
                "RoomType AS [Room Type], " &
                "CheckInDate AS [Check-In], " &
                "CheckOutDate AS [Check-Out], " &
                "ReservationStatus AS [Status] " &
                "FROM dbo.vw_TodaysCheckIns " &
                "ORDER BY RoomNumber"

            LoadGrid(query)

            statusLabel.Text =
                "Showing today's scheduled check-ins."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' TODAY'S CHECK-OUTS
    '===========================================================

    Private Sub LoadTodaysCheckOuts()

        Try

            Dim query As String =
                "SELECT " &
                "ReservationID AS [Reservation ID], " &
                "GuestName AS [Guest Name], " &
                "RoomNumber AS [Room Number], " &
                "RoomType AS [Room Type], " &
                "CheckInDate AS [Check-In], " &
                "CheckOutDate AS [Check-Out], " &
                "ReservationStatus AS [Status] " &
                "FROM dbo.vw_TodaysCheckOuts " &
                "ORDER BY RoomNumber"

            LoadGrid(query)

            statusLabel.Text =
                "Showing today's scheduled check-outs."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' REVENUE REPORT
    '===========================================================

    Private Sub LoadRevenueReport()

        Try

            Dim query As String =
                "SELECT " &
                "PaymentCount AS [Payment Count], " &
                "TotalRevenue AS [Total Revenue], " &
                "CashRevenue AS [Cash], " &
                "CreditCardRevenue AS [Credit Card], " &
                "DebitCardRevenue AS [Debit Card], " &
                "BankTransferRevenue AS [Bank Transfer], " &
                "GCashRevenue AS [GCash], " &
                "MayaRevenue AS [Maya] " &
                "FROM dbo.vw_RevenueSummary"

            LoadGrid(query)

            statusLabel.Text =
                "Showing hotel revenue and payment-method summary."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' LOAD GRID
    '===========================================================

    Private Sub LoadGrid(query As String)

        Dim table As New DataTable()

        Using connection =
            DatabaseHelper.GetConnection()

            Using command As New SqlCommand(
                query,
                connection
            )

                Using adapter As New SqlDataAdapter(
                    command
                )

                    adapter.Fill(table)

                End Using

            End Using

        End Using

        reportGrid.DataSource =
            table

        FormatGrid()

    End Sub

    '===========================================================
    ' GRID FORMATTING
    '===========================================================

    Private Sub FormatGrid()

        If reportGrid.Columns.Count = 0 Then
            Return
        End If


        For Each column As DataGridViewColumn
            In reportGrid.Columns
            column.SortMode = DataGridViewColumnSortMode.Automatic
            column.MinimumWidth = 90
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            column.Width = 130
        Next

        'Give common fields readable widths; other columns retain a consistent
        'width and remain accessible using the horizontal scrollbar.
        Dim preferredWidths As New Dictionary(Of String, Integer) From {
            {"Reservation ID", 105}, {"Guest Name", 165},
            {"Contact Number", 135}, {"Email", 190},
            {"Room Number", 110}, {"Room Type", 145},
            {"Check-In", 120}, {"Check-Out", 120},
            {"Guests", 80}, {"Nights", 80},
            {"Rate/Night", 120}, {"Total Amount", 130},
            {"Reservation Status", 150}, {"Payment Status", 135},
            {"Guest ID", 90}, {"Payment ID", 95}, {"Payment Date", 130},
            {"Payment Amount", 135}, {"Payment Method", 140},
            {"Reference Number", 160}, {"Room ID", 90},
            {"Capacity", 90}, {"Status", 120}, {"Floor", 80},
            {"Description", 220}
        }
        For Each pair In preferredWidths
            If reportGrid.Columns.Contains(pair.Key) Then
                reportGrid.Columns(pair.Key).Width = pair.Value
            End If
        Next


        Dim currencyColumns As String() =
        {
            "Rate/Night",
            "Total Amount",
            "Reservation Value",
            "Reservation Total",
            "Payment Amount",
            "Total Revenue",
            "Cash",
            "Credit Card",
            "Debit Card",
            "Bank Transfer",
            "GCash",
            "Maya"
        }


        For Each columnName As String
            In currencyColumns

            If reportGrid.Columns.Contains(
                columnName
            ) Then

                reportGrid.Columns(
                    columnName
                ).DefaultCellStyle.Format =
                    "₱#,##0.00"

            End If

        Next


        Dim dateColumns As String() =
        {
            "Payment Date",
            "Check-In",
            "Check-Out"
        }


        For Each columnName As String
            In dateColumns

            If reportGrid.Columns.Contains(
                columnName
            ) Then

                reportGrid.Columns(
                    columnName
                ).DefaultCellStyle.Format =
                    "MMM dd, yyyy"

            End If

        Next

    End Sub

    '===========================================================
    ' REFRESH
    '===========================================================

    Private Sub RefreshButton_Click(
        sender As Object,
        e As EventArgs
    )

        LoadSummary()
        LoadReport()

        statusLabel.Text =
            "Reports refreshed successfully."

    End Sub

    '===========================================================
    ' REPORT TYPE CHANGE
    '===========================================================

    Private Sub ReportTypeComboBox_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    )

        LoadReport()

    End Sub

    '===========================================================
    ' EXPORT CSV
    '===========================================================

    Private Sub ExportButton_Click(
        sender As Object,
        e As EventArgs
    )

        If reportGrid.Rows.Count = 0 Then

            MessageBox.Show(
                "There are no records to export.",
                "Export",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            Return

        End If


        Try

            Dim reportName As String =
                reportTypeComboBox.Text.Replace(
                    " ",
                    "_"
                )


            Dim saveDialog As New SaveFileDialog()

            saveDialog.Filter =
                "CSV Files (*.csv)|*.csv|" &
                "All Files (*.*)|*.*"

            saveDialog.FileName =
                reportName &
                "_" &
                DateTime.Now.ToString(
                    "yyyyMMdd_HHmmss"
                ) &
                ".csv"


            If saveDialog.ShowDialog() <>
               DialogResult.OK Then

                Return

            End If


            Dim csv As New StringBuilder()


            '---------------------------------------------------
            ' HEADERS
            '---------------------------------------------------

            For i As Integer =
                0 To reportGrid.Columns.Count - 1

                If i > 0 Then
                    csv.Append(",")
                End If

                csv.Append(
                    EscapeCsv(
                        reportGrid.Columns(i).HeaderText
                    )
                )

            Next

            csv.AppendLine()


            '---------------------------------------------------
            ' DATA
            '---------------------------------------------------

            For Each row As DataGridViewRow
                In reportGrid.Rows

                If row.IsNewRow Then
                    Continue For
                End If


                For i As Integer =
                    0 To reportGrid.Columns.Count - 1

                    If i > 0 Then
                        csv.Append(",")
                    End If


                    Dim value As Object =
                        row.Cells(i).Value

                    Dim text As String =
                        ""

                    If value IsNot Nothing AndAlso
                       Not IsDBNull(value) Then

                        text =
                            value.ToString()

                    End If


                    csv.Append(
                        EscapeCsv(text)
                    )

                Next

                csv.AppendLine()

            Next


            File.WriteAllText(
                saveDialog.FileName,
                csv.ToString(),
                Encoding.UTF8
            )


            MessageBox.Show(
                "Report exported successfully.",
                "Export Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

        Catch ex As Exception

            MessageBox.Show(
                "Unable to export the report." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Export Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    '===========================================================
    ' CSV ESCAPING
    '===========================================================

    Private Function EscapeCsv(
        value As String
    ) As String

        If value Is Nothing Then
            Return """"""
        End If

        value =
            value.Replace(
                """",
                """"""
            )

        Return """" &
               value &
               """"

    End Function

    '===========================================================
    ' CLOSE
    '===========================================================

    Private Sub CloseButton_Click(
        sender As Object,
        e As EventArgs
    )

        Me.Close()

    End Sub

    '===========================================================
    ' DATABASE ERROR
    '===========================================================

    Private Sub ShowDatabaseError(
        ex As Exception
    )

        MessageBox.Show(
            "Unable to load the report." &
            Environment.NewLine &
            Environment.NewLine &
            ex.Message,
            "Database Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

    End Sub

    '===========================================================
    ' FORM RESIZE
    '===========================================================

    Private Sub ReportsForm_Resize(
        sender As Object,
        e As EventArgs
    )

        If summaryPanel Is Nothing Then
            Return
        End If


        Dim availableWidth As Integer =
            summaryPanel.ClientSize.Width

        Dim cardWidth As Integer =
            175

        Dim spacing As Integer =
            10

        Dim totalWidth As Integer =
            (cardWidth * 6) +
            (spacing * 5)

        Dim startX As Integer =
            Math.Max(
                10,
                (availableWidth - totalWidth) \ 2
            )

        Dim x As Integer =
            startX


        For Each control As Control
            In summaryPanel.Controls

            If TypeOf control Is Panel Then

                control.Left =
                    x

                x +=
                    cardWidth +
                    spacing

            End If

        Next

    End Sub

End Class