using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                LabelCdComp1.BackgroundColor = Color.FromRgb(180,12,234);
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
                degatsRecusNum += PourcentageDegatsSubisSupplementaire * degatsRecusNum / 100;
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

        private void ButtonPourcentageDegatsSubisSupplementaire_Click(object sender, EventArgs e)
        {
            PourcentageDegatsSubisSupplementaire = modifStats();
            LabelInfo.Text = "Les dégâts que vous subissez sont augmentés de " + modifStats() + "%\n";
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

                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
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

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += 35 * degatsInfliges /100;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges += 35 * degatsInfliges / 100;
            }

            else
            {
                degatsInfliges += 25 * degatsInfliges / 100;
            }

            bool AutoAttack = await DisplayAlert("Auto-Attack", "Voulez-vous infliger jusqu'à " + degatsInfliges + " dégâts?", "Ok", "Annuler");

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
                LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts";

                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }

                affichageInitial();

                PopUpDegatsReelsAutoAttack.IsVisible = false;
            }
        }
        private void ButtonAnnulerDegatsAutoAttack_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsAutoAttack.IsVisible = false;
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsInfliges = 16 + 2 * Agilité + 15 * (Pm-3);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += 35 * degatsInfliges / 100;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges += 35 * degatsInfliges / 100;
            }

            else
            {
                degatsInfliges += 25 * degatsInfliges / 100;
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

                        affichageInitial();

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

                        PopUpDegatsReelsComp1.IsVisible = true;

                        break;

                    case false:

                        break;
                }
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
            degatsInfliges = defDegatsTotauxArmes();

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += 35 * degatsInfliges / 100;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges += 35 * degatsInfliges / 100;
            }

            else
            {
                degatsInfliges += 25 * degatsInfliges / 100;
            }

            bool Comp2 = await DisplayAlert("Fourberie", "- Echec critique : La compétence échoue\n\n- Coup Normal : Se téléporter dans le dos de la cible et infliger " +degatsInfliges + " dégâts\n\n- Coup critique : dégâts = " + (degatsInfliges + (degatsInfliges/2)), "Ok", "Annuler");

            switch (Comp2)
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
                if (RadioButtonComp2EchecCritique.IsChecked == true)
                {
                    LabelInfo.Text = "La compétence échoue";
                }

                else if (RadioButtonComp2CoupNormal.IsChecked == true)
                {
                    LabelInfo.Text = "Vous vous téléportez derrière la cible et lui inligez " + DegatsReelsComp2 + " dégâts";
                }

                else if (RadioButtonComp2CoupCritique.IsChecked == true)
                {

                    LabelInfo.Text = "Vous vous téléportez derrière la cible et lui inligez " + DegatsReelsComp2 + " dégâts";
                }

                CdComp2 = 3;

                affichageInitial();

                PopUpDegatsReelsComp2.IsVisible = false;
            }
        }
        private void ButtonAnnulerDegatsComp2_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp2.IsVisible = false;
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            int degatsInfliges1 = 75 * defDegatsTotauxArmes() /100;
            int degatsInfliges2 = defDegatsTotauxArmes();
            degatsInfliges2 += degatsInfliges2 / 2;
            int degatsInfliges3 = defDegatsTotauxArmes()* 2;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges1 += 35 * degatsInfliges1 / 100;
                degatsInfliges2 += 35 * degatsInfliges2 / 100;
                degatsInfliges3 += 35 * degatsInfliges3 / 100;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges1 += 35 * degatsInfliges1 / 100;
                degatsInfliges2 += 35 * degatsInfliges2 / 100;
                degatsInfliges3 += 35 * degatsInfliges3 / 100;
            }

            else
            {
                degatsInfliges1 += 25 * degatsInfliges1 / 100;
                degatsInfliges2 += 35 * degatsInfliges2 / 100;
                degatsInfliges3 += 35 * degatsInfliges3 / 100;
            }

            bool comp3 = await DisplayAlert("Une vie longue pour une mort douloureuse", "- Echec critique : La compétence échoue. Les charges ne sont pas consommés.\n\n- 1 charge :\nDégâts = " + degatsInfliges1 + "\n\n- 2 charges :\nArmure ignorée = 50%\nDégâts = " + degatsInfliges2 + "\n\n- 3 charges :\nArmure ignorée = 100%\nDégâts = " + degatsInfliges3, "Ok", "Annuler");

            switch (comp3)
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
                    LabelInfo.Text = "La compétence échoue mais les charges ne sont pas consommées";
                }

                else if (RadioButtonComp3CoupNormal.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp3Num + " dégâts";
                }


                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }

                affichageInitial();
                PopUpDegatsReelsComp3.IsVisible = false;
            }   
        }
        private void ButtonAnnulerDegatsComp3_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp3.IsVisible = false;
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
