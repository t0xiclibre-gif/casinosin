Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

''' <summary>
''' Boite a outils GDI+ : formes arrondies, textes dores, feutre de casino et jetons.
''' </summary>
Public Module Draw

#Region "Geometrie"

    Public Function RoundedPath(r As RectangleF, radius As Single) As GraphicsPath
        Dim p As New GraphicsPath()
        Dim d = Math.Min(radius * 2.0F, Math.Min(r.Width, r.Height))

        If d <= 0.5F Then
            p.AddRectangle(r)
            Return p
        End If

        p.AddArc(r.X, r.Y, d, d, 180.0F, 90.0F)
        p.AddArc(r.Right - d, r.Y, d, d, 270.0F, 90.0F)
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0.0F, 90.0F)
        p.AddArc(r.X, r.Bottom - d, d, d, 90.0F, 90.0F)
        p.CloseFigure()
        Return p
    End Function

    Public Function DiamondPath(r As RectangleF) As GraphicsPath
        Dim p As New GraphicsPath()
        p.AddPolygon(New PointF() {
            New PointF(r.X + r.Width * 0.5F, r.Y),
            New PointF(r.Right, r.Y + r.Height * 0.5F),
            New PointF(r.X + r.Width * 0.5F, r.Bottom),
            New PointF(r.X, r.Y + r.Height * 0.5F)})
        Return p
    End Function

    Public Function Grow(r As RectangleF, amount As Single) As RectangleF
        Return New RectangleF(r.X - amount, r.Y - amount, r.Width + amount * 2.0F, r.Height + amount * 2.0F)
    End Function

    Public Function CircleRect(cx As Single, cy As Single, radius As Single) As RectangleF
        Return New RectangleF(cx - radius, cy - radius, radius * 2.0F, radius * 2.0F)
    End Function

    Public Function Lerp(a As Single, b As Single, t As Single) As Single
        Return a + (b - a) * t
    End Function

    Public Function Clamp01(v As Single) As Single
        If v < 0.0F Then Return 0.0F
        If v > 1.0F Then Return 1.0F
        Return v
    End Function

    Public Function EaseOutCubic(t As Single) As Single
        Dim u = 1.0F - Clamp01(t)
        Return 1.0F - u * u * u
    End Function

    Public Function EaseOutQuint(t As Single) As Single
        Dim u = 1.0F - Clamp01(t)
        Return 1.0F - u * u * u * u * u
    End Function

    Public Function EaseInOut(t As Single) As Single
        t = Clamp01(t)
        Return CSng(t * t * (3.0F - 2.0F * t))
    End Function

    Public Function Mix(a As Color, b As Color, t As Single) As Color
        t = Clamp01(t)
        Return Color.FromArgb(
            CInt(a.A + (b.A - a.A) * t),
            CInt(a.R + (b.R - a.R) * t),
            CInt(a.G + (b.G - a.G) * t),
            CInt(a.B + (b.B - a.B) * t))
    End Function

    Public Function Alpha(c As Color, a As Integer) As Color
        If a < 0 Then a = 0
        If a > 255 Then a = 255
        Return Color.FromArgb(a, c.R, c.G, c.B)
    End Function

#End Region

#Region "Remplissages"

    Public Sub FillRound(g As Graphics, r As RectangleF, radius As Single, c As Color)
        Using p = RoundedPath(r, radius)
            Using b As New SolidBrush(c)
                g.FillPath(b, p)
            End Using
        End Using
    End Sub

    Public Sub GradientRound(g As Graphics, r As RectangleF, radius As Single, top As Color, bottom As Color)
        If r.Width <= 0.0F OrElse r.Height <= 0.0F Then Return

        Using p = RoundedPath(r, radius)
            Using b As New LinearGradientBrush(Grow(r, 1.0F), top, bottom, LinearGradientMode.Vertical)
                g.FillPath(b, p)
            End Using
        End Using
    End Sub

    Public Sub StrokeRound(g As Graphics, r As RectangleF, radius As Single, c As Color, width As Single)
        Using p = RoundedPath(r, radius)
            Using pen As New Pen(c, width)
                pen.LineJoin = LineJoin.Round
                g.DrawPath(pen, p)
            End Using
        End Using
    End Sub

    ''' <summary>Ombre portee douce sous une forme arrondie.</summary>
    Public Sub DropShadow(g As Graphics, r As RectangleF, radius As Single, depth As Single, Optional strength As Integer = 70)
        For i = CInt(depth) To 1 Step -1
            Dim a = CInt(strength * (1.0F - (i - 1) / depth) / depth)
            If a <= 0 Then Continue For
            FillRound(g, New RectangleF(r.X - i * 0.4F, r.Y + i * 0.8F, r.Width + i * 0.8F, r.Height + i * 0.2F), radius + i * 0.4F, Color.FromArgb(a, 0, 0, 0))
        Next
    End Sub

    ''' <summary>Halo lumineux circulaire (spots, gains, bille).</summary>
    Public Sub DrawGlow(g As Graphics, cx As Single, cy As Single, radius As Single, c As Color, Optional strength As Integer = 110)
        If radius <= 0.0F Then Return

        Using path As New GraphicsPath()
            path.AddEllipse(CircleRect(cx, cy, radius))
            Using pgb As New PathGradientBrush(path)
                pgb.CenterColor = Alpha(c, strength)
                pgb.SurroundColors = New Color() {Color.FromArgb(0, c.R, c.G, c.B)}
                pgb.CenterPoint = New PointF(cx, cy)
                g.FillPath(pgb, path)
            End Using
        End Using
    End Sub

#End Region

#Region "Textes"

    Private ReadOnly _fmtCache As New Dictionary(Of Integer, StringFormat)()

    Private Function Fmt(h As StringAlignment, v As StringAlignment, wrap As Boolean) As StringFormat
        Dim key = CInt(h) * 100 + CInt(v) * 10 + If(wrap, 1, 0)

        Dim cached As StringFormat = Nothing
        If _fmtCache.TryGetValue(key, cached) Then Return cached

        Dim sf As New StringFormat(StringFormat.GenericTypographic)
        sf.Alignment = h
        sf.LineAlignment = v
        sf.Trimming = StringTrimming.None
        sf.FormatFlags = sf.FormatFlags Or StringFormatFlags.NoClip
        If Not wrap Then sf.FormatFlags = sf.FormatFlags Or StringFormatFlags.NoWrap

        _fmtCache(key) = sf
        Return sf
    End Function

    Public Sub DrawText(g As Graphics, s As String, f As Font, c As Color, r As RectangleF,
                    Optional h As StringAlignment = StringAlignment.Center,
                    Optional v As StringAlignment = StringAlignment.Center)
        If String.IsNullOrEmpty(s) Then Return
        Using b As New SolidBrush(c)
            g.DrawString(s, f, b, r, Fmt(h, v, False))
        End Using
    End Sub

    Public Sub DrawTextAt(g As Graphics, s As String, f As Font, c As Color, x As Single, y As Single,
                      Optional h As StringAlignment = StringAlignment.Center,
                      Optional v As StringAlignment = StringAlignment.Center)
        DrawText(g, s, f, c, New RectangleF(x - 2000.0F, y - 200.0F, 4000.0F, 400.0F), h, v)
    End Sub

    Public Sub DrawTextWrapped(g As Graphics, s As String, f As Font, c As Color, r As RectangleF,
                           Optional h As StringAlignment = StringAlignment.Near)
        If String.IsNullOrEmpty(s) Then Return
        Using b As New SolidBrush(c)
            g.DrawString(s, f, b, r, Fmt(h, StringAlignment.Near, True))
        End Using
    End Sub

    ''' <summary>Texte avec ombre portee, pour rester lisible sur le feutre.</summary>
    Public Sub DrawTextShadow(g As Graphics, s As String, f As Font, c As Color, r As RectangleF,
                          Optional h As StringAlignment = StringAlignment.Center,
                          Optional v As StringAlignment = StringAlignment.Center)
        DrawText(g, s, f, Color.FromArgb(150, 0, 0, 0), New RectangleF(r.X + 1.5F, r.Y + 1.5F, r.Width, r.Height), h, v)
        DrawText(g, s, f, c, r, h, v)
    End Sub

    Public Function MeasureText(g As Graphics, s As String, f As Font) As SizeF
        If String.IsNullOrEmpty(s) Then Return SizeF.Empty
        Return g.MeasureString(s, f, 4000, Fmt(StringAlignment.Near, StringAlignment.Near, False))
    End Function

    ''' <summary>Grand titre en or grave, utilise sur l'ecran d'accueil et les bandeaux.</summary>
    Public Sub GoldTitle(g As Graphics, s As String, family As String, emSize As Single, style As FontStyle,
                         centerX As Single, centerY As Single, Optional outline As Boolean = True)
        If String.IsNullOrEmpty(s) Then Return

        Using gp As New GraphicsPath()
            Using sf As New StringFormat(StringFormat.GenericTypographic)
                sf.Alignment = StringAlignment.Center
                sf.LineAlignment = StringAlignment.Center
                sf.FormatFlags = sf.FormatFlags Or StringFormatFlags.NoWrap Or StringFormatFlags.NoClip
                gp.AddString(s, Theme.GetFamily(family), CInt(style), emSize,
                             New RectangleF(centerX - 3000.0F, centerY - emSize * 1.4F, 6000.0F, emSize * 2.8F), sf)
            End Using

            Dim bounds = gp.GetBounds()
            If bounds.Width <= 0.0F OrElse bounds.Height <= 0.0F Then Return

            ' Ombre
            Using shadow As New SolidBrush(Color.FromArgb(140, 0, 0, 0))
                Dim state = g.Save()
                g.TranslateTransform(0.0F, emSize * 0.06F)
                g.FillPath(shadow, gp)
                g.Restore(state)
            End Using

            ' Degrade metallique
            Using lg As New LinearGradientBrush(New RectangleF(bounds.X, bounds.Y - 1.0F, bounds.Width, bounds.Height + 2.0F),
                                                Theme.GoldBright, Theme.GoldDark, LinearGradientMode.Vertical)
                Dim blend As New ColorBlend(5)
                blend.Colors = New Color() {Theme.GoldDark, Theme.Gold, Theme.GoldBright, Theme.Gold, Theme.GoldDark}
                blend.Positions = New Single() {0.0F, 0.34F, 0.5F, 0.66F, 1.0F}
                lg.InterpolationColors = blend
                g.FillPath(lg, gp)
            End Using

            If outline Then
                Using pen As New Pen(Color.FromArgb(210, 46, 30, 8), Math.Max(1.2F, emSize * 0.035F))
                    pen.LineJoin = LineJoin.Round
                    g.DrawPath(pen, gp)
                End Using
            End If
        End Using
    End Sub

#End Region

#Region "Decor de casino"

    ''' <summary>Fond de table : feutre vert, motif losange, halo central et vignettage.</summary>
    Public Function CreateFelt(width As Integer, height As Integer) As Bitmap
        If width < 1 Then width = 1
        If height < 1 Then height = 1

        Dim bmp As New Bitmap(width, height, PixelFormat.Format32bppPArgb)

        Using g = Graphics.FromImage(bmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.Clear(Theme.FeltDeep)

            ' Halo central (lustre au-dessus de la table)
            Using path As New GraphicsPath()
                path.AddEllipse(-width * 0.3F, -height * 0.7F, width * 1.6F, height * 2.3F)
                Using pgb As New PathGradientBrush(path)
                    pgb.CenterColor = Theme.FeltLight
                    pgb.SurroundColors = New Color() {Theme.FeltDeep}
                    pgb.CenterPoint = New PointF(width * 0.5F, height * 0.40F)
                    g.FillRectangle(pgb, 0, 0, width, height)
                End Using
            End Using

            ' Motif losange discret
            Using pen As New Pen(Color.FromArgb(10, 255, 255, 255), 1.0F)
                Const stepPx As Integer = 30
                Dim x = -height
                While x < width + height
                    g.DrawLine(pen, x, 0, x + height, height)
                    g.DrawLine(pen, x, height, x + height, 0)
                    x += stepPx
                End While
            End Using

            ' Vignettage
            Using path As New GraphicsPath()
                path.AddEllipse(-width * 0.12F, -height * 0.16F, width * 1.24F, height * 1.32F)
                Using pgb As New PathGradientBrush(path)
                    pgb.CenterColor = Color.FromArgb(0, 0, 0, 0)
                    pgb.SurroundColors = New Color() {Color.FromArgb(205, 0, 0, 0)}
                    pgb.CenterPoint = New PointF(width * 0.5F, height * 0.48F)
                    g.FillRectangle(pgb, 0, 0, width, height)
                End Using
            End Using
        End Using

        Return bmp
    End Function

    ''' <summary>Cadre art deco dore autour d'une zone.</summary>
    Public Sub GoldFrame(g As Graphics, r As RectangleF, Optional radius As Single = 14.0F)
        StrokeRound(g, r, radius, Color.FromArgb(180, Theme.GoldDark), 3.0F)
        StrokeRound(g, Grow(r, -5.0F), Math.Max(2.0F, radius - 4.0F), Color.FromArgb(150, Theme.Gold), 1.4F)

        ' Losanges aux quatre coins
        Dim s = 9.0F
        Dim corners = New PointF() {
            New PointF(r.X, r.Y), New PointF(r.Right, r.Y),
            New PointF(r.Right, r.Bottom), New PointF(r.X, r.Bottom)}

        For Each c In corners
            Using dp = DiamondPath(New RectangleF(c.X - s, c.Y - s, s * 2.0F, s * 2.0F))
                Using b As New SolidBrush(Theme.Gold)
                    g.FillPath(b, dp)
                End Using
                Using pen As New Pen(Theme.GoldDark, 1.2F)
                    g.DrawPath(pen, dp)
                End Using
            End Using
        Next
    End Sub

    ''' <summary>Guirlande d'ampoules clignotantes facon enseigne de casino.</summary>
    Public Sub DrawMarquee(g As Graphics, r As RectangleF, phase As Single, Optional spacing As Single = 34.0F, Optional bulbRadius As Single = 5.0F)
        Dim pts As New List(Of PointF)()

        Dim countX = Math.Max(2, CInt(r.Width / spacing))
        Dim countY = Math.Max(2, CInt(r.Height / spacing))

        For i = 0 To countX - 1
            Dim t = i / CSng(countX)
            pts.Add(New PointF(r.X + r.Width * t, r.Y))
            pts.Add(New PointF(r.Right - r.Width * t, r.Bottom))
        Next
        For i = 0 To countY - 1
            Dim t = i / CSng(countY)
            pts.Add(New PointF(r.Right, r.Y + r.Height * t))
            pts.Add(New PointF(r.X, r.Bottom - r.Height * t))
        Next

        For i = 0 To pts.Count - 1
            Dim wave = CSng(0.5 + 0.5 * Math.Sin(phase * 3.2 + i * 0.55))
            Dim c = Mix(Color.FromArgb(120, 90, 40), Theme.GoldBright, wave)
            DrawGlow(g, pts(i).X, pts(i).Y, bulbRadius * 4.0F, c, CInt(40 + 80 * wave))
            Using b As New SolidBrush(c)
                g.FillEllipse(b, CircleRect(pts(i).X, pts(i).Y, bulbRadius))
            End Using
        Next
    End Sub

#End Region

#Region "Jetons"

    ''' <summary>Couleur officielle d'un jeton selon sa valeur.</summary>
    Public Function ChipColor(value As Integer) As Color
        Select Case value
            Case Is >= 1000 : Return Color.FromArgb(198, 154, 44)
            Case Is >= 500 : Return Color.FromArgb(96, 44, 136)
            Case Is >= 100 : Return Color.FromArgb(28, 28, 34)
            Case Is >= 25 : Return Color.FromArgb(18, 116, 66)
            Case Is >= 5 : Return Color.FromArgb(168, 30, 38)
            Case Else : Return Color.FromArgb(226, 222, 212)
        End Select
    End Function

    Public Function ChipTextColor(value As Integer) As Color
        Return If(value = 1, Theme.Ink, Theme.Cream)
    End Function

    ''' <summary>Dessine un jeton de casino vu de dessus.</summary>
    Public Sub DrawChip(g As Graphics, cx As Single, cy As Single, radius As Single, value As Integer,
                    Optional showValue As Boolean = True, Optional highlight As Boolean = False)
        If radius <= 1.0F Then Return

        Dim baseColor = ChipColor(value)
        Dim stripe = If(value = 1, Color.FromArgb(150, 40, 46), Color.FromArgb(240, 238, 232))

        ' Ombre
        Using b As New SolidBrush(Color.FromArgb(90, 0, 0, 0))
            g.FillEllipse(b, CircleRect(cx + radius * 0.06F, cy + radius * 0.14F, radius))
        End Using

        ' Corps
        Using path As New GraphicsPath()
            path.AddEllipse(CircleRect(cx, cy, radius))
            Using pgb As New PathGradientBrush(path)
                pgb.CenterColor = Mix(baseColor, Color.White, 0.30F)
                pgb.SurroundColors = New Color() {Mix(baseColor, Color.Black, 0.30F)}
                pgb.CenterPoint = New PointF(cx - radius * 0.25F, cy - radius * 0.30F)
                g.FillPath(pgb, path)
            End Using
        End Using

        ' Encoches du bord
        Using pen As New Pen(stripe, radius * 0.40F)
            Dim ring = CircleRect(cx, cy, radius * 0.80F)
            For k = 0 To 5
                g.DrawArc(pen, ring, k * 60.0F - 11.0F, 22.0F)
            Next
        End Using

        ' Anneaux
        Using pen As New Pen(Color.FromArgb(120, 0, 0, 0), Math.Max(1.0F, radius * 0.06F))
            g.DrawEllipse(pen, CircleRect(cx, cy, radius * 0.985F))
        End Using
        Using pen As New Pen(Color.FromArgb(70, 255, 255, 255), Math.Max(1.0F, radius * 0.05F))
            g.DrawEllipse(pen, CircleRect(cx, cy, radius * 0.60F))
        End Using

        ' Pastille centrale
        Using b As New SolidBrush(Mix(baseColor, Color.Black, 0.12F))
            g.FillEllipse(b, CircleRect(cx, cy, radius * 0.55F))
        End Using

        If showValue Then
            Dim label = If(value >= 1000, CStr(value \ 1000) & "K", CStr(value))
            Dim f = Theme.Display(Math.Max(7.0F, radius * 0.66F), FontStyle.Bold)
            DrawTextAt(g, label, f, ChipTextColor(value), cx, cy + radius * 0.02F)
        End If

        If highlight Then
            DrawGlow(g, cx, cy, radius * 1.9F, Theme.GoldBright, 120)
            Using pen As New Pen(Theme.GoldBright, Math.Max(1.5F, radius * 0.10F))
                g.DrawEllipse(pen, CircleRect(cx, cy, radius * 1.10F))
            End Using
        End If
    End Sub

    ''' <summary>Pile de jetons representant un montant mise sur une case.</summary>
    Public Sub DrawChipStack(g As Graphics, cx As Single, cy As Single, radius As Single, amount As Long)
        If amount <= 0L Then Return

        Dim denominations = New Integer() {500, 100, 25, 5, 1}
        Dim pile As New List(Of Integer)()
        Dim rest = amount

        For Each d In denominations
            While rest >= d AndAlso pile.Count < 5
                pile.Add(d)
                rest -= d
            End While
        Next
        If pile.Count = 0 Then pile.Add(1)

        Dim lift = radius * 0.30F
        For i = pile.Count - 1 To 0 Step -1
            DrawChip(g, cx, cy - (pile.Count - 1 - i) * lift, radius, pile(i), False)
        Next

        ' Montant total au-dessus de la pile
        Dim top = cy - (pile.Count - 1) * lift
        Dim label = AppState.Money(amount)
        Dim f = Theme.Display(Math.Max(8.0F, radius * 0.80F), FontStyle.Bold)

        DrawTextAt(g, label, f, Color.FromArgb(190, 0, 0, 0), cx + 1.0F, top + 1.0F)
        DrawTextAt(g, label, f, Theme.GoldBright, cx, top)
    End Sub

#End Region

End Module
