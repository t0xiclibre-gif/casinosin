using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RouletteRusse
{
    public partial class FormRouletteRusse : Form
    {
        private const int TicksSuspense = 35;
        private const int TicksRecul = 25;

        private static readonly Color CouleurFond = Color.FromArgb(18, 60, 40);
        private static readonly Color CouleurBoutonActif = Color.DarkRed;
        private static readonly Color CouleurBoutonInactif = Color.FromArgb(60, 60, 66);

        private readonly Partie partie = new Partie();
        private readonly Random rng = new Random();

        private PictureBox[] portraits;
        private PictureBox[] revolvers;
        private Label[] noms;
        private Label[] coups;
        private Button[] boutonsTirer;
        private BarilletControl[] barillets;
        private Point[] positionsPortraits;
        private Point[] positionsRevolvers;

        // Animation du tir en cours
        private int tick;
        private int tireur;
        private Balle balleTiree;
        private ResultatTir resultat;

        public FormRouletteRusse()
        {
            InitializeComponent();

            portraits = new[] { picJoueur1, picJoueur2 };
            revolvers = new[] { picRevolver1, picRevolver2 };
            noms = new[] { lblNom1, lblNom2 };
            coups = new[] { lblCoups1, lblCoups2 };
            boutonsTirer = new[] { btnTirer1, btnTirer2 };
            barillets = new[] { barillet1, barillet2 };
            positionsPortraits = new[] { picJoueur1.Location, picJoueur2.Location };
            positionsRevolvers = new[] { picRevolver1.Location, picRevolver2.Location };

            ChargerImage(picJoueur1, "joueur1.png");
            ChargerImage(picJoueur2, "joueur2.png");
            ChargerImage(picRevolver1, "revolver1.png");
            ChargerImage(picRevolver2, "revolver2.png");

            barillet1.Revolver = partie.Revolvers[0];
            barillet2.Revolver = partie.Revolvers[1];

            NouvellePartie();
        }

        private static void ChargerImage(PictureBox pic, string fichier)
        {
            // Ne remplace pas une image déjà mise dans le concepteur
            if (pic.Image != null)
                return;

            string chemin = Path.Combine(Application.StartupPath, "Images", fichier);
            if (File.Exists(chemin))
                pic.Image = Image.FromFile(chemin);
        }

        private void NouvellePartie()
        {
            timerAnimation.Stop();
            partie.Nouvelle();
            RestaurerPositions();
            BackColor = CouleurFond;

            barillet1.Revolver = partie.Revolvers[0];
            barillet2.Revolver = partie.Revolvers[1];

            lstHistorique.Items.Clear();
            AjouterHistorique("Nouvelle partie. " + noms[partie.JoueurActif].Text + " commence.");
            lblResultat.Text = "";
            MettreAJourInterface();
        }

        private void MettreAJourInterface()
        {
            // Pendant l'animation, on garde le tireur en avant pour ne pas trahir le résultat
            bool animation = timerAnimation.Enabled;
            int joueurEnAvant = animation ? tireur : partie.JoueurActif;
            bool termine = partie.EstTerminee && !animation;

            for (int j = 0; j < 2; j++)
            {
                bool peutTirer = !termine && !animation && partie.JoueurActif == j;
                boutonsTirer[j].Enabled = peutTirer;
                boutonsTirer[j].BackColor = peutTirer ? CouleurBoutonActif : CouleurBoutonInactif;
                barillets[j].Actif = !termine && joueurEnAvant == j;
                barillets[j].Invalidate();
                noms[j].ForeColor = barillets[j].Actif ? Color.Gold : Color.White;
                coups[j].Text = "Coups restants : " + partie.Revolvers[j].CoupsRestants;
            }

            if (animation)
                lblTour.Text = noms[tireur].Text + " appuie sur la détente...";
            else if (termine)
                lblTour.Text = noms[partie.Gagnant].Text + " GAGNE LA PARTIE !";
            else
                lblTour.Text = "Tour de : " + noms[partie.JoueurActif].Text;
        }

        private void Tirer(int joueur)
        {
            if (partie.EstTerminee || partie.JoueurActif != joueur || timerAnimation.Enabled)
                return;

            tireur = joueur;
            resultat = partie.Tirer(out balleTiree);

            tick = 0;
            lblResultat.ForeColor = Color.White;
            timerAnimation.Start();
            MettreAJourInterface();
        }

        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            tick++;

            if (tick < TicksSuspense)
            {
                // Suspense : le revolver tremble
                lblResultat.Text = new string('.', 1 + (tick / 8) % 3);
                Trembler(revolvers[tireur], positionsRevolvers[tireur], 2);
                return;
            }

            if (tick == TicksSuspense)
            {
                RevelerTir();
                return;
            }

            int t = tick - TicksSuspense;
            if (t < TicksRecul)
            {
                // Recul du revolver, qui revient petit à petit
                int recul = (TicksRecul - t) * 20 / TicksRecul;
                int sens = tireur == 0 ? -1 : 1;
                revolvers[tireur].Location = new Point(positionsRevolvers[tireur].X + sens * recul, positionsRevolvers[tireur].Y);

                if (resultat == ResultatTir.Touche)
                    Trembler(portraits[1 - tireur], positionsPortraits[1 - tireur], 8);
                else if (resultat == ResultatTir.Ricochet)
                    Trembler(portraits[tireur], positionsPortraits[tireur], 8);

                if (resultat != ResultatTir.Clic && t == 6)
                    BackColor = CouleurFond;
                return;
            }

            timerAnimation.Stop();
            RestaurerPositions();
            MettreAJourInterface();
        }

        private void RevelerTir()
        {
            string nomTireur = noms[tireur].Text;
            string nomAdversaire = noms[1 - tireur].Text;
            barillets[tireur].Tourner();

            switch (resultat)
            {
                case ResultatTir.Touche:
                    lblResultat.Text = "BANG !";
                    lblResultat.ForeColor = Color.Red;
                    BackColor = Color.DarkRed;
                    AjouterHistorique(nomTireur + " : balle ROUGE, " + nomAdversaire + " est touché !");
                    break;
                case ResultatTir.Ricochet:
                    lblResultat.Text = "RICOCHET !";
                    lblResultat.ForeColor = Color.Black;
                    BackColor = Color.FromArgb(10, 10, 10);
                    AjouterHistorique(nomTireur + " : balle NOIRE, ricochet, " + nomTireur + " est touché !");
                    break;
                default:
                    lblResultat.Text = "clic";
                    lblResultat.ForeColor = Color.Gainsboro;
                    AjouterHistorique(nomTireur + " : à blanc (" + partie.Revolvers[tireur].CoupsRestants + " coups restants)");
                    break;
            }

            if (partie.EstTerminee)
                AjouterHistorique("*** " + noms[partie.Gagnant].Text + " gagne ! ***");
        }

        private void Trembler(Control controle, Point origine, int force)
        {
            controle.Location = new Point(
                origine.X + rng.Next(-force, force + 1),
                origine.Y + rng.Next(-force, force + 1));
        }

        private void RestaurerPositions()
        {
            for (int j = 0; j < 2; j++)
            {
                portraits[j].Location = positionsPortraits[j];
                revolvers[j].Location = positionsRevolvers[j];
            }
        }

        private void AjouterHistorique(string texte)
        {
            lstHistorique.Items.Add(texte);
            lstHistorique.TopIndex = lstHistorique.Items.Count - 1;
        }

        private void btnTirer1_Click(object sender, EventArgs e)
        {
            Tirer(0);
        }

        private void btnTirer2_Click(object sender, EventArgs e)
        {
            Tirer(1);
        }

        private void btnNouvellePartie_Click(object sender, EventArgs e)
        {
            NouvellePartie();
        }
    }
}
