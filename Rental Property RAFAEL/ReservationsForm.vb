Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class ReservationsForm
    Inherits Form

    '==========================================================
    ' AURELIA GRAND HOTEL COLORS
    '==========================================================

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

    Private ReadOnly Sage As Color =
        Color.FromArgb(105, 125, 108)

    Private ReadOnly Rose As Color =
        Color.FromArgb(145, 75, 70)


    '==========================================================
    ' CONTROLS
    '==========================================================

    Private lblTitle As Label
    Private lblSubtitle As Label

    Private txtSearch As TextBox
    Private btnSearch As Button
    Private btnRefresh As Button

    Private txtReservationID As TextBox

    Private cboGuest As ComboBox
    Private txtGuestContact As TextBox
    Private txtGuestEmail As TextBox

    Private cboRoom As ComboBox
    Private txtRoomType As TextBox
    Private txtRoomCapacity As TextBox
    Private txtRoomRate As TextBox

    Private dtpCheckIn As DateTimePicker
    Private dtpCheckOut As DateTimePicker

    Private txtNights As TextBox
    Private txtTotalAmount As TextBox

    Private cboReservationStatus As ComboBox
    Private cboPaymentStatus As ComboBox

    Private txtSpecialRequests As TextBox

    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnClear As Button

    Private dgvReservations As DataGridView

    Private selectedReservationID As Integer = 0


    '==========================================================
    ' CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildForm()

        LoadGuestsIntoComboBox()

        LoadRoomsIntoComboBox()

        LoadReservations()

        ClearFields()

    End Sub


    '==========================================================
    ' BUILD FORM
    '==========================================================

    Private Sub BuildForm()

        Me.Text =
            "Aurelia Grand Hotel - Reservations"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(1250, 850)

        Me.MinimumSize =
            New Size(1150, 780)

        Me.BackColor =
            Ivory

        Me.Font =
            New Font(
                "Segoe UI",
                9.5F
            )


        '======================================================
        ' TITLE
        '======================================================

        lblTitle = New Label()

        lblTitle.Text =
            "Reservation Management"

        lblTitle.Location =
            New Point(25, 18)

        lblTitle.Size =
            New Size(600, 40)

        lblTitle.Font =
            New Font(
                "Georgia",
                21,
                FontStyle.Bold
            )

        lblTitle.ForeColor =
            Charcoal

        Me.Controls.Add(lblTitle)


        '======================================================
        ' SUBTITLE
        '======================================================

        lblSubtitle = New Label()

        lblSubtitle.Text =
            "Create and manage guest reservations at Aurelia Grand Hotel."

        lblSubtitle.Location =
            New Point(27, 57)

        lblSubtitle.AutoSize = True

        lblSubtitle.ForeColor =
            MutedGray

        lblSubtitle.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        Me.Controls.Add(lblSubtitle)


        '======================================================
        ' SEARCH
        '======================================================

        Dim lblSearch As Label =
            CreateLabel(
                "Search Reservation",
                New Point(25, 91)
            )

        Me.Controls.Add(lblSearch)


        txtSearch = New TextBox()

        txtSearch.Location =
            New Point(25, 116)

        txtSearch.Size =
            New Size(350, 32)

        txtSearch.Font =
            New Font(
                "Segoe UI",
                10
            )

        txtSearch.BorderStyle =
            BorderStyle.FixedSingle

        Me.Controls.Add(txtSearch)


        btnSearch =
            CreateButton(
                "Search",
                New Point(385, 114),
                New Size(105, 36),
                Navy
            )

        Me.Controls.Add(btnSearch)


        btnRefresh =
            CreateButton(
                "Refresh",
                New Point(500, 114),
                New Size(105, 36),
                Champagne
            )

        Me.Controls.Add(btnRefresh)


        AddHandler btnSearch.Click,
            AddressOf SearchClicked

        AddHandler btnRefresh.Click,
            AddressOf RefreshClicked

        AddHandler txtSearch.KeyDown,
            AddressOf SearchKeyDown


        '======================================================
        ' RESERVATION PANEL
        '======================================================

        Dim formPanel As New Panel()

        formPanel.Location =
            New Point(25, 165)

        formPanel.Size =
            New Size(1180, 365)

        formPanel.BackColor =
            WarmWhite

        Me.Controls.Add(formPanel)


        '======================================================
        ' RESERVATION ID
        '======================================================

        Dim lblReservationID As Label =
            CreateLabel(
                "Reservation ID",
                New Point(20, 15)
            )

        formPanel.Controls.Add(lblReservationID)


        txtReservationID =
            CreateTextBox()

        txtReservationID.Location =
            New Point(20, 42)

        txtReservationID.Size =
            New Size(130, 30)

        txtReservationID.ReadOnly = True

        txtReservationID.BackColor =
            Color.FromArgb(
                238,
                234,
                225
            )

        formPanel.Controls.Add(txtReservationID)


        '======================================================
        ' GUEST
        '======================================================

        Dim lblGuest As Label =
            CreateLabel(
                "Guest",
                New Point(170, 15)
            )

        formPanel.Controls.Add(lblGuest)


        cboGuest =
            CreateComboBox()

        cboGuest.Location =
            New Point(170, 42)

        cboGuest.Size =
            New Size(330, 30)

        formPanel.Controls.Add(cboGuest)

        AddHandler cboGuest.SelectedIndexChanged,
            AddressOf GuestChanged


        '======================================================
        ' CONTACT
        '======================================================

        Dim lblGuestContact As Label =
            CreateLabel(
                "Contact Number",
                New Point(520, 15)
            )

        formPanel.Controls.Add(lblGuestContact)


        txtGuestContact =
            CreateTextBox()

        txtGuestContact.Location =
            New Point(520, 42)

        txtGuestContact.Size =
            New Size(250, 30)

        txtGuestContact.ReadOnly = True

        txtGuestContact.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtGuestContact)


        '======================================================
        ' EMAIL
        '======================================================

        Dim lblGuestEmail As Label =
            CreateLabel(
                "Email Address",
                New Point(790, 15)
            )

        formPanel.Controls.Add(lblGuestEmail)


        txtGuestEmail =
            CreateTextBox()

        txtGuestEmail.Location =
            New Point(790, 42)

        txtGuestEmail.Size =
            New Size(365, 30)

        txtGuestEmail.ReadOnly = True

        txtGuestEmail.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtGuestEmail)


        '======================================================
        ' ROOM
        '======================================================

        Dim lblRoom As Label =
            CreateLabel(
                "Room",
                New Point(20, 88)
            )

        formPanel.Controls.Add(lblRoom)


        cboRoom =
            CreateComboBox()

        cboRoom.Location =
            New Point(20, 115)

        cboRoom.Size =
            New Size(260, 30)

        formPanel.Controls.Add(cboRoom)

        AddHandler cboRoom.SelectedIndexChanged,
            AddressOf RoomChanged


        '======================================================
        ' ROOM TYPE
        '======================================================

        Dim lblRoomType As Label =
            CreateLabel(
                "Room Type",
                New Point(300, 88)
            )

        formPanel.Controls.Add(lblRoomType)


        txtRoomType =
            CreateTextBox()

        txtRoomType.Location =
            New Point(300, 115)

        txtRoomType.Size =
            New Size(230, 30)

        txtRoomType.ReadOnly = True

        txtRoomType.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtRoomType)


        '======================================================
        ' CAPACITY
        '======================================================

        Dim lblCapacity As Label =
            CreateLabel(
                "Capacity",
                New Point(550, 88)
            )

        formPanel.Controls.Add(lblCapacity)


        txtRoomCapacity =
            CreateTextBox()

        txtRoomCapacity.Location =
            New Point(550, 115)

        txtRoomCapacity.Size =
            New Size(120, 30)

        txtRoomCapacity.ReadOnly = True

        txtRoomCapacity.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtRoomCapacity)


        '======================================================
        ' ROOM RATE
        '======================================================

        Dim lblRoomRate As Label =
            CreateLabel(
                "Rate Per Night",
                New Point(690, 88)
            )

        formPanel.Controls.Add(lblRoomRate)


        txtRoomRate =
            CreateTextBox()

        txtRoomRate.Location =
            New Point(690, 115)

        txtRoomRate.Size =
            New Size(160, 30)

        txtRoomRate.ReadOnly = True

        txtRoomRate.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtRoomRate)


        '======================================================
        ' RESERVATION STATUS
        '======================================================

        Dim lblReservationStatus As Label =
            CreateLabel(
                "Reservation Status",
                New Point(870, 88)
            )

        formPanel.Controls.Add(lblReservationStatus)


        cboReservationStatus =
            CreateComboBox()

        cboReservationStatus.Location =
            New Point(870, 115)

        cboReservationStatus.Size =
            New Size(285, 30)

        cboReservationStatus.Items.Add(
            "Reserved"
        )

        cboReservationStatus.Items.Add(
            "Checked-In"
        )

        cboReservationStatus.Items.Add(
            "Checked-Out"
        )

        cboReservationStatus.Items.Add(
            "Cancelled"
        )

        formPanel.Controls.Add(cboReservationStatus)


        '======================================================
        ' CHECK-IN
        '======================================================

        Dim lblCheckIn As Label =
            CreateLabel(
                "Check-In Date",
                New Point(20, 161)
            )

        formPanel.Controls.Add(lblCheckIn)


        dtpCheckIn =
            CreateDatePicker()

        dtpCheckIn.Location =
            New Point(20, 188)

        dtpCheckIn.Size =
            New Size(190, 30)

        formPanel.Controls.Add(dtpCheckIn)


        '======================================================
        ' CHECK-OUT
        '======================================================

        Dim lblCheckOut As Label =
            CreateLabel(
                "Check-Out Date",
                New Point(230, 161)
            )

        formPanel.Controls.Add(lblCheckOut)


        dtpCheckOut =
            CreateDatePicker()

        dtpCheckOut.Location =
            New Point(230, 188)

        dtpCheckOut.Size =
            New Size(190, 30)

        formPanel.Controls.Add(dtpCheckOut)


        AddHandler dtpCheckIn.ValueChanged,
            AddressOf DateChanged

        AddHandler dtpCheckOut.ValueChanged,
            AddressOf DateChanged


        '======================================================
        ' NIGHTS
        '======================================================

        Dim lblNights As Label =
            CreateLabel(
                "Number of Nights",
                New Point(440, 161)
            )

        formPanel.Controls.Add(lblNights)


        txtNights =
            CreateTextBox()

        txtNights.Location =
            New Point(440, 188)

        txtNights.Size =
            New Size(140, 30)

        txtNights.ReadOnly = True

        txtNights.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtNights)


        '======================================================
        ' TOTAL
        '======================================================

        Dim lblTotal As Label =
            CreateLabel(
                "Total Amount",
                New Point(600, 161)
            )

        formPanel.Controls.Add(lblTotal)


        txtTotalAmount =
            CreateTextBox()

        txtTotalAmount.Location =
            New Point(600, 188)

        txtTotalAmount.Size =
            New Size(180, 30)

        txtTotalAmount.ReadOnly = True

        txtTotalAmount.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        txtTotalAmount.Font =
            New Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            )

        formPanel.Controls.Add(txtTotalAmount)


        '======================================================
        ' PAYMENT STATUS
        '======================================================

        Dim lblPaymentStatus As Label =
            CreateLabel(
                "Payment Status",
                New Point(800, 161)
            )

        formPanel.Controls.Add(lblPaymentStatus)


        cboPaymentStatus =
            CreateComboBox()

        cboPaymentStatus.Location =
            New Point(800, 188)

        cboPaymentStatus.Size =
            New Size(180, 30)

        cboPaymentStatus.Items.Add(
            "Paid"
        )

        cboPaymentStatus.Items.Add(
            "Partially Paid"
        )

        cboPaymentStatus.Items.Add(
            "Unpaid"
        )

        formPanel.Controls.Add(cboPaymentStatus)


        '======================================================
        ' SPECIAL REQUESTS
        '======================================================

        Dim lblSpecialRequests As Label =
            CreateLabel(
                "Special Requests / Notes",
                New Point(20, 235)
            )

        formPanel.Controls.Add(lblSpecialRequests)


        txtSpecialRequests =
            CreateTextBox()

        txtSpecialRequests.Location =
            New Point(20, 262)

        txtSpecialRequests.Size =
            New Size(760, 30)

        formPanel.Controls.Add(txtSpecialRequests)


        '======================================================
        ' BUTTONS
        '======================================================

        btnClear =
            CreateButton(
                "Clear",
                New Point(800, 255),
                New Size(105, 38),
                Sage
            )

        formPanel.Controls.Add(btnClear)


        btnAdd =
            CreateButton(
                "Add",
                New Point(915, 255),
                New Size(105, 38),
                Navy
            )

        formPanel.Controls.Add(btnAdd)


        btnUpdate =
            CreateButton(
                "Update",
                New Point(1030, 255),
                New Size(105, 38),
                Champagne
            )

        formPanel.Controls.Add(btnUpdate)


        '======================================================
        ' DELETE
        '======================================================

        btnDelete =
            CreateButton(
                "Delete",
                New Point(800, 305),
                New Size(105, 38),
                Rose
            )

        formPanel.Controls.Add(btnDelete)


        AddHandler btnClear.Click,
            AddressOf ClearClicked

        AddHandler btnAdd.Click,
            AddressOf AddReservationClicked

        AddHandler btnUpdate.Click,
            AddressOf UpdateReservationClicked

        AddHandler btnDelete.Click,
            AddressOf DeleteReservationClicked


        '======================================================
        ' DATA GRID
        '======================================================

        dgvReservations =
            New DataGridView()

        dgvReservations.Location =
            New Point(25, 550)

        dgvReservations.Size =
            New Size(1180, 220)

        dgvReservations.BackgroundColor =
            WarmWhite

        dgvReservations.BorderStyle =
            BorderStyle.None

        dgvReservations.ReadOnly =
            True

        dgvReservations.AllowUserToAddRows =
            False

        dgvReservations.AllowUserToDeleteRows =
            False

        dgvReservations.RowHeadersVisible =
            False

        dgvReservations.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvReservations.MultiSelect =
            False

        dgvReservations.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvReservations.EnableHeadersVisualStyles =
            False

        dgvReservations.ColumnHeadersDefaultCellStyle.BackColor =
            DeepNavy

        dgvReservations.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        dgvReservations.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold
            )

        dgvReservations.DefaultCellStyle.BackColor =
            WarmWhite

        dgvReservations.DefaultCellStyle.ForeColor =
            Charcoal

        dgvReservations.DefaultCellStyle.SelectionBackColor =
            SoftGold

        dgvReservations.DefaultCellStyle.SelectionForeColor =
            Charcoal

        dgvReservations.RowTemplate.Height =
            30

        Me.Controls.Add(dgvReservations)


        AddHandler dgvReservations.CellClick,
            AddressOf ReservationRowClicked

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

        label.AutoSize = True

        label.Font =
            New Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold
            )

        label.ForeColor =
            Charcoal

        Return label

    End Function


    '==========================================================
    ' CREATE TEXTBOX
    '==========================================================

    Private Function CreateTextBox() As TextBox

        Dim textbox As New TextBox()

        textbox.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        textbox.BorderStyle =
            BorderStyle.FixedSingle

        textbox.BackColor =
            Color.White

        textbox.ForeColor =
            Charcoal

        Return textbox

    End Function


    '==========================================================
    ' CREATE COMBOBOX
    '==========================================================

    Private Function CreateComboBox() As ComboBox

        Dim combo As New ComboBox()

        combo.DropDownStyle =
            ComboBoxStyle.DropDownList

        combo.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        combo.BackColor =
            Color.White

        combo.ForeColor =
            Charcoal

        Return combo

    End Function


    '==========================================================
    ' CREATE DATE PICKER
    '==========================================================

    Private Function CreateDatePicker() As DateTimePicker

        Dim picker As New DateTimePicker()

        picker.Format =
            DateTimePickerFormat.Short

        picker.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        picker.Value =
            Date.Today

        Return picker

    End Function


    '==========================================================
    ' CREATE BUTTON
    '==========================================================

    Private Function CreateButton(
        text As String,
        location As Point,
        size As Size,
        buttonColor As Color
    ) As Button

        Dim button As New Button()

        button.Text =
            text

        button.Location =
            location

        button.Size =
            size

        button.BackColor =
            buttonColor

        button.ForeColor =
            Color.White

        button.FlatStyle =
            FlatStyle.Flat

        button.Font =
            New Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold
            )

        button.Cursor =
            Cursors.Hand

        button.UseVisualStyleBackColor =
            False

        button.FlatAppearance.BorderSize =
            0

        Return button

    End Function


    '==========================================================
    ' LOAD GUESTS
    '==========================================================

    Private Sub LoadGuestsIntoComboBox()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "GuestID, " &
                    "FullName, " &
                    "ContactNumber, " &
                    "Email " &
                    "FROM Guests " &
                    "ORDER BY FullName"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using reader As SqlDataReader =
                        command.ExecuteReader()

                        cboGuest.Items.Clear()


                        While reader.Read()

                            Dim item As New GuestItem()

                            item.ID =
                                Convert.ToInt32(
                                    reader("GuestID")
                                )

                            item.Name =
                                reader(
                                    "FullName"
                                ).ToString()

                            item.Contact =
                                reader(
                                    "ContactNumber"
                                ).ToString()

                            item.Email =
                                reader(
                                    "Email"
                                ).ToString()


                            cboGuest.Items.Add(
                                item
                            )

                        End While

                    End Using

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load guests." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' GUEST ITEM
    '==========================================================

    Private Class GuestItem

        Public Property ID As Integer

        Public Property Name As String

        Public Property Contact As String

        Public Property Email As String


        Public Overrides Function ToString() As String

            Return ID.ToString() &
                   " - " &
                   Name

        End Function

    End Class


    '==========================================================
    ' LOAD ROOMS
    '==========================================================

    Private Sub LoadRoomsIntoComboBox()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "RoomID, " &
                    "RoomNumber, " &
                    "RoomType, " &
                    "Capacity, " &
                    "RatePerNight, " &
                    "Status " &
                    "FROM Rooms " &
                    "WHERE Status <> 'Maintenance' " &
                    "ORDER BY RoomNumber"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using reader As SqlDataReader =
                        command.ExecuteReader()

                        cboRoom.Items.Clear()


                        While reader.Read()

                            Dim item As New RoomItem()

                            item.ID =
                                Convert.ToInt32(
                                    reader("RoomID")
                                )

                            item.RoomNumber =
                                reader(
                                    "RoomNumber"
                                ).ToString()

                            item.RoomType =
                                reader(
                                    "RoomType"
                                ).ToString()

                            item.Capacity =
                                Convert.ToInt32(
                                    reader("Capacity")
                                )

                            item.Rate =
                                Convert.ToDecimal(
                                    reader("RatePerNight")
                                )

                            item.Status =
                                reader(
                                    "Status"
                                ).ToString()


                            cboRoom.Items.Add(
                                item
                            )

                        End While

                    End Using

                End Using

            End Using


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


    '==========================================================
    ' ROOM ITEM
    '==========================================================

    Private Class RoomItem

        Public Property ID As Integer

        Public Property RoomNumber As String

        Public Property RoomType As String

        Public Property Capacity As Integer

        Public Property Rate As Decimal

        Public Property Status As String


        Public Overrides Function ToString() As String

            Return RoomNumber &
                   " - " &
                   RoomType &
                   " (₱" &
                   Rate.ToString("N2") &
                   ")"

        End Function

    End Class


    '==========================================================
    ' GUEST CHANGED
    '==========================================================

    Private Sub GuestChanged(
        sender As Object,
        e As EventArgs
    )

        If cboGuest.SelectedIndex = -1 Then

            txtGuestContact.Clear()

            txtGuestEmail.Clear()

            Return

        End If


        Dim guest As GuestItem =
            DirectCast(
                cboGuest.SelectedItem,
                GuestItem
            )


        txtGuestContact.Text =
            guest.Contact

        txtGuestEmail.Text =
            guest.Email

    End Sub


    '==========================================================
    ' ROOM CHANGED
    '==========================================================

    Private Sub RoomChanged(
        sender As Object,
        e As EventArgs
    )

        If cboRoom.SelectedIndex = -1 Then

            txtRoomType.Clear()

            txtRoomCapacity.Clear()

            txtRoomRate.Clear()

            Return

        End If


        Dim room As RoomItem =
            DirectCast(
                cboRoom.SelectedItem,
                RoomItem
            )


        txtRoomType.Text =
            room.RoomType

        txtRoomCapacity.Text =
            room.Capacity.ToString()

        txtRoomRate.Text =
            room.Rate.ToString(
                "0.00"
            )


        CalculateReservationTotal()

    End Sub


    '==========================================================
    ' DATE CHANGED
    '==========================================================

    Private Sub DateChanged(
        sender As Object,
        e As EventArgs
    )

        CalculateReservationTotal()

    End Sub


    '==========================================================
    ' CALCULATE TOTAL
    '==========================================================

    Private Sub CalculateReservationTotal()

        If dtpCheckOut Is Nothing Then

            Return

        End If


        Dim nights As Integer =
            DateDiff(
                DateInterval.Day,
                dtpCheckIn.Value.Date,
                dtpCheckOut.Value.Date
            )


        If nights <= 0 Then

            txtNights.Text = "0"

            txtTotalAmount.Text =
                "₱0.00"

            Return

        End If


        txtNights.Text =
            nights.ToString()


        Dim rate As Decimal


        If Decimal.TryParse(
            txtRoomRate.Text,
            rate
        ) Then

            Dim total As Decimal =
                nights * rate


            txtTotalAmount.Text =
                "₱" &
                total.ToString("N2")

        Else

            txtTotalAmount.Text =
                "₱0.00"

        End If

    End Sub


    '==========================================================
    ' LOAD RESERVATIONS
    '==========================================================

    Private Sub LoadReservations()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "r.ReservationID, " &
                    "r.GuestID, " &
                    "g.FullName AS GuestName, " &
                    "r.RoomID, " &
                    "rm.RoomNumber, " &
                    "rm.RoomType, " &
                    "r.CheckInDate, " &
                    "r.CheckOutDate, " &
                    "r.Nights, " &
                    "r.TotalAmount, " &
                    "r.ReservationStatus, " &
                    "r.PaymentStatus, " &
                    "r.SpecialRequests " &
                    "FROM Reservations r " &
                    "INNER JOIN Guests g " &
                    "ON r.GuestID = g.GuestID " &
                    "INNER JOIN Rooms rm " &
                    "ON r.RoomID = rm.RoomID " &
                    "ORDER BY r.ReservationID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvReservations.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load reservations." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' FORMAT GRID
    '==========================================================

    Private Sub FormatGrid()

        If dgvReservations.Columns.Count = 0 Then

            Return

        End If


        SetHeader(
            "ReservationID",
            "ID"
        )

        SetHeader(
            "GuestID",
            "Guest ID"
        )

        SetHeader(
            "GuestName",
            "Guest"
        )

        SetHeader(
            "RoomID",
            "Room ID"
        )

        SetHeader(
            "RoomNumber",
            "Room"
        )

        SetHeader(
            "RoomType",
            "Room Type"
        )

        SetHeader(
            "CheckInDate",
            "Check-In"
        )

        SetHeader(
            "CheckOutDate",
            "Check-Out"
        )

        SetHeader(
            "Nights",
            "Nights"
        )

        SetHeader(
            "TotalAmount",
            "Total"
        )

        SetHeader(
            "ReservationStatus",
            "Status"
        )

        SetHeader(
            "PaymentStatus",
            "Payment"
        )

        SetHeader(
            "SpecialRequests",
            "Special Requests"
        )


        If dgvReservations.Columns.Contains(
            "TotalAmount"
        ) Then

            dgvReservations.Columns(
                "TotalAmount"
            ).DefaultCellStyle.Format =
                "₱#,##0.00"

        End If


        If dgvReservations.Columns.Contains(
            "CheckInDate"
        ) Then

            dgvReservations.Columns(
                "CheckInDate"
            ).DefaultCellStyle.Format =
                "MMM dd, yyyy"

        End If


        If dgvReservations.Columns.Contains(
            "CheckOutDate"
        ) Then

            dgvReservations.Columns(
                "CheckOutDate"
            ).DefaultCellStyle.Format =
                "MMM dd, yyyy"

        End If

    End Sub


    '==========================================================
    ' SET HEADER
    '==========================================================

    Private Sub SetHeader(
        columnName As String,
        headerText As String
    )

        If dgvReservations.Columns.Contains(
            columnName
        ) Then

            dgvReservations.Columns(
                columnName
            ).HeaderText =
                headerText

        End If

    End Sub


    '==========================================================
    ' ADD RESERVATION
    '==========================================================

    Private Sub AddReservationClicked(
        sender As Object,
        e As EventArgs
    )

        If Not ValidateReservation() Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim guest As GuestItem =
                    DirectCast(
                        cboGuest.SelectedItem,
                        GuestItem
                    )


                Dim room As RoomItem =
                    DirectCast(
                        cboRoom.SelectedItem,
                        RoomItem
                    )


                If HasOverlappingReservation(
                    connection,
                    room.ID,
                    dtpCheckIn.Value.Date,
                    dtpCheckOut.Value.Date
                ) Then

                    MessageBox.Show(
                        "This room already has an active reservation " &
                        "during the selected dates.",
                        "Room Unavailable",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    )

                    Return

                End If


                Dim nights As Integer =
                    DateDiff(
                        DateInterval.Day,
                        dtpCheckIn.Value.Date,
                        dtpCheckOut.Value.Date
                    )


                Dim totalAmount As Decimal =
                    nights * room.Rate


                Dim sql As String =
                    "INSERT INTO Reservations " &
                    "(GuestID, RoomID, CheckInDate, CheckOutDate, " &
                    "Nights, TotalAmount, ReservationStatus, " &
                    "PaymentStatus, SpecialRequests) " &
                    "VALUES " &
                    "(@GuestID, @RoomID, @CheckInDate, @CheckOutDate, " &
                    "@Nights, @TotalAmount, @ReservationStatus, " &
                    "@PaymentStatus, @SpecialRequests)"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@GuestID",
                        guest.ID
                    )

                    command.Parameters.AddWithValue(
                        "@RoomID",
                        room.ID
                    )

                    command.Parameters.AddWithValue(
                        "@CheckInDate",
                        dtpCheckIn.Value.Date
                    )

                    command.Parameters.AddWithValue(
                        "@CheckOutDate",
                        dtpCheckOut.Value.Date
                    )

                    command.Parameters.AddWithValue(
                        "@Nights",
                        nights
                    )

                    command.Parameters.AddWithValue(
                        "@TotalAmount",
                        totalAmount
                    )

                    command.Parameters.AddWithValue(
                        "@ReservationStatus",
                        cboReservationStatus.Text
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentStatus",
                        cboPaymentStatus.Text
                    )

                    command.Parameters.AddWithValue(
                        "@SpecialRequests",
                        txtSpecialRequests.Text.Trim()
                    )

                    command.ExecuteNonQuery()

                End Using


                UpdateRoomStatusAfterReservation(
                    connection,
                    room.ID,
                    cboReservationStatus.Text
                )

            End Using


            MessageBox.Show(
                "Reservation added successfully.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadReservations()

            LoadRoomsIntoComboBox()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to add reservation." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' UPDATE RESERVATION
    '==========================================================

    Private Sub UpdateReservationClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedReservationID = 0 Then

            MessageBox.Show(
                "Please select a reservation first.",
                "Update Reservation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        If Not ValidateReservation() Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim guest As GuestItem =
                    DirectCast(
                        cboGuest.SelectedItem,
                        GuestItem
                    )


                Dim room As RoomItem =
                    DirectCast(
                        cboRoom.SelectedItem,
                        RoomItem
                    )


                If HasOverlappingReservation(
                    connection,
                    room.ID,
                    dtpCheckIn.Value.Date,
                    dtpCheckOut.Value.Date,
                    selectedReservationID
                ) Then

                    MessageBox.Show(
                        "This room already has another active " &
                        "reservation during the selected dates.",
                        "Room Unavailable",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    )

                    Return

                End If


                Dim nights As Integer =
                    DateDiff(
                        DateInterval.Day,
                        dtpCheckIn.Value.Date,
                        dtpCheckOut.Value.Date
                    )


                Dim totalAmount As Decimal =
                    nights * room.Rate


                Dim sql As String =
                    "UPDATE Reservations SET " &
                    "GuestID = @GuestID, " &
                    "RoomID = @RoomID, " &
                    "CheckInDate = @CheckInDate, " &
                    "CheckOutDate = @CheckOutDate, " &
                    "Nights = @Nights, " &
                    "TotalAmount = @TotalAmount, " &
                    "ReservationStatus = @ReservationStatus, " &
                    "PaymentStatus = @PaymentStatus, " &
                    "SpecialRequests = @SpecialRequests " &
                    "WHERE ReservationID = @ReservationID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@GuestID",
                        guest.ID
                    )

                    command.Parameters.AddWithValue(
                        "@RoomID",
                        room.ID
                    )

                    command.Parameters.AddWithValue(
                        "@CheckInDate",
                        dtpCheckIn.Value.Date
                    )

                    command.Parameters.AddWithValue(
                        "@CheckOutDate",
                        dtpCheckOut.Value.Date
                    )

                    command.Parameters.AddWithValue(
                        "@Nights",
                        nights
                    )

                    command.Parameters.AddWithValue(
                        "@TotalAmount",
                        totalAmount
                    )

                    command.Parameters.AddWithValue(
                        "@ReservationStatus",
                        cboReservationStatus.Text
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentStatus",
                        cboPaymentStatus.Text
                    )

                    command.Parameters.AddWithValue(
                        "@SpecialRequests",
                        txtSpecialRequests.Text.Trim()
                    )

                    command.Parameters.AddWithValue(
                        "@ReservationID",
                        selectedReservationID
                    )

                    command.ExecuteNonQuery()

                End Using


                UpdateRoomStatusAfterReservation(
                    connection,
                    room.ID,
                    cboReservationStatus.Text
                )

            End Using


            MessageBox.Show(
                "Reservation updated successfully.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadReservations()

            LoadRoomsIntoComboBox()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to update reservation." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' DELETE RESERVATION
    '==========================================================

    Private Sub DeleteReservationClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedReservationID = 0 Then

            MessageBox.Show(
                "Please select a reservation first.",
                "Delete Reservation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        Dim result As DialogResult =
            MessageBox.Show(
                "Are you sure you want to delete this reservation?",
                "Delete Reservation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )


        If result <> DialogResult.Yes Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "DELETE FROM Reservations " &
                    "WHERE ReservationID = @ReservationID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@ReservationID",
                        selectedReservationID
                    )

                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Reservation deleted successfully.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadReservations()

            LoadRoomsIntoComboBox()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to delete reservation." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' VALIDATE RESERVATION
    '==========================================================

    Private Function ValidateReservation() As Boolean

        If cboGuest.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a guest.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboGuest.Focus()

            Return False

        End If


        If cboRoom.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a room.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboRoom.Focus()

            Return False

        End If


        If dtpCheckOut.Value.Date <=
           dtpCheckIn.Value.Date Then

            MessageBox.Show(
                "Check-out date must be after the check-in date.",
                "Invalid Dates",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            dtpCheckOut.Focus()

            Return False

        End If


        If cboReservationStatus.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a reservation status.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboReservationStatus.Focus()

            Return False

        End If


        If cboPaymentStatus.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a payment status.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboPaymentStatus.Focus()

            Return False

        End If


        Return True

    End Function


    '==========================================================
    ' CHECK OVERLAPPING RESERVATION
    '==========================================================

    Private Function HasOverlappingReservation(
        connection As SqlConnection,
        roomID As Integer,
        checkIn As Date,
        checkOut As Date,
        Optional ignoredReservationID As Integer = 0
    ) As Boolean

        Dim sql As String =
            "SELECT COUNT(*) " &
            "FROM Reservations " &
            "WHERE RoomID = @RoomID " &
            "AND ReservationStatus <> 'Cancelled' " &
            "AND CheckInDate < @CheckOutDate " &
            "AND CheckOutDate > @CheckInDate"


        If ignoredReservationID > 0 Then

            sql &=
                " AND ReservationID <> @ReservationID"

        End If


        Using command As New SqlCommand(
            sql,
            connection
        )

            command.Parameters.AddWithValue(
                "@RoomID",
                roomID
            )

            command.Parameters.AddWithValue(
                "@CheckInDate",
                checkIn
            )

            command.Parameters.AddWithValue(
                "@CheckOutDate",
                checkOut
            )


            If ignoredReservationID > 0 Then

                command.Parameters.AddWithValue(
                    "@ReservationID",
                    ignoredReservationID
                )

            End If


            Return Convert.ToInt32(
                command.ExecuteScalar()
            ) > 0

        End Using

    End Function


    '==========================================================
    ' UPDATE ROOM STATUS
    '==========================================================

    Private Sub UpdateRoomStatusAfterReservation(
        connection As SqlConnection,
        roomID As Integer,
        reservationStatus As String
    )

        Dim newStatus As String =
            "Available"


        If reservationStatus = "Reserved" Then

            newStatus =
                "Reserved"

        ElseIf reservationStatus = "Checked-In" Then

            newStatus =
                "Occupied"

        ElseIf reservationStatus = "Checked-Out" Then

            newStatus =
                "Available"

        ElseIf reservationStatus = "Cancelled" Then

            newStatus =
                "Available"

        End If


        Dim sql As String =
            "UPDATE Rooms " &
            "SET Status = @Status " &
            "WHERE RoomID = @RoomID"


        Using command As New SqlCommand(
            sql,
            connection
        )

            command.Parameters.AddWithValue(
                "@Status",
                newStatus
            )

            command.Parameters.AddWithValue(
                "@RoomID",
                roomID
            )

            command.ExecuteNonQuery()

        End Using

    End Sub


    '==========================================================
    ' RESERVATION ROW CLICKED
    '==========================================================

    Private Sub ReservationRowClicked(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then

            Return

        End If


        Dim row As DataGridViewRow =
            dgvReservations.Rows(e.RowIndex)


        If row.Cells("ReservationID").Value Is Nothing Then

            Return

        End If


        selectedReservationID =
            Convert.ToInt32(
                row.Cells("ReservationID").Value
            )


        txtReservationID.Text =
            selectedReservationID.ToString()


        Dim guestID As Integer =
            Convert.ToInt32(
                row.Cells("GuestID").Value
            )


        SelectGuestByID(
            guestID
        )


        Dim roomID As Integer =
            Convert.ToInt32(
                row.Cells("RoomID").Value
            )


        SelectRoomByID(
            roomID
        )


        dtpCheckIn.Value =
            Convert.ToDateTime(
                row.Cells("CheckInDate").Value
            )


        dtpCheckOut.Value =
            Convert.ToDateTime(
                row.Cells("CheckOutDate").Value
            )


        txtNights.Text =
            row.Cells("Nights").Value.ToString()


        txtTotalAmount.Text =
            "₱" &
            Convert.ToDecimal(
                row.Cells("TotalAmount").Value
            ).ToString("N2")


        cboReservationStatus.Text =
            row.Cells(
                "ReservationStatus"
            ).Value.ToString()


        cboPaymentStatus.Text =
            row.Cells(
                "PaymentStatus"
            ).Value.ToString()


        If row.Cells(
            "SpecialRequests"
        ).Value Is DBNull.Value Then

            txtSpecialRequests.Clear()

        Else

            txtSpecialRequests.Text =
                row.Cells(
                    "SpecialRequests"
                ).Value.ToString()

        End If

    End Sub


    '==========================================================
    ' SELECT GUEST
    '==========================================================

    Private Sub SelectGuestByID(
        guestID As Integer
    )

        For index As Integer =
            0 To cboGuest.Items.Count - 1

            Dim item As GuestItem =
                DirectCast(
                    cboGuest.Items(index),
                    GuestItem
                )


            If item.ID = guestID Then

                cboGuest.SelectedIndex =
                    index

                Exit For

            End If

        Next

    End Sub


    '==========================================================
    ' SELECT ROOM
    '==========================================================

    Private Sub SelectRoomByID(
        roomID As Integer
    )

        For index As Integer =
            0 To cboRoom.Items.Count - 1

            Dim item As RoomItem =
                DirectCast(
                    cboRoom.Items(index),
                    RoomItem
                )


            If item.ID = roomID Then

                cboRoom.SelectedIndex =
                    index

                Exit For

            End If

        Next

    End Sub


    '==========================================================
    ' SEARCH
    '==========================================================

    Private Sub SearchClicked(
        sender As Object,
        e As EventArgs
    )

        SearchReservations()

    End Sub


    '==========================================================
    ' SEARCH KEY
    '==========================================================

    Private Sub SearchKeyDown(
        sender As Object,
        e As KeyEventArgs
    )

        If e.KeyCode = Keys.Enter Then

            SearchReservations()

            e.SuppressKeyPress = True

        End If

    End Sub


    '==========================================================
    ' SEARCH RESERVATIONS
    '==========================================================

    Private Sub SearchReservations()

        If txtSearch.Text.Trim() = "" Then

            LoadReservations()

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "r.ReservationID, " &
                    "r.GuestID, " &
                    "g.FullName AS GuestName, " &
                    "r.RoomID, " &
                    "rm.RoomNumber, " &
                    "rm.RoomType, " &
                    "r.CheckInDate, " &
                    "r.CheckOutDate, " &
                    "r.Nights, " &
                    "r.TotalAmount, " &
                    "r.ReservationStatus, " &
                    "r.PaymentStatus, " &
                    "r.SpecialRequests " &
                    "FROM Reservations r " &
                    "INNER JOIN Guests g " &
                    "ON r.GuestID = g.GuestID " &
                    "INNER JOIN Rooms rm " &
                    "ON r.RoomID = rm.RoomID " &
                    "WHERE " &
                    "g.FullName LIKE @Search " &
                    "OR rm.RoomNumber LIKE @Search " &
                    "OR rm.RoomType LIKE @Search " &
                    "OR r.ReservationStatus LIKE @Search " &
                    "OR r.PaymentStatus LIKE @Search " &
                    "ORDER BY r.ReservationID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@Search",
                        "%" &
                        txtSearch.Text.Trim() &
                        "%"
                    )


                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvReservations.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to search reservations." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' REFRESH
    '==========================================================

    Private Sub RefreshClicked(
        sender As Object,
        e As EventArgs
    )

        txtSearch.Clear()

        LoadGuestsIntoComboBox()

        LoadRoomsIntoComboBox()

        LoadReservations()

        ClearFields()

    End Sub


    '==========================================================
    ' CLEAR
    '==========================================================

    Private Sub ClearClicked(
        sender As Object,
        e As EventArgs
    )

        ClearFields()

    End Sub


    '==========================================================
    ' CLEAR FIELDS
    '==========================================================

    Private Sub ClearFields()

        selectedReservationID = 0

        txtReservationID.Clear()

        cboGuest.SelectedIndex = -1

        txtGuestContact.Clear()

        txtGuestEmail.Clear()

        cboRoom.SelectedIndex = -1

        txtRoomType.Clear()

        txtRoomCapacity.Clear()

        txtRoomRate.Clear()

        dtpCheckIn.Value =
            Date.Today

        dtpCheckOut.Value =
            Date.Today.AddDays(1)

        txtNights.Text =
            "1"

        txtTotalAmount.Text =
            "₱0.00"

        cboReservationStatus.SelectedIndex =
            0

        cboPaymentStatus.SelectedIndex =
            2

        txtSpecialRequests.Clear()

        If dgvReservations IsNot Nothing Then

            dgvReservations.ClearSelection()

        End If

    End Sub

End Class