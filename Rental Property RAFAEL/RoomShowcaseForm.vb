Imports System.Drawing
Imports System.Collections.Generic
Imports System.Windows.Forms

Public Class RoomShowcaseForm
    Inherits Form

    Private ReadOnly roomTypes As New List(Of RoomShowcaseItem) From {
        New RoomShowcaseItem("Standard Room", "A practical, comfortable guest room with essential furnishings and a simple, welcoming layout.",
            "Comfortable bed", "Work desk", "Private bathroom",
            New String() {
                "https://images.unsplash.com/photo-1611892440504-42a792e24d32?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1590490360182-c33d57733427?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1566665797739-1674de7a421a?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Deluxe Room", "A more spacious room with upgraded finishes, layered lighting, and a refined guest-room atmosphere.",
            "King or twin setup", "Lounge corner", "Enhanced room finish",
            New String() {
                "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1582719508461-905c673771fd?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1595576508898-0ad5c879a061?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Superior Room", "An elevated room concept with a more generous layout, stylish furnishings, and a relaxing seating area.",
            "Comfort-focused layout", "Seating area", "City or garden outlook",
            New String() {
                "https://images.unsplash.com/photo-1618773928121-c32242e63f39?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1540518614846-7eded433c457?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Executive Room", "A polished, business-oriented retreat featuring premium finishes and a more sophisticated interior.",
            "Dedicated work area", "Premium furnishings", "Relaxation space",
            New String() {
                "https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1566073771259-6a8506099945?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Junior Suite", "A suite-style accommodation with a more open feel and space designed for sleeping and lounging.",
            "Bedroom and lounge feel", "Sofa or lounge seating", "Expanded floor space",
            New String() {
                "https://images.unsplash.com/photo-1600607687644-c7171b42498f?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1616486338812-3dadae4b4ace?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1564501049412-61c2a3083791?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Presidential Suite", "The highest room category, presented with grander interiors, distinct living spaces, and an upscale suite aesthetic.",
            "Separate living area", "Premium suite styling", "Spacious entertaining area",
            New String() {
                "https://images.unsplash.com/photo-1600210492486-724fe5c67fb0?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1571896349842-33c89424de2d?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?auto=format&fit=crop&w=1200&q=85"
            })
    }

    Private ReadOnly DeepNavy As Color = Color.FromArgb(20, 27, 38)
    Private ReadOnly Navy As Color = Color.FromArgb(31, 42, 56)
    Private ReadOnly Champagne As Color = Color.FromArgb(198, 166, 110)
    Private ReadOnly SoftGold As Color = Color.FromArgb(218, 192, 143)
    Private ReadOnly Ivory As Color = Color.FromArgb(247, 244, 237)
    Private ReadOnly WarmWhite As Color = Color.FromArgb(253, 251, 247)
    Private ReadOnly Charcoal As Color = Color.FromArgb(45, 45, 43)
    Private ReadOnly MutedGray As Color = Color.FromArgb(125, 125, 120)

    Public Sub New()
        Me.Text = "Room Showcase | Aurelia Grand Hotel"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Size = New Size(1260, 850)
        Me.MinimumSize = New Size(1080, 700)
        Me.BackColor = Ivory
        Me.Font = New Font("Segoe UI", 9.5F)

        BuildInterface()
    End Sub

    Private Sub BuildInterface()
        Dim root As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .BackColor = Ivory,
            .ColumnCount = 1,
            .RowCount = 3,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        root.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 112.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 38.0F))

        Dim header As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = DeepNavy,
            .Padding = New Padding(30, 17, 24, 14),
            .Margin = New Padding(0)
        }
        Dim title As New Label With {
            .Text = "ROOM SHOWCASE",
            .Font = New Font("Georgia", 23.0F, FontStyle.Bold),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(30, 17)
        }
        header.Controls.Add(title)

        Dim subtitle As New Label With {
            .Text = "Explore the room categories at Aurelia Grand Hotel",
            .Font = New Font("Segoe UI", 10.0F),
            .ForeColor = Color.FromArgb(220, 214, 202),
            .AutoSize = True,
            .Location = New Point(33, 64)
        }
        header.Controls.Add(subtitle)

        Dim content As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Ivory,
            .Padding = New Padding(22, 12, 22, 10),
            .Margin = New Padding(0)
        }

        Dim intro As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 58,
            .BackColor = Ivory,
            .Margin = New Padding(0)
        }
        Dim sectionTitle As New Label With {
            .Text = "FIND YOUR SPACE",
            .Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold),
            .ForeColor = DeepNavy,
            .AutoSize = True,
            .Location = New Point(2, 2)
        }
        intro.Controls.Add(sectionTitle)

        Dim note As New Label With {
            .Text = "Select a room category to view its photo gallery and highlights.",
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = MutedGray,
            .AutoSize = True,
            .Location = New Point(2, 27)
        }
        intro.Controls.Add(note)

        Dim gallery As New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .WrapContents = True,
            .FlowDirection = FlowDirection.LeftToRight,
            .BackColor = Ivory,
            .Padding = New Padding(4, 8, 4, 12),
            .Margin = New Padding(0)
        }

        For Each item As RoomShowcaseItem In roomTypes
            gallery.Controls.Add(CreateRoomCard(item))
        Next

        content.Controls.Add(gallery)
        content.Controls.Add(intro)

        Dim footer As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Ivory,
            .Margin = New Padding(0)
        }
        Dim footerText As New Label With {
            .Text = "Images are illustrative references. Confirm room details and availability in Room Management and Reservations.",
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Italic),
            .ForeColor = MutedGray,
            .AutoSize = True,
            .Location = New Point(28, 8)
        }
        footer.Controls.Add(footerText)

        root.Controls.Add(header, 0, 0)
        root.Controls.Add(content, 0, 1)
        root.Controls.Add(footer, 0, 2)
        Me.Controls.Add(root)
    End Sub

    Private Function CreateRoomCard(item As RoomShowcaseItem) As Panel
        Dim card As New Panel With {
            .Size = New Size(350, 330),
            .BackColor = WarmWhite,
            .Margin = New Padding(8, 7, 8, 12),
            .BorderStyle = BorderStyle.FixedSingle
        }

        Dim picture As New PictureBox With {
            .Location = New Point(0, 0),
            .Size = New Size(348, 175),
            .SizeMode = PictureBoxSizeMode.Zoom,
            .BackColor = Color.FromArgb(232, 229, 222)
        }
        LoadPicture(picture, item.Photos(0))
        card.Controls.Add(picture)

        Dim category As New Label With {
            .Text = item.Name.ToUpperInvariant(),
            .Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold),
            .ForeColor = DeepNavy,
            .AutoSize = False,
            .Location = New Point(18, 188),
            .Size = New Size(312, 25)
        }
        card.Controls.Add(category)

        Dim description As New Label With {
            .Text = item.Description,
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Charcoal,
            .Location = New Point(19, 218),
            .Size = New Size(312, 48)
        }
        card.Controls.Add(description)

        Dim viewButton As New Button With {
            .Text = "VIEW PHOTO GALLERY",
            .Location = New Point(18, 278),
            .Size = New Size(312, 36),
            .BackColor = DeepNavy,
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
            .Cursor = Cursors.Hand
        }
        viewButton.FlatAppearance.BorderSize = 0
        AddHandler viewButton.Click, Sub(sender, e) ShowRoomDetails(item)
        card.Controls.Add(viewButton)

        Return card
    End Function

    Private Sub ShowRoomDetails(item As RoomShowcaseItem)
        Using details As New Form()
            details.Text = item.Name & " | Aurelia Grand Hotel"
            details.StartPosition = FormStartPosition.CenterParent
            details.Size = New Size(1120, 760)
            details.MinimumSize = New Size(850, 600)
            details.BackColor = Ivory
            details.Font = New Font("Segoe UI", 9.5F)

            Dim layout As New TableLayoutPanel With {
                .Dock = DockStyle.Fill,
                .BackColor = Ivory,
                .ColumnCount = 1,
                .RowCount = 3,
                .Margin = New Padding(0),
                .Padding = New Padding(0)
            }
            layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
            layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 105.0F))
            layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
            layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 72.0F))

            Dim header As New Panel With {
                .Dock = DockStyle.Fill,
                .BackColor = DeepNavy,
                .Margin = New Padding(0)
            }
            Dim title As New Label With {
                .Text = item.Name.ToUpperInvariant(),
                .Font = New Font("Georgia", 21.0F, FontStyle.Bold),
                .ForeColor = Color.White,
                .AutoSize = True,
                .Location = New Point(25, 16)
            }
            header.Controls.Add(title)
            Dim subtitle As New Label With {
                .Text = item.Description,
                .Font = New Font("Segoe UI", 9.0F),
                .ForeColor = Color.FromArgb(220, 214, 202),
                .AutoSize = True,
                .Location = New Point(28, 61)
            }
            header.Controls.Add(subtitle)

            Dim content As New FlowLayoutPanel With {
                .Dock = DockStyle.Fill,
                .AutoScroll = True,
                .WrapContents = True,
                .FlowDirection = FlowDirection.LeftToRight,
                .Padding = New Padding(14, 14, 14, 18),
                .BackColor = Ivory,
                .Margin = New Padding(0)
            }

            For index As Integer = 0 To item.Photos.Length - 1
                Dim tile As New Panel With {
                    .Size = New Size(500, 300),
                    .BackColor = WarmWhite,
                    .Margin = New Padding(8),
                    .BorderStyle = BorderStyle.FixedSingle
                }
                Dim photo As New PictureBox With {
                    .Location = New Point(0, 0),
                    .Size = New Size(498, 255),
                    .SizeMode = PictureBoxSizeMode.Zoom,
                    .BackColor = Color.FromArgb(232, 229, 222)
                }
                LoadPicture(photo, item.Photos(index))
                tile.Controls.Add(photo)

                Dim caption As New Label With {
                    .Text = item.Name & " • View " & (index + 1).ToString(),
                    .Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold),
                    .ForeColor = DeepNavy,
                    .Location = New Point(14, 263),
                    .Size = New Size(470, 25)
                }
                tile.Controls.Add(caption)
                content.Controls.Add(tile)
            Next

            Dim footer As New Panel With {
                .Dock = DockStyle.Fill,
                .BackColor = WarmWhite,
                .Margin = New Padding(0),
                .Padding = New Padding(20, 8, 20, 8)
            }
            Dim highlights As New Label With {
                .Text = "HIGHLIGHTS: " & String.Join("  •  ", item.Highlights),
                .Font = New Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                .ForeColor = Charcoal,
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleLeft
            }
            footer.Controls.Add(highlights)

            layout.Controls.Add(header, 0, 0)
            layout.Controls.Add(content, 0, 1)
            layout.Controls.Add(footer, 0, 2)
            details.Controls.Add(layout)
            details.ShowDialog(Me)
        End Using
    End Sub

    Private Shared Sub LoadPicture(picture As PictureBox, imageUrl As String)
        AddHandler picture.LoadCompleted,
            Sub(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
                If picture.IsDisposed Then Return
                If e.Error IsNot Nothing OrElse e.Cancelled Then
                    picture.Image = CreatePlaceholderImage()
                End If
            End Sub

        Try
            picture.WaitOnLoad = False
            picture.LoadAsync(imageUrl)
        Catch
            picture.Image = CreatePlaceholderImage()
        End Try
    End Sub

    Private Shared Function CreatePlaceholderImage() As Image
        Dim placeholder As New Bitmap(800, 450)
        Using graphics As Graphics = Graphics.FromImage(placeholder)
            graphics.Clear(Color.FromArgb(232, 229, 222))
            Using titleFont As New Font("Georgia", 22.0F, FontStyle.Bold),
                  subtitleFont As New Font("Segoe UI", 12.0F),
                  titleBrush As New SolidBrush(Color.FromArgb(31, 42, 56)),
                  subtitleBrush As New SolidBrush(Color.FromArgb(125, 125, 120))
                Dim titleText As String = "AURELIA GRAND HOTEL"
                Dim subtitleText As String = "ROOM PHOTO PREVIEW"
                Dim titleSize As SizeF = graphics.MeasureString(titleText, titleFont)
                Dim subtitleSize As SizeF = graphics.MeasureString(subtitleText, subtitleFont)
                graphics.DrawString(titleText, titleFont, titleBrush,
                    (placeholder.Width - titleSize.Width) / 2.0F, 190.0F)
                graphics.DrawString(subtitleText, subtitleFont, subtitleBrush,
                    (placeholder.Width - subtitleSize.Width) / 2.0F, 240.0F)
            End Using
        End Using
        Return placeholder
    End Function

    Private Class RoomShowcaseItem
        Public ReadOnly Name As String
        Public ReadOnly Description As String
        Public ReadOnly Highlights As String()
        Public ReadOnly Photos As String()

        Public Sub New(name As String, description As String, highlight1 As String, highlight2 As String, highlight3 As String, photos As String())
            Me.Name = name
            Me.Description = description
            Me.Highlights = New String() {highlight1, highlight2, highlight3}
            Me.Photos = photos
        End Sub
    End Class
End Class
