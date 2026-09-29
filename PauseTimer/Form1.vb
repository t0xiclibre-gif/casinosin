' Exemple : faire une pause grâce au Timer (VB.NET WinForms)
' Sur le formulaire : un Button "btnJouer", un Label "lblResultat"
' et un Timer "tmrPause" (glissé depuis la Boîte à outils).

Public Class Form1

    Private ReadOnly rnd As New Random()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tmrPause.Interval = 2000   ' durée de la pause en millisecondes (2 s)
        tmrPause.Enabled = False
    End Sub

    Private Sub btnJouer_Click(sender As Object, e As EventArgs) Handles btnJouer.Click
        btnJouer.Enabled = False           ' on bloque le bouton pendant la pause
        lblResultat.Text = "Tirage en cours..."
        tmrPause.Start()                   ' début de la pause
    End Sub

    ' Appelé automatiquement quand la pause est terminée
    Private Sub tmrPause_Tick(sender As Object, e As EventArgs) Handles tmrPause.Tick
        tmrPause.Stop()                    ' IMPORTANT : sinon le Tick se répète
        lblResultat.Text = "Résultat : " & rnd.Next(1, 7).ToString()
        btnJouer.Enabled = True
    End Sub

End Class
