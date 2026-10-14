Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class SpecialRequestsForm
    Inherits Form

    Private ReadOnly isStaff As Boolean =
        LoginForm.LoggedInRole.Equals("Staff", StringComparison.OrdinalIgnoreCase)

    Private grid As DataGridView
    Private txtGuest As TextBox
    Private txtRoom As TextBox
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

        Dim header As New Panel With {.Dock = DockStyle.Top, .Height = 82, .BackColor = navy}
        Dim title As New Label With {.Text = "SPECIAL GUEST REQUESTS", .ForeColor = Color.White, .Font = New Font("Georgia", 21, FontStyle.Bold), .Location = New Point(26, 14), .AutoSize = True}
        Dim subtitle As New Label With {.Text = If(isStaff, "View requests and update their progress.", "Create and track guest service requests."), .ForeColor = Color.Gainsboro, .Location = New Point(29, 52), .AutoSize = True}
        header.Controls.AddRange(New Control() {title, subtitle})
        Controls.Add(header)

        Dim editor As New Panel With {.Dock = DockStyle.Top, .Height = If(isStaff, 92, 190), .Padding = New Padding(18), .BackColor = Color.FromArgb(253, 251, 247)}
        Controls.Add(editor)

        If Not isStaff Then
            editor.Controls.Add(MakeLabel("Guest name *", 18, 12))
            txtGuest = New TextBox With {.Location = New Point(18, 36), .Size = New Size(230, 28), .MaxLength = 160}
            editor.Controls.Add(txtGuest)
            editor.Controls.Add(MakeLabel("Room number (optional)", 265, 12))
            txtRoom = New TextBox With {.Location = New Point(265, 36), .Size = New Size(150, 28), .MaxLength = 20}
            editor.Controls.Add(txtRoom)
            editor.Controls.Add(MakeLabel("Priority", 432, 12))
            cboPriority = New ComboBox With {.Location = New Point(432, 36), .Size = New Size(150, 28), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboPriority.Items.AddRange(New Object() {"Normal", "High", "Urgent"})
            cboPriority.SelectedIndex = 0
            editor.Controls.Add(cboPriority)
            editor.Controls.Add(MakeLabel("Request details *", 18, 72))
            txtDetails = New TextBox With {.Location = New Point(18, 96), .Size = New Size(760, 58), .Multiline = True, .MaxLength = 1000, .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right}
            editor.Controls.Add(txtDetails)
            btnCreate = MakeButton("CREATE REQUEST", 800, 96, gold)
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
        Dim gridHost As New Panel With {.Dock = DockStyle.Fill, .Padding = New Padding(18, 12, 18, 18), .BackColor = ivory}
        gridHost.Controls.Add(grid)
        Controls.Add(gridHost)
        gridHost.BringToFront()
        editor.BringToFront()
        header.BringToFront()
        LoadRequests()
    End Sub

    Private Function MakeLabel(value As String, x As Integer, y As Integer) As Label
        Return New Label With {.Text = value, .Location = New Point(x, y), .AutoSize = True, .ForeColor = Color.FromArgb(90, 90, 85)}
    End Function

    Private Function MakeButton(value As String, x As Integer, y As Integer, color As Color) As Button
        Return New Button With {.Text = value, .Location = New Point(x, y), .Size = New Size(165, 38), .FlatStyle = FlatStyle.Flat, .FlatAppearance = New FlatButtonAppearance With {.BorderSize = 0}, .BackColor = color, .ForeColor = If(color = gold, Color.White, Color.White), .Cursor = Cursors.Hand}
    End Function

    Private Sub LoadRequests()
        Try
            Using con = DatabaseHelper.GetConnection()
                Using da As New SqlDataAdapter(
                    "SELECT RequestID, GuestName, RoomNumber, RequestDetails, Priority, Status, CreatedByUsername, CreatedAt, UpdatedAt FROM dbo.SpecialGuestRequests ORDER BY CASE Status WHEN 'Urgent' THEN 0 WHEN 'Pending' THEN 1 WHEN 'In Progress' THEN 2 ELSE 3 END, CreatedAt DESC", con)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    grid.DataSource = dt
                    If grid.Columns.Contains("RequestID") Then grid.Columns("RequestID").Visible = False
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
        If String.IsNullOrWhiteSpace(txtGuest.Text) OrElse String.IsNullOrWhiteSpace(txtDetails.Text) Then
            MessageBox.Show("Enter the guest name and request details.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            Using con = DatabaseHelper.GetConnection()
                con.Open()
                Using cmd As New SqlCommand("INSERT INTO dbo.SpecialGuestRequests (GuestName, RoomNumber, RequestDetails, Priority, Status, CreatedByUserID, CreatedByUsername, CreatedAt, UpdatedAt) VALUES (@GuestName, NULLIF(@RoomNumber, ''), @Details, @Priority, 'Pending', @UserID, @Username, SYSDATETIME(), SYSDATETIME())", con)
                    cmd.Parameters.Add("@GuestName", SqlDbType.NVarChar, 160).Value = txtGuest.Text.Trim()
                    cmd.Parameters.Add("@RoomNumber", SqlDbType.NVarChar, 20).Value = If(txtRoom.Text.Trim(), "")
                    cmd.Parameters.Add("@Details", SqlDbType.NVarChar, 1000).Value = txtDetails.Text.Trim()
                    cmd.Parameters.Add("@Priority", SqlDbType.NVarChar, 20).Value = cboPriority.SelectedItem.ToString()
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = LoginForm.LoggedInUserID
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = LoginForm.LoggedInUsername
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            AuditLogger.Log("Created special guest request", "Guest: " & txtGuest.Text.Trim() & "; priority: " & cboPriority.SelectedItem.ToString())
            txtGuest.Clear()
            txtRoom.Clear()
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
