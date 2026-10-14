Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class SpecialRequestsForm
    Inherits Form

    Private ReadOnly isStaff As Boolean =
        LoginForm.LoggedInRole.Equals("Staff", StringComparison.OrdinalIgnoreCase)

    Private grid As DataGridView
    Private cboGuest As ComboBox
    Private txtRoom As TextBox
    Private guestTable As DataTable
    Private txtDetails As TextBox
    Private cboPriority As ComboBox
    Private cboStatus As ComboBox
    Private btnCreate As Button
    Private btnUpdateStatus As Button
    Private selectedRequestId As Integer = 0

    Private ReadOnly navy As Color = Color.FromArgb(20, 27, 38)
    Private ReadOnly ivory As Color = Color.FromArgb(247, 244, 237)
    Private ReadOnly gold As Color = Color.FromArgb(198, 166, 110)

    Public Sub New()
        Text = "Special Guest Requests | Aurelia Grand Hotel"
        Size = New Size(1200, 760)
        MinimumSize = New Size(950, 600)
        StartPosition = FormStartPosition.CenterParent
        BackColor = ivory
        Font = New Font("Segoe UI", 9.5F)

        Dim header As New Panel With {.Dock = DockStyle.Fill, .Margin = New Padding(0), .BackColor = navy}
        Dim title As New Label With {.Text = "SPECIAL GUEST REQUESTS", .ForeColor = Color.White, .Font = New Font("Georgia", 21, FontStyle.Bold), .Location = New Point(26, 14), .AutoSize = True}
        Dim subtitle As New Label With {.Text = If(isStaff, "View requests and update their progress.", "Create and track guest service requests."), .ForeColor = Color.Gainsboro, .Location = New Point(29, 52), .AutoSize = True}
        header.Controls.AddRange(New Control() {title, subtitle})

        Dim editor As New Panel With {.Dock = DockStyle.Fill, .Margin = New Padding(0), .Padding = New Padding(18), .BackColor = Color.FromArgb(253, 251, 247)}


        If Not isStaff Then
            editor.Controls.Add(MakeLabel("Guest name *", 18, 8))
            cboGuest = New ComboBox With {.Location = New Point(18, 31), .Size = New Size(270, 28), .DropDownStyle = ComboBoxStyle.DropDownList, .DisplayMember = "FullName", .ValueMember = "GuestID"}
            AddHandler cboGuest.SelectedIndexChanged, AddressOf GuestSelectionChanged
            editor.Controls.Add(cboGuest)

            editor.Controls.Add(MakeLabel("Room number (automatic)", 305, 8))
            txtRoom = New TextBox With {.Location = New Point(305, 31), .Size = New Size(165, 28), .ReadOnly = True, .BackColor = Color.FromArgb(239, 236, 228)}
            editor.Controls.Add(txtRoom)

            editor.Controls.Add(MakeLabel("Priority", 488, 8))
            cboPriority = New ComboBox With {.Location = New Point(488, 31), .Size = New Size(140, 28), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboPriority.Items.AddRange(New Object() {"Normal", "High", "Urgent"})
            cboPriority.SelectedIndex = 0
            editor.Controls.Add(cboPriority)

            editor.Controls.Add(MakeLabel("Request details *", 18, 67))
            txtDetails = New TextBox With {.Location = New Point(18, 89), .Size = New Size(680, 58), .Multiline = True, .MaxLength = 1000, .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right}
            editor.Controls.Add(txtDetails)
            btnCreate = MakeButton("CREATE REQUEST", 720, 95, gold)
            btnCreate.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            AddHandler btnCreate.Click, AddressOf CreateRequest
            editor.Controls.Add(btnCreate)
        Else
            editor.Controls.Add(MakeLabel("Selected request status", 18, 12))
            cboStatus = New ComboBox With {.Location = New Point(18, 38), .Size = New Size(220, 28), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboStatus.Items.AddRange(New Object() {"Pending", "In Progress", "Completed", "Cancelled"})
            cboStatus.SelectedIndex = 0
            editor.Controls.Add(cboStatus)
            btnUpdateStatus = MakeButton("UPDATE STATUS", 255, 35, gold)
            AddHandler btnUpdateStatus.Click, AddressOf UpdateStatus
            editor.Controls.Add(btnUpdateStatus)
        End If

        grid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .BackgroundColor = Color.White,
            .RowHeadersVisible = False,
            .Margin = New Padding(18)
        }
        AddHandler grid.CellClick, AddressOf GridCellClick
        Dim gridHost As New Panel With {.Dock = DockStyle.Fill, .Margin = New Padding(0), .Padding = New Padding(18, 12, 18, 18), .BackColor = ivory}
        gridHost.Controls.Add(grid)

        ' Use explicit rows so the header and request editor keep their space
        ' and the table fills only the remaining window height.
        Dim root As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .Margin = New Padding(0),
            .Padding = New Padding(0),
            .ColumnCount = 1,
            .RowCount = 3,
            .BackColor = ivory
        }
        root.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 82.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, If(isStaff, 82.0F, 178.0F)))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        root.Controls.Add(header, 0, 0)
        root.Controls.Add(editor, 0, 1)
        root.Controls.Add(gridHost, 0, 2)
        Controls.Add(root)

        If Not isStaff Then LoadGuestsForRequests()
        LoadRequests()
    End Sub

    Private Function MakeLabel(value As String, x As Integer, y As Integer) As Label
        Return New Label With {.Text = value, .Location = New Point(x, y), .AutoSize = True, .ForeColor = Color.FromArgb(90, 90, 85)}
    End Function

    Private Function MakeButton(value As String, x As Integer, y As Integer, color As Color) As Button
        Dim button As New Button()
        button.Text = value
        button.Location = New Point(x, y)
        button.Size = New Size(165, 38)
        button.FlatStyle = FlatStyle.Flat
        button.FlatAppearance.BorderSize = 0
        button.BackColor = color
        button.ForeColor = Color.White
        button.Cursor = Cursors.Hand
        Return button
    End Function

    Private Sub LoadGuestsForRequests()
        Try
            Using con = DatabaseHelper.GetConnection()
                Dim sql As String =
                    "SELECT g.GuestID, g.FullName, " &
                    "COALESCE((SELECT TOP (1) rm.RoomNumber " &
                    "FROM dbo.Reservations r INNER JOIN dbo.Rooms rm ON rm.RoomID = r.RoomID " &
                    "WHERE r.GuestID = g.GuestID AND ISNULL(r.IsArchived, 0) = 0 AND ISNULL(rm.IsArchived, 0) = 0 " &
                    "AND r.ReservationStatus <> 'Cancelled' " &
                    "ORDER BY CASE " &
                    "WHEN CAST(GETDATE() AS date) >= CAST(r.CheckInDate AS date) AND CAST(GETDATE() AS date) < CAST(r.CheckOutDate AS date) THEN 0 " &
                    "WHEN CAST(r.CheckInDate AS date) > CAST(GETDATE() AS date) THEN 1 ELSE 2 END, " &
                    "r.CheckInDate DESC, r.ReservationID DESC), '') AS RoomNumber " &
                    "FROM dbo.Guests g WHERE g.IsArchived = 0 ORDER BY g.FullName;"
                Using da As New SqlDataAdapter(sql, con)
                    guestTable = New DataTable()
                    da.Fill(guestTable)
                End Using
            End Using
            cboGuest.DataSource = Nothing
            cboGuest.DisplayMember = "FullName"
            cboGuest.ValueMember = "GuestID"
            cboGuest.DataSource = guestTable
            If guestTable.Rows.Count = 0 Then
                txtRoom.Text = ""
                MessageBox.Show("There are no active guest profiles to select.", "Special Requests", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cboGuest.SelectedIndex = 0
                UpdateRoomForSelectedGuest()
            End If
        Catch ex As Exception
            MessageBox.Show("Could not load guest list and room assignments." & Environment.NewLine & Environment.NewLine & ex.Message, "Special Requests", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GuestSelectionChanged(sender As Object, e As EventArgs)
        UpdateRoomForSelectedGuest()
    End Sub

    Private Sub UpdateRoomForSelectedGuest()
        If cboGuest Is Nothing OrElse cboGuest.SelectedIndex < 0 OrElse guestTable Is Nothing Then Return
        Dim rowView As DataRowView = TryCast(cboGuest.SelectedItem, DataRowView)
        If rowView Is Nothing Then Return
        Dim room = Convert.ToString(rowView("RoomNumber"))
        txtRoom.Text = If(String.IsNullOrWhiteSpace(room), "No current room", room)
    End Sub

    Private Sub LoadRequests()
        Try
            Using con = DatabaseHelper.GetConnection()
                Using da As New SqlDataAdapter(
                    "SELECT RequestID, RequestNumber, GuestName, RoomNumber, RequestDetails, Priority, Status, CreatedByUsername, CreatedAt, UpdatedAt FROM dbo.SpecialGuestRequests ORDER BY CASE Priority WHEN 'Urgent' THEN 0 WHEN 'High' THEN 1 ELSE 2 END, CASE Status WHEN 'Pending' THEN 0 WHEN 'In Progress' THEN 1 ELSE 2 END, CreatedAt DESC", con)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    grid.DataSource = dt
                    If grid.Columns.Contains("RequestID") Then grid.Columns("RequestID").Visible = False
                    If grid.Columns.Contains("RequestNumber") Then
                        grid.Columns("RequestNumber").HeaderText = "Queue Number"
                        grid.Columns("RequestNumber").FillWeight = 75
                    End If
                    If grid.Columns.Contains("CreatedAt") Then grid.Columns("CreatedAt").HeaderText = "Created"
                    If grid.Columns.Contains("UpdatedAt") Then grid.Columns("UpdatedAt").HeaderText = "Updated"
                    If grid.Columns.Contains("Status") Then grid.Columns("Status").FillWeight = 75
                    If grid.Columns.Contains("Priority") Then grid.Columns("Priority").FillWeight = 65
                    If grid.Columns.Contains("RoomNumber") Then grid.Columns("RoomNumber").FillWeight = 60
                    grid.RowTemplate.Height = 30
                    grid.ColumnHeadersHeight = 34
                    grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                    grid.ScrollBars = ScrollBars.Both
                    grid.Dock = DockStyle.Fill
                    If grid.Columns.Contains("RequestDetails") Then grid.Columns("RequestDetails").FillWeight = 220
                    If grid.Columns.Contains("CreatedByUsername") Then grid.Columns("CreatedByUsername").HeaderText = "Requested By"
                    If grid.Columns.Contains("GuestName") Then grid.Columns("GuestName").HeaderText = "Guest"
                    If grid.Columns.Contains("RoomNumber") Then grid.Columns("RoomNumber").HeaderText = "Room"
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Could not load special requests. Make sure SpecialRequestsMigration.sql has been run in SQL Server Management Studio." & Environment.NewLine & Environment.NewLine & ex.Message, "Special Requests", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CreateRequest(sender As Object, e As EventArgs)
        If cboGuest.SelectedIndex < 0 OrElse String.IsNullOrWhiteSpace(txtDetails.Text) Then
            MessageBox.Show("Select a guest and enter the request details.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim selectedGuest As DataRowView = TryCast(cboGuest.SelectedItem, DataRowView)
        If selectedGuest Is Nothing Then Return
        Dim selectedGuestName As String = Convert.ToString(selectedGuest("FullName"))
        Dim selectedRoom As String = Convert.ToString(selectedGuest("RoomNumber"))
        If String.Equals(selectedRoom, "No current room", StringComparison.OrdinalIgnoreCase) Then selectedRoom = ""

        Try
            Using con = DatabaseHelper.GetConnection()
                con.Open()
                Using cmd As New SqlCommand("INSERT INTO dbo.SpecialGuestRequests (GuestName, RoomNumber, RequestDetails, Priority, Status, CreatedByUserID, CreatedByUsername, CreatedAt, UpdatedAt) VALUES (@GuestName, NULLIF(@RoomNumber, ''), @Details, @Priority, 'Pending', @UserID, @Username, SYSDATETIME(), SYSDATETIME())", con)
                    cmd.Parameters.Add("@GuestName", SqlDbType.NVarChar, 160).Value = selectedGuestName
                    cmd.Parameters.Add("@RoomNumber", SqlDbType.NVarChar, 20).Value = selectedRoom
                    cmd.Parameters.Add("@Details", SqlDbType.NVarChar, 1000).Value = txtDetails.Text.Trim()
                    cmd.Parameters.Add("@Priority", SqlDbType.NVarChar, 20).Value = cboPriority.SelectedItem.ToString()
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = LoginForm.LoggedInUserID
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = LoginForm.LoggedInUsername
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            AuditLogger.Log("Created special guest request", "Guest: " & selectedGuestName & "; priority: " & cboPriority.SelectedItem.ToString())
            If cboGuest.Items.Count > 0 Then cboGuest.SelectedIndex = 0
            UpdateRoomForSelectedGuest()
            txtDetails.Clear()
            cboPriority.SelectedIndex = 0
            LoadRequests()
            MessageBox.Show("Special request created and made visible to the staff team.", "Aurelia Grand Hotel", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Could not create the request. Confirm the migration has been run." & Environment.NewLine & Environment.NewLine & ex.Message, "Special Requests", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridCellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        selectedRequestId = Convert.ToInt32(grid.Rows(e.RowIndex).Cells("RequestID").Value)
        If isStaff AndAlso cboStatus IsNot Nothing Then
            Dim status = Convert.ToString(grid.Rows(e.RowIndex).Cells("Status").Value)
            Dim idx = cboStatus.Items.IndexOf(status)
            If idx >= 0 Then cboStatus.SelectedIndex = idx
        End If
    End Sub

    Private Sub UpdateStatus(sender As Object, e As EventArgs)
        If selectedRequestId <= 0 Then
            MessageBox.Show("Select a request from the table first.", "No Request Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            Using con = DatabaseHelper.GetConnection()
                con.Open()
                Using cmd As New SqlCommand("UPDATE dbo.SpecialGuestRequests SET Status = @Status, UpdatedAt = SYSDATETIME(), CompletedAt = CASE WHEN @Status = 'Completed' THEN SYSDATETIME() ELSE NULL END, CompletedByUsername = CASE WHEN @Status = 'Completed' THEN @Username ELSE NULL END WHERE RequestID = @ID", con)
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = cboStatus.SelectedItem.ToString()
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = LoginForm.LoggedInUsername
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = selectedRequestId
                    If cmd.ExecuteNonQuery() <> 1 Then Throw New InvalidOperationException("The request was not found.")
                End Using
            End Using
            AuditLogger.Log("Updated special guest request", "Request ID " & selectedRequestId.ToString() & " status changed to " & cboStatus.SelectedItem.ToString())
            LoadRequests()
        Catch ex As Exception
            MessageBox.Show("Could not update request status." & Environment.NewLine & Environment.NewLine & ex.Message, "Special Requests", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
