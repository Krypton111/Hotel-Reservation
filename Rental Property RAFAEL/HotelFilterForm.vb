Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Globalization
Imports System.Drawing
Imports System.Windows.Forms

Public Class HotelFilterForm
    Inherits Form

    Private ReadOnly sourceTable As DataTable
    Private ReadOnly filterControls As New Dictionary(Of String, Control())()
    Private ReadOnly scrollPanel As FlowLayoutPanel
    Private ReadOnly DeepNavy As Color = Color.FromArgb(20, 27, 38)
    Private ReadOnly Navy As Color = Color.FromArgb(31, 42, 56)
    Private ReadOnly Champagne As Color = Color.FromArgb(198, 166, 110)
    Private ReadOnly Ivory As Color = Color.FromArgb(247, 244, 237)
    Private ReadOnly WarmWhite As Color = Color.FromArgb(253, 251, 247)
    Private ReadOnly Charcoal As Color = Color.FromArgb(45, 45, 43)
    Private ReadOnly LightBorder As Color = Color.FromArgb(224, 216, 202)

    Public Property FilterExpression As String = String.Empty

    Public Sub New(table As DataTable, windowTitle As String, Optional fieldNames As String() = Nothing)
        sourceTable = table

        Me.Text = windowTitle
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Size = New Size(790, 700)
        Me.MinimumSize = New Size(650, 520)
        Me.BackColor = Ivory
        Me.Font = New Font("Segoe UI", 9.5F)
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = False

        Dim header As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 96,
            .BackColor = DeepNavy,
            .Padding = New Padding(26, 15, 20, 12)
        }

        Dim title As New Label With {
            .Text = windowTitle.ToUpperInvariant(),
            .Font = New Font("Georgia", 19.0F, FontStyle.Bold),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(25, 13)
        }
        header.Controls.Add(title)

        Dim subtitle As New Label With {
            .Text = "Refine the records using one or more conditions. Conditions are combined.",
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Color.FromArgb(220, 214, 202),
            .AutoSize = True,
            .Location = New Point(27, 55)
        }
        header.Controls.Add(subtitle)

        scrollPanel = New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .AutoScroll = True,
            .Padding = New Padding(22, 18, 22, 14),
            .BackColor = Ivory
        }

        Dim footer As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 78,
            .BackColor = WarmWhite,
            .Padding = New Padding(20, 15, 20, 14)
        }

        Dim clearButton As New Button With {
            .Text = "Clear all",
            .Size = New Size(105, 40),
            .Location = New Point(20, 18),
            .BackColor = Color.FromArgb(232, 226, 215),
            .ForeColor = Charcoal,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI Semibold", 9.0F),
            .Cursor = Cursors.Hand
        }
        clearButton.FlatAppearance.BorderColor = LightBorder
        AddHandler clearButton.Click, AddressOf ClearClicked
        footer.Controls.Add(clearButton)

        Dim cancelButton As New Button With {
            .Text = "Cancel",
            .Size = New Size(100, 40),
            .Location = New Point(footer.ClientSize.Width - 230, 18),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .BackColor = Color.White,
            .ForeColor = DeepNavy,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI Semibold", 9.0F),
            .Cursor = Cursors.Hand,
            .DialogResult = DialogResult.Cancel
        }
        cancelButton.FlatAppearance.BorderColor = Champagne
        footer.Controls.Add(cancelButton)

        Dim applyButton As New Button With {
            .Text = "Apply filters",
            .Size = New Size(120, 40),
            .Location = New Point(footer.ClientSize.Width - 120, 18),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .BackColor = DeepNavy,
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI Semibold", 9.0F),
            .Cursor = Cursors.Hand
        }
        applyButton.FlatAppearance.BorderSize = 0
        AddHandler applyButton.Click, AddressOf ApplyClicked
        footer.Controls.Add(applyButton)

        Me.Controls.Add(scrollPanel)
        Me.Controls.Add(footer)
        Me.Controls.Add(header)

        Dim fields As New List(Of String)()
        If fieldNames Is Nothing Then
            For Each column As DataColumn In sourceTable.Columns
                fields.Add(column.ColumnName)
            Next
        Else
            fields.AddRange(fieldNames)
        End If

        For Each fieldName As String In fields
            If sourceTable.Columns.Contains(fieldName) Then
                AddFieldControl(fieldName, sourceTable.Columns(fieldName).DataType)
            End If
        Next

        If filterControls.Count = 0 Then
            scrollPanel.Controls.Add(New Label With {
                .Text = "There are no available fields to filter in this view.",
                .AutoSize = True,
                .ForeColor = Charcoal,
                .Padding = New Padding(4, 12, 4, 4)
            })
        End If

        Me.AcceptButton = applyButton
        Me.CancelButton = cancelButton
    End Sub

    Private Sub AddFieldControl(fieldName As String, dataType As Type)
        Dim row As New Panel With {
            .Width = 700,
            .Height = 78,
            .BackColor = WarmWhite,
            .Margin = New Padding(0, 0, 0, 11),
            .Padding = New Padding(12)
        }

        Dim label As New Label With {
            .Text = FriendlyName(fieldName),
            .Location = New Point(13, 8),
            .AutoSize = True,
            .Font = New Font("Segoe UI Semibold", 9.0F),
            .ForeColor = DeepNavy
        }
        row.Controls.Add(label)

        If dataType Is GetType(DateTime) Then
            Dim fromPicker As New DateTimePicker With {
                .Format = DateTimePickerFormat.Custom,
                .CustomFormat = "'From' MMM dd, yyyy",
                .ShowCheckBox = True,
                .Checked = False,
                .Width = 260,
                .Location = New Point(13, 35),
                .Font = New Font("Segoe UI", 9.0F)
            }
            Dim toPicker As New DateTimePicker With {
                .Format = DateTimePickerFormat.Custom,
                .CustomFormat = "'To' MMM dd, yyyy",
                .ShowCheckBox = True,
                .Checked = False,
                .Width = 260,
                .Location = New Point(290, 35),
                .Font = New Font("Segoe UI", 9.0F)
            }
            row.Controls.Add(fromPicker)
            row.Controls.Add(toPicker)
            filterControls(fieldName) = New Control() {fromPicker, toPicker}

        ElseIf IsNumericType(dataType) Then
            Dim minimum As New TextBox With {
                .Width = 260,
                .Location = New Point(13, 35),
                .Font = New Font("Segoe UI", 9.0F),
                .BorderStyle = BorderStyle.FixedSingle,
                .PlaceholderText = "Minimum"
            }
            Dim maximum As New TextBox With {
                .Width = 260,
                .Location = New Point(290, 35),
                .Font = New Font("Segoe UI", 9.0F),
                .BorderStyle = BorderStyle.FixedSingle,
                .PlaceholderText = "Maximum"
            }
            row.Controls.Add(minimum)
            row.Controls.Add(maximum)
            filterControls(fieldName) = New Control() {minimum, maximum}

        ElseIf ShouldUseDropdown(fieldName) Then
            Dim choice As New ComboBox With {
                .Width = 537,
                .Location = New Point(13, 35),
                .Font = New Font("Segoe UI", 9.0F),
                .DropDownStyle = ComboBoxStyle.DropDownList
            }
            choice.Items.Add("(Any)")
            Dim values As New List(Of String)()
            For Each dataRow As DataRow In sourceTable.Rows
                If Not dataRow.IsNull(fieldName) Then
                    Dim item As String = Convert.ToString(dataRow(fieldName))
                    If Not values.Contains(item) Then values.Add(item)
                End If
            Next
            values.Sort(StringComparer.CurrentCultureIgnoreCase)
            For Each item As String In values
                choice.Items.Add(item)
            Next
            choice.SelectedIndex = 0
            row.Controls.Add(choice)
            filterControls(fieldName) = New Control() {choice}

        Else
            Dim search As New TextBox With {
                .Width = 537,
                .Location = New Point(13, 35),
                .Font = New Font("Segoe UI", 9.0F),
                .BorderStyle = BorderStyle.FixedSingle,
                .PlaceholderText = "Contains..."
            }
            row.Controls.Add(search)
            filterControls(fieldName) = New Control() {search}
        End If

        scrollPanel.Controls.Add(row)
    End Sub

    Private Function ShouldUseDropdown(fieldName As String) As Boolean
        Dim normalized As String = fieldName.ToLowerInvariant()
        If normalized.Contains("status") OrElse normalized.Contains("type") OrElse
           normalized.Contains("method") OrElse normalized = "country" OrElse
           normalized = "city" OrElse normalized.Contains("floor") Then Return True

        Dim values As New HashSet(Of String)(StringComparer.CurrentCultureIgnoreCase)
        For Each row As DataRow In sourceTable.Rows
            If Not row.IsNull(fieldName) Then values.Add(Convert.ToString(row(fieldName)))
            If values.Count > 20 Then Return False
        Next
        Return values.Count > 0 AndAlso values.Count <= 20
    End Function

    Private Shared Function IsNumericType(dataType As Type) As Boolean
        Return dataType Is GetType(Byte) OrElse dataType Is GetType(Short) OrElse
               dataType Is GetType(Integer) OrElse dataType Is GetType(Long) OrElse
               dataType Is GetType(Single) OrElse dataType Is GetType(Double) OrElse
               dataType Is GetType(Decimal)
    End Function

    Private Shared Function FriendlyName(value As String) As String
        Dim result As String = value
        result = result.Replace("RoomNumber", "Room Number").
                        Replace("RoomType", "Room Type").
                        Replace("RatePerNight", "Rate per Night").
                        Replace("FloorNumber", "Floor Number").
                        Replace("GuestName", "Guest Name").
                        Replace("FullName", "Guest Name").
                        Replace("FirstName", "First Name").
                        Replace("LastName", "Last Name").
                        Replace("ContactNumber", "Contact Number").
                        Replace("CheckInDate", "Check-In Date").
                        Replace("CheckOutDate", "Check-Out Date").
                        Replace("PaymentDate", "Payment Date").
                        Replace("PaymentAmount", "Payment Amount").
                        Replace("PaymentMethod", "Payment Method").
                        Replace("PaymentStatus", "Payment Status").
                        Replace("ReservationStatus", "Reservation Status").
                        Replace("NumberOfGuests", "Number of Guests").
                        Replace("TotalAmount", "Total Amount").
                        Replace("ReferenceNumber", "Reference Number")
        Return result
    End Function

    Private Sub ApplyClicked(sender As Object, e As EventArgs)
        Dim expressions As New List(Of String)()

        For Each entry In filterControls
            Dim columnName As String = "[" & entry.Key.Replace("]", "]]") & "]"
            Dim columnType As Type = sourceTable.Columns(entry.Key).DataType
            Dim controls As Control() = entry.Value

            If columnType Is GetType(DateTime) Then
                Dim fromPicker = DirectCast(controls(0), DateTimePicker)
                Dim toPicker = DirectCast(controls(1), DateTimePicker)

                If fromPicker.Checked AndAlso toPicker.Checked AndAlso fromPicker.Value.Date > toPicker.Value.Date Then
                    MessageBox.Show("The start date cannot be later than the end date.", "Check date range", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                If fromPicker.Checked Then expressions.Add(columnName & " >= #" & fromPicker.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) & "#")
                If toPicker.Checked Then expressions.Add(columnName & " < #" & toPicker.Value.Date.AddDays(1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) & "#")

            ElseIf IsNumericType(columnType) Then
                Dim minText = DirectCast(controls(0), TextBox).Text.Trim()
                Dim maxText = DirectCast(controls(1), TextBox).Text.Trim()
                Dim minValue As Decimal
                Dim maxValue As Decimal

                If minText <> "" Then
                    If Not Decimal.TryParse(minText, NumberStyles.Number, CultureInfo.CurrentCulture, minValue) Then
                        MessageBox.Show("Enter a valid minimum value for " & FriendlyName(entry.Key) & ".", "Invalid number", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    expressions.Add(columnName & " >= " & minValue.ToString(CultureInfo.InvariantCulture))
                End If

                If maxText <> "" Then
                    If Not Decimal.TryParse(maxText, NumberStyles.Number, CultureInfo.CurrentCulture, maxValue) Then
                        MessageBox.Show("Enter a valid maximum value for " & FriendlyName(entry.Key) & ".", "Invalid number", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    expressions.Add(columnName & " <= " & maxValue.ToString(CultureInfo.InvariantCulture))
                End If

                If minText <> "" AndAlso maxText <> "" AndAlso minValue > maxValue Then
                    MessageBox.Show("The minimum value cannot be greater than the maximum value.", "Check value range", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

            ElseIf TypeOf controls(0) Is ComboBox Then
                Dim choice = DirectCast(controls(0), ComboBox)
                If choice.SelectedIndex > 0 Then
                    Dim selected = Convert.ToString(choice.SelectedItem).Replace("'", "''")
                    expressions.Add(columnName & " = '" & selected & "'")
                End If

            Else
                Dim value = DirectCast(controls(0), TextBox).Text.Trim()
                If value <> "" Then
                    value = value.Replace("'", "''").Replace("[", "[[]").Replace("]", "[]]").Replace("%", "[%]").Replace("*", "[*]")
                    expressions.Add(columnName & " LIKE '%" & value & "%'")
                End If
            End If
        Next

        FilterExpression = String.Join(" AND ", expressions)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ClearClicked(sender As Object, e As EventArgs)
        FilterExpression = String.Empty
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Public Shared Function GetSourceTable(source As Object) As DataTable
        If TypeOf source Is DataTable Then Return DirectCast(source, DataTable)
        If TypeOf source Is DataView Then Return DirectCast(source, DataView).Table
        If TypeOf source Is BindingSource Then
            Dim binding = DirectCast(source, BindingSource)
            If TypeOf binding.DataSource Is DataTable Then Return DirectCast(binding.DataSource, DataTable)
            If TypeOf binding.DataSource Is DataView Then Return DirectCast(binding.DataSource, DataView).Table
        End If
        Return Nothing
    End Function

    Public Shared Sub StyleFilterButton(button As Button)
        button.Text = "Filter"
        button.BackColor = Color.FromArgb(20, 27, 38)
        button.ForeColor = Color.White
        button.FlatStyle = FlatStyle.Flat
        button.FlatAppearance.BorderSize = 0
        button.Font = New Font("Segoe UI Semibold", 9.0F)
        button.Cursor = Cursors.Hand
        button.TextAlign = ContentAlignment.MiddleRight
        button.Padding = New Padding(0, 0, 12, 0)
        AddHandler button.Paint, AddressOf DrawFilterIcon
    End Sub

    Private Shared Sub DrawFilterIcon(sender As Object, e As PaintEventArgs)
        Dim button = DirectCast(sender, Button)
        Dim x As Integer = 13
        Dim y As Integer = (button.Height - 16) \ 2
        Using pen As New Pen(Color.White, 1.8F)
            pen.StartCap = Drawing2D.LineCap.Round
            pen.EndCap = Drawing2D.LineCap.Round
            e.Graphics.DrawLine(pen, x, y, x + 18, y)
            e.Graphics.DrawLine(pen, x + 2, y + 1, x + 7, y + 7)
            e.Graphics.DrawLine(pen, x + 16, y + 1, x + 11, y + 7)
            e.Graphics.DrawLine(pen, x + 7, y + 7, x + 7, y + 12)
            e.Graphics.DrawLine(pen, x + 7, y + 12, x + 11, y + 15)
            e.Graphics.DrawLine(pen, x + 11, y + 15, x + 11, y + 7)
        End Using
    End Sub

End Class
