using System;
using System.Collections.Generic;

namespace RouletteRusse
{
    /// <summary>Type de balle dans une chambre du barillet.</summary>
    public enum Balle
    {
        Blanc,
        Noir,
        Rouge
    }

    /// <summary>
    /// Revolver à 12 chambres : 10 balles à blanc, 1 noire, 1 rouge, mélangées.
    /// </summary>
    public class Revolver
    {
        public const int NombreChambres = 12;

        private readonly Balle[] chambres = new Balle[NombreChambres];
        private readonly bool[] tirees = new bool[NombreChambres];

        /// <summary>Index de la prochaine chambre à tirer.</summary>
        public int Position { get; private set; }

        public int CoupsRestants
        {
            get { return NombreChambres - Position; }
        }

        public bool EstVide
        {
            get { return Position >= NombreChambres; }
        }

        public Revolver(Random rng)
        {
            Charger(rng);
        }

        /// <summary>Recharge et fait tourner le barillet.</summary>
        public void Charger(Random rng)
        {
            var balles = new List<Balle>();
            for (int i = 0; i < 10; i++)
                balles.Add(Balle.Blanc);
            balles.Add(Balle.Noir);
            balles.Add(Balle.Rouge);

            // Mélange de Fisher-Yates
            for (int i = balles.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                Balle tmp = balles[i];
                balles[i] = balles[j];
                balles[j] = tmp;
            }

            for (int i = 0; i < NombreChambres; i++)
            {
                chambres[i] = balles[i];
                tirees[i] = false;
            }
            Position = 0;
        }

        public Balle Tirer()
        {
            if (EstVide)
                throw new InvalidOperationException("Le barillet est vide.");

            Balle balle = chambres[Position];
            tirees[Position] = true;
            Position++;
            return balle;
        }

        public bool EstTiree(int chambre)
        {
            return tirees[chambre];
        }

        /// <summary>Contenu d'une chambre. Ne l'afficher que si elle a été tirée.</summary>
        public Balle BalleA(int chambre)
        {
            return chambres[chambre];
        }
    }
}
