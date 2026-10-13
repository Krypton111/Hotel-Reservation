Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class PropertiesForm
    Inherits Form

    '==========================================================
    ' COZY COFFEE COLORS
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
    ' FORM CONTROLS
    '==========================================================

    Private lblTitle As Label
    Private lblSubtitle As Label

    Private txtSearch As TextBox
    Private btnSearch As Button
    Private btnRefresh As Button

    Private txtPropertyID As TextBox
    Private txtPropertyName As TextBox
    Private cboPropertyType As ComboBox

    Private txtTenantName As TextBox
    Private txtContactNumber As TextBox

    Private txtMonthlyRent As TextBox
    Private txtSecurityDeposit As TextBox

    Private dtpLeaseStart As DateTimePicker
    Private dtpLeaseEnd As DateTimePicker

    Private cboPropertyStatus As ComboBox
    Private cboPaymentStatus As ComboBox

    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnClear As Button

    Private dgvProperties As DataGridView

    Private selectedPropertyID As Integer = 0


    '==========================================================
    ' FORM CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildForm()

        LoadProperties()

    End Sub


    '==========================================================
    ' BUILD FORM
    '==========================================================

    Private Sub BuildForm()

        Me.Text =
            "Casa Brew - Properties"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(1100, 700)

        Me.MinimumSize =
            New Size(1000, 650)

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

        lblTitle = New Label()

        lblTitle.Text =
            "Rental Properties"

        lblTitle.Location =
            New Point(25, 20)

        lblTitle.Size =
            New Size(500, 40)

        lblTitle.Font =
            New Font(
                "Georgia",
                21,
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
            "Manage your properties, tenants, leases, and rental status."

        lblSubtitle.Location =
            New Point(27, 58)

        lblSubtitle.AutoSize = True

        lblSubtitle.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        lblSubtitle.ForeColor =
            Coffee

        Me.Controls.Add(lblSubtitle)


        '======================================================
        ' SEARCH AREA
        '======================================================

        Dim searchLabel As Label =
            CreateLabel(
                "Search",
                New Point(25, 95)
            )

        Me.Controls.Add(searchLabel)


        txtSearch = New TextBox()

        txtSearch.Location =
            New Point(25, 120)

        txtSearch.Size =
            New Size(340, 32)

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
                New Point(375, 118),
                New Size(100, 36),
                Coffee
            )

        Me.Controls.Add(btnSearch)


        btnRefresh =
            CreateButton(
                "Refresh",
                New Point(485, 118),
                New Size(100, 36),
                Caramel
            )

        Me.Controls.Add(btnRefresh)


        AddHandler btnSearch.Click,
            AddressOf SearchClicked

        AddHandler btnRefresh.Click,
            AddressOf RefreshClicked


        '======================================================
        ' FORM CARD
        '======================================================

        Dim formPanel As New Panel()

        formPanel.Location =
            New Point(25, 170)

        formPanel.Size =
            New Size(1020, 260)

        formPanel.BackColor =
            CardColor

        Me.Controls.Add(formPanel)


        '======================================================
        ' PROPERTY ID
        '======================================================

        txtPropertyID =
            CreateTextBox()

        txtPropertyID.Location =
            New Point(20, 45)

        txtPropertyID.Size =
            New Size(145, 30)

        txtPropertyID.ReadOnly = True

        txtPropertyID.BackColor =
            Color.FromArgb(
                235,
                225,
                215
            )

        formPanel.Controls.Add(txtPropertyID)


        Dim lblPropertyID As Label =
            CreateLabel(
                "Property ID",
                New Point(20, 15)
            )

        formPanel.Controls.Add(lblPropertyID)


        '======================================================
        ' PROPERTY NAME / ADDRESS
        '======================================================

        txtPropertyName =
            CreateTextBox()

        txtPropertyName.Location =
            New Point(185, 45)

        txtPropertyName.Size =
            New Size(300, 30)

        formPanel.Controls.Add(txtPropertyName)


        Dim lblPropertyName As Label =
            CreateLabel(
                "Property Name / Address",
                New Point(185, 15)
            )

        formPanel.Controls.Add(lblPropertyName)


        '======================================================
        ' PROPERTY TYPE
        '======================================================

        cboPropertyType =
            CreateComboBox()

        cboPropertyType.Location =
            New Point(505, 45)

        cboPropertyType.Size =
            New Size(200, 30)

        cboPropertyType.Items.Add(
            "Apartment"
        )

        cboPropertyType.Items.Add(
            "House"
        )

        cboPropertyType.Items.Add(
            "Condominium"
        )

        cboPropertyType.Items.Add(
            "Room"
        )

        cboPropertyType.Items.Add(
            "Commercial Space"
        )

        formPanel.Controls.Add(cboPropertyType)


        Dim lblPropertyType As Label =
            CreateLabel(
                "Property Type",
                New Point(505, 15)
            )

        formPanel.Controls.Add(lblPropertyType)


        '======================================================
        ' TENANT NAME
        '======================================================

        txtTenantName =
            CreateTextBox()

        txtTenantName.Location =
            New Point(725, 45)

        txtTenantName.Size =
            New Size(270, 30)

        formPanel.Controls.Add(txtTenantName)


        Dim lblTenantName As Label =
            CreateLabel(
                "Tenant Name",
                New Point(725, 15)
            )

        formPanel.Controls.Add(lblTenantName)


        '======================================================
        ' CONTACT NUMBER
        '======================================================

        txtContactNumber =
            CreateTextBox()

        txtContactNumber.Location =
            New Point(20, 115)

        txtContactNumber.Size =
            New Size(200, 30)

        formPanel.Controls.Add(txtContactNumber)


        Dim lblContact As Label =
            CreateLabel(
                "Contact Number",
                New Point(20, 85)
            )

        formPanel.Controls.Add(lblContact)


        '======================================================
        ' MONTHLY RENT
        '======================================================

        txtMonthlyRent =
            CreateTextBox()

        txtMonthlyRent.Location =
            New Point(240, 115)

        txtMonthlyRent.Size =
            New Size(180, 30)

        formPanel.Controls.Add(txtMonthlyRent)


        Dim lblRent As Label =
            CreateLabel(
                "Monthly Rent",
                New Point(240, 85)
            )

        formPanel.Controls.Add(lblRent)


        '======================================================
        ' SECURITY DEPOSIT
        '======================================================

        txtSecurityDeposit =
            CreateTextBox()

        txtSecurityDeposit.Location =
            New Point(440, 115)

        txtSecurityDeposit.Size =
            New Size(180, 30)

        formPanel.Controls.Add(txtSecurityDeposit)


        Dim lblDeposit As Label =
            CreateLabel(
                "Security Deposit",
                New Point(440, 85)
            )

        formPanel.Controls.Add(lblDeposit)


        '======================================================
        ' PROPERTY STATUS
        '======================================================

        cboPropertyStatus =
            CreateComboBox()

        cboPropertyStatus.Location =
            New Point(640, 115)

        cboPropertyStatus.Size =
            New Size(160, 30)

        cboPropertyStatus.Items.Add(
            "Available"
        )

        cboPropertyStatus.Items.Add(
            "Occupied"
        )

        cboPropertyStatus.Items.Add(
            "Reserved"
        )

        cboPropertyStatus.Items.Add(
            "Under Maintenance"
        )

        formPanel.Controls.Add(
            cboPropertyStatus
        )


        Dim lblPropertyStatus As Label =
            CreateLabel(
                "Property Status",
                New Point(640, 85)
            )

        formPanel.Controls.Add(
            lblPropertyStatus
        )


        '======================================================
        ' PAYMENT STATUS
        '======================================================

        cboPaymentStatus =
            CreateComboBox()

        cboPaymentStatus.Location =
            New Point(820, 115)

        cboPaymentStatus.Size =
            New Size(175, 30)

        cboPaymentStatus.Items.Add(
            "Paid"
        )

        cboPaymentStatus.Items.Add(
            "Unpaid"
        )

        cboPaymentStatus.Items.Add(
            "Partially Paid"
        )

        formPanel.Controls.Add(
            cboPaymentStatus
        )


        Dim lblPaymentStatus As Label =
            CreateLabel(
                "Payment Status",
                New Point(820, 85)
            )

        formPanel.Controls.Add(
            lblPaymentStatus
        )


        '======================================================
        ' LEASE START
        '======================================================

        dtpLeaseStart =
            CreateDatePicker()

        dtpLeaseStart.Location =
            New Point(20, 195)

        dtpLeaseStart.Size =
            New Size(200, 30)

        formPanel.Controls.Add(
            dtpLeaseStart
        )


        Dim lblLeaseStart As Label =
            CreateLabel(
                "Lease Start Date",
                New Point(20, 165)
            )

        formPanel.Controls.Add(
            lblLeaseStart
        )


        '======================================================
        ' LEASE END
        '======================================================

        dtpLeaseEnd =
            CreateDatePicker()

        dtpLeaseEnd.Location =
            New Point(240, 195)

        dtpLeaseEnd.Size =
            New Size(200, 30)

        formPanel.Controls.Add(
            dtpLeaseEnd
        )


        Dim lblLeaseEnd As Label =
            CreateLabel(
                "Lease End Date",
                New Point(240, 165)
            )

        formPanel.Controls.Add(
            lblLeaseEnd
        )


        '======================================================
        ' BUTTONS
        '======================================================

        btnAdd =
            CreateButton(
                "Add Property",
                New Point(650, 185),
                New Size(105, 38),
                Coffee
            )

        formPanel.Controls.Add(btnAdd)


        btnUpdate =
            CreateButton(
                "Update",
                New Point(765, 185),
                New Size(105, 38),
                Caramel
            )

        formPanel.Controls.Add(btnUpdate)


        btnDelete =
            CreateButton(
                "Delete",
                New Point(880, 185),
                New Size(105, 38),
                Color.FromArgb(
                    145,
                    75,
                    65
                )
            )

        formPanel.Controls.Add(btnDelete)


        btnClear =
            CreateButton(
                "Clear",
                New Point(535, 185),
                New Size(105, 38),
                Sage
            )

        formPanel.Controls.Add(btnClear)


        AddHandler btnAdd.Click,
            AddressOf AddPropertyClicked

        AddHandler btnUpdate.Click,
            AddressOf UpdatePropertyClicked

        AddHandler btnDelete.Click,
            AddressOf DeletePropertyClicked

        AddHandler btnClear.Click,
            AddressOf ClearClicked


        '======================================================
        ' DATA GRID
        '======================================================

        dgvProperties =
            New DataGridView()

        dgvProperties.Location =
            New Point(25, 450)

        dgvProperties.Size =
            New Size(1020, 190)

        dgvProperties.BackgroundColor =
            CardColor

        dgvProperties.BorderStyle =
            BorderStyle.None

        dgvProperties.ReadOnly = True

        dgvProperties.AllowUserToAddRows =
            False

        dgvProperties.AllowUserToDeleteRows =
            False

        dgvProperties.RowHeadersVisible =
            False

        dgvProperties.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvProperties.MultiSelect =
            False

        dgvProperties.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvProperties.EnableHeadersVisualStyles =
            False

        dgvProperties.ColumnHeadersDefaultCellStyle.BackColor =
            CoffeeDark

        dgvProperties.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        dgvProperties.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            )

        dgvProperties.DefaultCellStyle.BackColor =
            CardColor

        dgvProperties.DefaultCellStyle.ForeColor =
            TextDark

        dgvProperties.DefaultCellStyle.SelectionBackColor =
            Latte

        dgvProperties.DefaultCellStyle.SelectionForeColor =
            TextDark

        Me.Controls.Add(
            dgvProperties
        )


        AddHandler dgvProperties.CellClick,
            AddressOf PropertyRowClicked

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
            TextDark

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
            TextDark

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
            TextDark

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
    ' LOAD PROPERTIES
    '==========================================================

    Private Sub LoadProperties()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "PropertyID, " &
                    "PropertyName, " &
                    "PropertyType, " &
                    "TenantName, " &
                    "ContactNumber, " &
                    "MonthlyRent, " &
                    "SecurityDeposit, " &
                    "LeaseStartDate, " &
                    "LeaseEndDate, " &
                    "PropertyStatus, " &
                    "PaymentStatus " &
                    "FROM Properties " &
                    "WHERE IsActive = 1 " &
                    "ORDER BY PropertyID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvProperties.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load properties." &
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

        If dgvProperties.Columns.Count = 0 Then

            Return

        End If


        If dgvProperties.Columns.Contains(
            "PropertyID"
        ) Then

            dgvProperties.Columns(
                "PropertyID"
            ).HeaderText =
                "ID"

        End If


        If dgvProperties.Columns.Contains(
            "PropertyName"
        ) Then

            dgvProperties.Columns(
                "PropertyName"
            ).HeaderText =
                "Property / Address"

        End If


        If dgvProperties.Columns.Contains(
            "PropertyType"
        ) Then

            dgvProperties.Columns(
                "PropertyType"
            ).HeaderText =
                "Type"

        End If


        If dgvProperties.Columns.Contains(
            "TenantName"
        ) Then

            dgvProperties.Columns(
                "TenantName"
            ).HeaderText =
                "Tenant"

        End If


        If dgvProperties.Columns.Contains(
            "ContactNumber"
        ) Then

            dgvProperties.Columns(
                "ContactNumber"
            ).HeaderText =
                "Contact"

        End If


        If dgvProperties.Columns.Contains(
            "MonthlyRent"
        ) Then

            dgvProperties.Columns(
                "MonthlyRent"
            ).HeaderText =
                "Monthly Rent"

        End If


        If dgvProperties.Columns.Contains(
            "SecurityDeposit"
        ) Then

            dgvProperties.Columns(
                "SecurityDeposit"
            ).HeaderText =
                "Deposit"

        End If


        If dgvProperties.Columns.Contains(
            "LeaseStartDate"
        ) Then

            dgvProperties.Columns(
                "LeaseStartDate"
            ).HeaderText =
                "Lease Start"

        End If


        If dgvProperties.Columns.Contains(
            "LeaseEndDate"
        ) Then

            dgvProperties.Columns(
                "LeaseEndDate"
            ).HeaderText =
                "Lease End"

        End If


        If dgvProperties.Columns.Contains(
            "PropertyStatus"
        ) Then

            dgvProperties.Columns(
                "PropertyStatus"
            ).HeaderText =
                "Property Status"

        End If


        If dgvProperties.Columns.Contains(
            "PaymentStatus"
        ) Then

            dgvProperties.Columns(
                "PaymentStatus"
            ).HeaderText =
                "Payment Status"

        End If


        dgvProperties.ClearSelection()

    End Sub


    '==========================================================
    ' ADD PROPERTY
    '==========================================================

    Private Sub AddPropertyClicked(
        sender As Object,
        e As EventArgs
    )

        If Not ValidateFields() Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "INSERT INTO Properties " &
                    "(" &
                    "PropertyName, " &
                    "PropertyType, " &
                    "TenantName, " &
                    "ContactNumber, " &
                    "MonthlyRent, " &
                    "SecurityDeposit, " &
                    "LeaseStartDate, " &
                    "LeaseEndDate, " &
                    "PropertyStatus, " &
                    "PaymentStatus, " &
                    "IsActive" &
                    ") " &
                    "VALUES " &
                    "(" &
                    "@PropertyName, " &
                    "@PropertyType, " &
                    "@TenantName, " &
                    "@ContactNumber, " &
                    "@MonthlyRent, " &
                    "@SecurityDeposit, " &
                    "@LeaseStartDate, " &
                    "@LeaseEndDate, " &
                    "@PropertyStatus, " &
                    "@PaymentStatus, " &
                    "1" &
                    ")"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddParameters(command)


                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Property added successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadProperties()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to add the property." &
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
    ' UPDATE PROPERTY
    '==========================================================

    Private Sub UpdatePropertyClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedPropertyID = 0 Then

            MessageBox.Show(
                "Please select a property first.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            Return

        End If


        If Not ValidateFields() Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "UPDATE Properties SET " &
                    "PropertyName = @PropertyName, " &
                    "PropertyType = @PropertyType, " &
                    "TenantName = @TenantName, " &
                    "ContactNumber = @ContactNumber, " &
                    "MonthlyRent = @MonthlyRent, " &
                    "SecurityDeposit = @SecurityDeposit, " &
                    "LeaseStartDate = @LeaseStartDate, " &
                    "LeaseEndDate = @LeaseEndDate, " &
                    "PropertyStatus = @PropertyStatus, " &
                    "PaymentStatus = @PaymentStatus " &
                    "WHERE PropertyID = @PropertyID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddParameters(command)


                    command.Parameters.Add(
                        "@PropertyID",
                        SqlDbType.Int
                    ).Value =
                        selectedPropertyID


                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Property updated successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadProperties()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to update the property." &
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
    ' DELETE PROPERTY
    '==========================================================

    Private Sub DeletePropertyClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedPropertyID = 0 Then

            MessageBox.Show(
                "Please select a property first.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            Return

        End If


        Dim answer As DialogResult =
            MessageBox.Show(
                "Are you sure you want to remove this property?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )


        If answer =
            DialogResult.No Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                '------------------------------------------------
                ' SOFT DELETE
                '------------------------------------------------

                Dim sql As String =
                    "UPDATE Properties " &
                    "SET IsActive = 0 " &
                    "WHERE PropertyID = @PropertyID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.Add(
                        "@PropertyID",
                        SqlDbType.Int
                    ).Value =
                        selectedPropertyID


                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Property removed successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadProperties()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to delete the property." &
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
    ' SEARCH
    '==========================================================

    Private Sub SearchClicked(
        sender As Object,
        e As EventArgs
    )

        Dim searchText As String =
            txtSearch.Text.Trim()


        If searchText = "" Then

            LoadProperties()

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "PropertyID, " &
                    "PropertyName, " &
                    "PropertyType, " &
                    "TenantName, " &
                    "ContactNumber, " &
                    "MonthlyRent, " &
                    "SecurityDeposit, " &
                    "LeaseStartDate, " &
                    "LeaseEndDate, " &
                    "PropertyStatus, " &
                    "PaymentStatus " &
                    "FROM Properties " &
                    "WHERE IsActive = 1 " &
                    "AND (" &
                    "CAST(PropertyID AS NVARCHAR(20)) LIKE @Search " &
                    "OR PropertyName LIKE @Search " &
                    "OR PropertyType LIKE @Search " &
                    "OR ISNULL(TenantName, '') LIKE @Search " &
                    "OR ISNULL(ContactNumber, '') LIKE @Search " &
                    "OR PropertyStatus LIKE @Search " &
                    "OR PaymentStatus LIKE @Search" &
                    ") " &
                    "ORDER BY PropertyID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.Add(
                        "@Search",
                        SqlDbType.NVarChar,
                        200
                    ).Value =
                        "%" & searchText & "%"


                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvProperties.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


        Catch ex As Exception

            MessageBox.Show(
                "Search failed." &
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

        LoadProperties()

        ClearFields()

    End Sub


    '==========================================================
    ' SELECT ROW
    '==========================================================

    Private Sub PropertyRowClicked(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then

            Return

        End If


        Dim row As DataGridViewRow =
            dgvProperties.Rows(
                e.RowIndex
            )


        If row.Cells("PropertyID").Value Is Nothing Then

            Return

        End If


        If IsDBNull(
            row.Cells("PropertyID").Value
        ) Then

            Return

        End If


        selectedPropertyID =
            Convert.ToInt32(
                row.Cells("PropertyID").Value
            )


        txtPropertyID.Text =
            selectedPropertyID.ToString()


        txtPropertyName.Text =
            GetCellString(
                row,
                "PropertyName"
            )


        cboPropertyType.Text =
            GetCellString(
                row,
                "PropertyType"
            )


        txtTenantName.Text =
            GetCellString(
                row,
                "TenantName"
            )


        txtContactNumber.Text =
            GetCellString(
                row,
                "ContactNumber"
            )


        txtMonthlyRent.Text =
            GetCellString(
                row,
                "MonthlyRent"
            )


        txtSecurityDeposit.Text =
            GetCellString(
                row,
                "SecurityDeposit"
            )


        cboPropertyStatus.Text =
            GetCellString(
                row,
                "PropertyStatus"
            )


        cboPaymentStatus.Text =
            GetCellString(
                row,
                "PaymentStatus"
            )


        If Not IsDBNull(
            row.Cells("LeaseStartDate").Value
        ) Then

            dtpLeaseStart.Value =
                Convert.ToDateTime(
                    row.Cells("LeaseStartDate").Value
                )

        End If


        If Not IsDBNull(
            row.Cells("LeaseEndDate").Value
        ) Then

            dtpLeaseEnd.Value =
                Convert.ToDateTime(
                    row.Cells("LeaseEndDate").Value
                )

        End If

    End Sub


    '==========================================================
    ' GET GRID CELL TEXT
    '==========================================================

    Private Function GetCellString(
        row As DataGridViewRow,
        columnName As String
    ) As String

        If row.Cells(columnName).Value Is Nothing Then

            Return ""

        End If


        If IsDBNull(
            row.Cells(columnName).Value
        ) Then

            Return ""

        End If


        Return row.Cells(
            columnName
        ).Value.ToString()

    End Function


    '==========================================================
    ' VALIDATE
    '==========================================================

    Private Function ValidateFields() As Boolean

        If txtPropertyName.Text.Trim() = "" Then

            MessageBox.Show(
                "Please enter the property name or address.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtPropertyName.Focus()

            Return False

        End If


        If cboPropertyType.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a property type.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboPropertyType.Focus()

            Return False

        End If


        If txtMonthlyRent.Text.Trim() = "" Then

            MessageBox.Show(
                "Please enter the monthly rent.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtMonthlyRent.Focus()

            Return False

        End If


        Dim monthlyRent As Decimal


        If Not Decimal.TryParse(
            txtMonthlyRent.Text.Trim(),
            monthlyRent
        ) Then

            MessageBox.Show(
                "Monthly rent must be a valid number.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtMonthlyRent.Focus()

            Return False

        End If


        If monthlyRent < 0 Then

            MessageBox.Show(
                "Monthly rent cannot be negative.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return False

        End If


        Dim deposit As Decimal


        If txtSecurityDeposit.Text.Trim() <> "" Then

            If Not Decimal.TryParse(
                txtSecurityDeposit.Text.Trim(),
                deposit
            ) Then

                MessageBox.Show(
                    "Security deposit must be a valid number.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                txtSecurityDeposit.Focus()

                Return False

            End If

        End If


        If cboPropertyStatus.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a property status.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboPropertyStatus.Focus()

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


        If dtpLeaseEnd.Value <
           dtpLeaseStart.Value Then

            MessageBox.Show(
                "Lease end date cannot be earlier than the lease start date.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return False

        End If


        Return True

    End Function


    '==========================================================
    ' ADD SQL PARAMETERS
    '==========================================================

    Private Sub AddParameters(
        command As SqlCommand
    )

        command.Parameters.Add(
            "@PropertyName",
            SqlDbType.NVarChar,
            200
        ).Value =
            txtPropertyName.Text.Trim()


        command.Parameters.Add(
            "@PropertyType",
            SqlDbType.NVarChar,
            50
        ).Value =
            cboPropertyType.Text


        If txtTenantName.Text.Trim() = "" Then

            command.Parameters.Add(
                "@TenantName",
                SqlDbType.NVarChar,
                150
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@TenantName",
                SqlDbType.NVarChar,
                150
            ).Value =
                txtTenantName.Text.Trim()

        End If


        If txtContactNumber.Text.Trim() = "" Then

            command.Parameters.Add(
                "@ContactNumber",
                SqlDbType.NVarChar,
                30
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@ContactNumber",
                SqlDbType.NVarChar,
                30
            ).Value =
                txtContactNumber.Text.Trim()

        End If


        command.Parameters.Add(
            "@MonthlyRent",
            SqlDbType.Decimal
        ).Value =
            Decimal.Parse(
                txtMonthlyRent.Text.Trim()
            )


        Dim deposit As Decimal = 0


        If txtSecurityDeposit.Text.Trim() <> "" Then

            Decimal.TryParse(
                txtSecurityDeposit.Text.Trim(),
                deposit
            )

        End If


        command.Parameters.Add(
            "@SecurityDeposit",
            SqlDbType.Decimal
        ).Value =
            deposit


        command.Parameters.Add(
            "@LeaseStartDate",
            SqlDbType.Date
        ).Value =
            dtpLeaseStart.Value.Date


        command.Parameters.Add(
            "@LeaseEndDate",
            SqlDbType.Date
        ).Value =
            dtpLeaseEnd.Value.Date


        command.Parameters.Add(
            "@PropertyStatus",
            SqlDbType.NVarChar,
            50
        ).Value =
            cboPropertyStatus.Text


        command.Parameters.Add(
            "@PaymentStatus",
            SqlDbType.NVarChar,
            50
        ).Value =
            cboPaymentStatus.Text

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

        selectedPropertyID = 0

        txtPropertyID.Clear()

        txtPropertyName.Clear()

        cboPropertyType.SelectedIndex = -1

        txtTenantName.Clear()

        txtContactNumber.Clear()

        txtMonthlyRent.Clear()

        txtSecurityDeposit.Clear()

        cboPropertyStatus.SelectedIndex = -1

        cboPaymentStatus.SelectedIndex = -1

        dtpLeaseStart.Value =
            Date.Today

        dtpLeaseEnd.Value =
            Date.Today


        dgvProperties.ClearSelection()

    End Sub

End Class