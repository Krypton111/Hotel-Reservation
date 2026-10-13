Imports System.Drawing
Imports System.Windows.Forms

Public Module AureliaTheme

    '==========================================================
    ' AURELIA GRAND HOTEL PALETTE
    '==========================================================

    Public ReadOnly DeepNavy As Color =
        Color.FromArgb(20, 27, 38)

    Public ReadOnly Navy As Color =
        Color.FromArgb(31, 42, 56)

    Public ReadOnly DarkBrown As Color =
        Color.FromArgb(52, 45, 39)

    Public ReadOnly Champagne As Color =
        Color.FromArgb(198, 166, 110)

    Public ReadOnly SoftGold As Color =
        Color.FromArgb(218, 192, 143)

    Public ReadOnly Ivory As Color =
        Color.FromArgb(247, 244, 237)

    Public ReadOnly WarmWhite As Color =
        Color.FromArgb(253, 251, 247)

    Public ReadOnly Charcoal As Color =
        Color.FromArgb(45, 45, 43)

    Public ReadOnly MutedGray As Color =
        Color.FromArgb(125, 125, 120)

    Public ReadOnly LightBorder As Color =
        Color.FromArgb(224, 216, 202)


    '==========================================================
    ' FONTS
    '==========================================================

    Public ReadOnly DisplayFont As New Font(
        "Georgia",
        22,
        FontStyle.Regular
    )

    Public ReadOnly HeadingFont As New Font(
        "Georgia",
        17,
        FontStyle.Regular
    )

    Public ReadOnly SectionFont As New Font(
        "Segoe UI",
        9.0F,
        FontStyle.Bold
    )

    Public ReadOnly BodyFont As New Font(
        "Segoe UI",
        9.5F,
        FontStyle.Regular
    )

    Public ReadOnly SmallFont As New Font(
        "Segoe UI",
        8.0F,
        FontStyle.Regular
    )


    '==========================================================
    ' STANDARD BUTTON
    '==========================================================

    Public Function CreateButton(
        text As String,
        width As Integer,
        height As Integer,
        primary As Boolean
    ) As Button

        Dim button As New Button()

        button.Text = text.ToUpper()

        button.Size =
            New Size(width, height)

        button.Font =
            New Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold
            )

        button.FlatStyle =
            FlatStyle.Flat

        button.FlatAppearance.BorderSize =
            1

        button.UseVisualStyleBackColor =
            False

        button.Cursor =
            Cursors.Hand

        If primary Then

            button.BackColor =
                DeepNavy

            button.ForeColor =
                Color.White

            button.FlatAppearance.BorderColor =
                Champagne

        Else

            button.BackColor =
                WarmWhite

            button.ForeColor =
                Charcoal

            button.FlatAppearance.BorderColor =
                LightBorder

        End If

        Return button

    End Function


    '==========================================================
    ' STYLE TEXTBOX
    '==========================================================

    Public Sub StyleTextBox(
        box As TextBox
    )

        box.BackColor =
            WarmWhite

        box.ForeColor =
            Charcoal

        box.BorderStyle =
            BorderStyle.FixedSingle

        box.Font =
            BodyFont

    End Sub


    '==========================================================
    ' STYLE COMBOBOX
    '==========================================================

    Public Sub StyleComboBox(
        box As ComboBox
    )

        box.BackColor =
            WarmWhite

        box.ForeColor =
            Charcoal

        box.FlatStyle =
            FlatStyle.Flat

        box.Font =
            BodyFont

        box.DropDownStyle =
            ComboBoxStyle.DropDownList

    End Sub


    '==========================================================
    ' STYLE DATE PICKER
    '==========================================================

    Public Sub StyleDatePicker(
        picker As DateTimePicker
    )

        picker.CalendarMonthBackground =
            WarmWhite

        picker.CalendarTitleBackColor =
            DeepNavy

        picker.CalendarTitleForeColor =
            Color.White

        picker.Font =
            BodyFont

        picker.Format =
            DateTimePickerFormat.Short

    End Sub


    '==========================================================
    ' STYLE DATA GRID
    '==========================================================

    Public Sub StyleGrid(
        grid As DataGridView
    )

        grid.BackgroundColor =
            WarmWhite

        grid.BorderStyle =
            BorderStyle.None

        grid.GridColor =
            LightBorder

        grid.EnableHeadersVisualStyles =
            False

        grid.ColumnHeadersDefaultCellStyle =
            New DataGridViewCellStyle With {
                .BackColor = DeepNavy,
                .ForeColor = Color.White,
                .Font = New Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Bold
                ),
                .Alignment =
                    DataGridViewContentAlignment.MiddleLeft
            }

        grid.DefaultCellStyle =
            New DataGridViewCellStyle With {
                .BackColor = WarmWhite,
                .ForeColor = Charcoal,
                .Font = BodyFont,
                .SelectionBackColor =
                    Color.FromArgb(235, 226, 207),
                .SelectionForeColor =
                    Charcoal
            }

        grid.RowHeadersVisible =
            False

        grid.AllowUserToAddRows =
            False

        grid.AllowUserToResizeRows =
            False

        grid.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        grid.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        grid.MultiSelect =
            False

    End Sub


    '==========================================================
    ' STYLE CHILD FORM
    '==========================================================

    Public Sub StyleChildForm(
        form As Form
    )

        form.BackColor =
            Ivory

        form.Font =
            BodyFont

    End Sub

End Module