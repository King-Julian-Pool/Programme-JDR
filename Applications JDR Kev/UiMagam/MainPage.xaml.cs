using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UiMagam
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ------------- Définition des variables ------------- 
        int PVmax = 120;
        int PVactuels = 120;
        int Bouclier = 0;
        int Armure = 4;
        int ResistMagique = 14;
        int Intelligence = 38;
        int Force = 10;
        int Agilité = 10;
        int Initiative = 1;
        int Pm = 4;
        int degatsInfliges;
        int degatsBaseArmes = 11;
        int degatsTotauxArmes;
        int CdComp1;
        int CdComp2;
        int CdComp3;
        int BouclierComp2;
        int CdBouleLumiere;
        int CdPassifComp3;
        int degatsPassifComp3;


        // ------------- Définition des fonctions ------------- 
        void couleurBarrePV()
        {
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

        private async void AnimProgressBarPv(ProgressBar ProgressBarPv)
        {
            await ProgressBarPv.ProgressTo(Math.Round((double)PVactuels / PVmax, 1), 1000, Easing.Linear);
        }

        void affichageInitial()
        {
            LabelPv.Text = "PV : " + PVactuels + "/" + PVmax;
            couleurBarrePV();
            AnimProgressBarPv(ProgressBarPv);
            LabelArmure.Text = "Armure : " + Armure;
            LabelResistMagique.Text = "Résit. Magique : " + ResistMagique;

            LabelIntelligence.Text = "Intelligence : " + Intelligence;
            LabelForce.Text = "Force : " + Force;
            LabelAgilite.Text = "Agilité : " + Agilité;
            LabelInitiative.Text = "Initiative : " + Initiative;
            LabelPm.Text = "PM : " + Pm;

            LabelInfo.FontSize = 16;

            LabelCdComp1.Text = "" + CdComp1;
            LabelCdComp2.Text = "" + CdComp2;
            LabelCdComp3.Text = "" + CdComp3;

            if (CdComp1 != 0)
            {
                ButtonComp1.IsEnabled = false;
                LabelCdComp1.BackgroundColor = Color.FromRgb(38, 170, 51);
            }
            else
            {
                ButtonComp1.IsEnabled = true;
                LabelCdComp1.BackgroundColor = Color.Default;
                LabelCdComp1.Text = "";
            }

            if (CdComp2 != 0 && CdComp2 != 3)
            {
                ButtonComp2.IsEnabled = false;
                LabelCdComp2.BackgroundColor = Color.FromRgb(38, 170, 51);
            }
            else if (CdComp2 == 3)
            {
                Bouclier -= BouclierComp2;
            }
            else
            {
                ButtonComp2.IsEnabled = true;
                LabelCdComp2.BackgroundColor = Color.Default;
                LabelCdComp2.Text = "";
            }

            if (CdBouleLumiere == 0)
            {
                LabelBouleLumière.Text = "";
            }

            LabelBouclier.Text = "Bouclier : " + Bouclier;

            if (CdComp3 != 0)
            {
                ButtonComp3.IsEnabled = false;
                LabelCdComp3.BackgroundColor = Color.FromRgb(38, 170, 51);
            }
            else
            {
                ButtonComp3.IsEnabled = true;
                LabelCdComp3.BackgroundColor = Color.Default;
                LabelCdComp3.Text = "";
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

            if (CdPassifComp3 == 1)
            {
                LabelPassifComp3.Text = "Passif comp 3 = " + degatsPassifComp3;
            }
            else
            {
                LabelPassifComp3.Text = "";
            }


        }

        void heal()
        {
            string PVsoignes = EntryDegatsRecus.Text;
            int PVsoignesNum;

            if (int.TryParse(PVsoignes, out PVsoignesNum) == false)
            {
                EntryDegatsRecus.Text = "E";
            }
            else
            {
                LabelInfo.Text = "Vous vous soignez de " + PVsoignesNum + " PV\n";
            }

            if ((PVactuels + PVsoignesNum) < PVmax)
            {
                PVactuels = PVactuels + PVsoignesNum;
            }
            else
            {
                PVactuels = PVmax;
            }
            affichageInitial();
        }

        void gainBouclier()
        {
            string GainBouclier = EntryDegatsRecus.Text;
            int GainBouclierNum;

            if (int.TryParse(GainBouclier, out GainBouclierNum) == false)
            {
                EntryDegatsRecus.Text = "E";
            }
            else
            {
                LabelInfo.Text = "Vous gagnez " + GainBouclierNum + " points de bouclier";
            }
            Bouclier = Bouclier + GainBouclierNum;
            affichageInitial();
        }

        void receptionDegats()
        {
            string degatsRecus = EntryDegatsRecus.Text;
            int degatsRecusNum;

            if (int.TryParse(degatsRecus, out degatsRecusNum) == false)
            {
                EntryDegatsRecus.Text = "E";
            }
            else
            {
                LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts";
            }

            if (Bouclier == 0)
            {
                PVactuels = PVactuels - degatsRecusNum;
            }
            else if (Bouclier >= degatsRecusNum)
            {
                Bouclier = Bouclier - degatsRecusNum;
            }
            else
            {
                degatsRecusNum = degatsRecusNum - Bouclier;
                Bouclier = Bouclier - Bouclier;
                PVactuels = PVactuels - degatsRecusNum;
            }

            if (PVactuels < 0)
            {
                PVactuels = 0;
            }

            affichageInitial();

            return;
        }


        int modifStats()
        {
            string ModifStats = EntryModifStats.Text;
            int ModifStatsNum;

            if (int.TryParse(ModifStats, out ModifStatsNum) == false)
            {
                EntryModifStats.Text = "E";
            }
            return ModifStatsNum;
        }
        int defDegatsBaseArmes()
        {
            string DegatsBaseArmes = EntryDegatsBaseArmes.Text;
            int DegatsBaseArmesNum;

            if (int.TryParse(DegatsBaseArmes, out DegatsBaseArmesNum) == false)
            {
                EntryDegatsBaseArmes.Text = "E";
            }
            return DegatsBaseArmesNum;
        }
        int defDegatsTotauxArmes()
        {
            degatsBaseArmes = defDegatsBaseArmes();
            int varForce;
            if (SwitchForce.IsToggled == true)
            {
                string ArmeForce = EntryArmeForce.Text;
                int ArmeForceNum;
                if (int.TryParse(ArmeForce, out ArmeForceNum) == false)
                {
                    EntryArmeForce.Text = "1";
                    varForce = Force;
                }

                if (ArmeForceNum == 0)
                {
                    EntryArmeForce.Text = "1";
                    varForce = Force;
                }
                else
                {
                    varForce = Force / ArmeForceNum;
                }
            }
            else
            {
                varForce = 0;
            }

            int varAgilite;
            if (SwitchAgilite.IsToggled == true)
            {
                string ArmeAgilite = EntryArmeAgilite.Text;
                int ArmeAgiliteNum;
                if (int.TryParse(ArmeAgilite, out ArmeAgiliteNum) == false)
                {
                    EntryArmeAgilite.Text = "1";
                    varAgilite = Agilité;
                }

                if (ArmeAgiliteNum == 0)
                {
                    EntryArmeAgilite.Text = "1";
                    varAgilite = Force;
                }
                else
                {
                    varAgilite = Agilité / ArmeAgiliteNum;
                }
            }
            else
            {
                varAgilite = 0;
            }

            int varIntelligence;
            if (SwitchIntelligence.IsToggled == true)
            {
                string ArmeIntelligence = EntryArmeIntelligence.Text;
                int ArmeIntelligenceNum;
                if (int.TryParse(ArmeIntelligence, out ArmeIntelligenceNum) == false)
                {
                    EntryArmeIntelligence.Text = "1";
                    varIntelligence = Intelligence;
                }

                if (ArmeIntelligenceNum == 0)
                {
                    EntryArmeIntelligence.Text = "1";
                    varIntelligence = Intelligence;
                }
                else
                {
                    varIntelligence = Intelligence / ArmeIntelligenceNum;
                }
            }
            else
            {
                varIntelligence = 0;
            }

            int varPVmax;
            if (SwitchPVmax.IsToggled == true)
            {
                string ArmePVmax = EntryArmePVmax.Text;
                int ArmePVmaxNum;
                if (int.TryParse(ArmePVmax, out ArmePVmaxNum) == false)
                {
                    EntryArmePVmax.Text = "1";
                    varPVmax = PVmax;
                }

                if (ArmePVmaxNum == 0)
                {
                    EntryArmePVmax.Text = "1";
                    varPVmax = PVmax;
                }
                else
                {
                    varPVmax = PVmax / ArmePVmaxNum;
                }
            }
            else
            {
                varPVmax = 0;
            }

            degatsTotauxArmes = degatsBaseArmes + varForce + varAgilite + varIntelligence + varPVmax;

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
            affichageInitial();
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
            Armure = modifStats();
            LabelInfo.Text = "Votre armure passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonResistMagique_Click(object sender, EventArgs e)
        {
            ResistMagique = modifStats();
            LabelInfo.Text = "Votre résistance magique passe à " + modifStats();
            affichageInitial();
        }

        private void ButtonForce_Click(object sender, EventArgs e)
        {
            Force = modifStats();
            LabelInfo.Text = "Votre force passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonAgilite_Click(object sender, EventArgs e)
        {
            Agilité = modifStats();
            LabelInfo.Text = "Votre agilité passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonInitiative_Click(object sender, EventArgs e)
        {
            Initiative = modifStats();
            LabelInfo.Text = "Votre initiative passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonPm_Click(object sender, EventArgs e)
        {
            Pm = modifStats();
            LabelInfo.Text = "Vos PM passent à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonPVactuels_Click(object sender, EventArgs e)
        {
            PVactuels = modifStats();
            LabelInfo.Text = "Vos PV actuels passent à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonPVmax_Click(object sender, EventArgs e)
        {
            PVmax = modifStats();
            LabelInfo.Text = "Vos PV maximum passent à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonBouclier_Click(object sender, EventArgs e)
        {
            Bouclier = modifStats();
            LabelInfo.Text = "Vos points de bouclier passent à " + modifStats();
            affichageInitial();
        }

        private void ButtonIntelligence_Click(object sender, EventArgs e)
        {
            Intelligence = modifStats();
            LabelInfo.Text = "Votre intelligence passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonCdComp1_Click(object sender, EventArgs e)
        {
            CdComp1 = modifStats();
            LabelInfo.Text = "Le Cd de votre compétence 1 passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonCdComp2_Click(object sender, EventArgs e)
        {
            CdComp2 = modifStats();
            LabelInfo.Text = "Le Cd de votre compétence 2 passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonCdComp3_Click(object sender, EventArgs e)
        {
            CdComp3 = modifStats();
            CdPassifComp3 = CdComp3 + 1;
            LabelInfo.Text = "Le Cd de votre compétence 3 passe à " + modifStats() + "\n";
            affichageInitial();
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
            affichageInitial();
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
            affichageInitial();
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
            receptionDegats();
        }

        private void ButtonHeal_Click(object sender, EventArgs e)
        {
            heal();
        }

        private void ButtonGainBouclier_Click(object sender, EventArgs e)
        {
            gainBouclier();
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
                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }
                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }
                    if (CdBouleLumiere > 0)
                    {
                        CdBouleLumiere -= 1;
                    }
                    if (CdComp3 > 0)
                    {
                        CdComp3 -= 1;
                    }
                    if (CdPassifComp3 > 0)
                    {
                        CdPassifComp3 -= 1;
                    }

                    affichageInitial();
                    LabelInfo.Text = "Fin de tour\n";

                    break;

                case false:

                    break;
            }
        }

        private async void ButtonAutoAttack_Click(object sender, EventArgs e)
        {
            degatsInfliges = defDegatsTotauxArmes();
            int quartDegatsInfliges = degatsInfliges / 4;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges += degatsInfliges / 10;
            }

            bool AutoAttack = await DisplayAlert("Auto-Attack", "- Infliger jusqu'à " + (degatsInfliges + quartDegatsInfliges) + " dégâts à une cible unique\n\nOu\n\n - Infliger jusqu'à " + degatsInfliges + " dégâts à une cible et " + quartDegatsInfliges + " dégâts à jusqu'à 3 autres cibles ?", "Oui", "Non");

            switch (AutoAttack)
            {
                case true:

                    PopUpDegatsReelsAutoAttack.IsVisible = true;

                    break;

                case false:

                    break;
            }
        }
        private void ButtonValiderDegatsAutoAttack_Click(object sender, EventArgs e)
        {

            string DegatsReelsAutoAttack = EntryDegatsReelsAutoAttack.Text;
            int DegatsReelsAutoAttackNum;

            if (int.TryParse(DegatsReelsAutoAttack, out DegatsReelsAutoAttackNum) == false)
            {
                EntryDegatsReelsAutoAttack.Text = "E";
            }
            else
            {
                if (RadioButtonAutoAttack1Cible.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts à une seule cible";
                }

                else if (RadioButtonAutoAttackPlusieursCibles.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts à une cible et " + (DegatsReelsAutoAttackNum / 4) + " aux autres cibles";
                }

                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }
                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }
                if (CdBouleLumiere > 0)
                {
                    CdBouleLumiere -= 1;
                }
                if (CdComp3 > 0)
                {
                    CdComp3 -= 1;
                }
                if (CdPassifComp3 > 0)
                {
                    CdPassifComp3 -= 1;
                }

                affichageInitial();

                PopUpDegatsReelsAutoAttack.IsVisible = false;
            }
            return;
        }
        private void ButtonAnnulerDegatsAutoAttack_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsAutoAttack.IsVisible = false;
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsInfliges = 2 + 2 * Intelligence / 3;
            int degatsInfliges1Cible = 2 + Intelligence / 2;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
                degatsInfliges1Cible += degatsInfliges1Cible / 10;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges += degatsInfliges / 10;
                degatsInfliges1Cible += degatsInfliges1Cible / 10;
            }

            bool comp1 = await DisplayAlert("Requiem", "- 1 cible :\nDégâts = " + degatsInfliges1Cible + "\n\n- Plusieurs cibles :\nDégâts = " + degatsInfliges + " + 1 par 2 ennemis touchés", "Ok", "Annuler");

            switch (comp1)
            {
                case true:

                    PopUpDegatsReelsComp1.IsVisible = true;

                    break;

                case false:

                    break;
            }
        }
        private void ButtonValiderDegatsComp1_Click(object sender, EventArgs e)
        {
            string DegatsReelsComp1 = EntryDegatsReelsComp1.Text;
            int DegatsReelsComp1Num;

            if (int.TryParse(DegatsReelsComp1, out DegatsReelsComp1Num) == false)
            {
                EntryDegatsReelsComp1.Text = "E";
            }

            else
            {
                if (RadioButtonComp11Cible.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts à une cible unique";
                }
                else if (RadioButtonComp1PlusieursCibles.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts à tous les ennemis";
                }

                CdComp1 = 2;

                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }
                if (CdBouleLumiere > 0)
                {
                    CdBouleLumiere -= 1;
                }
                if (CdComp3 > 0)
                {
                    CdComp3 -= 1;
                }
                if (CdPassifComp3 > 0)
                {
                    CdPassifComp3 -= 1;
                }


                affichageInitial();
                PopUpDegatsReelsComp1.IsVisible = false;
            }
        }
        private void ButtonAnnulerDegatsComp1_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp1.IsVisible = false;
        }

        private async void ButtonComp2_Click(object sender, EventArgs e)
        {
            degatsInfliges = 2 + ResistMagique / 2;
            int bouclierComp2 = degatsInfliges * 4;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges += degatsInfliges / 10;
            }

            bool comp2 = await DisplayAlert("Décharge lumineuse", "- Echec critique :\nDégâts = " + degatsInfliges + "\nBouclier = +" + bouclierComp2 + " pour vous et votre cible pendant 1 tour\nLa boule n'emmagasine aucun dégât\n\n- Coup normal :\nDégâts = " + degatsInfliges + "\nBouclier = + " + bouclierComp2 + " pendant 1 tour\nLa boule emmagasine 33% des dégâts subis pendant un tour\n\n- Coup critique :\nDégâts = " + degatsInfliges + " à deux cibles\nBouclier = + " + bouclierComp2 + " pendant 1 tour\nLes 2 boules emmagasinent 33% des dégâts subis pendant un tour","Ok","Annuler");

            switch (comp2)
            {
                case true:

                    PopUpDegatsReelsComp2.IsVisible = true;

                    break;

                case false:

                    break;
            }
        }
        private void ButtonValiderDegatsComp2_Click(object sender, EventArgs e)
        {
            string DegatsReelsComp2 = EntryDegatsReelsComp2.Text;
            int DegatsReelsComp2Num;

            if (int.TryParse(DegatsReelsComp2, out DegatsReelsComp2Num) == false)
            {
                EntryDegatsReelsComp2.Text = "E";
            }
            else
            {
                BouclierComp2 = DegatsReelsComp2Num * 4;

                if (RadioButtonComp2EchecCritique.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp2Num + " dégâts.\nVotre cible et vous recevez " + (DegatsReelsComp2Num * 4) + " points de bouclier pour 1 tour.\nLa boule n'emmagasine aucun dégât.";
                }
                else if (RadioButtonComp2CoupNormal.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp2Num + " dégâts.\nVous recevez " + (DegatsReelsComp2Num * 4) + " points de bouclier pour 1 tour.\nLa boule emmagasine 33% des dégâts subis pendant un tour.";
                }
                else if (RadioButtonComp2CoupCritique.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp2Num + " dégâts à deux ennemis.\nVous recevez " + (DegatsReelsComp2Num * 4) + " points de bouclier pour 1 tour.\nLes 2 boules emmagasinent 33% des dégâts subis pendant un tour.";
                }

                Bouclier += DegatsReelsComp2Num * 4;
                LabelBouleLumière.Text = "Boule(s) de lumière activée(s)";
                CdComp2 = 4;
                CdBouleLumiere = 1;
                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }
                if (CdComp3 > 0)
                {
                    CdComp3 -= 1;
                }
                if (CdPassifComp3 > 0)
                {
                    CdPassifComp3 -= 1;
                }


                affichageInitial();
                LabelInfo.FontSize = 12;
                PopUpDegatsReelsComp2.IsVisible = false;
            }
            
        }
        private void ButtonAnnulerDegatsComp2_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp2.IsVisible = false;
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            degatsInfliges = 3 + Intelligence / 2;
            int degatsEnnemiA = degatsInfliges;
            int degatsEnnemiB = degatsEnnemiA + degatsEnnemiA / 4;
            int degatsEnnemiC = degatsEnnemiA + degatsEnnemiA / 2;
            int degatsEnnemiD = 6 + Intelligence;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
                degatsEnnemiA += degatsEnnemiA / 10;
                degatsEnnemiB += degatsEnnemiB / 10;
                degatsEnnemiC += degatsEnnemiC / 10;
                degatsEnnemiD += degatsEnnemiD / 10;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges += degatsInfliges / 10;
                degatsEnnemiA += degatsEnnemiA / 10;
                degatsEnnemiB += degatsEnnemiB / 10;
                degatsEnnemiC += degatsEnnemiC / 10;
                degatsEnnemiD += degatsEnnemiD / 10;
            }

            bool comp2 = await DisplayAlert("Vous ne devriez pas exister", "- Echec critique : La compétence échoue\n\n- Coup normal :\nDégats ennemi A = " + degatsEnnemiA + "\nDégats ennemi B = " + degatsEnnemiB + "\nDégats ennemi C = " + degatsEnnemiC + "\nDégats ennemi D = " + degatsEnnemiD, "Ok", "Annuler");

            switch (comp2)
            {
                case true:

                    PopUpDegatsReelsComp3.IsVisible = true;

                    break;

                case false:

                    break;
            }


        }
        private void ButtonValiderDegatsComp3_Click(object sender, EventArgs e)
        {
            string DegatsReelsComp3 = EntryDegatsReelsComp3.Text;
            int DegatsReelsComp3Num;

            if (int.TryParse(DegatsReelsComp3, out DegatsReelsComp3Num) == false)
            {
                EntryDegatsReelsComp3.Text = "E";
            }
            else
            {
                if (RadioButtonComp3EchecCritique.IsChecked == true)
                {
                    LabelInfo.Text = "La compétence échoue\n";
                }

                if (RadioButtonComp3CoupNormal.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp3Num + " dégâts à la cible A, " + (125 * DegatsReelsComp3Num / 100) + " dégâts à la cible B, " + (150 * DegatsReelsComp3Num / 100) + " dégâts à la cible C, " + (DegatsReelsComp3Num * 2) + " dégâts à la cible D";
                }

                degatsPassifComp3 = DegatsReelsComp3Num + (125 * DegatsReelsComp3Num / 100) + (150 * DegatsReelsComp3Num / 100) + (DegatsReelsComp3Num * 2);

                CdComp3 = 4;
                CdPassifComp3 = 5;
                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }
                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }
                if (CdBouleLumiere > 0)
                {
                    CdBouleLumiere -= 1;
                }

                affichageInitial();
                LabelInfo.FontSize = 14;
                PopUpDegatsReelsComp3.IsVisible = false;
            }
            
        }
        private void ButtonAnnulerDegatsComp3_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp3.IsVisible = false;
        }
    }
}
