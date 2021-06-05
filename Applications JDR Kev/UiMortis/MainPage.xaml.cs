using System;
using Xamarin.Forms;

namespace UiMortis
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ------------- Définition des variables ------------- 
        int PVmax = 101;
        int PVactuels = 101;
        int Bouclier = 0;
        int Armure = 11;
        int ResistMagique = 12;
        int Intelligence = 2;
        int Force = 2;
        int Agilité = 37;
        int Initiative = 51;
        int Pm = 5;
        int degatsInfliges;
        int degatsBaseArmes = 12;
        int degatsTotauxArmes;
        int CdComp1;
        int Comp1Tour = 0;
        int CdComp2;
        bool concentre;
        int ReussiteConcentre = 17;
        int PourcentageDegatsSubisSupplementaire = 15;

        // ------------- Définition des fonctions ------------- 
        private async void BarPv(ProgressBar ProgressBarPv)
        {
            await ProgressBarPv.ProgressTo(Math.Round((double)PVactuels / PVmax, 1), 1000, Easing.CubicInOut);

            if (PVactuels <= PVmax / 4)
            {
                ProgressBarPv.ProgressColor = Color.Red;
            }
            else if (PVactuels <= PVmax / 2)
            {
                ProgressBarPv.ProgressColor = Color.Orange;
            }
            else
            {
                ProgressBarPv.ProgressColor = Color.Green;
            }
        }

        void AffichageInitial()
        {
            LabelPv.Text = "PV : " + PVactuels + "/" + PVmax;
            BarPv(ProgressBarPv);
            LabelBouclier.Text = "Bouclier : " + Bouclier;
            LabelArmure.Text = "Armure : " + Armure;
            LabelResistMagique.Text = "Résit. Magique : " + ResistMagique;

            LabelIntelligence.Text = "Intelligence : " + Intelligence;
            LabelForce.Text = "Force : " + Force;
            LabelAgilite.Text = "Agilité : " + Agilité;
            LabelInitiative.Text = "Initiative : " + Initiative;
            LabelPm.Text = "PM : " + Pm;

            LabelInfo.FontSize = 24;

            LabelCdComp1.Text = "" + CdComp1;
            LabelCdComp2.Text = "" + CdComp2;

            if (CdComp1 != 0)
            {
                ButtonComp1.IsEnabled = false;
                LabelCdComp1.BackgroundColor = Color.FromRgb(180, 12, 234);
            }
            else
            {
                ButtonComp1.IsEnabled = true;
                LabelCdComp1.BackgroundColor = Color.Default;
                LabelCdComp1.Text = "";
            }

            if (CdComp2 != 0)
            {
                ButtonComp2.IsEnabled = false;
                LabelCdComp2.BackgroundColor = Color.FromRgb(180, 12, 234);
            }
            else
            {
                ButtonComp2.IsEnabled = true;
                LabelCdComp2.BackgroundColor = Color.Default;
                LabelCdComp2.Text = "";
            }


            if (Beni() == true && Maudit() == true)
            {
                LabelBeniMaudit.Text = "Béni et maudit";
            }
            else if (Beni() == true)
            {
                LabelBeniMaudit.Text = "Béni";
            }
            else if (Maudit() == true)
            {
                LabelBeniMaudit.Text = "Maudit";
            }
            else
            {
                LabelBeniMaudit.Text = "";
            }

            LabelPourcentageDegatsSubisSupplementaire.Text = "Dgts subis : + " + PourcentageDegatsSubisSupplementaire + "%";

        }

        void Heal()
        {
            string PVsoignes = EntryDegatsRecus.Text;

            if (int.TryParse(PVsoignes, out int PVsoignesNum) == false)
            {
                EntryDegatsRecus.Text = "E";
            }
            else
            {
                LabelInfo.Text = "Vous vous soignez de " + PVsoignesNum + " PV\n";
            }

            if ((PVactuels + PVsoignesNum) < PVmax)
            {
                PVactuels += PVsoignesNum;
            }
            else
            {
                PVactuels = PVmax;
            }
            AffichageInitial();
        }

        void GainBouclier()
        {
            string GainBouclier = EntryDegatsRecus.Text;

            if (int.TryParse(GainBouclier, out int GainBouclierNum) == false)
            {
                EntryDegatsRecus.Text = "E";
            }
            else
            {
                LabelInfo.Text = "Vous gagnez " + GainBouclierNum + " points de bouclier";
            }
            Bouclier += GainBouclierNum;
            AffichageInitial();
        }

        void ReceptionDegats()
        {
            string degatsRecus = EntryDegatsRecus.Text;

            if (int.TryParse(degatsRecus, out int degatsRecusNum) == false)
            {
                EntryDegatsRecus.Text = "E";
            }
            else
            {
                degatsRecusNum += (int)Math.Round((double)PourcentageDegatsSubisSupplementaire * degatsRecusNum / 100.0, MidpointRounding.AwayFromZero);
                LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts";
            }

            if (Bouclier == 0)
            {
                PVactuels -= degatsRecusNum;
            }
            else if (Bouclier >= degatsRecusNum)
            {
                Bouclier -= degatsRecusNum;
            }
            else
            {
                degatsRecusNum -= Bouclier;
                Bouclier -= Bouclier;
                PVactuels -= degatsRecusNum;
            }

            if (PVactuels < 0)
            {
                PVactuels = 0;
            }

            AffichageInitial();

            return;
        }


        int ModifStats()
        {
            string ModifStats = EntryModifStats.Text;

            if (int.TryParse(ModifStats, out int ModifStatsNum) == false)
            {
                EntryModifStats.Text = "E";
            }
            return ModifStatsNum;
        }
        int DefDegatsBaseArmes()
        {
            string DegatsBaseArmes = EntryDegatsBaseArmes.Text;

            if (int.TryParse(DegatsBaseArmes, out int DegatsBaseArmesNum) == false)
            {
                EntryDegatsBaseArmes.Text = "E";
            }
            return DegatsBaseArmesNum;
        }
        int DefDegatsTotauxArmes()
        {
            degatsBaseArmes = DefDegatsBaseArmes();
            double varForce;
            if (SwitchForce.IsToggled == true)
            {
                string ArmeForce = EntryArmeForce.Text;
                if (int.TryParse(ArmeForce, out int ArmeForceNum) == false)
                {
                    EntryArmeForce.Text = "1";
                }

                if (ArmeForceNum == 0)
                {
                    EntryArmeForce.Text = "1";
                    varForce = Force;
                }
                else
                {
                    varForce = (int)Math.Round((double)Force / ArmeForceNum, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                varForce = 0;
            }

            double varAgilite;
            if (SwitchAgilite.IsToggled == true)
            {
                string ArmeAgilite = EntryArmeAgilite.Text;
                if (int.TryParse(ArmeAgilite, out int ArmeAgiliteNum) == false)
                {
                    EntryArmeAgilite.Text = "1";
                }

                if (ArmeAgiliteNum == 0)
                {
                    EntryArmeAgilite.Text = "1";
                    varAgilite = Force;
                }
                else
                {
                    varAgilite = (int)Math.Round((double)Agilité / ArmeAgiliteNum, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                varAgilite = 0;
            }

            double varIntelligence;
            if (SwitchIntelligence.IsToggled == true)
            {
                string ArmeIntelligence = EntryArmeIntelligence.Text;
                if (int.TryParse(ArmeIntelligence, out int ArmeIntelligenceNum) == false)
                {
                    EntryArmeIntelligence.Text = "1";
                }

                if (ArmeIntelligenceNum == 0)
                {
                    EntryArmeIntelligence.Text = "1";
                    varIntelligence = Intelligence;
                }
                else
                {
                    varIntelligence = (int)Math.Round((double)Intelligence / ArmeIntelligenceNum, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                varIntelligence = 0;
            }

            double varPVmax;
            if (SwitchPVmax.IsToggled == true)
            {
                string ArmePVmax = EntryArmePVmax.Text;
                if (int.TryParse(ArmePVmax, out int ArmePVmaxNum) == false)
                {
                    EntryArmePVmax.Text = "1";
                }

                if (ArmePVmaxNum == 0)
                {
                    EntryArmePVmax.Text = "1";
                    varPVmax = PVmax;
                }
                else
                {
                    varPVmax = (int)Math.Round((double)PVmax / ArmePVmaxNum, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                varPVmax = 0;
            }

            degatsTotauxArmes = (int)Math.Round(degatsBaseArmes + varForce + varAgilite + varIntelligence + varPVmax, MidpointRounding.AwayFromZero);

            return degatsTotauxArmes;
        }

        bool Beni()
        {
            if (CheckBoxBeni.IsChecked == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool Maudit()
        {
            if (CheckBoxMaudit.IsChecked == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // ------------- Assignation des contrôles ------------- 
        private void Page_Appearing(object sender, EventArgs e)
        {
            AffichageInitial();
            LabelInfo.Text = " \n ";
        }



        private void ButtonModifStats_Click(object sender, EventArgs e)
        {
            if (PopUpModifStats.IsVisible == false)
            {
                PopUpModifStats.IsVisible = true;
                PopUpDegatsSoins.IsVisible = false;
                PopUpLiens.IsVisible = false;
            }
            else
            {
                PopUpModifStats.IsVisible = false;
                PopUpModifArme.IsVisible = false;
            }
        }


        private void ButtonArmure_Click(object sender, EventArgs e)
        {
            Armure = ModifStats();
            LabelInfo.Text = "Votre armure passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonResistMagique_Click(object sender, EventArgs e)
        {
            ResistMagique = ModifStats();
            LabelInfo.Text = "Votre résistance magique passe à " + ModifStats();
            AffichageInitial();
        }

        private void ButtonForce_Click(object sender, EventArgs e)
        {
            Force = ModifStats();
            LabelInfo.Text = "Votre force passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonAgilite_Click(object sender, EventArgs e)
        {
            Agilité = ModifStats();
            LabelInfo.Text = "Votre agilité passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonInitiative_Click(object sender, EventArgs e)
        {
            Initiative = ModifStats();
            LabelInfo.Text = "Votre initiative passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonPm_Click(object sender, EventArgs e)
        {
            Pm = ModifStats();
            LabelInfo.Text = "Vos PM passent à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonPVactuels_Click(object sender, EventArgs e)
        {
            PVactuels = ModifStats();
            LabelInfo.Text = "Vos PV actuels passent à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonPVmax_Click(object sender, EventArgs e)
        {
            PVmax = ModifStats();
            LabelInfo.Text = "Vos PV maximum passent à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonPourcentageDegatsSubisSupplementaire_Click(object sender, EventArgs e)
        {
            PourcentageDegatsSubisSupplementaire = ModifStats();
            LabelInfo.Text = "Les dégâts que vous subissez sont augmentés de " + ModifStats() + "%\n";
            AffichageInitial();
        }

        private void ButtonBouclier_Click(object sender, EventArgs e)
        {
            Bouclier = ModifStats();
            LabelInfo.Text = "Vos points de bouclier passent à " + ModifStats();
            AffichageInitial();
        }

        private void ButtonIntelligence_Click(object sender, EventArgs e)
        {
            Intelligence = ModifStats();
            LabelInfo.Text = "Votre intelligence passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonCdComp1_Click(object sender, EventArgs e)
        {
            CdComp1 = ModifStats();
            LabelInfo.Text = "Le Cd de votre compétence 1 passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonCdComp2_Click(object sender, EventArgs e)
        {
            CdComp2 = ModifStats();
            LabelInfo.Text = "Le Cd de votre compétence 2 passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonArme_Click(object sender, EventArgs e)
        {
            if (PopUpModifArme.IsVisible == false)
            {
                PopUpModifArme.IsVisible = true;
            }
            else
            {
                PopUpModifArme.IsVisible = false;
            }
        }

        private void CheckBoxBeni_CheckedChanged(object sender, EventArgs e)
        {
            AffichageInitial();
            if (CheckBoxBeni.IsChecked == true)
            {
                LabelInfo.Text = "Vous êtes désormais béni\n";
            }
            else
            {
                LabelInfo.Text = "Vous n'êtes plus béni\n";
            }
        }

        private void CheckBoxMaudit_CheckedChanged(object sender, EventArgs e)
        {
            AffichageInitial();
            if (CheckBoxMaudit.IsChecked == true)
            {
                LabelInfo.Text = "Vous êtes désormais maudit\n";
            }
            else
            {
                LabelInfo.Text = "Vous n'êtes plus maudit\n";
            }
        }



        private void ButtonDegatsSoins_Click(object sender, EventArgs e)
        {
            if (PopUpDegatsSoins.IsVisible == false)
            {
                PopUpDegatsSoins.IsVisible = true;
                PopUpModifStats.IsVisible = false;
                PopUpModifArme.IsVisible = false;
            }
            else
            {
                PopUpDegatsSoins.IsVisible = false;
                PopUpLiens.IsVisible = false;
            }
        }


        private void ButtonSubirDegats_Click(object sender, EventArgs e)
        {
            ReceptionDegats();
        }

        private void ButtonHeal_Click(object sender, EventArgs e)
        {
            Heal();
        }

        private void ButtonGainBouclier_Click(object sender, EventArgs e)
        {
            GainBouclier();
        }

        private void ButtonLiens_Click(object sender, EventArgs e)
        {
            if (PopUpLiens.IsVisible == true)
            {
                PopUpLiens.IsVisible = false;
            }
            else
            {
                PopUpLiens.IsVisible = true;
            }
        }


        private async void ButtonFinTour_Click(object sender, EventArgs e)
        {
            bool FinTour = await DisplayAlert("Fin de tour", "Voulez vous vraiment passer votre tour ?", "Oui", "Non");

            switch (FinTour)
            {
                case true:

                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }

                    AffichageInitial();
                    LabelInfo.Text = "Fin de tour\n";

                    break;

                case false:

                    break;
            }
        }

        private async void ButtonAutoAttack_Click(object sender, EventArgs e)
        {
            degatsInfliges = DefDegatsTotauxArmes();

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += (int)Math.Round((double)degatsInfliges * 0.35, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= (int)Math.Round((double)degatsInfliges * 0.35, MidpointRounding.AwayFromZero);
            }

            else
            {
                degatsInfliges += (int)Math.Round((double)degatsInfliges * 0.25, MidpointRounding.AwayFromZero);
            }

            bool AutoAttack = await DisplayAlert("Auto-Attack", "Voulez-vous infliger jusqu'à " + degatsInfliges + " dégâts?", "Ok", "Annuler");

            switch (AutoAttack)
            {
                case true:

                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts";

                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }

                    AffichageInitial();

                    break;

                case false:

                    break;
            }
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)16 + 2 * Agilité + 15 * (Pm - 3), MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += (int)Math.Round((double)degatsInfliges * 0.35, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= (int)Math.Round((double)degatsInfliges * 0.35, MidpointRounding.AwayFromZero);
            }

            else
            {
                degatsInfliges += (int)Math.Round((double)degatsInfliges * 0.25, MidpointRounding.AwayFromZero);
            }


            if (Comp1Tour == 0)
            {
                bool comp1 = await DisplayAlert("Envolée mortelle", "Voulez-vous devenir inciblable pendant un tour puis infliger jusqu'à " + degatsInfliges + " dégâts au tour suivant ?", "Ok", "Annuler");

                switch (comp1)
                {
                    case true:

                        LabelInfo.Text = "Vous vous envolez et devenez inciblable pendant 1 tour";
                        Comp1Tour = 1;
                        if (CdComp2 > 0)
                        {
                            CdComp2 -= 1;
                        }

                        AffichageInitial();

                        ButtonFinTour.IsEnabled = false;
                        ButtonAutoAttack.IsEnabled = false;
                        ButtonComp2.IsEnabled = false;
                        ButtonComp3.IsEnabled = false;

                        break;

                    case false:

                        break;
                }
            }

            else if (Comp1Tour == 1)
            {
                bool comp1 = await DisplayAlert("Envolée mortelle", "Voulez vous retomber et infliger jusqu'à " + degatsInfliges + " dégâts ?", "Ok", "Annuler");

                switch (comp1)
                {
                    case true:

                        CdComp1 = 99;

                        if (CdComp2 > 0)
                        {
                            CdComp2 -= 1;
                        }

                        Comp1Tour = 0;
                        ButtonFinTour.IsEnabled = true;
                        ButtonAutoAttack.IsEnabled = true;
                        ButtonComp2.IsEnabled = true;
                        ButtonComp3.IsEnabled = true;

                        LabelInfo.Text = "Vous retombez en infligeant " + degatsInfliges + " dégâts";

                        AffichageInitial();

                        break;

                    case false:

                        break;
                }
            }
        }

        private async void ButtonComp2_Click(object sender, EventArgs e)
        {
            degatsInfliges = DefDegatsTotauxArmes();

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += (int)Math.Round((double)degatsInfliges * 0.35, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= (int)Math.Round((double)degatsInfliges * 0.35, MidpointRounding.AwayFromZero);
            }

            else
            {
                degatsInfliges += (int)Math.Round((double)degatsInfliges * 0.25, MidpointRounding.AwayFromZero);
            }
            int degatsInfligesCritique = (int)Math.Round((double)degatsInfliges * 1.5, MidpointRounding.AwayFromZero);

            string Comp2 = await DisplayActionSheet("Fourberie", "Annuler", null, "- Echec critique : La compétence échoue", "- Coup Normal : Se téléporter dans le dos de la cible et infliger " + degatsInfliges + " dégâts", "- Coup critique : dégâts = " + degatsInfligesCritique);

            if (Comp2 == "Annuler" || Comp2 == null)
            {
                return;
            }
            else
            {
                if (Comp2 == "- Echec critique : La compétence échoue")
                {
                    LabelInfo.Text = "La compétence échoue";
                }

                if (Comp2 == "- Coup Normal : Se téléporter dans le dos de la cible et infliger " + degatsInfliges + " dégâts")
                {
                    LabelInfo.Text = "Vous vous téléportez derrière la cible et lui inligez " + degatsInfliges + " dégâts";
                }

                if (Comp2 == "- Coup critique : dégâts = " + degatsInfligesCritique)
                {
                    LabelInfo.Text = "Vous vous téléportez derrière la cible et lui inligez " + degatsInfligesCritique + " dégâts";
                }

                CdComp2 = 3;

                AffichageInitial();
            }
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            int degatsInfliges1 = (int)Math.Round((double)DefDegatsTotauxArmes() * 0.75, MidpointRounding.AwayFromZero);
            int degatsInfliges2 = (int)Math.Round((double)DefDegatsTotauxArmes() * 1.5, MidpointRounding.AwayFromZero);
            int degatsInfliges3 = (int)Math.Round((double)DefDegatsTotauxArmes() * 2, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges1 += (int)Math.Round((double)degatsInfliges1 * 0.35, MidpointRounding.AwayFromZero);
                degatsInfliges2 += (int)Math.Round((double)degatsInfliges2 * 0.35, MidpointRounding.AwayFromZero);
                degatsInfliges3 += (int)Math.Round((double)degatsInfliges3 * 0.35, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges1 -= (int)Math.Round((double)degatsInfliges1 * 0.35, MidpointRounding.AwayFromZero);
                degatsInfliges2 -= (int)Math.Round((double)degatsInfliges2 * 0.35, MidpointRounding.AwayFromZero);
                degatsInfliges3 -= (int)Math.Round((double)degatsInfliges3 * 0.35, MidpointRounding.AwayFromZero);
            }

            else
            {
                degatsInfliges1 += (int)Math.Round((double)degatsInfliges1 * 0.25, MidpointRounding.AwayFromZero);
                degatsInfliges2 += (int)Math.Round((double)degatsInfliges2 * 0.25, MidpointRounding.AwayFromZero);
                degatsInfliges3 += (int)Math.Round((double)degatsInfliges3 * 0.25, MidpointRounding.AwayFromZero);
            }

            string comp3 = await DisplayActionSheet("Une vie longue pour une mort douloureuse", "Annuler", null, "- Echec critique : La compétence échoue. Les charges ne sont pas consommés", "- 1 charge :\nDégâts = " + degatsInfliges1, "- 2 charges :\nArmure ignorée = 50%\nDégâts = " + degatsInfliges2, "- 3 charges :\nArmure ignorée = 100%\nDégâts = " + degatsInfliges3);
            if (comp3 == "Annuler" || comp3 == null)
            {
                return;
            }
            else
            {
                if (comp3 == "- Echec critique : La compétence échoue. Les charges ne sont pas consommés")
                {
                    LabelInfo.Text = "La compétence échoue mais les charges ne sont pas consommées";
                }

                if (comp3 == "- 1 charge :\nDégâts = " + degatsInfliges1)
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges1 + " dégâts";
                }

                if (comp3 == "- 2 charges :\nArmure ignorée = 50%\nDégâts = " + degatsInfliges2)
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges2 + " dégâts";
                }

                if (comp3 == "- 3 charges :\nArmure ignorée = 100%\nDégâts = " + degatsInfliges3)
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges3 + " dégâts";
                }

                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }

                AffichageInitial();
            }
        }

        private async void ChecBoxConcentre_CheckChanged(object sender, EventArgs e)
        {
            if (ChecBoxConcentre.IsChecked == true)
            {
                concentre = await DisplayAlert("Passage à l'état concentré", null, "Oui", "Non");

                switch (concentre)
                {
                    case true:
                        {
                            LabelConcentre.Text = "Vous êtes concentré";
                            break;
                        }
                    case false:
                        {
                            ReussiteConcentre -= 1;
                            LabelConcentre.Text = ReussiteConcentre + " à 20 pour être concentré";
                            ChecBoxConcentre.IsChecked = false;
                            break;
                        }
                }
            }
            else
            {
                if (concentre == true)
                {
                    concentre = false;
                    ReussiteConcentre = 17;
                }
                LabelConcentre.Text = ReussiteConcentre + " à 20 pour être concentré";
            }
        }
    }
}
