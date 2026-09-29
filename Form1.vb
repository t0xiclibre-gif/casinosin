Public Class Form1

    ' 2 balles sur 12 chambres
    Private Const Chambres As Integer = 12
    Private Const Balles As Integer = 2

    ' Attente (ms) avant que l'adversaire joue, puis avant de pouvoir rejouer
    Private Const AttenteAdversaire As Integer = 1500
    Private Const AttenteRejouer As Integer = 3000

    Private ReadOnly rnd As New Random()

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False

        ' Tour du joueur
        TextBox1.Text = Tirer("Toi")

        ' Tour de l'adversaire, tout seul
        TextBox2.Text = "L'adversaire prend le revolver..."
        Await Task.Delay(AttenteAdversaire)
        TextBox2.Text = Tirer("Adversaire")

        ' Attente avant de pouvoir rejouer
        Button1.Text = "Attends..."
        Await Task.Delay(AttenteRejouer)
        Button1.Text = "Tirer"
        Button1.Enabled = True
    End Sub

    ' 2/12 que ça tire, puis 1/2 noir ou rouge
    Private Function Tirer(nom As String) As String
        If rnd.Next(Chambres) < Balles Then
            Dim couleur As String = If(rnd.Next(2) = 0, "NOIRE", "ROUGE")
            Return nom & " : PAN ! Balle " & couleur
        End If
        Return nom & " : clic... rien"
    End Function

End Class
