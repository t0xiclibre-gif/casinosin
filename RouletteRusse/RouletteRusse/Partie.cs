using System;

namespace RouletteRusse
{
    public enum ResultatTir
    {
        /// <summary>Balle à blanc : rien ne se passe, l'adversaire joue.</summary>
        Clic,
        /// <summary>Balle rouge : l'adversaire est touché, le tireur gagne.</summary>
        Touche,
        /// <summary>Balle noire : ricochet, le tireur est touché et perd.</summary>
        Ricochet
    }

    /// <summary>
    /// Règles d'une partie à 2 joueurs. Chaque joueur tire avec son propre revolver
    /// sur l'adversaire, chacun son tour.
    /// Pour changer l'effet des balles noire et rouge, modifier uniquement <see cref="Tirer"/>.
    /// </summary>
    public class Partie
    {
        private readonly Random rng = new Random();

        public Revolver[] Revolvers { get; private set; }

        /// <summary>0 = joueur 1, 1 = joueur 2.</summary>
        public int JoueurActif { get; private set; }

        /// <summary>-1 tant que la partie n'est pas finie.</summary>
        public int Gagnant { get; private set; }

        public bool EstTerminee
        {
            get { return Gagnant >= 0; }
        }

        public Partie()
        {
            Revolvers = new[] { new Revolver(rng), new Revolver(rng) };
            Nouvelle();
        }

        public void Nouvelle()
        {
            Revolvers[0].Charger(rng);
            Revolvers[1].Charger(rng);
            JoueurActif = rng.Next(2);
            Gagnant = -1;
        }

        public ResultatTir Tirer(out Balle balle)
        {
            if (EstTerminee)
                throw new InvalidOperationException("La partie est terminée.");

            int tireur = JoueurActif;
            int adversaire = 1 - tireur;
            balle = Revolvers[tireur].Tirer();

            switch (balle)
            {
                case Balle.Rouge:
                    Gagnant = tireur;
                    return ResultatTir.Touche;
                case Balle.Noir:
                    Gagnant = adversaire;
                    return ResultatTir.Ricochet;
                default:
                    JoueurActif = adversaire;
                    return ResultatTir.Clic;
            }
        }
    }
}
