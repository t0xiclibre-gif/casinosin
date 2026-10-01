# casinosin
Petit jeu de casino en Visual Basic (Visual Studio 2019, .NET Framework 4.7.2).

Ouvrir `Casinosin.sln` dans Visual Studio 2019 puis lancer (F5).

## RNG décroissante

À chaque clic sur **Essayer**, un nombre est tiré entre 1 et le maximum actuel,
puis le maximum **baisse de 1** pour l'essai suivant (100, 99, 98, ...), sans descendre sous 1.
Tirer `1` fait gagner — les chances augmentent donc à chaque essai.

La logique est dans `Casinosin/RngDecroissant.vb` (méthode `Essayer`).
