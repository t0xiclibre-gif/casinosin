namespace RouletteRusse
{
    partial class FormRouletteRusse
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picJoueur1 = new System.Windows.Forms.PictureBox();
            this.picJoueur2 = new System.Windows.Forms.PictureBox();
            this.picRevolver1 = new System.Windows.Forms.PictureBox();
            this.picRevolver2 = new System.Windows.Forms.PictureBox();
            this.lblNom1 = new System.Windows.Forms.Label();
            this.lblNom2 = new System.Windows.Forms.Label();
            this.btnTirer1 = new System.Windows.Forms.Button();
            this.btnTirer2 = new System.Windows.Forms.Button();
            this.lblCoups1 = new System.Windows.Forms.Label();
            this.lblCoups2 = new System.Windows.Forms.Label();
            this.lblTitre = new System.Windows.Forms.Label();
            this.lblTour = new System.Windows.Forms.Label();
            this.lblResultat = new System.Windows.Forms.Label();
            this.barillet1 = new RouletteRusse.BarilletControl();
            this.barillet2 = new RouletteRusse.BarilletControl();
            this.lstHistorique = new System.Windows.Forms.ListBox();
            this.btnNouvellePartie = new System.Windows.Forms.Button();
            this.lblLegende = new System.Windows.Forms.Label();
            this.timerAnimation = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picJoueur1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picJoueur2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRevolver1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRevolver2)).BeginInit();
            this.SuspendLayout();
            //
            // picJoueur1
            //
            this.picJoueur1.BackColor = System.Drawing.Color.Transparent;
            this.picJoueur1.Location = new System.Drawing.Point(17, 17);
            this.picJoueur1.Name = "picJoueur1";
            this.picJoueur1.Size = new System.Drawing.Size(300, 250);
            this.picJoueur1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picJoueur1.TabIndex = 0;
            this.picJoueur1.TabStop = false;
            //
            // picJoueur2
            //
            this.picJoueur2.BackColor = System.Drawing.Color.Transparent;
            this.picJoueur2.Location = new System.Drawing.Point(947, 17);
            this.picJoueur2.Name = "picJoueur2";
            this.picJoueur2.Size = new System.Drawing.Size(300, 250);
            this.picJoueur2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picJoueur2.TabIndex = 1;
            this.picJoueur2.TabStop = false;
            //
            // picRevolver1
            //
            this.picRevolver1.BackColor = System.Drawing.Color.Transparent;
            this.picRevolver1.Location = new System.Drawing.Point(19, 330);
            this.picRevolver1.Name = "picRevolver1";
            this.picRevolver1.Size = new System.Drawing.Size(370, 168);
            this.picRevolver1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRevolver1.TabIndex = 2;
            this.picRevolver1.TabStop = false;
            //
            // picRevolver2
            //
            this.picRevolver2.BackColor = System.Drawing.Color.Transparent;
            this.picRevolver2.Location = new System.Drawing.Point(875, 330);
            this.picRevolver2.Name = "picRevolver2";
            this.picRevolver2.Size = new System.Drawing.Size(370, 168);
            this.picRevolver2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRevolver2.TabIndex = 3;
            this.picRevolver2.TabStop = false;
            //
            // lblNom1
            //
            this.lblNom1.BackColor = System.Drawing.Color.Transparent;
            this.lblNom1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblNom1.ForeColor = System.Drawing.Color.White;
            this.lblNom1.Location = new System.Drawing.Point(17, 272);
            this.lblNom1.Name = "lblNom1";
            this.lblNom1.Size = new System.Drawing.Size(300, 30);
            this.lblNom1.TabIndex = 4;
            this.lblNom1.Text = "JOUEUR 1";
            this.lblNom1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblNom2
            //
            this.lblNom2.BackColor = System.Drawing.Color.Transparent;
            this.lblNom2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblNom2.ForeColor = System.Drawing.Color.White;
            this.lblNom2.Location = new System.Drawing.Point(947, 272);
            this.lblNom2.Name = "lblNom2";
            this.lblNom2.Size = new System.Drawing.Size(300, 30);
            this.lblNom2.TabIndex = 5;
            this.lblNom2.Text = "JOUEUR 2";
            this.lblNom2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnTirer1
            //
            this.btnTirer1.BackColor = System.Drawing.Color.DarkRed;
            this.btnTirer1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTirer1.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.btnTirer1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTirer1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnTirer1.ForeColor = System.Drawing.Color.White;
            this.btnTirer1.Location = new System.Drawing.Point(94, 515);
            this.btnTirer1.Name = "btnTirer1";
            this.btnTirer1.Size = new System.Drawing.Size(220, 60);
            this.btnTirer1.TabIndex = 6;
            this.btnTirer1.Text = "TIRER";
            this.btnTirer1.UseVisualStyleBackColor = false;
            this.btnTirer1.Click += new System.EventHandler(this.btnTirer1_Click);
            //
            // btnTirer2
            //
            this.btnTirer2.BackColor = System.Drawing.Color.DarkRed;
            this.btnTirer2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTirer2.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.btnTirer2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTirer2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnTirer2.ForeColor = System.Drawing.Color.White;
            this.btnTirer2.Location = new System.Drawing.Point(950, 515);
            this.btnTirer2.Name = "btnTirer2";
            this.btnTirer2.Size = new System.Drawing.Size(220, 60);
            this.btnTirer2.TabIndex = 7;
            this.btnTirer2.Text = "TIRER";
            this.btnTirer2.UseVisualStyleBackColor = false;
            this.btnTirer2.Click += new System.EventHandler(this.btnTirer2_Click);
            //
            // lblCoups1
            //
            this.lblCoups1.BackColor = System.Drawing.Color.Transparent;
            this.lblCoups1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCoups1.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblCoups1.Location = new System.Drawing.Point(19, 585);
            this.lblCoups1.Name = "lblCoups1";
            this.lblCoups1.Size = new System.Drawing.Size(370, 25);
            this.lblCoups1.TabIndex = 8;
            this.lblCoups1.Text = "Coups restants : 12";
            this.lblCoups1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblCoups2
            //
            this.lblCoups2.BackColor = System.Drawing.Color.Transparent;
            this.lblCoups2.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCoups2.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblCoups2.Location = new System.Drawing.Point(875, 585);
            this.lblCoups2.Name = "lblCoups2";
            this.lblCoups2.Size = new System.Drawing.Size(370, 25);
            this.lblCoups2.TabIndex = 9;
            this.lblCoups2.Text = "Coups restants : 12";
            this.lblCoups2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblTitre
            //
            this.lblTitre.BackColor = System.Drawing.Color.Transparent;
            this.lblTitre.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblTitre.ForeColor = System.Drawing.Color.Gold;
            this.lblTitre.Location = new System.Drawing.Point(340, 15);
            this.lblTitre.Name = "lblTitre";
            this.lblTitre.Size = new System.Drawing.Size(584, 50);
            this.lblTitre.TabIndex = 10;
            this.lblTitre.Text = "ROULETTE RUSSE";
            this.lblTitre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblTour
            //
            this.lblTour.BackColor = System.Drawing.Color.Transparent;
            this.lblTour.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblTour.ForeColor = System.Drawing.Color.White;
            this.lblTour.Location = new System.Drawing.Point(340, 70);
            this.lblTour.Name = "lblTour";
            this.lblTour.Size = new System.Drawing.Size(584, 30);
            this.lblTour.TabIndex = 11;
            this.lblTour.Text = "Tour de : JOUEUR 1";
            this.lblTour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblResultat
            //
            this.lblResultat.BackColor = System.Drawing.Color.Transparent;
            this.lblResultat.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Bold);
            this.lblResultat.ForeColor = System.Drawing.Color.White;
            this.lblResultat.Location = new System.Drawing.Point(340, 105);
            this.lblResultat.Name = "lblResultat";
            this.lblResultat.Size = new System.Drawing.Size(584, 90);
            this.lblResultat.TabIndex = 12;
            this.lblResultat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // barillet1
            //
            this.barillet1.Location = new System.Drawing.Point(430, 205);
            this.barillet1.Name = "barillet1";
            this.barillet1.Size = new System.Drawing.Size(190, 190);
            this.barillet1.TabIndex = 13;
            //
            // barillet2
            //
            this.barillet2.Location = new System.Drawing.Point(644, 205);
            this.barillet2.Name = "barillet2";
            this.barillet2.Size = new System.Drawing.Size(190, 190);
            this.barillet2.TabIndex = 14;
            //
            // lstHistorique
            //
            this.lstHistorique.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(40)))), ((int)(((byte)(26)))));
            this.lstHistorique.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstHistorique.Font = new System.Drawing.Font("Consolas", 10F);
            this.lstHistorique.ForeColor = System.Drawing.Color.Gainsboro;
            this.lstHistorique.FormattingEnabled = true;
            this.lstHistorique.ItemHeight = 15;
            this.lstHistorique.Location = new System.Drawing.Point(400, 410);
            this.lstHistorique.Name = "lstHistorique";
            this.lstHistorique.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstHistorique.Size = new System.Drawing.Size(464, 152);
            this.lstHistorique.TabIndex = 15;
            //
            // btnNouvellePartie
            //
            this.btnNouvellePartie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNouvellePartie.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNouvellePartie.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.btnNouvellePartie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNouvellePartie.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnNouvellePartie.ForeColor = System.Drawing.Color.Gold;
            this.btnNouvellePartie.Location = new System.Drawing.Point(532, 575);
            this.btnNouvellePartie.Name = "btnNouvellePartie";
            this.btnNouvellePartie.Size = new System.Drawing.Size(200, 45);
            this.btnNouvellePartie.TabIndex = 16;
            this.btnNouvellePartie.Text = "NOUVELLE PARTIE";
            this.btnNouvellePartie.UseVisualStyleBackColor = false;
            this.btnNouvellePartie.Click += new System.EventHandler(this.btnNouvellePartie_Click);
            //
            // lblLegende
            //
            this.lblLegende.BackColor = System.Drawing.Color.Transparent;
            this.lblLegende.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLegende.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblLegende.Location = new System.Drawing.Point(240, 635);
            this.lblLegende.Name = "lblLegende";
            this.lblLegende.Size = new System.Drawing.Size(784, 30);
            this.lblLegende.TabIndex = 17;
            this.lblLegende.Text = "Chaque revolver : 12 coups (10 à blanc, 1 noir, 1 rouge).   Blanc = clic   |   Rouge = adversaire touché, tu gagnes   |   Noir = ricochet, tu perds";
            this.lblLegende.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // timerAnimation
            //
            this.timerAnimation.Interval = 15;
            this.timerAnimation.Tick += new System.EventHandler(this.timerAnimation_Tick);
            //
            // FormRouletteRusse
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(60)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.lblLegende);
            this.Controls.Add(this.btnNouvellePartie);
            this.Controls.Add(this.lstHistorique);
            this.Controls.Add(this.barillet2);
            this.Controls.Add(this.barillet1);
            this.Controls.Add(this.lblResultat);
            this.Controls.Add(this.lblTour);
            this.Controls.Add(this.lblTitre);
            this.Controls.Add(this.lblCoups2);
            this.Controls.Add(this.lblCoups1);
            this.Controls.Add(this.btnTirer2);
            this.Controls.Add(this.btnTirer1);
            this.Controls.Add(this.lblNom2);
            this.Controls.Add(this.lblNom1);
            this.Controls.Add(this.picRevolver2);
            this.Controls.Add(this.picRevolver1);
            this.Controls.Add(this.picJoueur2);
            this.Controls.Add(this.picJoueur1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormRouletteRusse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roulette Russe";
            ((System.ComponentModel.ISupportInitialize)(this.picJoueur1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picJoueur2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRevolver1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRevolver2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picJoueur1;
        private System.Windows.Forms.PictureBox picJoueur2;
        private System.Windows.Forms.PictureBox picRevolver1;
        private System.Windows.Forms.PictureBox picRevolver2;
        private System.Windows.Forms.Label lblNom1;
        private System.Windows.Forms.Label lblNom2;
        private System.Windows.Forms.Button btnTirer1;
        private System.Windows.Forms.Button btnTirer2;
        private System.Windows.Forms.Label lblCoups1;
        private System.Windows.Forms.Label lblCoups2;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblTour;
        private System.Windows.Forms.Label lblResultat;
        private BarilletControl barillet1;
        private BarilletControl barillet2;
        private System.Windows.Forms.ListBox lstHistorique;
        private System.Windows.Forms.Button btnNouvellePartie;
        private System.Windows.Forms.Label lblLegende;
        private System.Windows.Forms.Timer timerAnimation;
    }
}
