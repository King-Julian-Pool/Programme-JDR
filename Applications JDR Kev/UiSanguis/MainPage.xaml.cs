using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UiSanguis
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ------------- Définition des variables ------------- 
        int PVmax = 200;
        int PVmaxInitiaux = 200;
        int PVactuels = 200;
        int PVperdus = 0;
        int Bouclier = 0;
        int Armure = 6;
        int ResistMagique = -3;
        int Intelligence = 10;
        int Force = 18;
        int Agilité = 18;
        int Initiative = 32;
        int Pm = 4;
        int degatsInfliges;
        int degatsBaseArmes = 12;
        int degatsTotauxArmes;
        int PVsacrifiés;
        int CdComp1;
        int CdComp2;

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
            LabelPv.Text = "PV : " + PVactuels + "/" + PVmax + "(" + PVmaxInitiaux + ")";
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

            LabelCdComp1.Text = "" + CdComp1;
            LabelCdComp2.Text = "" + CdComp2;

            if (CdComp1 != 0)
            {
                ButtonComp1.IsEnabled = false;
                LabelCdComp1.BackgroundColor = Color.Red;
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
                LabelCdComp2.BackgroundColor = Color.Red;
            }
            else
            {
                ButtonComp2.IsEnabled = true;
                LabelCdComp2.BackgroundColor = Color.Default;
                LabelCdComp2.Text = "";
            }


            if (degatsAugmentes() == true)
            {
                LabelDegatsAugmentes.IsVisible = true;
            }
            else
            {
                LabelDegatsAugmentes.IsVisible = false;
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

            if (Bouclier == 0)
            {
                PVactuels = PVactuels - degatsRecusNum;

                if (PVmaxInitiaux - (PVperdus / 2) >= PVmaxInitiaux / 2)
                {
                    if (PVperdus + degatsRecusNum < 200)
                    {
                        PVperdus += degatsRecusNum;
                    }
                    else
                    {
                        PVperdus = 200;
                    }
                    LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts et vos PVmax sont diminués de " + PVperdus / 2;
                    PVmax = PVmaxInitiaux - (PVperdus / 2); // PV max réduit si 2PV perdu d'un coup ou tous les 2 PV perdus (ex : Si 2 attaques de 1 dégats, PVmax réduits ou pas ?)
                }
                else
                {
                    LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts et vos PVmax sont diminués de " + (PVmax - (PVmaxInitiaux / 2));
                    PVmax = PVmaxInitiaux / 2;
                    PVperdus += (PVmax - (PVmaxInitiaux / 2));
                }
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

                if (PVmaxInitiaux - (PVperdus / 2) >= PVmaxInitiaux / 2)
                {
                    if (PVperdus + degatsRecusNum < 200)
                    {
                        PVperdus += degatsRecusNum;
                    }
                    else
                    {
                        PVperdus = 200;
                    }

                    LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts et vos PVmax sont diminués de " + (PVmax - (PVperdus / 2));
                    PVmax = PVmaxInitiaux - (PVperdus / 2);
                }
                else
                {
                    LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts et vos PVmax sont diminués de " + (PVmax - (PVmaxInitiaux / 2));
                    PVmax = PVmaxInitiaux / 2;
                    PVperdus += (PVmax - (PVmaxInitiaux / 2));
                }
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

        bool degatsAugmentes()
        {
            if (PVactuels < PVmax / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
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

            PVperdus = (PVmax * 2) - PVmaxInitiaux;

            affichageInitial();
        }

        private void ButtonPVmaxInitiaux_Click(object sender, EventArgs e)
        {
            PVmaxInitiaux = modifStats();
            LabelInfo.Text = "Vos PV maximum initiaux passent à " + modifStats() + "\n";
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

                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }

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
                degatsInfliges += degatsInfliges / 10;

                if (degatsAugmentes() == true)
                {
                    degatsInfliges += degatsInfliges / 4;
                }
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= degatsInfliges / 10;

                if (degatsAugmentes() == true)
                {
                    degatsInfliges += degatsInfliges / 4;
                }
            }

            bool AutoAttack = await DisplayAlert("Auto-Attack", "Voulez-vous infliger jusqu'à " + degatsInfliges + " dégâts et vous soigner jusqu'à " + degatsInfliges / 2 + "PV?", "Oui", "Non");

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
                if ((PVactuels + DegatsReelsAutoAttackNum / 2) < PVmax)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts et vous soignez de " + DegatsReelsAutoAttackNum / 2 + "PV";
                    PVactuels = PVactuels + DegatsReelsAutoAttackNum / 2;
                }
                else
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts et vous soignez de " + (PVmax - PVactuels) + "PV";
                    PVactuels = PVmax;
                }

                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }

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
            degatsInfliges = 3 + 3 * (Force / 2) + 3 * (Agilité / 2);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;

                if (degatsAugmentes() == true)
                {
                    degatsInfliges += degatsInfliges / 4;
                }
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= degatsInfliges / 10;

                if (degatsAugmentes() == true)
                {
                    degatsInfliges += degatsInfliges / 4;
                }
            }

                bool comp1 = await DisplayAlert("Cela ne t'est pas si vital", "Echec critique :\n- Dégâts = " + degatsInfliges + "\n- Soin = 0\n- Cible terrifiée\n\nCoup normal :\n- Dégâts = " + degatsInfliges + "\n- Soin = " + degatsInfliges + "\n- Cible terrifiée\n\nCoup critique :\n- Dégâts = " + degatsInfliges + "\n- Soin = " + degatsInfliges + "\n- PVmax = +" + degatsInfliges + "\n- Cible terrifiée", "Ok", "Annuler");

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
                if (RadioButtonComp1EchecCritique.IsChecked == true)
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts mais vous ne vous soignez pas.\nLa cible est terrifiée.";

                    CdComp1 = 2;
                }

                else if (RadioButtonComp1CoupNormal.IsChecked == true)
                {
                    if (PVactuels + degatsInfliges < PVmax)
                    {
                        PVactuels = PVactuels + degatsInfliges;
                        LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts et vous soignez de " + degatsInfliges + "PV.\nLa cible est terrifiée.";
                    }
                    else
                    {
                        LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts et vous soignez de " + (PVmax - PVactuels) + "PV.\nLa cible est terrifiée.";
                        PVactuels = PVmax;
                    }

                    CdComp1 = 2;
                }

                else if (RadioButtonComp1CoupCritique.IsChecked == true)
                {
                    PVmax += degatsInfliges;

                    if (PVactuels + degatsInfliges < PVmax)
                    {
                        PVactuels = PVactuels + degatsInfliges;
                        LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts et vous soignez de " + degatsInfliges + "PV.\nVos PVmax augmentent de " + degatsInfliges + ".\nLa cible est terrifiée.";
                    }
                    else
                    {
                        LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts et vous soignez de " + (PVmax - PVactuels) + "PV.\nVos PVmax augmentent de " + degatsInfliges + ".\nLa cible est terrifiée.";
                        PVactuels = PVmax;
                    }

                    CdComp1 = 2;
                }

                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
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
            degatsInfliges = 1 + Force / 3 + Agilité / 3;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;

                if (degatsAugmentes() == true)
                {
                    degatsInfliges += degatsInfliges / 4;
                }
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= degatsInfliges / 10;

                if (degatsAugmentes() == true)
                {
                    degatsInfliges += degatsInfliges / 4;
                }
            }

            bool Comp2 = await DisplayAlert("Pluie de sang", "Voulez-vous infliger jusqu'à " + degatsInfliges + " dégâts à tous les ennemis et vous soigner de " + (PVmax - PVactuels) + "PV?", "Oui", "Non");

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
                LabelInfo.Text = "Vous infligez " + DegatsReelsComp2Num + " dégâts à tous les ennemis et vous soignez de " + (PVmax - PVactuels) + "PV."; // PV soignés = basés  sur dégats théoriques ou effectifs ?
                PVactuels = PVmax;
                CdComp2 = 6;

                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }

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
            PVsacrifiés = 20 * PVactuels / 100;
            degatsInfliges = defDegatsTotauxArmes() + defDegatsTotauxArmes() / 2;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;

                if (degatsAugmentes() == true)
                {
                    degatsInfliges += degatsInfliges / 4;
                }
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsInfliges -= degatsInfliges / 10;

                if (degatsAugmentes() == true)
                {
                    degatsInfliges += degatsInfliges / 4;
                }
            }

            bool comp3 = await DisplayAlert("Sous l'armure il y a une victime", "Echec critique :\n- PVsacrifiés = " + PVsacrifiés + "\n- Dégâts = 0\n\nCoup normal :\n- PVsacrifiés = " + PVsacrifiés + "\n- Dégâts = " + degatsInfliges + "\n\nSi PV< 20% des PV max :\n- PV soignés = PV sacrifiés\n- Bouclier = +" + (degatsInfliges / 2), "Ok", "Annuler");

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

                    if ((PVmax + PVsacrifiés) < PVmaxInitiaux)
                    {
                        LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV mais la compétence échoue et vous n'infligez pas de dégâts. Vous gagnez " + PVsacrifiés + " PVmax";
                        PVmax += PVsacrifiés;
                        PVperdus -= PVsacrifiés * 2;
                    }
                    else
                    {
                        LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV mais la compétence échoue et vous n'infligez pas de dégâts. Vous gagnez " + (PVmaxInitiaux - PVmax) + " PVmax";
                        PVmax = PVmaxInitiaux;
                        PVperdus = 0;
                    }


                    PVactuels = PVactuels - PVsacrifiés;

                    if (CdComp2 > 0 && CdComp2 <= 4)
                    {
                        CdComp2 -= 1;
                    }
                }

                else if (RadioButtonComp3CoupNormal.IsChecked == true)
                {

                    if (PVactuels <= PVmax / 4)
                    {


                        if ((PVmax + PVsacrifiés) < PVmaxInitiaux)
                        {
                            LabelInfo.Text = "Vous ne sacrifiez pas de PV et infligez " + DegatsReelsComp3Num + " dégâts.\nVous gagnez " + PVsacrifiés + " PVmax et un bouclier de " + (DegatsReelsComp3Num / 2) + ".";
                            PVmax += PVsacrifiés;
                            PVperdus -= PVsacrifiés * 2;
                        }
                        else
                        {
                            LabelInfo.Text = "Vous ne sacrifiez pas de PV et infligez " + DegatsReelsComp3Num + " dégâts.\nVous gagnez " + (PVmaxInitiaux - PVmax) + " PVmax et un bouclier de " + (DegatsReelsComp3Num / 2) + ".";
                            PVmax = PVmaxInitiaux;
                            PVperdus = 0;
                        }



                        Bouclier = Bouclier + DegatsReelsComp3Num / 2;

                        if (CdComp2 > 0 && CdComp2 <= 4)
                        {
                            CdComp2 -= 1;
                        }
                    }

                    else
                    {

                        if ((PVmax + PVsacrifiés) < PVmaxInitiaux)
                        {
                            LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV et infligez " + DegatsReelsComp3Num + " dégâts. Vous gagnez " + PVsacrifiés + " PVmax";
                            PVmax += PVsacrifiés;
                            PVperdus -= PVsacrifiés * 2;
                        }
                        else
                        {
                            LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV et infligez " + DegatsReelsComp3Num + " dégâts. Vous gagnez " + (PVmaxInitiaux - PVmax) + " PVmax";
                            PVmax = PVmaxInitiaux;
                            PVperdus = 0;
                        }


                        PVactuels = PVactuels - PVsacrifiés;

                        if (CdComp2 > 0 && CdComp2 <= 4)
                        {
                            CdComp2 -= 1;
                        }

                    }
                }

                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
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
    }
}

