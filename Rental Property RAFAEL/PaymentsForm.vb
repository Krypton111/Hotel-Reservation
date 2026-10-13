Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class PaymentsForm
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

    Private txtPaymentID As TextBox

    Private cboReservation As ComboBox

    Private txtGuestName As TextBox
    Private txtRoomNumber As TextBox
    Private txtReservationTotal As TextBox

    Private txtPaymentAmount As TextBox
    Private txtTotalPaid As TextBox
    Private txtBalance As TextBox

    Private cboPaymentMethod As ComboBox
    Private cboPaymentStatus As ComboBox

    Private dtpPaymentDate As DateTimePicker

    Private txtReferenceNumber As TextBox
    Private txtNotes As TextBox

    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnClear As Button

    Private dgvPayments As DataGridView

    Private selectedPaymentID As Integer = 0


    '==========================================================
    ' CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildForm()

        LoadReservationsIntoComboBox()

        LoadPayments()

        ClearFields()

    End Sub


    '==========================================================
    ' BUILD FORM
    '==========================================================

    Private Sub BuildForm()

        Me.Text =
            "Aurelia Grand Hotel - Payments"

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
            "Payment Management"

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
            "Record guest payments and monitor outstanding reservation balances."

        lblSubtitle.Location =
            New Point(27, 57)

        lblSubtitle.AutoSize =
            True

        lblSubtitle.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        lblSubtitle.ForeColor =
            MutedGray

        Me.Controls.Add(lblSubtitle)


        '======================================================
        ' SEARCH
        '======================================================

        Dim lblSearch As Label =
            CreateLabel(
                "Search Payment",
                New Point(25, 91)
            )

        Me.Controls.Add(lblSearch)


        txtSearch =
            CreateTextBox()

        txtSearch.Location =
            New Point(25, 116)

        txtSearch.Size =
            New Size(350, 32)

        txtSearch.Font =
            New Font(
                "Segoe UI",
                10
            )

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
        ' FORM PANEL
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
        ' PAYMENT ID
        '======================================================

        Dim lblPaymentID As Label =
            CreateLabel(
                "Payment ID",
                New Point(20, 15)
            )

        formPanel.Controls.Add(lblPaymentID)


        txtPaymentID =
            CreateTextBox()

        txtPaymentID.Location =
            New Point(20, 42)

        txtPaymentID.Size =
            New Size(130, 30)

        txtPaymentID.ReadOnly =
            True

        txtPaymentID.BackColor =
            Color.FromArgb(
                238,
                234,
                225
            )

        formPanel.Controls.Add(txtPaymentID)


        '======================================================
        ' RESERVATION
        '======================================================

        Dim lblReservation As Label =
            CreateLabel(
                "Reservation",
                New Point(170, 15)
            )

        formPanel.Controls.Add(lblReservation)


        cboReservation =
            CreateComboBox()

        cboReservation.Location =
            New Point(170, 42)

        cboReservation.Size =
            New Size(380, 30)

        formPanel.Controls.Add(cboReservation)

        AddHandler cboReservation.SelectedIndexChanged,
            AddressOf ReservationChanged


        '======================================================
        ' GUEST
        '======================================================

        Dim lblGuest As Label =
            CreateLabel(
                "Guest",
                New Point(570, 15)
            )

        formPanel.Controls.Add(lblGuest)


        txtGuestName =
            CreateTextBox()

        txtGuestName.Location =
            New Point(570, 42)

        txtGuestName.Size =
            New Size(270, 30)

        txtGuestName.ReadOnly =
            True

        txtGuestName.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtGuestName)


        '======================================================
        ' ROOM
        '======================================================

        Dim lblRoom As Label =
            CreateLabel(
                "Room",
                New Point(860, 15)
            )

        formPanel.Controls.Add(lblRoom)


        txtRoomNumber =
            CreateTextBox()

        txtRoomNumber.Location =
            New Point(860, 42)

        txtRoomNumber.Size =
            New Size(150, 30)

        txtRoomNumber.ReadOnly =
            True

        txtRoomNumber.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtRoomNumber)


        '======================================================
        ' RESERVATION TOTAL
        '======================================================

        Dim lblReservationTotal As Label =
            CreateLabel(
                "Reservation Total",
                New Point(1030, 15)
            )

        formPanel.Controls.Add(lblReservationTotal)


        txtReservationTotal =
            CreateTextBox()

        txtReservationTotal.Location =
            New Point(1030, 42)

        txtReservationTotal.Size =
            New Size(125, 30)

        txtReservationTotal.ReadOnly =
            True

        txtReservationTotal.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtReservationTotal)


        '======================================================
        ' PAYMENT DATE
        '======================================================

        Dim lblPaymentDate As Label =
            CreateLabel(
                "Payment Date",
                New Point(20, 88)
            )

        formPanel.Controls.Add(lblPaymentDate)


        dtpPaymentDate =
            CreateDatePicker()

        dtpPaymentDate.Location =
            New Point(20, 115)

        dtpPaymentDate.Size =
            New Size(190, 30)

        formPanel.Controls.Add(dtpPaymentDate)


        '======================================================
        ' PAYMENT AMOUNT
        '======================================================

        Dim lblPaymentAmount As Label =
            CreateLabel(
                "Payment Amount",
                New Point(230, 88)
            )

        formPanel.Controls.Add(lblPaymentAmount)


        txtPaymentAmount =
            CreateTextBox()

        txtPaymentAmount.Location =
            New Point(230, 115)

        txtPaymentAmount.Size =
            New Size(170, 30)

        formPanel.Controls.Add(txtPaymentAmount)

        AddHandler txtPaymentAmount.TextChanged,
            AddressOf PaymentAmountChanged


        '======================================================
        ' TOTAL PAID
        '======================================================

        Dim lblTotalPaid As Label =
            CreateLabel(
                "Total Paid",
                New Point(420, 88)
            )

        formPanel.Controls.Add(lblTotalPaid)


        txtTotalPaid =
            CreateTextBox()

        txtTotalPaid.Location =
            New Point(420, 115)

        txtTotalPaid.Size =
            New Size(170, 30)

        txtTotalPaid.ReadOnly =
            True

        txtTotalPaid.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        formPanel.Controls.Add(txtTotalPaid)


        '======================================================
        ' BALANCE
        '======================================================

        Dim lblBalance As Label =
            CreateLabel(
                "Remaining Balance",
                New Point(610, 88)
            )

        formPanel.Controls.Add(lblBalance)


        txtBalance =
            CreateTextBox()

        txtBalance.Location =
            New Point(610, 115)

        txtBalance.Size =
            New Size(190, 30)

        txtBalance.ReadOnly =
            True

        txtBalance.BackColor =
            Color.FromArgb(
                244,
                242,
                236
            )

        txtBalance.Font =
            New Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            )

        formPanel.Controls.Add(txtBalance)


        '======================================================
        ' PAYMENT METHOD
        '======================================================

        Dim lblPaymentMethod As Label =
            CreateLabel(
                "Payment Method",
                New Point(820, 88)
            )

        formPanel.Controls.Add(lblPaymentMethod)


        cboPaymentMethod =
            CreateComboBox()

        cboPaymentMethod.Location =
            New Point(820, 115)

        cboPaymentMethod.Size =
            New Size(160, 30)

        cboPaymentMethod.Items.Add(
            "Cash"
        )

        cboPaymentMethod.Items.Add(
            "Credit Card"
        )

        cboPaymentMethod.Items.Add(
            "Debit Card"
        )

        cboPaymentMethod.Items.Add(
            "Bank Transfer"
        )

        cboPaymentMethod.Items.Add(
            "GCash"
        )

        cboPaymentMethod.Items.Add(
            "Other"
        )

        formPanel.Controls.Add(cboPaymentMethod)


        '======================================================
        ' PAYMENT STATUS
        '======================================================

        Dim lblPaymentStatus As Label =
            CreateLabel(
                "Payment Status",
                New Point(1000, 88)
            )

        formPanel.Controls.Add(lblPaymentStatus)


        cboPaymentStatus =
            CreateComboBox()

        cboPaymentStatus.Location =
            New Point(1000, 115)

        cboPaymentStatus.Size =
            New Size(155, 30)

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
        ' REFERENCE NUMBER
        '======================================================

        Dim lblReference As Label =
            CreateLabel(
                "Reference Number",
                New Point(20, 161)
            )

        formPanel.Controls.Add(lblReference)


        txtReferenceNumber =
            CreateTextBox()

        txtReferenceNumber.Location =
            New Point(20, 188)

        txtReferenceNumber.Size =
            New Size(280, 30)

        formPanel.Controls.Add(txtReferenceNumber)


        '======================================================
        ' NOTES
        '======================================================

        Dim lblNotes As Label =
            CreateLabel(
                "Payment Notes",
                New Point(320, 161)
            )

        formPanel.Controls.Add(lblNotes)


        txtNotes =
            CreateTextBox()

        txtNotes.Location =
            New Point(320, 188)

        txtNotes.Size =
            New Size(470, 30)

        formPanel.Controls.Add(txtNotes)


        '======================================================
        ' BUTTONS
        '======================================================

        btnClear =
            CreateButton(
                "Clear",
                New Point(20, 250),
                New Size(110, 40),
                Sage
            )

        formPanel.Controls.Add(btnClear)


        btnAdd =
            CreateButton(
                "Add Payment",
                New Point(140, 250),
                New Size(125, 40),
                Navy
            )

        formPanel.Controls.Add(btnAdd)


        btnUpdate =
            CreateButton(
                "Update",
                New Point(275, 250),
                New Size(110, 40),
                Champagne
            )

        formPanel.Controls.Add(btnUpdate)


        btnDelete =
            CreateButton(
                "Delete",
                New Point(395, 250),
                New Size(110, 40),
                Rose
            )

        formPanel.Controls.Add(btnDelete)


        AddHandler btnClear.Click,
            AddressOf ClearClicked

        AddHandler btnAdd.Click,
            AddressOf AddPaymentClicked

        AddHandler btnUpdate.Click,
            AddressOf UpdatePaymentClicked

        AddHandler btnDelete.Click,
            AddressOf DeletePaymentClicked


        '======================================================
        ' DATA GRID
        '======================================================

        dgvPayments =
            New DataGridView()

        dgvPayments.Location =
            New Point(25, 550)

        dgvPayments.Size =
            New Size(1180, 220)

        dgvPayments.BackgroundColor =
            WarmWhite

        dgvPayments.BorderStyle =
            BorderStyle.None

        dgvPayments.ReadOnly =
            True

        dgvPayments.AllowUserToAddRows =
            False

        dgvPayments.AllowUserToDeleteRows =
            False

        dgvPayments.RowHeadersVisible =
            False

        dgvPayments.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvPayments.MultiSelect =
            False

        dgvPayments.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvPayments.EnableHeadersVisualStyles =
            False

        dgvPayments.ColumnHeadersDefaultCellStyle.BackColor =
            DeepNavy

        dgvPayments.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        dgvPayments.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold
            )

        dgvPayments.DefaultCellStyle.BackColor =
            WarmWhite

        dgvPayments.DefaultCellStyle.ForeColor =
            Charcoal

        dgvPayments.DefaultCellStyle.SelectionBackColor =
            SoftGold

        dgvPayments.DefaultCellStyle.SelectionForeColor =
            Charcoal

        dgvPayments.RowTemplate.Height =
            30

        Me.Controls.Add(dgvPayments)


        AddHandler dgvPayments.CellClick,
            AddressOf PaymentRowClicked

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

        label.AutoSize =
            True

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
    ' LOAD RESERVATIONS
    '==========================================================

    Private Sub LoadReservationsIntoComboBox()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "r.ReservationID, " &
                    "g.FullName AS GuestName, " &
                    "rm.RoomNumber, " &
                    "r.TotalAmount, " &
                    "r.CheckInDate, " &
                    "r.CheckOutDate, " &
                    "r.ReservationStatus " &
                    "FROM Reservations r " &
                    "INNER JOIN Guests g " &
                    "ON r.GuestID = g.GuestID " &
                    "INNER JOIN Rooms rm " &
                    "ON r.RoomID = rm.RoomID " &
                    "WHERE r.ReservationStatus <> 'Cancelled' " &
                    "ORDER BY r.ReservationID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using reader As SqlDataReader =
                        command.ExecuteReader()

                        cboReservation.Items.Clear()


                        While reader.Read()

                            Dim item As New ReservationItem()

                            item.ID =
                                Convert.ToInt32(
                                    reader("ReservationID")
                                )

                            item.GuestName =
                                reader(
                                    "GuestName"
                                ).ToString()

                            item.RoomNumber =
                                reader(
                                    "RoomNumber"
                                ).ToString()

                            item.TotalAmount =
                                Convert.ToDecimal(
                                    reader("TotalAmount")
                                )

                            item.CheckIn =
                                Convert.ToDateTime(
                                    reader("CheckInDate")
                                )

                            item.CheckOut =
                                Convert.ToDateTime(
                                    reader("CheckOutDate")
                                )

                            item.Status =
                                reader(
                                    "ReservationStatus"
                                ).ToString()


                            cboReservation.Items.Add(
                                item
                            )

                        End While

                    End Using

                End Using

            End Using


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
    ' RESERVATION ITEM
    '==========================================================

    Private Class ReservationItem

        Public Property ID As Integer

        Public Property GuestName As String

        Public Property RoomNumber As String

        Public Property TotalAmount As Decimal

        Public Property CheckIn As Date

        Public Property CheckOut As Date

        Public Property Status As String


        Public Overrides Function ToString() As String

            Return "#" &
                   ID.ToString() &
                   " - " &
                   GuestName &
                   " - Room " &
                   RoomNumber &
                   " (" &
                   CheckIn.ToString("MMM dd") &
                   " - " &
                   CheckOut.ToString("MMM dd") &
                   ")"

        End Function

    End Class


    '==========================================================
    ' RESERVATION CHANGED
    '==========================================================

    Private Sub ReservationChanged(
        sender As Object,
        e As EventArgs
    )

        If cboReservation.SelectedIndex = -1 Then

            txtGuestName.Clear()

            txtRoomNumber.Clear()

            txtReservationTotal.Clear()

            txtTotalPaid.Clear()

            txtBalance.Clear()

            Return

        End If


        Dim reservation As ReservationItem =
            DirectCast(
                cboReservation.SelectedItem,
                ReservationItem
            )


        LoadReservationDetails(
            reservation.ID
        )

    End Sub


    '==========================================================
    ' LOAD RESERVATION DETAILS
    '==========================================================

    Private Sub LoadReservationDetails(
        reservationID As Integer
    )

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "g.FullName, " &
                    "rm.RoomNumber, " &
                    "r.TotalAmount " &
                    "FROM Reservations r " &
                    "INNER JOIN Guests g " &
                    "ON r.GuestID = g.GuestID " &
                    "INNER JOIN Rooms rm " &
                    "ON r.RoomID = rm.RoomID " &
                    "WHERE r.ReservationID = @ReservationID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@ReservationID",
                        reservationID
                    )


                    Using reader As SqlDataReader =
                        command.ExecuteReader()

                        If reader.Read() Then

                            txtGuestName.Text =
                                reader(
                                    "FullName"
                                ).ToString()

                            txtRoomNumber.Text =
                                reader(
                                    "RoomNumber"
                                ).ToString()

                            txtReservationTotal.Text =
                                "₱" &
                                Convert.ToDecimal(
                                    reader("TotalAmount")
                                ).ToString("N2")

                        End If

                    End Using

                End Using

            End Using


            LoadTotalPaid(
                reservationID
            )


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load reservation details." &
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
    ' LOAD TOTAL PAID
    '==========================================================

    Private Sub LoadTotalPaid(
        reservationID As Integer
    )

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT ISNULL(SUM(PaymentAmount), 0) " &
                    "FROM Payments " &
                    "WHERE ReservationID = @ReservationID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@ReservationID",
                        reservationID
                    )


                    Dim totalPaid As Decimal =
                        Convert.ToDecimal(
                            command.ExecuteScalar()
                        )


                    txtTotalPaid.Text =
                        "₱" &
                        totalPaid.ToString("N2")


                    CalculateBalance()

                End Using

            End Using


        Catch ex As Exception

            txtTotalPaid.Text =
                "₱0.00"

            txtBalance.Text =
                txtReservationTotal.Text

        End Try

    End Sub


    '==========================================================
    ' PAYMENT AMOUNT CHANGED
    '==========================================================

    Private Sub PaymentAmountChanged(
        sender As Object,
        e As EventArgs
    )

        CalculateBalance()

    End Sub


    '==========================================================
    ' CALCULATE BALANCE
    '==========================================================

    Private Sub CalculateBalance()

        Dim reservationTotal As Decimal = 0D

        Dim totalPaid As Decimal = 0D

        Dim currentPayment As Decimal = 0D


        Decimal.TryParse(
            txtReservationTotal.Text.Replace(
                "₱",
                ""
            ),
            reservationTotal
        )


        Decimal.TryParse(
            txtTotalPaid.Text.Replace(
                "₱",
                ""
            ),
            totalPaid
        )


        Decimal.TryParse(
            txtPaymentAmount.Text.Trim(),
            currentPayment
        )


        Dim balance As Decimal =
            reservationTotal -
            totalPaid -
            currentPayment


        If balance < 0 Then

            balance = 0D

        End If


        txtBalance.Text =
            "₱" &
            balance.ToString("N2")

    End Sub


    '==========================================================
    ' LOAD PAYMENTS
    '==========================================================

    Private Sub LoadPayments()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "p.PaymentID, " &
                    "p.ReservationID, " &
                    "g.FullName AS GuestName, " &
                    "rm.RoomNumber, " &
                    "p.PaymentDate, " &
                    "p.PaymentAmount, " &
                    "p.PaymentMethod, " &
                    "p.PaymentStatus, " &
                    "p.ReferenceNumber, " &
                    "p.Notes " &
                    "FROM Payments p " &
                    "INNER JOIN Reservations r " &
                    "ON p.ReservationID = r.ReservationID " &
                    "INNER JOIN Guests g " &
                    "ON r.GuestID = g.GuestID " &
                    "INNER JOIN Rooms rm " &
                    "ON r.RoomID = rm.RoomID " &
                    "ORDER BY p.PaymentID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvPayments.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load payments." &
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

        If dgvPayments.Columns.Count = 0 Then

            Return

        End If


        SetHeader(
            "PaymentID",
            "ID"
        )

        SetHeader(
            "ReservationID",
            "Reservation"
        )

        SetHeader(
            "GuestName",
            "Guest"
        )

        SetHeader(
            "RoomNumber",
            "Room"
        )

        SetHeader(
            "PaymentDate",
            "Payment Date"
        )

        SetHeader(
            "PaymentAmount",
            "Amount"
        )

        SetHeader(
            "PaymentMethod",
            "Method"
        )

        SetHeader(
            "PaymentStatus",
            "Status"
        )

        SetHeader(
            "ReferenceNumber",
            "Reference"
        )

        SetHeader(
            "Notes",
            "Notes"
        )


        If dgvPayments.Columns.Contains(
            "PaymentAmount"
        ) Then

            dgvPayments.Columns(
                "PaymentAmount"
            ).DefaultCellStyle.Format =
                "₱#,##0.00"

        End If


        If dgvPayments.Columns.Contains(
            "PaymentDate"
        ) Then

            dgvPayments.Columns(
                "PaymentDate"
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

        If dgvPayments.Columns.Contains(
            columnName
        ) Then

            dgvPayments.Columns(
                columnName
            ).HeaderText =
                headerText

        End If

    End Sub


    '==========================================================
    ' ADD PAYMENT
    '==========================================================

    Private Sub AddPaymentClicked(
        sender As Object,
        e As EventArgs
    )

        If Not ValidatePayment() Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim reservation As ReservationItem =
                    DirectCast(
                        cboReservation.SelectedItem,
                        ReservationItem
                    )


                Dim paymentAmount As Decimal =
                    Convert.ToDecimal(
                        txtPaymentAmount.Text.Trim()
                    )


                Dim reservationTotal As Decimal =
                    Convert.ToDecimal(
                        txtReservationTotal.Text.Replace(
                            "₱",
                            ""
                        )
                    )


                Dim totalPaidBefore As Decimal =
                    GetTotalPaid(
                        connection,
                        reservation.ID
                    )


                Dim totalAfterPayment As Decimal =
                    totalPaidBefore +
                    paymentAmount


                If totalAfterPayment >
                   reservationTotal Then

                    MessageBox.Show(
                        "The payment amount exceeds the " &
                        "remaining reservation balance.",
                        "Invalid Payment",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    )

                    Return

                End If


                Dim sql As String =
                    "INSERT INTO Payments " &
                    "(ReservationID, PaymentDate, PaymentAmount, " &
                    "PaymentMethod, PaymentStatus, " &
                    "ReferenceNumber, Notes) " &
                    "VALUES " &
                    "(@ReservationID, @PaymentDate, @PaymentAmount, " &
                    "@PaymentMethod, @PaymentStatus, " &
                    "@ReferenceNumber, @Notes)"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@ReservationID",
                        reservation.ID
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentDate",
                        dtpPaymentDate.Value.Date
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentAmount",
                        paymentAmount
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentMethod",
                        cboPaymentMethod.Text
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentStatus",
                        cboPaymentStatus.Text
                    )

                    command.Parameters.AddWithValue(
                        "@ReferenceNumber",
                        txtReferenceNumber.Text.Trim()
                    )

                    command.Parameters.AddWithValue(
                        "@Notes",
                        txtNotes.Text.Trim()
                    )

                    command.ExecuteNonQuery()
                    AuditLogger.Log("Changed payment data", "A payment record was created, updated, or otherwise changed.")

                End Using


                UpdateReservationPaymentStatus(
                    connection,
                    reservation.ID
                )

            End Using


            MessageBox.Show(
                "Payment recorded successfully.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadPayments()

            LoadReservationsIntoComboBox()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to record payment." &
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
    ' UPDATE PAYMENT
    '==========================================================

    Private Sub UpdatePaymentClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedPaymentID = 0 Then

            MessageBox.Show(
                "Please select a payment first.",
                "Update Payment",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        If Not ValidatePayment() Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim reservation As ReservationItem =
                    DirectCast(
                        cboReservation.SelectedItem,
                        ReservationItem
                    )


                Dim paymentAmount As Decimal =
                    Convert.ToDecimal(
                        txtPaymentAmount.Text.Trim()
                    )


                Dim reservationTotal As Decimal =
                    Convert.ToDecimal(
                        txtReservationTotal.Text.Replace(
                            "₱",
                            ""
                        )
                    )


                Dim totalPaidOtherPayments As Decimal =
                    GetTotalPaidExcludingCurrent(
                        connection,
                        reservation.ID,
                        selectedPaymentID
                    )


                If totalPaidOtherPayments +
                   paymentAmount >
                   reservationTotal Then

                    MessageBox.Show(
                        "The payment amount exceeds the " &
                        "remaining reservation balance.",
                        "Invalid Payment",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    )

                    Return

                End If


                Dim sql As String =
                    "UPDATE Payments SET " &
                    "ReservationID = @ReservationID, " &
                    "PaymentDate = @PaymentDate, " &
                    "PaymentAmount = @PaymentAmount, " &
                    "PaymentMethod = @PaymentMethod, " &
                    "PaymentStatus = @PaymentStatus, " &
                    "ReferenceNumber = @ReferenceNumber, " &
                    "Notes = @Notes " &
                    "WHERE PaymentID = @PaymentID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@ReservationID",
                        reservation.ID
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentDate",
                        dtpPaymentDate.Value.Date
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentAmount",
                        paymentAmount
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentMethod",
                        cboPaymentMethod.Text
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentStatus",
                        cboPaymentStatus.Text
                    )

                    command.Parameters.AddWithValue(
                        "@ReferenceNumber",
                        txtReferenceNumber.Text.Trim()
                    )

                    command.Parameters.AddWithValue(
                        "@Notes",
                        txtNotes.Text.Trim()
                    )

                    command.Parameters.AddWithValue(
                        "@PaymentID",
                        selectedPaymentID
                    )

                    command.ExecuteNonQuery()
                    AuditLogger.Log("Changed payment data", "A payment record was created, updated, or otherwise changed.")

                End Using


                UpdateReservationPaymentStatus(
                    connection,
                    reservation.ID
                )

            End Using


            MessageBox.Show(
                "Payment updated successfully.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadPayments()

            LoadReservationsIntoComboBox()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to update payment." &
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
    ' DELETE PAYMENT
    '==========================================================

    Private Sub DeletePaymentClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedPaymentID = 0 Then

            MessageBox.Show(
                "Please select a payment first.",
                "Delete Payment",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If


        Dim result As DialogResult =
            MessageBox.Show(
                "Are you sure you want to delete this payment?",
                "Delete Payment",
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


                Dim reservationID As Integer =
                    GetPaymentReservationID(
                        connection,
                        selectedPaymentID
                    )


                Dim sql As String =
                    "DELETE FROM Payments " &
                    "WHERE PaymentID = @PaymentID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@PaymentID",
                        selectedPaymentID
                    )

                    command.ExecuteNonQuery()
                    AuditLogger.Log("Changed payment data", "A payment record was created, updated, or otherwise changed.")

                End Using


                If reservationID > 0 Then

                    UpdateReservationPaymentStatus(
                        connection,
                        reservationID
                    )

                End If

            End Using


            MessageBox.Show(
                "Payment deleted successfully.",
                "Aurelia Grand Hotel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadPayments()

            LoadReservationsIntoComboBox()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to delete payment." &
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
    ' GET TOTAL PAID
    '==========================================================

    Private Function GetTotalPaid(
        connection As SqlConnection,
        reservationID As Integer
    ) As Decimal

        Dim sql As String =
            "SELECT ISNULL(SUM(PaymentAmount), 0) " &
            "FROM Payments " &
            "WHERE ReservationID = @ReservationID"


        Using command As New SqlCommand(
            sql,
            connection
        )

            command.Parameters.AddWithValue(
                "@ReservationID",
                reservationID
            )


            Return Convert.ToDecimal(
                command.ExecuteScalar()
            )

        End Using

    End Function


    '==========================================================
    ' GET TOTAL PAID EXCLUDING CURRENT PAYMENT
    '==========================================================

    Private Function GetTotalPaidExcludingCurrent(
        connection As SqlConnection,
        reservationID As Integer,
        paymentID As Integer
    ) As Decimal

        Dim sql As String =
            "SELECT ISNULL(SUM(PaymentAmount), 0) " &
            "FROM Payments " &
            "WHERE ReservationID = @ReservationID " &
            "AND PaymentID <> @PaymentID"


        Using command As New SqlCommand(
            sql,
            connection
        )

            command.Parameters.AddWithValue(
                "@ReservationID",
                reservationID
            )

            command.Parameters.AddWithValue(
                "@PaymentID",
                paymentID
            )


            Return Convert.ToDecimal(
                command.ExecuteScalar()
            )

        End Using

    End Function


    '==========================================================
    ' GET PAYMENT RESERVATION ID
    '==========================================================

    Private Function GetPaymentReservationID(
        connection As SqlConnection,
        paymentID As Integer
    ) As Integer

        Dim sql As String =
            "SELECT ReservationID " &
            "FROM Payments " &
            "WHERE PaymentID = @PaymentID"


        Using command As New SqlCommand(
            sql,
            connection
        )

            command.Parameters.AddWithValue(
                "@PaymentID",
                paymentID
            )


            Dim result =
                command.ExecuteScalar()


            If result Is Nothing OrElse
               result Is DBNull.Value Then

                Return 0

            End If


            Return Convert.ToInt32(
                result
            )

        End Using

    End Function


    '==========================================================
    ' UPDATE RESERVATION PAYMENT STATUS
    '==========================================================

    Private Sub UpdateReservationPaymentStatus(
        connection As SqlConnection,
        reservationID As Integer
    )

        Dim reservationTotal As Decimal =
            0D

        Dim totalPaid As Decimal =
            GetTotalPaid(
                connection,
                reservationID
            )


        Dim totalSQL As String =
            "SELECT TotalAmount " &
            "FROM Reservations " &
            "WHERE ReservationID = @ReservationID"


        Using command As New SqlCommand(
            totalSQL,
            connection
        )

            command.Parameters.AddWithValue(
                "@ReservationID",
                reservationID
            )


            Dim result =
                command.ExecuteScalar()


            If result IsNot Nothing AndAlso
               result IsNot DBNull.Value Then

                reservationTotal =
                    Convert.ToDecimal(
                        result
                    )

            End If

        End Using


        Dim paymentStatus As String =
            "Unpaid"


        If totalPaid >= reservationTotal AndAlso
           reservationTotal > 0 Then

            paymentStatus =
                "Paid"

        ElseIf totalPaid > 0 Then

            paymentStatus =
                "Partially Paid"

        End If


        Dim updateSQL As String =
            "UPDATE Reservations " &
            "SET PaymentStatus = @PaymentStatus " &
            "WHERE ReservationID = @ReservationID"


        Using command As New SqlCommand(
            updateSQL,
            connection
        )

            command.Parameters.AddWithValue(
                "@PaymentStatus",
                paymentStatus
            )

            command.Parameters.AddWithValue(
                "@ReservationID",
                reservationID
            )

            command.ExecuteNonQuery()
                    AuditLogger.Log("Changed payment data", "A payment record was created, updated, or otherwise changed.")

        End Using

    End Sub


    '==========================================================
    ' VALIDATE PAYMENT
    '==========================================================

    Private Function ValidatePayment() As Boolean

        If cboReservation.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a reservation.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboReservation.Focus()

            Return False

        End If


        Dim amount As Decimal


        If Not Decimal.TryParse(
            txtPaymentAmount.Text.Trim(),
            amount
        ) OrElse amount <= 0 Then

            MessageBox.Show(
                "Please enter a valid payment amount.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtPaymentAmount.Focus()

            Return False

        End If


        If cboPaymentMethod.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a payment method.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboPaymentMethod.Focus()

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
    ' PAYMENT ROW CLICKED
    '==========================================================

    Private Sub PaymentRowClicked(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then

            Return

        End If


        Dim row As DataGridViewRow =
            dgvPayments.Rows(e.RowIndex)


        If row.Cells("PaymentID").Value Is Nothing Then

            Return

        End If


        selectedPaymentID =
            Convert.ToInt32(
                row.Cells("PaymentID").Value
            )


        txtPaymentID.Text =
            selectedPaymentID.ToString()


        Dim reservationID As Integer =
            Convert.ToInt32(
                row.Cells("ReservationID").Value
            )


        SelectReservationByID(
            reservationID
        )


        dtpPaymentDate.Value =
            Convert.ToDateTime(
                row.Cells("PaymentDate").Value
            )


        txtPaymentAmount.Text =
            Convert.ToDecimal(
                row.Cells("PaymentAmount").Value
            ).ToString("0.00")


        cboPaymentMethod.Text =
            row.Cells(
                "PaymentMethod"
            ).Value.ToString()


        cboPaymentStatus.Text =
            row.Cells(
                "PaymentStatus"
            ).Value.ToString()


        If row.Cells(
            "ReferenceNumber"
        ).Value Is DBNull.Value Then

            txtReferenceNumber.Clear()

        Else

            txtReferenceNumber.Text =
                row.Cells(
                    "ReferenceNumber"
                ).Value.ToString()

        End If


        If row.Cells(
            "Notes"
        ).Value Is DBNull.Value Then

            txtNotes.Clear()

        Else

            txtNotes.Text =
                row.Cells(
                    "Notes"
                ).Value.ToString()

        End If


        LoadTotalPaid(
            reservationID
        )

    End Sub


    '==========================================================
    ' SELECT RESERVATION
    '==========================================================

    Private Sub SelectReservationByID(
        reservationID As Integer
    )

        For index As Integer =
            0 To cboReservation.Items.Count - 1

            Dim item As ReservationItem =
                DirectCast(
                    cboReservation.Items(index),
                    ReservationItem
                )


            If item.ID = reservationID Then

                cboReservation.SelectedIndex =
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

        SearchPayments()

    End Sub


    '==========================================================
    ' SEARCH KEY
    '==========================================================

    Private Sub SearchKeyDown(
        sender As Object,
        e As KeyEventArgs
    )

        If e.KeyCode = Keys.Enter Then

            SearchPayments()

            e.SuppressKeyPress = True

        End If

    End Sub


    '==========================================================
    ' SEARCH PAYMENTS
    '==========================================================

    Private Sub SearchPayments()

        If txtSearch.Text.Trim() = "" Then

            LoadPayments()

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "p.PaymentID, " &
                    "p.ReservationID, " &
                    "g.FullName AS GuestName, " &
                    "rm.RoomNumber, " &
                    "p.PaymentDate, " &
                    "p.PaymentAmount, " &
                    "p.PaymentMethod, " &
                    "p.PaymentStatus, " &
                    "p.ReferenceNumber, " &
                    "p.Notes " &
                    "FROM Payments p " &
                    "INNER JOIN Reservations r " &
                    "ON p.ReservationID = r.ReservationID " &
                    "INNER JOIN Guests g " &
                    "ON r.GuestID = g.GuestID " &
                    "INNER JOIN Rooms rm " &
                    "ON r.RoomID = rm.RoomID " &
                    "WHERE " &
                    "g.FullName LIKE @Search " &
                    "OR rm.RoomNumber LIKE @Search " &
                    "OR p.PaymentMethod LIKE @Search " &
                    "OR p.PaymentStatus LIKE @Search " &
                    "OR p.ReferenceNumber LIKE @Search " &
                    "ORDER BY p.PaymentID DESC"


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

                        dgvPayments.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to search payments." &
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

        LoadReservationsIntoComboBox()

        LoadPayments()

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

        selectedPaymentID = 0

        txtPaymentID.Clear()

        cboReservation.SelectedIndex = -1

        txtGuestName.Clear()

        txtRoomNumber.Clear()

        txtReservationTotal.Clear()

        dtpPaymentDate.Value =
            Date.Today

        txtPaymentAmount.Clear()

        txtTotalPaid.Text =
            "₱0.00"

        txtBalance.Text =
            "₱0.00"

        cboPaymentMethod.SelectedIndex = -1

        cboPaymentStatus.SelectedIndex = -1

        txtReferenceNumber.Clear()

        txtNotes.Clear()

        If dgvPayments IsNot Nothing Then

            dgvPayments.ClearSelection()

        End If

    End Sub

End Class