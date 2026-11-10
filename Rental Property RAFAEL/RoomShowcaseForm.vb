Imports System.Drawing
Imports System.Collections.Generic
Imports System.Windows.Forms

Public Class RoomShowcaseForm
    Inherits Form

    Private ReadOnly roomTypes As New List(Of RoomShowcaseItem) From {
        New RoomShowcaseItem("Standard Room", "Thoughtfully appointed essentials for a restful and practical stay.",
            "Comfortable bed", "Work desk", "Private bathroom",
            New String() {
                "https://antunovic.hr/PageFiles/image/7097f444-7808-43e8-8d41-b13a762df2c6.jpg",
                "https://images.unsplash.com/photo-1611892440504-42a792e24d32?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1590490360182-c33d57733427?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Deluxe Room", "A more spacious retreat with upgraded furnishings and a refined atmosphere.",
            "King or twin setup", "Lounge corner", "Enhanced room finish",
            New String() {
                "https://res.klook.com/images/fl_lossy.progressive%2Cq_65/c_fill%2Cw_1295%2Ch_863/w_80%2Cx_15%2Cy_15%2Cg_south_west%2Cl_Klook_water_br_trans_yhcmh3/activities/jxrjds8m23jbroturawu/%E6%88%90%E9%83%BD%E7%8E%AF%E7%90%83%E4%B8%AD%E5%BF%83%E5%A4%A9%E5%A0%82%E6%B4%B2%E9%99%85%E5%A4%A7%E9%A5%AD%E5%BA%97%E4%BD%8F%E5%AE%BF%E5%A5%97%E9%A4%90.jpg",
                "https://images.unsplash.com/photo-1566665797739-1674de7a421a?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Superior Room", "An elevated guest room with a calm palette and added space to unwind.",
            "Comfort-focused layout", "Seating area", "City or garden outlook",
            New String() {
                "https://azuraworldhotel.com/_next/image?q=75&url=%2F_next%2Fstatic%2Fmedia%2FOda5.56bd9464.webp&w=2560",
                "https://images.unsplash.com/photo-1582719508461-905c673771fd?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1595576508898-0ad5c879a061?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Executive Room", "A polished space designed to balance productive work and comfortable downtime.",
            "Dedicated work area", "Premium furnishings", "Relaxation space",
            New String() {
                "https://thekashitour.com/assets/backend/images/room/1740211034.jpg",
                "https://images.unsplash.com/photo-1618773928121-c32242e63f39?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Junior Suite", "A generous open-plan retreat with room for both sleeping and relaxing.",
            "Bedroom and lounge feel", "Sofa or lounge seating", "Expanded floor space",
            New String() {
                "https://www.olympicpalacehotel.com/app/uploads/sites/131/2023/05/Junior-Suite-resized.jpg",
                "https://images.unsplash.com/photo-1616486338812-3dadae4b4ace?auto=format&fit=crop&w=1200&q=85",
                "https://images.unsplash.com/photo-1590490360182-c33d57733427?auto=format&fit=crop&w=1200&q=85"
            }),
        New RoomShowcaseItem("Presidential Suite", "An expansive signature accommodation with distinct living and sleeping spaces.",
            "Separate living area", "Premium suite styling", "Spacious entertaining area",
            New String() {
                "https://img.nh-hotels.net/naRVzr/nLed1M/original/NH_Collection_Helsinki_Grand_Hansa_Room_Presidential_Suite_general_view_bedroom.jpg?composite-to=center&output-quality=80&resize=1110%3A%2A",
                "https://media.philstar.com/images/the-philippine-star/lifestyle/sunday-life/20170101/Manila-Hotel-Joey-Lina-1C.jpg",
                "https://images.unsplash.com/photo-1582719508461-905c673771fd?auto=format&fit=crop&w=1200&q=85"
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
        Dim header As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 112,
            .BackColor = DeepNavy,
            .Padding = New Padding(30, 17, 24, 14)
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
            .Padding = New Padding(22, 20, 22, 20)
        }

        Dim intro As New Label With {
            .Text = "FIND YOUR SPACE",
            .Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold),
            .ForeColor = DeepNavy,
            .AutoSize = True,
            .Location = New Point(24, 15)
        }
        content.Controls.Add(intro)

        Dim note As New Label With {
            .Text = "Select a room category to view its photo gallery and highlights.",
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = MutedGray,
            .AutoSize = True,
            .Location = New Point(24, 40)
        }
        content.Controls.Add(note)

        Dim gallery As New FlowLayoutPanel With {
            .Location = New Point(18, 76),
            .Size = New Size(1180, 620),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right,
            .AutoScroll = True,
            .WrapContents = True,
            .FlowDirection = FlowDirection.LeftToRight,
            .BackColor = Ivory,
            .Padding = New Padding(4, 6, 4, 10)
        }

        For index As Integer = 0 To roomTypes.Count - 1
            Dim item = roomTypes(index)
            gallery.Controls.Add(CreateRoomCard(item))
            If index = 2 Then gallery.SetFlowBreak(gallery.Controls(gallery.Controls.Count - 1), True)
        Next

        content.Controls.Add(gallery)

        Dim footer As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 38,
            .BackColor = Ivory
        }
        Dim footerText As New Label With {
            .Text = "Images are illustrative references. Confirm room details and availability in Room Management and Reservations.",
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Italic),
            .ForeColor = MutedGray,
            .AutoSize = True,
            .Location = New Point(28, 8)
        }
        footer.Controls.Add(footerText)

        Me.Controls.Add(content)
        Me.Controls.Add(footer)
        Me.Controls.Add(header)
    End Sub

    Private Function CreateRoomCard(item As RoomShowcaseItem) As Panel
        Dim card As New Panel With {
            .Size = New Size(365, 340),
            .BackColor = WarmWhite,
            .Margin = New Padding(8, 7, 8, 12),
            .BorderStyle = BorderStyle.FixedSingle
        }

        Dim picture As New PictureBox With {
            .Location = New Point(0, 0),
            .Size = New Size(363, 188),
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
            .Location = New Point(18, 201),
            .Size = New Size(325, 25)
        }
        card.Controls.Add(category)

        Dim description As New Label With {
            .Text = item.Description,
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Charcoal,
            .Location = New Point(19, 232),
            .Size = New Size(325, 42)
        }
        card.Controls.Add(description)

        Dim viewButton As New Button With {
            .Text = "VIEW PHOTO GALLERY",
            .Location = New Point(18, 286),
            .Size = New Size(325, 38),
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

            Dim header As New Panel With {
                .Dock = DockStyle.Top,
                .Height = 105,
                .BackColor = DeepNavy
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
                .Padding = New Padding(18, 18, 18, 18),
                .BackColor = Ivory
            }

            For index As Integer = 0 To item.Photos.Length - 1
                Dim tile As New Panel With {
                    .Size = New Size(510, 285),
                    .BackColor = WarmWhite,
                    .Margin = New Padding(8),
                    .BorderStyle = BorderStyle.FixedSingle
                }
                Dim photo As New PictureBox With {
                    .Location = New Point(0, 0),
                    .Size = New Size(508, 245),
                    .SizeMode = PictureBoxSizeMode.Zoom,
                    .BackColor = Color.FromArgb(232, 229, 222)
                }
                LoadPicture(photo, item.Photos(index))
                tile.Controls.Add(photo)

                Dim caption As New Label With {
                    .Text = item.Name & " • View " & (index + 1).ToString(),
                    .Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold),
                    .ForeColor = DeepNavy,
                    .Location = New Point(14, 251),
                    .Size = New Size(480, 25)
                }
                tile.Controls.Add(caption)
                content.Controls.Add(tile)
            Next

            Dim footer As New Panel With {
                .Dock = DockStyle.Bottom,
                .Height = 62,
                .BackColor = WarmWhite
            }
            Dim highlights As New Label With {
                .Text = "HIGHLIGHTS: " & String.Join("  •  ", item.Highlights),
                .Font = New Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                .ForeColor = Charcoal,
                .AutoSize = True,
                .Location = New Point(22, 22)
            }
            footer.Controls.Add(highlights)

            details.Controls.Add(content)
            details.Controls.Add(footer)
            details.Controls.Add(header)
            details.ShowDialog(Me)
        End Using
    End Sub

    Private Shared Sub LoadPicture(picture As PictureBox, imageUrl As String)
        Try
            picture.WaitOnLoad = False
            picture.LoadAsync(imageUrl)
        Catch
            picture.BackColor = Color.FromArgb(232, 229, 222)
        End Try
    End Sub

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
