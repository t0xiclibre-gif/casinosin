Imports System.Globalization
Imports System.IO

''' <summary>
''' Etat global du joueur : cave (bankroll), statistiques et sauvegarde sur disque.
''' Argent 100 % fictif, aucun paiement reel n'est possible.
''' </summary>
Public NotInheritable Class AppState

    Public Const StartingBankroll As Long = 1000
    Public Const EmergencyCredit As Long = 500

    Private Shared _bankroll As Long = StartingBankroll

    ''' <summary>Generateur aleatoire partage par les deux jeux.</summary>
    Public Shared ReadOnly Rng As New Random()

    Public Shared Property Best As Long = StartingBankroll
    Public Shared Property SpinCount As Long = 0
    Public Shared Property HandCount As Long = 0
    Public Shared Property TotalWagered As Long = 0

    Public Shared Property Bankroll As Long
        Get
            Return _bankroll
        End Get
        Set(value As Long)
            _bankroll = If(value < 0L, 0L, value)
            If _bankroll > Best Then Best = _bankroll
        End Set
    End Property

    ''' <summary>Retire une mise de la cave. Renvoie False si les fonds sont insuffisants.</summary>
    Public Shared Function TryWager(amount As Long) As Boolean
        If amount <= 0L OrElse amount > _bankroll Then Return False
        Bankroll = _bankroll - amount
        TotalWagered += amount
        Return True
    End Function

    ''' <summary>Cave de secours offerte quand le joueur est ruine (jetons fictifs).</summary>
    Public Shared Function Refill() As Boolean
        If _bankroll >= 50L Then Return False
        Bankroll = _bankroll + EmergencyCredit
        Return True
    End Function

    ''' <summary>Formate un montant en jetons : 12345 -> "12 345".</summary>
    Public Shared Function Money(amount As Long) As String
        Dim neg = amount < 0L
        Dim s = Math.Abs(amount).ToString("#,##0", CultureInfo.InvariantCulture).Replace(","c, " "c)
        Return If(neg, "-" & s, s)
    End Function

    Private Shared ReadOnly Property SaveFile As String
        Get
            Dim dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CasinoSin")
            Return Path.Combine(dir, "profil.txt")
        End Get
    End Property

    Public Shared Sub Load()
        Try
            Dim savePath = SaveFile
            If Not IO.File.Exists(savePath) Then Return

            For Each raw As String In IO.File.ReadAllLines(savePath)
                Dim sep = raw.IndexOf("="c)
                If sep <= 0 Then Continue For

                Dim key = raw.Substring(0, sep).Trim()
                Dim value As Long
                If Not Long.TryParse(raw.Substring(sep + 1).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, value) Then Continue For

                Select Case key
                    Case "cave" : _bankroll = If(value < 0L, 0L, value)
                    Case "record" : Best = value
                    Case "tours" : SpinCount = value
                    Case "mains" : HandCount = value
                    Case "mise_totale" : TotalWagered = value
                End Select
            Next

            If Best < _bankroll Then Best = _bankroll
        Catch
            ' Une sauvegarde illisible ne doit jamais empecher de jouer.
        End Try
    End Sub

    Public Shared Sub Save()
        Try
            Dim savePath = SaveFile
            Directory.CreateDirectory(Path.GetDirectoryName(savePath))

            Dim sb As New Text.StringBuilder()
            sb.AppendLine("# Profil Casino Sin (jetons fictifs)")
            sb.AppendLine("cave=" & _bankroll.ToString(CultureInfo.InvariantCulture))
            sb.AppendLine("record=" & Best.ToString(CultureInfo.InvariantCulture))
            sb.AppendLine("tours=" & SpinCount.ToString(CultureInfo.InvariantCulture))
            sb.AppendLine("mains=" & HandCount.ToString(CultureInfo.InvariantCulture))
            sb.AppendLine("mise_totale=" & TotalWagered.ToString(CultureInfo.InvariantCulture))

            IO.File.WriteAllText(savePath, sb.ToString(), Text.Encoding.UTF8)
        Catch
            ' Sauvegarde best-effort.
        End Try
    End Sub

    Private Sub New()
    End Sub

End Class
