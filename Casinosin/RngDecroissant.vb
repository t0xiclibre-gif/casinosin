Imports System

''' <summary>
''' Générateur aléatoire dont la borne maximale baisse de 1 à chaque essai.
''' Exemple avec un max de 10 : essai 1 -> 1..10, essai 2 -> 1..9, essai 3 -> 1..8, etc.
''' La borne ne descend jamais en dessous du minimum.
''' </summary>
Public Class RngDecroissant
    Private ReadOnly _random As New Random()
    Private ReadOnly _maxInitial As Integer

    Public ReadOnly Property Minimum As Integer
    Public Property MaximumActuel As Integer
    Public Property NombreEssais As Integer

    Public Sub New(minimum As Integer, maximumInitial As Integer)
        If maximumInitial < minimum Then
            Throw New ArgumentException("Le maximum doit être supérieur ou égal au minimum.")
        End If
        Me.Minimum = minimum
        _maxInitial = maximumInitial
        Reinitialiser()
    End Sub

    ''' <summary>
    ''' Tire un nombre entre Minimum et MaximumActuel (inclus),
    ''' puis baisse MaximumActuel de 1 pour le prochain essai.
    ''' </summary>
    Public Function Essayer() As Integer
        Dim resultat As Integer = _random.Next(Minimum, MaximumActuel + 1)
        NombreEssais += 1
        If MaximumActuel > Minimum Then
            MaximumActuel -= 1
        End If
        Return resultat
    End Function

    Public Sub Reinitialiser()
        MaximumActuel = _maxInitial
        NombreEssais = 0
    End Sub
End Class
