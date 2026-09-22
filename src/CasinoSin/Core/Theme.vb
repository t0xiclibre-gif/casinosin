Imports System.Drawing

''' <summary>
''' Palette et polices communes a tous les ecrans : bois, feutre, or et jetons.
''' </summary>
Public NotInheritable Class Theme

    ' --- Feutre / decor ---
    Public Shared ReadOnly FeltDeep As Color = Color.FromArgb(4, 32, 21)
    Public Shared ReadOnly FeltMid As Color = Color.FromArgb(12, 74, 45)
    Public Shared ReadOnly FeltLight As Color = Color.FromArgb(24, 110, 67)
    Public Shared ReadOnly Wood As Color = Color.FromArgb(52, 27, 16)
    Public Shared ReadOnly WoodLight As Color = Color.FromArgb(124, 70, 36)
    Public Shared ReadOnly Night As Color = Color.FromArgb(10, 12, 14)

    ' --- Or ---
    Public Shared ReadOnly Gold As Color = Color.FromArgb(214, 176, 74)
    Public Shared ReadOnly GoldBright As Color = Color.FromArgb(252, 233, 156)
    Public Shared ReadOnly GoldDark As Color = Color.FromArgb(116, 88, 26)

    ' --- Jeu ---
    Public Shared ReadOnly TableRed As Color = Color.FromArgb(166, 24, 32)
    Public Shared ReadOnly TableRedLight As Color = Color.FromArgb(206, 46, 55)
    Public Shared ReadOnly TableBlack As Color = Color.FromArgb(26, 26, 30)
    Public Shared ReadOnly TableBlackLight As Color = Color.FromArgb(56, 56, 64)
    Public Shared ReadOnly TableGreen As Color = Color.FromArgb(13, 112, 68)
    Public Shared ReadOnly Cream As Color = Color.FromArgb(244, 239, 224)
    Public Shared ReadOnly Ink As Color = Color.FromArgb(17, 17, 19)
    Public Shared ReadOnly WinGreen As Color = Color.FromArgb(86, 214, 134)
    Public Shared ReadOnly LoseRed As Color = Color.FromArgb(232, 96, 96)

    Public Const DisplayFamily As String = "Georgia"
    Public Const UiFamily As String = "Segoe UI"
    Public Const SymbolFamily As String = "Segoe UI Symbol"

    Private Shared ReadOnly _fonts As New Dictionary(Of String, Font)()
    Private Shared ReadOnly _families As New Dictionary(Of String, FontFamily)()

    ''' <summary>Police serif utilisee pour les titres et les montants.</summary>
    Public Shared Function Display(size As Single, Optional style As FontStyle = FontStyle.Bold) As Font
        Return GetFont(DisplayFamily, size, style)
    End Function

    ''' <summary>Police d'interface pour les boutons et les libelles.</summary>
    Public Shared Function Ui(size As Single, Optional style As FontStyle = FontStyle.Regular) As Font
        Return GetFont(UiFamily, size, style)
    End Function

    ''' <summary>Police contenant les symboles de cartes.</summary>
    Public Shared Function Symbol(size As Single, Optional style As FontStyle = FontStyle.Regular) As Font
        Return GetFont(SymbolFamily, size, style)
    End Function

    ''' <summary>Polices mises en cache (taille exprimee en pixels).</summary>
    Public Shared Function GetFont(family As String, size As Single, style As FontStyle) As Font
        If size < 4.0F Then size = 4.0F
        Dim key = family & "|" & size.ToString("0.#", Globalization.CultureInfo.InvariantCulture) & "|" & CInt(style).ToString()

        Dim cached As Font = Nothing
        If _fonts.TryGetValue(key, cached) Then Return cached

        Dim created As Font
        Try
            created = New Font(GetFamily(family), size, style, GraphicsUnit.Pixel)
        Catch
            created = New Font(FontFamily.GenericSansSerif, size, style, GraphicsUnit.Pixel)
        End Try

        _fonts(key) = created
        Return created
    End Function

    ''' <summary>Famille de police avec repli si elle n'est pas installee.</summary>
    Public Shared Function GetFamily(name As String) As FontFamily
        Dim cached As FontFamily = Nothing
        If _families.TryGetValue(name, cached) Then Return cached

        Dim fam As FontFamily
        Try
            fam = New FontFamily(name)
        Catch
            fam = If(name = DisplayFamily, FontFamily.GenericSerif, FontFamily.GenericSansSerif)
        End Try

        _families(name) = fam
        Return fam
    End Function

    Private Sub New()
    End Sub

End Class
