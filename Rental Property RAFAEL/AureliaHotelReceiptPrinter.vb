Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public NotInheritable Class AureliaHotelReceiptPrinter

    Private Sub New()
    End Sub

    Public Shared Sub Preview(paymentID As Integer)
        Dim receipt As New DataTable()

        Using connection = DatabaseHelper.GetConnection()
            connection.Open()

            Dim sql As String =
                "SELECT p.PaymentID, p.ReferenceNumber, p.PaymentDate, p.PaymentAmount, " &
                "p.PaymentMethod, p.PaymentStatus, p.Notes, " &
                "r.ReferenceNumber AS ReservationReference, r.CheckInDate, r.CheckOutDate, " &
                "r.NumberOfNights, r.NumberOfGuests, r.RoomRate, r.TotalAmount, r.PaymentStatus AS ReservationPaymentStatus, " &
                "g.FullName, g.ContactNumber, g.Email, g.Address, g.City, g.Country, " &
                "rm.RoomNumber, rm.RoomType, " &
                "(SELECT ISNULL(SUM(paid.PaymentAmount), 0) FROM Payments paid " &
                "WHERE paid.ReservationID = r.ReservationID AND paid.IsArchived = 0) AS TotalPaid " &
                "FROM Payments p " &
                "INNER JOIN Reservations r ON p.ReservationID = r.ReservationID " &
                "INNER JOIN Guests g ON r.GuestID = g.GuestID " &
                "INNER JOIN Rooms rm ON r.RoomID = rm.RoomID " &
                "WHERE p.PaymentID = @PaymentID"

            Using command As New SqlCommand(sql, connection)
                command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = paymentID
                Using adapter As New SqlDataAdapter(command)
                    adapter.Fill(receipt)
                End Using
            End Using
        End Using

        If receipt.Rows.Count = 0 Then
            MessageBox.Show("The selected payment could not be found.", "Aurelia Grand Hotel",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim row As DataRow = receipt.Rows(0)
        Dim reservationTotal As Decimal = Convert.ToDecimal(row("TotalAmount"))
        Dim totalPaid As Decimal = Convert.ToDecimal(row("TotalPaid"))
        Dim balance As Decimal = Math.Max(0D, reservationTotal - totalPaid)
        Dim document As New PrintDocument()
        document.DocumentName = "Aurelia Grand Hotel - Folio #" & paymentID.ToString()
        document.DefaultPageSettings.Margins = New Margins(55, 55, 45, 45)

        AddHandler document.PrintPage,
            Sub(sender As Object, e As PrintPageEventArgs)
                Dim g As Graphics = e.Graphics
                Dim left As Single = e.MarginBounds.Left
                Dim right As Single = e.MarginBounds.Right
                Dim width As Single = e.MarginBounds.Width
                Dim y As Single = e.MarginBounds.Top
                Dim navy As Color = Color.FromArgb(20, 27, 38)
                Dim champagne As Color = Color.FromArgb(198, 166, 110)
                Dim charcoal As Color = Color.FromArgb(45, 45, 43)
                Dim muted As Color = Color.FromArgb(125, 125, 120)
                Using mastheadFont As New Font("Georgia", 23, FontStyle.Regular),
                      subtitleFont As New Font("Segoe UI", 8, FontStyle.Bold),
                      sectionFont As New Font("Segoe UI", 8.5F, FontStyle.Bold),
                      bodyFont As New Font("Segoe UI", 9),
                      smallFont As New Font("Segoe UI", 8),
                      totalFont As New Font("Georgia", 18, FontStyle.Bold),
                      navyBrush As New SolidBrush(navy),
                      goldBrush As New SolidBrush(champagne),
                      charcoalBrush As New SolidBrush(charcoal),
                      mutedBrush As New SolidBrush(muted),
                      goldPen As New Pen(champagne, 2),
                      lightPen As New Pen(Color.FromArgb(218, 212, 199), 1)
                    g.FillRectangle(navyBrush, left, y, width, 76)
                    g.DrawString("AURELIA", mastheadFont, Brushes.White, left + 18, y + 8)
                    g.DrawString("GRAND HOTEL", subtitleFont, goldBrush, left + 21, y + 48)
                    g.DrawString("GUEST PAYMENT FOLIO", sectionFont, Brushes.White,
                                 New RectangleF(left + width / 2, y + 28, width / 2 - 18, 20),
                                 New StringFormat With {.Alignment = StringAlignment.Far})
                    y += 92

                    g.DrawString("PAYMENT RECEIPT", sectionFont, goldBrush, left, y)
                    g.DrawString("FOLIO / RECEIPT NO.", smallFont, mutedBrush,
                                 New RectangleF(left + width / 2, y, width / 2, 16),
                                 New StringFormat With {.Alignment = StringAlignment.Far})
                    y += 18
                    g.DrawString(If(String.IsNullOrWhiteSpace(Safe(row("ReferenceNumber"))), "AGH-PAY-" & paymentID.ToString("D6"), Safe(row("ReferenceNumber"))),
                                 bodyFont, charcoalBrush, left, y)
                    g.DrawString("TRANSACTION #" & paymentID.ToString("D6"), bodyFont, charcoalBrush,
                                 New RectangleF(left + width / 2, y, width / 2, 18),
                                 New StringFormat With {.Alignment = StringAlignment.Far})
                    y += 25
                    g.DrawLine(goldPen, left, y, right, y)
                    y += 14

                    g.DrawString("GUEST", sectionFont, goldBrush, left, y)
                    g.DrawString("RESERVATION", sectionFont, goldBrush, left + width / 2, y)
                    y += 19
                    g.DrawString(Safe(row("FullName")), bodyFont, charcoalBrush, left, y)
                    g.DrawString(Safe(row("ReservationReference")), bodyFont, charcoalBrush, left + width / 2, y)
                    y += 17
                    DrawOptional(g, Safe(row("ContactNumber")), smallFont, mutedBrush, left, y, width / 2 - 10)
                    DrawOptional(g, Safe(row("Email")), smallFont, mutedBrush, left, y + 14, width / 2 - 10)
                    g.DrawString("Room " & Safe(row("RoomNumber")) & " · " & Safe(row("RoomType")),
                                 bodyFont, charcoalBrush, left + width / 2, y)
                    y += 42

                    g.DrawLine(lightPen, left, y, right, y)
                    y += 12
                    g.DrawString("STAY DETAILS", sectionFont, goldBrush, left, y)
                    y += 19
                    DrawPair(g, "Arrival", Convert.ToDateTime(row("CheckInDate")).ToString("MMM dd, yyyy"),
                             left, left + 125, y, width, bodyFont, charcoalBrush) : y += 19
                    DrawPair(g, "Departure", Convert.ToDateTime(row("CheckOutDate")).ToString("MMM dd, yyyy"),
                             left, left + 125, y, width, bodyFont, charcoalBrush) : y += 19
                    DrawPair(g, "Duration / Guests", Safe(row("NumberOfNights")) & " nights / " & Safe(row("NumberOfGuests")),
                             left, left + 125, y, width, bodyFont, charcoalBrush) : y += 19
                    DrawPair(g, "Room rate", "₱" & Convert.ToDecimal(row("RoomRate")).ToString("#,##0.00") & " per night",
                             left, left + 125, y, width, bodyFont, charcoalBrush) : y += 25

                    g.DrawLine(lightPen, left, y, right, y)
                    y += 12
                    g.DrawString("ACCOUNT SUMMARY", sectionFont, goldBrush, left, y)
                    y += 21
                    DrawPair(g, "Reservation total", "₱" & reservationTotal.ToString("#,##0.00"),
                             left, left + 180, y, width, bodyFont, charcoalBrush) : y += 20
                    DrawPair(g, "Payments received to date", "₱" & totalPaid.ToString("#,##0.00"),
                             left, left + 180, y, width, bodyFont, charcoalBrush) : y += 24
                    g.FillRectangle(navyBrush, left, y, width, 48)
                    g.DrawString("OUTSTANDING BALANCE", sectionFont, goldBrush, left + 12, y + 15)
                    g.DrawString("₱" & balance.ToString("#,##0.00"), totalFont, Brushes.White,
                                 New RectangleF(left + width / 2, y + 7, width / 2 - 12, 32),
                                 New StringFormat With {.Alignment = StringAlignment.Far})
                    y += 61

                    g.DrawString("THIS TRANSACTION", sectionFont, goldBrush, left, y)
                    y += 20
                    DrawPair(g, "Date processed", Convert.ToDateTime(row("PaymentDate")).ToString("MMMM dd, yyyy"),
                             left, left + 180, y, width, bodyFont, charcoalBrush) : y += 19
                    DrawPair(g, "Amount received", "₱" & Convert.ToDecimal(row("PaymentAmount")).ToString("#,##0.00"),
                             left, left + 180, y, width, bodyFont, charcoalBrush) : y += 19
                    DrawPair(g, "Payment method", Safe(row("PaymentMethod")), left, left + 180, y, width, bodyFont, charcoalBrush) : y += 19
                    DrawPair(g, "Transaction status", Safe(row("PaymentStatus")), left, left + 180, y, width, bodyFont, charcoalBrush) : y += 19
                    If Safe(row("Notes")) <> "" Then
                        DrawPair(g, "Remarks", Safe(row("Notes")), left, left + 180, y, width, smallFont, charcoalBrush)
                        y += 28
                    End If

                    y += 8
                    g.DrawLine(goldPen, left, y, right, y)
                    y += 13
                    g.DrawString("Thank you for staying with Aurelia Grand Hotel.", bodyFont, navyBrush,
                                 New RectangleF(left, y, width, 20),
                                 New StringFormat With {.Alignment = StringAlignment.Center})
                    y += 23
                    g.DrawString("We appreciate your visit and look forward to welcoming you again.",
                                 smallFont, mutedBrush, New RectangleF(left, y, width, 18),
                                 New StringFormat With {.Alignment = StringAlignment.Center})
                    y += 37
                    g.DrawLine(lightPen, left + 20, y, left + 215, y)
                    g.DrawLine(lightPen, right - 215, y, right - 20, y)
                    g.DrawString("Authorized cashier", smallFont, mutedBrush, left + 75, y + 5)
                    g.DrawString("Guest acknowledgement", smallFont, mutedBrush, right - 195, y + 5)
                End Using
                e.HasMorePages = False
            End Sub

        Using preview As New PrintPreviewDialog()
            preview.Document = document
            preview.Width = 950
            preview.Height = 750
            preview.StartPosition = FormStartPosition.CenterParent
            preview.ShowDialog()
        End Using
        document.Dispose()
    End Sub

    Private Shared Sub DrawPair(g As Graphics, label As String, value As String,
                                left As Single, valueX As Single, y As Single, width As Single,
                                font As Font, brush As Brush)
        g.DrawString(label, font, brush, left, y)
        g.DrawString(value, font, brush, New RectangleF(valueX, y, width - (valueX - left), 34))
    End Sub

    Private Shared Sub DrawOptional(g As Graphics, value As String, font As Font, brush As Brush,
                                    x As Single, y As Single, width As Single)
        If value.Trim() <> "" Then g.DrawString(value, font, brush, New RectangleF(x, y, width, 28))
    End Sub

    Private Shared Function Safe(value As Object) As String
        If value Is Nothing OrElse value Is DBNull.Value Then Return ""
        Return Convert.ToString(value).Trim()
    End Function

End Class
