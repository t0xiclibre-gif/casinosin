Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FormCasino
    Inherits Form

    Private Const NombreGagnant As Integer = 1
    Private Const MaxDepart As Integer = 100

    Private ReadOnly _rng As New RngDecroissant(1, MaxDepart)

    Private ReadOnly lblPlage As New Label()
    Private ReadOnly lblResultat As New Label()
    Private ReadOnly lblEssais As New Label()
    Private WithEvents btnEssayer As New Button()
    Private WithEvents btnReset As New Button()

    Public Sub New()
        Text = "Casinosin"
        ClientSize = New Size(360, 220)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        StartPosition = FormStartPosition.CenterScreen

        lblPlage.SetBounds(20, 20, 320, 24)
        lblPlage.Font = New Font("Segoe UI", 11.0F)

        lblResultat.SetBounds(20, 55, 320, 40)
        lblResultat.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)

        lblEssais.SetBounds(20, 100, 320, 24)
        lblEssais.Font = New Font("Segoe UI", 10.0F)

        btnEssayer.Text = "Essayer"
        btnEssayer.SetBounds(20, 150, 150, 40)

        btnReset.Text = "Recommencer"
        btnReset.SetBounds(190, 150, 150, 40)

        Controls.AddRange(New Control() {lblPlage, lblResultat, lblEssais, btnEssayer, btnReset})
        MettreAJourAffichage("—")
    End Sub

    Private Sub btnEssayer_Click(sender As Object, e As EventArgs) Handles btnEssayer.Click
        ' Le tirage utilise la plage actuelle, puis la RNG baisse de 1 pour le prochain essai.
        Dim tirage As Integer = _rng.Essayer()

        If tirage = NombreGagnant Then
            MettreAJourAffichage(tirage & " — GAGNÉ !")
            btnEssayer.Enabled = False
        Else
            MettreAJourAffichage(tirage.ToString())
        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        _rng.Reinitialiser()
        btnEssayer.Enabled = True
        MettreAJourAffichage("—")
    End Sub

    Private Sub MettreAJourAffichage(resultat As String)
        lblPlage.Text = "Prochain tirage entre " & _rng.Minimum & " et " & _rng.MaximumActuel
        lblResultat.Text = resultat
        lblEssais.Text = "Essais : " & _rng.NombreEssais & "   (gagnant = " & NombreGagnant & ")"
    End Sub
End Class
