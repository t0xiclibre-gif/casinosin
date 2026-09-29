using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RouletteRusse
{
    /// <summary>
    /// Dessine le barillet à 12 chambres d'un revolver.
    /// La chambre du prochain tir est en haut, entourée en doré.
    /// Les chambres déjà tirées montrent leur balle (blanc, noir, rouge).
    /// </summary>
    public class BarilletControl : Control
    {
        private Revolver revolver;
        private float positionAffichee;
        private readonly Timer timerRotation = new Timer { Interval = 15 };

        public BarilletControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            timerRotation.Tick += TimerRotation_Tick;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Revolver Revolver
        {
            get { return revolver; }
            set
            {
                revolver = value;
                positionAffichee = revolver == null ? 0 : revolver.Position;
                Invalidate();
            }
        }

        [Category("Apparence")]
        [DefaultValue(false)]
        public bool Actif { get; set; }

        /// <summary>Lance l'animation de rotation vers la position actuelle du revolver.</summary>
        public void Tourner()
        {
            timerRotation.Start();
        }

        private void TimerRotation_Tick(object sender, EventArgs e)
        {
            if (revolver == null)
            {
                timerRotation.Stop();
                return;
            }

            float cible = revolver.Position;
            positionAffichee += (cible - positionAffichee) * 0.2f;
            if (Math.Abs(cible - positionAffichee) < 0.01f)
            {
                positionAffichee = cible;
                timerRotation.Stop();
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float taille = Math.Min(ClientSize.Width, ClientSize.Height) - 4;
            if (taille <= 0)
                return;

            float cx = ClientSize.Width / 2f;
            float cy = ClientSize.Height / 2f;
            float rayon = taille / 2f;
            float rayonChambres = rayon * 0.68f;
            float rayonTrou = rayon * 0.15f;

            // Corps du barillet
            var corps = new RectangleF(cx - rayon, cy - rayon, taille, taille);
            using (var degrade = new LinearGradientBrush(corps, Color.FromArgb(110, 110, 120), Color.FromArgb(40, 40, 48), 45f))
                g.FillEllipse(degrade, corps);
            using (var bord = new Pen(Actif ? Color.Gold : Color.FromArgb(25, 25, 30), Actif ? 4f : 2f))
                g.DrawEllipse(bord, corps);

            // Axe central
            float rayonAxe = rayon * 0.14f;
            using (var axe = new SolidBrush(Color.FromArgb(30, 30, 35)))
                g.FillEllipse(axe, cx - rayonAxe, cy - rayonAxe, rayonAxe * 2, rayonAxe * 2);

            // Repère du canon (en haut)
            using (var repere = new SolidBrush(Color.Gold))
            {
                g.FillPolygon(repere, new[]
                {
                    new PointF(cx - 7, 0),
                    new PointF(cx + 7, 0),
                    new PointF(cx, 10)
                });
            }

            for (int i = 0; i < Revolver.NombreChambres; i++)
            {
                double angle = (-90 + (i - positionAffichee) * (360.0 / Revolver.NombreChambres)) * Math.PI / 180.0;
                float x = cx + (float)Math.Cos(angle) * rayonChambres;
                float y = cy + (float)Math.Sin(angle) * rayonChambres;
                var trou = new RectangleF(x - rayonTrou, y - rayonTrou, rayonTrou * 2, rayonTrou * 2);

                Color remplissage = Color.FromArgb(15, 15, 18);
                Color contour = Color.FromArgb(70, 70, 78);
                // Une chambre n'est révélée qu'une fois passée sous le canon (animation finie)
                if (revolver != null && revolver.EstTiree(i) && i < positionAffichee - 0.5f)
                {
                    switch (revolver.BalleA(i))
                    {
                        case Balle.Rouge:
                            remplissage = Color.Red;
                            contour = Color.DarkRed;
                            break;
                        case Balle.Noir:
                            remplissage = Color.Black;
                            contour = Color.White;
                            break;
                        default:
                            remplissage = Color.WhiteSmoke;
                            contour = Color.Gray;
                            break;
                    }
                }

                using (var pinceau = new SolidBrush(remplissage))
                    g.FillEllipse(pinceau, trou);
                using (var stylo = new Pen(contour, 2f))
                    g.DrawEllipse(stylo, trou);

                bool prochaine = revolver != null && i == (int)Math.Round(positionAffichee) && i < Revolver.NombreChambres;
                if (prochaine && Actif)
                {
                    using (var halo = new Pen(Color.Gold, 3f))
                        g.DrawEllipse(halo, RectangleF.Inflate(trou, 4, 4));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                timerRotation.Dispose();
            base.Dispose(disposing);
        }
    }
}
