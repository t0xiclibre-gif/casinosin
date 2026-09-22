# Casino Sin — jeu de casino 2D en VB.NET

Projet personnel, **solo uniquement** : on joue contre la banque (un bot), jamais
contre d'autres joueurs. Les jetons sont **100 % fictifs**, il n'y a aucun achat,
aucun paiement, aucune connexion réseau.

> **État : en cours de développement — le projet ne compile pas encore.**
> Le moteur de rendu et les fondations sont écrits, les écrans de jeu ne le sont
> pas. Voir « Où j'en suis » plus bas.

---

## Le concept

1. **Écran d'accueil** — une seule entrée : le bouton `START`.
2. **Menu des jeux** — deux cartes sur un fond de casino cohérent (feutre vert,
   bois, dorures, guirlande d'ampoules) :
   - **Roulette européenne** (0 à 36, un seul zéro) — mises sur le tapis,
     animation de la roue et de la bille ;
   - **Blackjack** contre un croupier-bot.
3. La cave (bankroll) est partagée entre les deux jeux et sauvegardée entre deux
   parties.

---

## Où j'en suis

### Fait

| Fichier | Rôle | Lignes |
|---|---|---|
| `CasinoSin.sln` | Solution Visual Studio | — |
| `src/CasinoSin/CasinoSin.vbproj` | Projet VB.NET, `net8.0-windows`, WinForms | — |
| `src/CasinoSin/Program.vb` | Point d'entrée `Sub Main`, DPI, chargement du profil | 23 |
| `src/CasinoSin/Core/AppState.vb` | Cave, statistiques, mise/ruine, sauvegarde dans `%AppData%\CasinoSin\profil.txt` | 112 |
| `src/CasinoSin/Core/Theme.vb` | Palette (feutre, bois, or, rouge/noir) + cache de polices | 92 |
| `src/CasinoSin/Core/Draw.vb` | Boîte à outils GDI+ : formes arrondies, ombres, halos, titres dorés gravés, **fond de feutre** (halo + motif losange + vignettage), cadre art déco, guirlande d'ampoules, **dessin des jetons et des piles de jetons** | 459 |
| `src/CasinoSin/Core/Widgets.vb` | `UiButton` (bouton entièrement dessiné, animation de survol) et `GameCard` (grande carte du menu) | 135 |

Tout est **dessiné à la main en GDI+**, sans contrôles WinForms ni Designer :
c'est ce qui permet les fondus enchaînés entre écrans et les animations.

### Reste à faire

- [ ] `Core/CasinoScreen.vb` — classe de base des écrans : double buffering,
      boucle à 60 fps, gestion souris, fond de feutre en cache, voile de
      transition.
- [ ] `MainForm.vb` — fenêtre hôte + navigation avec fondu au noir.
- [ ] `Screens/StartScreen.vb` — titre doré, bouton `START`, guirlande animée.
- [ ] `Screens/MenuScreen.vb` — les deux `GameCard` (roulette / blackjack).
- [ ] `Roulette/RouletteData.vb` — ordre réel des cases de la roue européenne
      (`0, 32, 15, 19, 4, 21, 2, 25, …`) et couleur de chaque numéro.
- [ ] `Roulette/BetBoard.vb` — **le tapis** : géométrie (zéro + grille 12 × 3 +
      colonnes « 2:1 » + douzaines + chances simples) et surtout le *hit-testing*
      des mises : plein, cheval, transversale, carré, sixain, douzaine, colonne,
      rouge/noir, pair/impair, manque/passe.
- [ ] `Roulette/RouletteScreen.vb` — pose des jetons sur le tapis, annuler /
      effacer / répéter / doubler, limites de table, **animation de la roue**
      (roue qui tourne, bille en sens inverse qui décélère, rebondit puis se cale
      exactement dans la case gagnante), historique des numéros, paiements.
- [ ] `Blackjack/*.vb` — sabot de 6 jeux, cartes dessinées en GDI+, tirage
      animé, bot croupier (tire jusqu'à 17), tirer / rester / doubler / séparer.

### Décisions techniques déjà prises

- **Cible `net8.0-windows` + WinForms**, projet au format SDK, `Option Strict Off`
  (défaut VB) et `MyType=Empty` pour garder un `Sub Main` explicite.
- **Aucun fichier `.Designer.vb`** : tout est écrit à la main, les écrans héritent
  d'un `Control` custom et se peignent eux-mêmes.
- **Bitmaps en cache** pour le feutre, la face de la roue et le tapis, afin de
  tenir 60 fps ; seuls les éléments mobiles (bille, jetons, surbrillance) sont
  redessinés à chaque image.
- **Roue en deux couches** : la cuvette extérieure (piste de bille) reste fixe,
  seule la tête de roue (cases + moyeu) tourne — comme une vraie roulette.
- **Animation de la roue** : on tire d'abord le numéro gagnant, puis on calcule
  les angles de départ pour que la bille finisse *exactement* dans la bonne case
  après décélération (pas de triche visuelle, pas d'à-peu-près).
- Fichiers sources en **UTF-8 avec BOM** pour que les accents soient lus
  correctement par le compilateur VB.

### Limite de cet environnement

Le SDK .NET n'est pas installé ici et le téléchargement est bloqué par la
politique réseau : **le code n'a donc pas pu être compilé ni exécuté**. Il faudra
une passe de compilation sous Windows pour corriger les éventuelles coquilles.

---

## Lancer le projet (une fois terminé)

Prérequis : Windows + [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
(ou Visual Studio 2022 avec la charge de travail « Développement .NET Desktop »).

```powershell
git clone <ce-dépôt>
cd casinosin
dotnet run --project src/CasinoSin/CasinoSin.vbproj
```

Ou : ouvrir `CasinoSin.sln` dans Visual Studio et appuyer sur F5.

## Arborescence

```
CasinoSin.sln
src/CasinoSin/
├── CasinoSin.vbproj
├── Program.vb            point d'entrée
├── Core/                 moteur : état, thème, dessin, widgets
├── Screens/              accueil, menu, écrans de jeu       (à écrire)
├── Roulette/             données, tapis, animation de la roue (à écrire)
└── Blackjack/            cartes, sabot, croupier-bot         (à écrire)
```

## Avertissement

Jeu de simulation à but personnel et pédagogique. Aucune monnaie réelle, aucun
gain réel, aucun lien avec un opérateur de jeux d'argent.
