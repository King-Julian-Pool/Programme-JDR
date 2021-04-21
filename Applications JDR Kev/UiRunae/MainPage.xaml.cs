using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UiRunae
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ------------- Définition des variables ------------- 
        int PVmax = 129;
        int PVactuels = 129;
        int Bouclier = 0;
        int Armure = 8;
        int ResistMagique = 4;
        int Intelligence = 21;
        int Force = 10;
        int Agilité = 15;
        int Initiative = 29;
        int Pm = 3;
        int degatsInfliges;
        int degatsBaseArmes = 7;
        int degatsTotauxArmes;
        int degatsRuneA ;
        int degatsRuneB ;
        int CdComp1;
        int CdComp2;
        int CdComp3;
        int multDegats;
        int degatsStockes;

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

            LabelInfo.FontSize = 16;

            LabelCdComp1.Text = "" + CdComp1;
            LabelCdComp2.Text = "" + CdComp2;
            LabelCdComp3.Text = "" + CdComp3;

            if (CdComp1 != 0)
            {
                ButtonComp1.IsEnabled = false;
                LabelCdComp1.BackgroundColor = Color.Blue;
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
                LabelCdComp2.BackgroundColor = Color.Blue;
            }
            else
            {
                ButtonComp2.IsEnabled = true;
                LabelCdComp2.BackgroundColor = Color.Default;
                LabelCdComp2.Text = "";
            }

            if (CdComp3 != 0)
            {
                ButtonComp3.IsEnabled = false;
                LabelCdComp3.BackgroundColor = Color.Blue;
            }
            else
            {
                ButtonComp3.IsEnabled = true;
                LabelCdComp3.BackgroundColor = Color.Default;
                LabelCdComp3.Text = "";
            }

            if (CdComp2 < 4)
            {
                multDegats = 0;
            }

            if (multDegats != 0)
            {
                LabelDegatsAugmentes.Text = "Dégâts augmentés de " + multDegats + "%";
            }
            else
            {
                LabelDegatsAugmentes.Text = "";
            }

            if (degatsStockes != 0)
            {
                LabelDegatsStockes.Text = "Dégâts stockés : " + degatsStockes;
            }
            else
            {
                LabelDegatsStockes.Text = "";
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
                degatsStockes = 10 * degatsRecusNum / 100;
                degatsRecusNum -= degatsStockes;
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

                    if (CdComp3 > 0)
                    {
                        CdComp3 -= 1;
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
                degatsInfliges += degatsInfliges / 10;
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= degatsInfliges / 10;
            }

            degatsInfliges += multDegats * degatsInfliges / 100;
            degatsInfliges += degatsStockes;

            bool AutoAttack = await DisplayAlert("Auto-Attack",  "Echec critique :\n- Dégâts = 0\n\nCoup normal :\n- Dégâts = " + degatsInfliges + "\n\nCoup critique :\n- Dégâts = " + degatsInfliges + "\n- Vous pouvez rejouer un tour", "Ok", "Annuler");

            switch (AutoAttack)
            {
                case true:

                    degatsStockes = 0;
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
                DegatsReelsAutoAttackNum += multDegats * DegatsReelsAutoAttackNum / 100;

                if (RadioButtonAutoAttackEchecCritique.IsChecked == true)
                {
                    LabelInfo.Text = "L'attaque échoue, vous n'infligez pas de dégâts";
                }

                else if (RadioButtonAutoAttackCoupNormal.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts";
                    multDegats = 0;
                }

                else if (RadioButtonAutoAttackCoupCritique.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts et pouver jouer immédiatement un second tour";
                    multDegats = 0;
                }

                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }

                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }

                if (CdComp3 > 0)
                {
                    CdComp3 -= 1;
                }

                affichageInitial();

                PopUpDegatsReelsAutoAttack.IsVisible = false;

                return;
            }
           
        }
        private void ButtonAnnulerDegatsAutoAttack_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsAutoAttack.IsVisible = false;
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsRuneA = 4 + 1 * (Intelligence / 2) + 1 * (Agilité / 2);

            if (Intelligence % 2 != 0 && Agilité % 2 != 0)
            {
                degatsRuneA += Intelligence % 2;
            }

            degatsRuneB = 8 + 2 * Intelligence + 2 * Agilité;

            if (Beni() == true && Maudit() == false)
            {
                degatsRuneA += degatsRuneA / 10;
                degatsRuneB += degatsRuneB / 10;
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsRuneA -= degatsRuneA / 10;
                degatsRuneB -= degatsRuneB / 10;
            }

            degatsRuneA += multDegats * degatsRuneA / 100;
            degatsRuneB += multDegats * degatsRuneB / 100;
            degatsRuneA += degatsStockes;
            degatsRuneB += degatsStockes;

            bool comp1 = await DisplayAlert("Annihilation runique", "Echec critique :\n- Dégâts = " + degatsRuneA + " à votre cible et à vous-même\n\nRuneA :\n- Dégâts = " + degatsRuneA + "\n\nRune B :\n- Dégâts = " + degatsRuneB+ "\n\nCoup critique :\n- Dégâts = " + (degatsRuneA + degatsRuneB - degatsStockes), "Ok", "Annuler");

            switch (comp1)
            {
                case true:

                    degatsStockes = 0;
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
                DegatsReelsComp1Num += multDegats * DegatsReelsComp1Num / 100;

                if (RadioButtonComp1EchecCritique.IsChecked == true)
                {
                    LabelInfo.Text = "Votre rune A inflige " + DegatsReelsComp1Num + " dégâts à votre cible et à vous même";
                    multDegats = 0;
                }

                else if (RadioButtonComp1CoupNormalA.IsChecked == true)
                {
                    LabelInfo.Text = "Votre rune A inflige " + DegatsReelsComp1Num + " dégâts";
                    multDegats = 0;
                }

                else if (RadioButtonComp1CoupNormalB.IsChecked == true)
                {
                    LabelInfo.Text = "Votre rune B inflige " + DegatsReelsComp1Num + " dégâts";
                    multDegats = 0;
                }

                else if (RadioButtonComp1CoupCritique.IsChecked == true)
                {
                    LabelInfo.Text = "Vos runes A et B infligent " + DegatsReelsComp1Num + " dégâts";
                    multDegats = 0;
                }

                CdComp1 = 2;

                if (CdComp2 > 0)
                {
                    CdComp2 -= 2;
                }

                if (CdComp3 > 0)
                {
                    CdComp3 -= 1;
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
            string action = await DisplayActionSheet("Stase", "Annuler", null, "- Prochain tour : dégâts inchangés", "\n- Prochain tour : dégâts +25%", "\n- Vous forcez un ennemi à vous attaquer. Prochain tour : dégâts +50%", "\n- Vous forcez un ennemi à vous attaquer. Prochain tour : dégâts +100% et vous pouvez annuler un dé 3- et le transformer en 20");

            switch (action)
            {
                case "- Prochain tour : dégâts inchangés":
                    {
                        CdComp2 = 4;

                        if (CdComp1 > 0)
                        {
                            CdComp1 -= 1;
                        }

                        if (CdComp3 > 0)
                        {
                            CdComp3 -= 1;
                        }

                        LabelInfo.Text = "Vous stasez pendant 1 tour.";
                    }
                    affichageInitial();
                    break;

                case "\n- Prochain tour : dégâts +25%":
                    {
                        CdComp2 = 4;

                        if (CdComp1 > 0)
                        {
                            CdComp1 -= 1;
                        }

                        if (CdComp3 > 0)
                        {
                            CdComp3 -= 1;
                        }

                        multDegats = 25;
                        LabelInfo.Text = "Vous stasez pendant 1 tour. Prochain tour : dégâts +25%";
                    }
                    affichageInitial();
                    break;

                case "\n- Vous forcez un ennemi à vous attaquer. Prochain tour : dégâts +50%":
                    {
                        CdComp2 = 4;

                        if (CdComp1 > 0)
                        {
                            CdComp1 -= 1;
                        }

                        if (CdComp3 > 0)
                        {
                            CdComp3 -= 1;
                        }

                        multDegats = 50;
                        LabelInfo.Text = "Vous stasez pendant 1 tour et forcez un ennemi à vous attaquer. Prochain tour : dégâts +50% ";
                    }
                    affichageInitial();
                    LabelInfo.FontSize = 16;
                    break;

                case "\n- Vous forcez un ennemi à vous attaquer. Prochain tour : dégâts +100% et vous pouvez annuler un dé 3- et le transformer en 20":
                    {
                        CdComp2 = 4;

                        if (CdComp1 > 0)
                        {
                            CdComp1 -= 1;
                        }

                        if (CdComp3 > 0)
                        {
                            CdComp3 -= 1;
                        }

                        multDegats = 100;
                        LabelInfo.Text = "Vous stasez pendant 1 tour et forcez un ennemi à vous attaquer. Prochain tour : dégâts +100% et vous pouvez annuler un dé 3- et le transformer en 20";
                        affichageInitial();
                        LabelInfo.FontSize = 14;
                    }
                    break;
            }
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            degatsInfliges = 2 + 1 * (Intelligence/3) + 1 * (Agilité / 3);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= degatsInfliges / 10;
            }

            degatsInfliges += multDegats * degatsInfliges / 100;
            degatsInfliges += degatsStockes;

            bool comp3 = await DisplayAlert("Rafale runique", "Echec critique :\n- Dégâts = 0\n- Vous pouvez :\n   -> dévier la prochaine attaque que\n   vous recevez vers un allié au\n   hasard \n\n   Ou\n\n   -> réinitialiser le temps de recharge\n   de la compétence\n\nCoup critique :\n- Chaque rune réussie inflige " + degatsInfliges + "\n  dégâts", "Ok", "Annuler");

            switch (comp3)
            {
                case true:

                    degatsStockes = 0;
                    PopUpDegatsReelsComp3.IsVisible = true;

                    break;

                case false:

                    break;
            }
        }
        private async void ButtonValiderDegatsComp3_Click(object sender, EventArgs e)
        {

            string DegatsReelsComp3 = EntryDegatsReelsComp3.Text;
            int DegatsReelsComp3Num;

            if (int.TryParse(DegatsReelsComp3, out DegatsReelsComp3Num) == false)
            {
                EntryDegatsReelsComp3.Text = "E";
            }

            else
            {
                DegatsReelsComp3Num += multDegats * DegatsReelsComp3Num / 100;

                if (RadioButtonComp3EchecCritique.IsChecked == true)
                {
                    string action = await DisplayActionSheet("Choisir", "Annuler", null, "- Dévier la prochaine attaque que vous recevez vers un allié au hasard", "- Réinitialiser le temps de recharge de la compétence");

                    switch (action)
                    {
                        case "- Dévier la prochaine attaque que vous recevez vers un allié au hasard":
                            {
                                CdComp3 = 3;

                                if (CdComp1 > 0)
                                {
                                    CdComp1 -= 1;
                                }

                                if (CdComp2 > 0)
                                {
                                    CdComp2 -= 1;
                                }

                                LabelInfo.Text = "La compétence échoue mais vous dévierez la prochaine attaque que vous recevez vers un allié au hasard.";

                                break;
                            }

                        case "- Réinitialiser le temps de recharge de la compétence":
                            {
                                CdComp3 = 0;

                                if (CdComp1 > 0)
                                {
                                    CdComp1 -= 1;
                                }

                                if (CdComp2 > 0)
                                {
                                    CdComp2 -= 1;
                                }

                                LabelInfo.Text = "La compétence échoue mais le temps de recharge de la compétence est réinitialisé.";

                                break;
                            }
                    }
                }

                else if (RadioButtonComp3CoupCritique.IsChecked == true)
                {
                    CdComp3 = 3;

                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }

                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }

                    LabelInfo.Text = "Vos runes infligent " + DegatsReelsComp3Num + " dégâts.";
                    multDegats = 0;
                }

                affichageInitial();
                PopUpDegatsReelsComp3.IsVisible = false;
            }   
        }
        private void ButtonAnnulerDegatsComp3_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp3.IsVisible = false;
        }
    }
}

