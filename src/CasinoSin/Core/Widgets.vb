Imports System.Drawing
Imports System.Drawing.Drawing2D

''' <summary>
''' Bouton dessine entierement en GDI+ (pas un controle WinForms) pour qu'il
''' participe aux fondus enchaines et aux animations des ecrans.
''' </summary>
Public Class UiButton

    Public Property Bounds As RectangleF
    Public Property Caption As String = ""
    Public Property Subtitle As String = ""
    Public Property Accent As Color = Theme.Gold
    Public Property Enabled As Boolean = True
    Public Property Visible As Boolean = True
    Public Property FontSize As Single = 0.0F
    Public Property Payload As Object
    Public Property OnClick As Action

    Friend Hover As Single = 0.0F
    Friend Pressed As Boolean = False

    Public Sub New(caption As String, onClick As Action)
        Me.Caption = caption
        Me.OnClick = onClick
    End Sub

    Public Function HitTest(p As PointF) As Boolean
        Return Visible AndAlso Enabled AndAlso Bounds.Contains(p)
    End Function

    Public Sub Animate(dt As Single, mouse As PointF, mouseInside As Boolean)
        Dim wanted = If(Visible AndAlso Enabled AndAlso mouseInside AndAlso Bounds.Contains(mouse), 1.0F, 0.0F)
        Dim speed = dt * 9.0F
        If speed > 1.0F Then speed = 1.0F
        Hover += (wanted - Hover) * speed
    End Sub

    Public Overridable Sub Render(g As Graphics)
        If Not Visible OrElse Bounds.Width <= 0.0F Then Return

        Dim r = Bounds
        If Pressed Then r = New RectangleF(r.X, r.Y + 2.0F, r.Width, r.Height)

        Dim radius = Math.Min(12.0F, r.Height * 0.32F)
        Dim dim_ = Not Enabled

        Dim topColor = If(dim_, Color.FromArgb(48, 52, 50), Draw.Mix(Color.FromArgb(34, 48, 40), Draw.Mix(Accent, Color.Black, 0.45F), Hover))
        Dim bottomColor = If(dim_, Color.FromArgb(30, 34, 32), Draw.Mix(Color.FromArgb(14, 24, 19), Draw.Mix(Accent, Color.Black, 0.68F), Hover))

        If Enabled AndAlso Hover > 0.02F Then
            Draw.DrawGlow(g, r.X + r.Width * 0.5F, r.Y + r.Height * 0.5F, r.Width * 0.62F, Accent, CInt(60.0F * Hover))
        End If

        Draw.DropShadow(g, r, radius, 5.0F, If(Pressed, 40, 80))
        Draw.GradientRound(g, r, radius, topColor, bottomColor)

        Dim border = If(dim_, Color.FromArgb(90, 120, 120, 120), Draw.Mix(Draw.Alpha(Theme.Gold, 170), Theme.GoldBright, Hover))
        Draw.StrokeRound(g, r, radius, border, 1.6F)
        Draw.StrokeRound(g, Draw.Grow(r, -4.0F), Math.Max(2.0F, radius - 3.0F), Draw.Alpha(Theme.GoldDark, CInt(70 + 60 * Hover)), 1.0F)

        ' Reflet superieur
        Using path = Draw.RoundedPath(New RectangleF(r.X + 3.0F, r.Y + 3.0F, r.Width - 6.0F, r.Height * 0.45F), radius * 0.7F)
            Using b As New LinearGradientBrush(New RectangleF(r.X, r.Y + 2.0F, r.Width, r.Height * 0.5F + 2.0F),
                                               Color.FromArgb(34, 255, 255, 255), Color.FromArgb(0, 255, 255, 255),
                                               LinearGradientMode.Vertical)
                g.FillPath(b, path)
            End Using
        End Using

        Dim size = If(FontSize > 0.0F, FontSize, Math.Max(11.0F, r.Height * 0.36F))
        Dim textColor = If(dim_, Color.FromArgb(140, 150, 145), Draw.Mix(Theme.Cream, Theme.GoldBright, Hover))

        If String.IsNullOrEmpty(Subtitle) Then
            Draw.DrawTextShadow(g, Caption, Theme.Ui(size, FontStyle.Bold), textColor, r)
        Else
            Dim half = r.Height * 0.5F
            Draw.DrawTextShadow(g, Caption, Theme.Ui(size, FontStyle.Bold), textColor,
                                New RectangleF(r.X, r.Y + half * 0.28F, r.Width, half))
            Draw.DrawText(g, Subtitle, Theme.Ui(size * 0.62F), Draw.Alpha(textColor, 170),
                          New RectangleF(r.X, r.Y + half * 0.95F, r.Width, half))
        End If
    End Sub

End Class

''' <summary>
''' Grande carte cliquable du menu : illustration, titre et description.
''' </summary>
Public Class GameCard
    Inherits UiButton

    Public Property Description As String = ""
    Public Property Painter As Action(Of Graphics, RectangleF)

    Public Sub New(caption As String, onClick As Action)
        MyBase.New(caption, onClick)
    End Sub

    Public Overrides Sub Render(g As Graphics)
        If Not Visible OrElse Bounds.Width <= 0.0F Then Return

        Dim lift = Hover * 8.0F
        Dim r = New RectangleF(Bounds.X, Bounds.Y - lift, Bounds.Width, Bounds.Height)
        If Pressed Then r.Y += 3.0F

        Draw.DrawGlow(g, r.X + r.Width * 0.5F, r.Y + r.Height * 0.5F, r.Width * 0.75F, Accent, CInt(70.0F * Hover))
        Draw.DropShadow(g, r, 18.0F, 14.0F, CInt(90 + 50 * Hover))
        Draw.GradientRound(g, r, 18.0F, Draw.Mix(Color.FromArgb(20, 44, 33), Accent, 0.10F + 0.14F * Hover),
                           Color.FromArgb(8, 22, 16))

        Draw.GoldFrame(g, Draw.Grow(r, -7.0F), 12.0F)

        ' Illustration
        Dim artHeight = r.Height * 0.52F
        Dim art = New RectangleF(r.X + 18.0F, r.Y + 20.0F, r.Width - 36.0F, artHeight)
        If Painter IsNot Nothing Then Painter(g, art)

        Dim titleY = r.Y + artHeight + 34.0F
        Draw.GoldTitle(g, Caption, Theme.DisplayFamily, Math.Max(20.0F, r.Width * 0.085F), FontStyle.Bold,
                       r.X + r.Width * 0.5F, titleY)

        Draw.DrawText(g, Subtitle, Theme.Ui(13.0F, FontStyle.Italic), Draw.Alpha(Theme.Gold, 210),
                      New RectangleF(r.X, titleY + 22.0F, r.Width, 20.0F))

        Draw.DrawTextWrapped(g, Description, Theme.Ui(13.0F), Draw.Mix(Theme.Cream, Color.Gray, 0.25F),
                             New RectangleF(r.X + 26.0F, titleY + 46.0F, r.Width - 52.0F, r.Bottom - titleY - 58.0F),
                             StringAlignment.Center)

        If Hover > 0.05F Then
            Draw.StrokeRound(g, Draw.Grow(r, -2.0F), 18.0F, Draw.Alpha(Theme.GoldBright, CInt(200 * Hover)), 2.0F)
        End If
    End Sub

End Class
