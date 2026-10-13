Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class MainForm
    Inherits Form

    '===========================================================
    ' MAIN LAYOUT
    '===========================================================

    Private pnlNavigation As Panel
    Private pnlContent As Panel
    Private pnlPageHeader As Panel
    Private pnlPageHost As Panel

    Private lblPageTitle As Label
    Private lblUser As Label

    Private lblHotelName As Label
    Private lblHotelSubtitle As Label


    '===========================================================
    ' NAVIGATION BUTTONS
    '===========================================================

    Private btnDashboard As Button
    Private btnReservations As Button
    Private btnGuests As Button
    Private btnRooms As Button
    Private btnPayments As Button
    Private btnReports As Button
    Private btnLogout As Button


    '===========================================================
    ' COLORS
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
    ' CONSTRUCTOR
    '===========================================================

    Public Sub New()

        BuildMainForm()

        ApplyRolePermissions()

        ShowDashboard()

    End Sub


    '===========================================================
    ' BUILD MAIN FORM
    '===========================================================

    Private Sub BuildMainForm()

        Me.Text =
            "Aurelia Grand Hotel"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(1400, 820)

        Me.MinimumSize =
            New Size(1150, 700)

        Me.BackColor =
            Ivory

        Me.Font =
            New Font(
                "Segoe UI",
                10.0F
            )


        '=======================================================
        ' MAIN CONTENT
        '=======================================================

        pnlContent = New Panel()

        pnlContent.Dock =
            DockStyle.Fill

        pnlContent.BackColor =
            Ivory

        Me.Controls.Add(
            pnlContent
        )


        '=======================================================
        ' LEFT NAVIGATION
        '=======================================================

        pnlNavigation = New Panel()

        pnlNavigation.Dock =
            DockStyle.Left

        pnlNavigation.Width =
            245

        pnlNavigation.BackColor =
            DeepNavy

        Me.Controls.Add(
            pnlNavigation
        )

        pnlNavigation.BringToFront()


        '=======================================================
        ' PAGE HEADER
        '=======================================================

        pnlPageHeader = New Panel()

        pnlPageHeader.Dock =
            DockStyle.Top

        pnlPageHeader.Height =
            75

        pnlPageHeader.BackColor =
            Ivory

        pnlContent.Controls.Add(
            pnlPageHeader
        )


        '=======================================================
        ' PAGE HOST
        '
        ' IMPORTANT:
        ' Child forms are placed HERE.
        ' They can never overlap the sidebar.
        '=======================================================

        pnlPageHost = New Panel()

        pnlPageHost.Dock =
            DockStyle.Fill

        pnlPageHost.BackColor =
            Ivory

        pnlPageHost.Padding =
            New Padding(
                0,
                0,
                0,
                0
            )

        pnlContent.Controls.Add(
            pnlPageHost
        )

        pnlPageHost.BringToFront()


        '=======================================================
        ' HOTEL LOGO
        '=======================================================

        lblHotelName = New Label()

        lblHotelName.Text =
            "AURELIA"

        lblHotelName.Location =
            New Point(
                25,
                24
            )

        lblHotelName.Size =
            New Size(
                195,
                38
            )

        lblHotelName.Font =
            New Font(
                "Georgia",
                21,
                FontStyle.Bold
            )

        lblHotelName.ForeColor =
            Color.White

        lblHotelName.TextAlign =
            ContentAlignment.MiddleCenter

        pnlNavigation.Controls.Add(
            lblHotelName
        )


        lblHotelSubtitle = New Label()

        lblHotelSubtitle.Text =
            "GRAND HOTEL"

        lblHotelSubtitle.Location =
            New Point(
                25,
                62
            )

        lblHotelSubtitle.Size =
            New Size(
                195,
                25
            )

        lblHotelSubtitle.Font =
            New Font(
                "Segoe UI Semibold",
                8.5F
            )

        lblHotelSubtitle.ForeColor =
            Champagne

        lblHotelSubtitle.TextAlign =
            ContentAlignment.MiddleCenter

        pnlNavigation.Controls.Add(
            lblHotelSubtitle
        )


        '=======================================================
        ' DECORATIVE LINE
        '=======================================================

        Dim logoLine As New Panel()

        logoLine.Location =
            New Point(
                40,
                98
            )

        logoLine.Size =
            New Size(
                165,
                1
            )

        logoLine.BackColor =
            Color.FromArgb(
                100,
                90,
                70
            )

        pnlNavigation.Controls.Add(
            logoLine
        )


        '=======================================================
        ' NAVIGATION BUTTONS
        '=======================================================

        btnDashboard =
            CreateNavigationButton(
                "Dashboard"
            )

        btnReservations =
            CreateNavigationButton(
                "Reservations"
            )

        btnGuests =
            CreateNavigationButton(
                "Guests"
            )

        btnRooms =
            CreateNavigationButton(
                "Rooms"
            )

        btnPayments =
            CreateNavigationButton(
                "Payments"
            )

        btnReports =
            CreateNavigationButton(
                "Reports"
            )

        btnLogout =
            CreateNavigationButton(
                "Logout"
            )


        '=======================================================
        ' NAVIGATION LOCATIONS
        '=======================================================

        btnDashboard.Location =
            New Point(
                20,
                130
            )

        btnReservations.Location =
            New Point(
                20,
                182
            )

        btnGuests.Location =
            New Point(
                20,
                234
            )

        btnRooms.Location =
            New Point(
                20,
                286
            )

        btnPayments.Location =
            New Point(
                20,
                338
            )

        btnReports.Location =
            New Point(
                20,
                390
            )


        btnLogout.Anchor =
            AnchorStyles.Left Or
            AnchorStyles.Bottom

        btnLogout.Location =
            New Point(
                20,
                pnlNavigation.ClientSize.Height -
                btnLogout.Height -
                25
            )


        '=======================================================
        ' ADD NAVIGATION BUTTONS
        '=======================================================

        pnlNavigation.Controls.Add(
            btnDashboard
        )

        pnlNavigation.Controls.Add(
            btnReservations
        )

        pnlNavigation.Controls.Add(
            btnGuests
        )

        pnlNavigation.Controls.Add(
            btnRooms
        )

        pnlNavigation.Controls.Add(
            btnPayments
        )

        pnlNavigation.Controls.Add(
            btnReports
        )

        pnlNavigation.Controls.Add(
            btnLogout
        )


        '=======================================================
        ' PAGE TITLE
        '=======================================================

        lblPageTitle = New Label()

        lblPageTitle.Location =
            New Point(
                28,
                15
            )

        lblPageTitle.Size =
            New Size(
                700,
                45
            )

        lblPageTitle.Font =
            New Font(
                "Georgia",
                22,
                FontStyle.Bold
            )

        lblPageTitle.ForeColor =
            Charcoal

        pnlPageHeader.Controls.Add(
            lblPageTitle
        )


        '=======================================================
        ' CURRENT USER
        '=======================================================

        lblUser = New Label()

        lblUser.Anchor =
            AnchorStyles.Top Or
            AnchorStyles.Right

        lblUser.Location =
            New Point(
                700,
                20
            )

        lblUser.Size =
            New Size(
                600,
                35
            )

        lblUser.TextAlign =
            ContentAlignment.MiddleRight

        lblUser.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        lblUser.ForeColor =
            MutedGray

        pnlPageHeader.Controls.Add(
            lblUser
        )


        '=======================================================
        ' NAVIGATION EVENTS
        '=======================================================

        AddHandler btnDashboard.Click,
            AddressOf DashboardClicked

        AddHandler btnReservations.Click,
            AddressOf ReservationsClicked

        AddHandler btnGuests.Click,
            AddressOf GuestsClicked

        AddHandler btnRooms.Click,
            AddressOf RoomsClicked

        AddHandler btnPayments.Click,
            AddressOf PaymentsClicked

        AddHandler btnReports.Click,
            AddressOf ReportsClicked

        AddHandler btnLogout.Click,
            AddressOf LogoutClicked

    End Sub


    '===========================================================
    ' NAVIGATION BUTTON
    '===========================================================

    Private Function CreateNavigationButton(
        buttonText As String
    ) As Button

        Dim button As New Button()

        button.Text =
            buttonText

        button.Size =
            New Size(
                205,
                45
            )

        button.FlatStyle =
            FlatStyle.Flat

        button.FlatAppearance.BorderSize =
            0

        button.BackColor =
            DeepNavy

        button.ForeColor =
            Color.FromArgb(
                232,
                227,
                217
            )

        button.TextAlign =
            ContentAlignment.MiddleLeft

        button.Padding =
            New Padding(
                18,
                0,
                0,
                0
            )

        button.Font =
            New Font(
                "Segoe UI Semibold",
                9.5F
            )

        button.Cursor =
            Cursors.Hand

        button.UseVisualStyleBackColor =
            False

        button.Anchor =
            AnchorStyles.Left


        AddHandler button.MouseEnter,
            Sub(sender As Object, e As EventArgs)

                Dim currentButton =
                    DirectCast(
                        sender,
                        Button
                    )

                currentButton.BackColor =
                    Color.FromArgb(
                        40,
                        52,
                        68
                    )

                currentButton.ForeColor =
                    SoftGold

            End Sub


        AddHandler button.MouseLeave,
            Sub(sender As Object, e As EventArgs)

                Dim currentButton =
                    DirectCast(
                        sender,
                        Button
                    )

                currentButton.BackColor =
                    DeepNavy

                currentButton.ForeColor =
                    Color.FromArgb(
                        232,
                        227,
                        217
                    )

            End Sub


        Return button

    End Function


    '===========================================================
    ' ROLE PERMISSIONS
    '===========================================================

    Private Sub ApplyRolePermissions()

        Dim role As String =
            LoginForm.LoggedInRole.Trim()


        btnDashboard.Visible =
            True

        btnReservations.Visible =
            False

        btnGuests.Visible =
            False

        btnRooms.Visible =
            False

        btnPayments.Visible =
            False

        btnReports.Visible =
            False


        If role.Equals(
            "Admin",
            StringComparison.OrdinalIgnoreCase
        ) Then

            btnReservations.Visible =
                True

            btnGuests.Visible =
                True

            btnRooms.Visible =
                True

            btnPayments.Visible =
                True

            btnReports.Visible =
                True

            Return

        End If


        If role.Equals(
            "Manager",
            StringComparison.OrdinalIgnoreCase
        ) Then

            btnReservations.Visible =
                True

            btnGuests.Visible =
                True

            btnRooms.Visible =
                True

            btnPayments.Visible =
                True

            btnReports.Visible =
                True

            Return

        End If


        If role.Equals(
            "Receptionist",
            StringComparison.OrdinalIgnoreCase
        ) Then

            btnReservations.Visible =
                True

            btnGuests.Visible =
                True

            btnRooms.Visible =
                True

            btnPayments.Visible =
                True

            Return

        End If


        If role.Equals(
            "Staff",
            StringComparison.OrdinalIgnoreCase
        ) Then

            btnGuests.Visible =
                True

            btnRooms.Visible =
                True

            Return

        End If

    End Sub


    '===========================================================
    ' DASHBOARD
    '===========================================================

    Private Sub DashboardClicked(
        sender As Object,
        e As EventArgs
    )

        ShowDashboard()

    End Sub


    Private Sub ShowDashboard()

        ClearPageHost()

        lblPageTitle.Text =
            "Dashboard"

        UpdateUserInformation()


        '-------------------------------------------------------
        ' WELCOME
        '-------------------------------------------------------

        Dim lblWelcome As New Label()

        lblWelcome.Text =
            "Welcome to Aurelia Grand Hotel"

        lblWelcome.Location =
            New Point(
                30,
                15
            )

        lblWelcome.AutoSize =
            True

        lblWelcome.Font =
            New Font(
                "Georgia",
                18,
                FontStyle.Bold
            )

        lblWelcome.ForeColor =
            Charcoal

        pnlPageHost.Controls.Add(
            lblWelcome
        )


        Dim lblDate As New Label()

        lblDate.Text =
            DateTime.Now.ToString(
                "dddd, MMMM dd, yyyy"
            )

        lblDate.Location =
            New Point(
                32,
                49
            )

        lblDate.AutoSize =
            True

        lblDate.Font =
            New Font(
                "Segoe UI",
                10
            )

        lblDate.ForeColor =
            MutedGray

        pnlPageHost.Controls.Add(
            lblDate
        )


        '-------------------------------------------------------
        ' STATISTICS
        '-------------------------------------------------------

        Dim totalRooms As Integer =
            GetRoomCount()

        Dim availableRooms As Integer =
            GetRoomStatusCount(
                "Available"
            )

        Dim occupiedRooms As Integer =
            GetRoomStatusCount(
                "Occupied"
            )

        Dim reservedRooms As Integer =
            GetRoomStatusCount(
                "Reserved"
            )

        Dim todayCheckIns As Integer =
            GetTodayCheckInCount()

        Dim todayCheckOuts As Integer =
            GetTodayCheckOutCount()

        Dim totalReservations As Integer =
            GetReservationCount()


        AddStatisticCard(
            "TOTAL ROOMS",
            totalRooms.ToString(),
            Champagne,
            30
        )

        AddStatisticCard(
            "AVAILABLE",
            availableRooms.ToString(),
            Color.FromArgb(
                92,
                118,
                98
            ),
            225
        )

        AddStatisticCard(
            "OCCUPIED",
            occupiedRooms.ToString(),
            Color.FromArgb(
                115,
                78,
                69
            ),
            420
        )

        AddStatisticCard(
            "RESERVED",
            reservedRooms.ToString(),
            Color.FromArgb(
                103,
                91,
                69
            ),
            615
        )

        AddStatisticCard(
            "CHECK-INS TODAY",
            todayCheckIns.ToString(),
            Color.FromArgb(
                78,
                96,
                112
            ),
            810
        )

        AddStatisticCard(
            "CHECK-OUTS TODAY",
            todayCheckOuts.ToString(),
            Color.FromArgb(
                122,
                101,
                75
            ),
            1005
        )


        '-------------------------------------------------------
        ' OVERVIEW PANEL
        '-------------------------------------------------------

        Dim infoPanel As New Panel()

        infoPanel.Location =
            New Point(
                30,
                160
            )

        infoPanel.Size =
            New Size(
                750,
                285
            )

        infoPanel.BackColor =
            WarmWhite

        infoPanel.BorderStyle =
            BorderStyle.FixedSingle

        pnlPageHost.Controls.Add(
            infoPanel
        )


        Dim infoTitle As New Label()

        infoTitle.Text =
            "Hotel Overview"

        infoTitle.Location =
            New Point(
                25,
                22
            )

        infoTitle.AutoSize =
            True

        infoTitle.Font =
            New Font(
                "Georgia",
                17,
                FontStyle.Bold
            )

        infoTitle.ForeColor =
            Charcoal

        infoPanel.Controls.Add(
            infoTitle
        )


        AddOverviewValue(
            infoPanel,
            "Total Reservations",
            totalReservations.ToString(),
            28
        )


        Dim occupancy As Decimal = 0D

        If totalRooms > 0 Then

            occupancy =
                (
                    occupiedRooms /
                    Convert.ToDecimal(
                        totalRooms
                    )
                ) *
                100D

        End If


        AddOverviewValue(
            infoPanel,
            "Current Room Occupancy",
            occupancy.ToString("0.0") & "%",
            250
        )


        AddOverviewValue(
            infoPanel,
            "Rooms Currently Available",
            availableRooms.ToString(),
            500
        )


        Dim divider As New Panel()

        divider.Location =
            New Point(
                28,
                155
            )

        divider.Size =
            New Size(
                690,
                1
            )

        divider.BackColor =
            SoftGray

        infoPanel.Controls.Add(
            divider
        )


        Dim noteLabel As New Label()

        noteLabel.Text =
            "Aurelia Grand Hotel" &
            Environment.NewLine &
            "Reservation and room management system" &
            Environment.NewLine &
            "Please keep room availability and reservation records updated."

        noteLabel.Location =
            New Point(
                28,
                180
            )

        noteLabel.Size =
            New Size(
                680,
                85
            )

        noteLabel.Font =
            New Font(
                "Segoe UI",
                10
            )

        noteLabel.ForeColor =
            MutedGray

        infoPanel.Controls.Add(
            noteLabel
        )


        '-------------------------------------------------------
        ' QUICK ACTIONS
        '-------------------------------------------------------

        Dim quickPanel As New Panel()

        quickPanel.Location =
            New Point(
                810,
                160
            )

        quickPanel.Size =
            New Size(
                350,
                285
            )

        quickPanel.BackColor =
            WarmWhite

        quickPanel.BorderStyle =
            BorderStyle.FixedSingle

        pnlPageHost.Controls.Add(
            quickPanel
        )


        Dim quickTitle As New Label()

        quickTitle.Text =
            "Quick Actions"

        quickTitle.Location =
            New Point(
                25,
                22
            )

        quickTitle.AutoSize =
            True

        quickTitle.Font =
            New Font(
                "Georgia",
                17,
                FontStyle.Bold
            )

        quickTitle.ForeColor =
            Charcoal

        quickPanel.Controls.Add(
            quickTitle
        )


        Dim quickY As Integer =
            70


        If btnReservations.Visible Then

            AddQuickAction(
                quickPanel,
                "New Reservation",
                quickY,
                AddressOf ReservationsClicked
            )

            quickY += 55

        End If


        If btnGuests.Visible Then

            AddQuickAction(
                quickPanel,
                "Manage Guests",
                quickY,
                AddressOf GuestsClicked
            )

            quickY += 55

        End If


        If btnRooms.Visible Then

            AddQuickAction(
                quickPanel,
                "Manage Rooms",
                quickY,
                AddressOf RoomsClicked
            )

            quickY += 55

        End If


        If btnPayments.Visible Then

            AddQuickAction(
                quickPanel,
                "Payments",
                quickY,
                AddressOf PaymentsClicked
            )

        End If

    End Sub


    '===========================================================
    ' STATISTIC CARD
    '===========================================================

    Private Sub AddStatisticCard(
        title As String,
        value As String,
        accent As Color,
        x As Integer
    )

        Dim panel As New Panel()

        panel.Location =
            New Point(
                x,
                90
            )

        panel.Size =
            New Size(
                180,
                115
            )

        panel.BackColor =
            WarmWhite

        panel.BorderStyle =
            BorderStyle.FixedSingle

        pnlPageHost.Controls.Add(
            panel
        )


        Dim accentPanel As New Panel()

        accentPanel.Location =
            New Point(
                0,
                0
            )

        accentPanel.Size =
            New Size(
                5,
                115
            )

        accentPanel.BackColor =
            accent

        panel.Controls.Add(
            accentPanel
        )


        Dim titleLabel As New Label()

        titleLabel.Text =
            title

        titleLabel.Location =
            New Point(
                20,
                18
            )

        titleLabel.AutoSize =
            True

        titleLabel.Font =
            New Font(
                "Segoe UI Semibold",
                8.5F
            )

        titleLabel.ForeColor =
            MutedGray

        panel.Controls.Add(
            titleLabel
        )


        Dim valueLabel As New Label()

        valueLabel.Text =
            value

        valueLabel.Location =
            New Point(
                20,
                45
            )

        valueLabel.AutoSize =
            True

        valueLabel.Font =
            New Font(
                "Georgia",
                27,
                FontStyle.Bold
            )

        valueLabel.ForeColor =
            Charcoal

        panel.Controls.Add(
            valueLabel
        )

    End Sub


    '===========================================================
    ' OVERVIEW VALUE
    '===========================================================

    Private Sub AddOverviewValue(
        parent As Panel,
        title As String,
        value As String,
        x As Integer
    )

        Dim titleLabel As New Label()

        titleLabel.Text =
            title

        titleLabel.Location =
            New Point(
                x,
                75
            )

        titleLabel.AutoSize =
            True

        titleLabel.Font =
            New Font(
                "Segoe UI Semibold",
                10
            )

        titleLabel.ForeColor =
            MutedGray

        parent.Controls.Add(
            titleLabel
        )


        Dim valueLabel As New Label()

        valueLabel.Text =
            value

        valueLabel.Location =
            New Point(
                x,
                100
            )

        valueLabel.AutoSize =
            True

        valueLabel.Font =
            New Font(
                "Georgia",
                22,
                FontStyle.Bold
            )

        valueLabel.ForeColor =
            If(
                title = "Current Room Occupancy",
                Champagne,
                Charcoal
            )

        parent.Controls.Add(
            valueLabel
        )

    End Sub


    '===========================================================
    ' QUICK ACTION
    '===========================================================

    Private Sub AddQuickAction(
        parent As Panel,
        text As String,
        y As Integer,
        handler As EventHandler
    )

        Dim button As New Button()

        button.Text =
            text

        button.Location =
            New Point(
                25,
                y
            )

        button.Size =
            New Size(
                295,
                42
            )

        button.BackColor =
            DeepNavy

        button.ForeColor =
            Color.White

        button.FlatStyle =
            FlatStyle.Flat

        button.FlatAppearance.BorderSize =
            0

        button.Font =
            New Font(
                "Segoe UI Semibold",
                9
            )

        button.TextAlign =
            ContentAlignment.MiddleLeft

        button.Padding =
            New Padding(
                15,
                0,
                0,
                0
            )

        button.Cursor =
            Cursors.Hand

        AddHandler button.Click,
            handler

        parent.Controls.Add(
            button
        )

    End Sub


    '===========================================================
    ' RESERVATIONS
    '===========================================================

    Private Sub ReservationsClicked(
        sender As Object,
        e As EventArgs
    )

        OpenPage(
            New ReservationsForm(),
            "Reservations"
        )

    End Sub


    '===========================================================
    ' GUESTS
    '===========================================================

    Private Sub GuestsClicked(
        sender As Object,
        e As EventArgs
    )

        OpenPage(
            New GuestsForm(),
            "Guest Management"
        )

    End Sub


    '===========================================================
    ' ROOMS
    '===========================================================

    Private Sub RoomsClicked(
        sender As Object,
        e As EventArgs
    )

        OpenPage(
            New RoomsForm(),
            "Room Management"
        )

    End Sub


    '===========================================================
    ' PAYMENTS
    '===========================================================

    Private Sub PaymentsClicked(
        sender As Object,
        e As EventArgs
    )

        OpenPage(
            New PaymentsForm(),
            "Payments"
        )

    End Sub


    '===========================================================
    ' REPORTS
    '===========================================================

    Private Sub ReportsClicked(
        sender As Object,
        e As EventArgs
    )

        OpenPage(
            New ReportsForm(),
            "Reports & Analytics"
        )

    End Sub


    '===========================================================
    ' OPEN CHILD PAGE
    '===========================================================

    Private Sub OpenPage(
        page As Form,
        pageTitle As String
    )

        ClearPageHost()

        lblPageTitle.Text =
            pageTitle

        UpdateUserInformation()


        page.TopLevel =
            False

        page.FormBorderStyle =
            FormBorderStyle.None

        page.Dock =
            DockStyle.Fill

        page.StartPosition =
            FormStartPosition.Manual

        page.Margin =
            New Padding(0)

        page.Padding =
            New Padding(0)


        '-------------------------------------------------------
        ' IMPORTANT:
        ' Add the child ONLY to pnlPageHost.
        '
        ' pnlPageHost is already inside pnlContent.
        ' pnlContent is already beside the sidebar.
        '-------------------------------------------------------

        pnlPageHost.Controls.Add(
            page
        )

        page.BringToFront()

        page.Show()

    End Sub


    '===========================================================
    ' CLEAR CURRENT PAGE
    '===========================================================

    Private Sub ClearPageHost()

        Dim controlsToRemove =
            pnlPageHost.Controls.Cast(Of Control)().
            ToArray()


        For Each control As Control In
            controlsToRemove

            pnlPageHost.Controls.Remove(
                control
            )


            If TypeOf control Is Form Then

                Dim childForm As Form =
                    DirectCast(
                        control,
                        Form
                    )

                childForm.Close()

                childForm.Dispose()

            Else

                control.Dispose()

            End If

        Next

    End Sub


    '===========================================================
    ' USER INFORMATION
    '===========================================================

    Private Sub UpdateUserInformation()

        lblUser.Text =
            LoginForm.LoggedInFullName &
            "  •  " &
            LoginForm.LoggedInRole

    End Sub


    '===========================================================
    ' DATABASE - TOTAL ROOMS
    '===========================================================

    Private Function GetRoomCount() As Integer

        Try

            Dim sql As String =
                "SELECT COUNT(*) FROM Rooms"


            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Return Convert.ToInt32(
                        command.ExecuteScalar()
                    )

                End Using

            End Using

        Catch

            Return 0

        End Try

    End Function


    '===========================================================
    ' DATABASE - ROOM STATUS
    '===========================================================

    Private Function GetRoomStatusCount(
        status As String
    ) As Integer

        Try

            Dim sql As String =
                "SELECT COUNT(*) " &
                "FROM Rooms " &
                "WHERE Status = @Status"


            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@Status",
                        status
                    )


                    Return Convert.ToInt32(
                        command.ExecuteScalar()
                    )

                End Using

            End Using

        Catch

            Return 0

        End Try

    End Function


    '===========================================================
    ' DATABASE - RESERVATION COUNT
    '===========================================================

    Private Function GetReservationCount() As Integer

        Try

            Dim sql As String =
                "SELECT COUNT(*) " &
                "FROM Reservations " &
                "WHERE ReservationStatus <> 'Cancelled'"


            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Return Convert.ToInt32(
                        command.ExecuteScalar()
                    )

                End Using

            End Using

        Catch

            Return 0

        End Try

    End Function


    '===========================================================
    ' DATABASE - TODAY'S CHECK-INS
    '===========================================================

    Private Function GetTodayCheckInCount() As Integer

        Try

            Dim sql As String =
                "SELECT COUNT(*) " &
                "FROM Reservations " &
                "WHERE CAST(CheckInDate AS DATE) = CAST(GETDATE() AS DATE) " &
                "AND ReservationStatus IN " &
                "('Reserved', 'Checked-In')"


            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Return Convert.ToInt32(
                        command.ExecuteScalar()
                    )

                End Using

            End Using

        Catch

            Return 0

        End Try

    End Function


    '===========================================================
    ' DATABASE - TODAY'S CHECK-OUTS
    '===========================================================

    Private Function GetTodayCheckOutCount() As Integer

        Try

            Dim sql As String =
                "SELECT COUNT(*) " &
                "FROM Reservations " &
                "WHERE CAST(CheckOutDate AS DATE) = CAST(GETDATE() AS DATE) " &
                "AND ReservationStatus = 'Checked-In'"


            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Return Convert.ToInt32(
                        command.ExecuteScalar()
                    )

                End Using

            End Using

        Catch

            Return 0

        End Try

    End Function


    '===========================================================
    ' LOGOUT
    '===========================================================

    Private Sub LogoutClicked(
        sender As Object,
        e As EventArgs
    )

        Dim result As DialogResult =
            MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )


        If result <> DialogResult.Yes Then

            Return

        End If


        LoginForm.LoggedInUserID =
            0

        LoginForm.LoggedInUsername =
            ""

        LoginForm.LoggedInFullName =
            ""

        LoginForm.LoggedInRole =
            ""


        Me.Close()

    End Sub


    '===========================================================
    ' RESIZE
    '===========================================================

    Protected Overrides Sub OnResize(
        e As EventArgs
    )

        MyBase.OnResize(e)


        If pnlNavigation Is Nothing Then
            Return
        End If


        If btnLogout Is Nothing Then
            Return
        End If


        btnLogout.Location =
            New Point(
                20,
                pnlNavigation.ClientSize.Height -
                btnLogout.Height -
                25
            )

    End Sub

End Class
