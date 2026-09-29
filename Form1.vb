Public Class Form1
    Dim MyValue As Integer
    Dim Color As Integer
    Dim EnValue As Integer
    Dim EnColor As Integer

    ' Timer utilisé pour faire une pause de 3 secondes
    Private WithEvents TimerPause As New System.Windows.Forms.Timer()

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        Button1.Text = "attendez"

        MyValue = Int((12 * Rnd()) + 1)
        Color = Int((2 * Rnd()) + 1)
        EnValue = Int((12 * Rnd()) + 1)
        EnColor = Int((2 * Rnd()) + 1)

        If MyValue = 1 Or MyValue = 2 Then
            If Color = 1 Then
                TextBox1.Text = "Toucher rouge vous avez gagnez le double de la mise"
            Else
                TextBox1.Text = "Toucher noir vous avez gagnez la moitier de la mise"
            End If
        Else
            TextBox1.Text = "rien"

            ' Pause de 3 secondes : le bouton sera réactivé dans TimerPause_Tick
            TimerPause.Interval = 3000
            TimerPause.Start()
            Exit Sub
        End If

        Button1.Enabled = True
        Button1.Text = "tirer"
    End Sub

    Private Sub TimerPause_Tick(sender As Object, e As EventArgs) Handles TimerPause.Tick
        TimerPause.Stop()
        Button1.Enabled = True
        Button1.Text = "tirer"
    End Sub

End Class
