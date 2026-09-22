Imports System.Windows.Forms

''' <summary>
''' Point d'entree de l'application.
''' </summary>
Module Program

    <STAThread>
    Public Sub Main()
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        AppState.Load()

        Using shell As New MainForm()
            Application.Run(shell)
        End Using

        AppState.Save()
    End Sub

End Module
