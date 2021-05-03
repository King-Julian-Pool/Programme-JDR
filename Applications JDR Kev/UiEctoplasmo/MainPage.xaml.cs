using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UiEctoplasmo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ------------- Définition des variables ------------- 
        int PVmax = 1;
        int PVactuels = 1;
        int Bouclier = 0;
        int Armure = 6;
        int ResistMagique = 8;
        int Intelligence = 14;
        int Force = 22;
        int Agilité = 2;
        int Initiative = 1000;
        int Pm = 5;
        int degatsInfliges;
        int degatsBaseArmes = 15;
        int degatsTotauxArmes;
        int CdComp1;
        int CdComp2;
        int CdComp3;
        int PourcentageBouclier = 200;
        int ReserveBouclier = 0;
        int NombrePusilli;


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

            LabelPourcentages.Text = "Salvos : " + PourcentageBouclier + "% Réserve : " + ReserveBouclier;
            LabelPusilli.Text = "Pusilli : " + NombrePusilli;

            LabelInfo.FontSize = 16;

            LabelCdComp1.Text = "" + CdComp1;
            LabelCdComp2.Text = "" + CdComp2;
            LabelCdComp3.Text = "" + CdComp3;

            if (CdComp1 != 0)
            {
                ButtonComp1.IsEnabled = false;
                LabelCdComp1.BackgroundColor = Color.FromRgb(171, 221, 224);
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
                LabelCdComp2.BackgroundColor = Color.FromRgb(171, 221, 224);
            }
            else
            {
                ButtonComp2.IsEnabled = true;
                LabelCdComp2.BackgroundColor = Color.Default;
                LabelCdComp2.Text = "";
            }

            LabelBouclier.Text = "Bouclier : " + Bouclier;

            if (CdComp3 != 0)
            {
                ButtonComp3.IsEnabled = false;
                LabelCdComp3.BackgroundColor = Color.FromRgb(171, 221, 224);
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
                if (NombrePusilli > 0)
                {
                    NombrePusilli -= 1;
                    LabelInfo.Text = "Un de vos spectre encaisse le coup à votre place et meurt.";
                }
                else
                {
                    LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts";

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
                }
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

        private void ButtonSalvos_Click(object sender, EventArgs e)
        {
            PourcentageBouclier = modifStats();
            LabelInfo.Text = "Le pourcentage de puissance de votre relique passe à " + modifStats() + "%\n";
            affichageInitial();
        }

        private void ButtonReserve_Click(object sender, EventArgs e)
        {
            ReserveBouclier = modifStats();
            LabelInfo.Text = "Votre réserve passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonPusilli_Click(object sender, EventArgs e)
        {
            NombrePusilli = modifStats();
            LabelInfo.Text = "Le nombre de vos Pusilli passe à " + modifStats() + "\n";
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

        private void ButtonDebutTour_Click(object sender, EventArgs e)
        {
            if (CdComp2 != 4)
            {
                if (PourcentageBouclier - Bouclier > 50)
                {
                    PourcentageBouclier -= Bouclier;
                }
                else
                {
                    PourcentageBouclier = 50;
                }

                if (ReserveBouclier + Bouclier < 150)
                {
                    ReserveBouclier += Bouclier;
                }
                else
                {
                    ReserveBouclier = 150;
                }
            }
           
            Bouclier = 0;
            affichageInitial();
        }
        private async void ButtonFinTour_Click(object sender, EventArgs e)
        {
            bool FinTour = await DisplayAlert("Fin de tour", "Voulez vous vraiment passer votre tour ?", "Oui", "Non");

            switch (FinTour)
            {
                case true:

                    Bouclier = 0;

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
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = 90 * degatsInfliges / 100;
            }

            bool AutoAttack = await DisplayAlert("Auto-Attack", "Infliger jusqu'à " + degatsInfliges +" et gagner " + (PourcentageBouclier * degatsInfliges / 100) + " points de boucliers par ennemi touché ?", "Oui", "Non");

            switch (AutoAttack)
            {
                case true:

                    PopUpDegatsReelsAutoAttack.IsVisible = true;

                    break;

                case false:

                    break;
            }
        }
        private async void ButtonValiderDegatsAutoAttack_Click(object sender, EventArgs e)
        {

            string DegatsReelsAutoAttack = EntryDegatsReelsAutoAttack.Text;
            int DegatsReelsAutoAttackNum;

            if (int.TryParse(DegatsReelsAutoAttack, out DegatsReelsAutoAttackNum) == false)
            {
                EntryDegatsReelsAutoAttack.Text = "E";
            }
            else
            {
                string BouclierSouhaite = await DisplayPromptAsync("Bouclier souhaité", "", "Ok", "Annuler",null,-1,Keyboard.Numeric);
                int BouclierSouhaiteNum;
                if (int.TryParse(BouclierSouhaite, out BouclierSouhaiteNum) == false)
                {
                    LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
                }

                else
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts et gagnez " + BouclierSouhaiteNum + " points de bouclier.";
                    Bouclier = BouclierSouhaiteNum;


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
            return;
        }
        private void ButtonAnnulerDegatsAutoAttack_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsAutoAttack.IsVisible = false;
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsInfliges = 5 + 3 * Intelligence / 2 + 3 * Force / 2;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = 90 * degatsInfliges / 100;
            }

            bool comp1 = await DisplayAlert("Transfert", "Cette compétence ne peut pas toucher un point faible.\n\nCoup normal :\nDégâts = " + degatsInfliges + "\nBouclier = " + (PourcentageBouclier * degatsInfliges / 100) + "\n\n- Ennemi touché 2 fois par la compétence :\nDégâts = " + (degatsInfliges * 2) + "\nBouclier = " + (PourcentageBouclier * (degatsInfliges * 2) / 100) + "\n\n- Coup Critique :\nDégâts +50%" , "Ok", "Annuler");

            switch (comp1)
            {
                case true:

                    PopUpDegatsReelsComp1.IsVisible = true;

                    break;

                case false:

                    break;
            }
        }
        private async void ButtonValiderDegatsComp1_Click(object sender, EventArgs e)
        {
            string DegatsReelsComp1 = EntryDegatsReelsComp1.Text;
            int DegatsReelsComp1Num;

            if (int.TryParse(DegatsReelsComp1, out DegatsReelsComp1Num) == false)
            {
                EntryDegatsReelsComp1.Text = "E";
            }

            else
            {
                string BouclierSouhaite = await DisplayPromptAsync("Bouclier souhaité", "", "Ok", "Annuler", null, -1, Keyboard.Numeric);
                int BouclierSouhaiteNum;
                if (int.TryParse(BouclierSouhaite, out BouclierSouhaiteNum) == false)
                {
                    LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
                }

                else
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts et gagnez " + BouclierSouhaiteNum + " points de bouclier";
                    Bouclier = BouclierSouhaiteNum;

                    CdComp1 = 4;

                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }


                    affichageInitial();
                    PopUpDegatsReelsComp1.IsVisible = false;
                }
            }
        }
        private void ButtonAnnulerDegatsComp1_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp1.IsVisible = false;
        }

        private async void ButtonComp2_Click(object sender, EventArgs e)
        {
            degatsInfliges = ReserveBouclier / 2;

            if (degatsInfliges < degatsTotauxArmes)
            {
                degatsInfliges = degatsTotauxArmes;
            }

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = 90 * degatsInfliges / 100;
            }

            bool comp2 = await DisplayAlert("Rejoins moi", "Vous attirez une cible à votre corps à corps et lui infligez jusqu'à " + degatsInfliges + " dégâts et gagnez jusqu'à " + ((PourcentageBouclier + 2 * ReserveBouclier / 3) * degatsInfliges / 100) + " points de bouclier.\n\nVotre réserve se vide.\n\nLe bouclier généré n'affaiblit pas votre relique.", "Ok", "Annuler");

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
                PourcentageBouclier += 2 * ReserveBouclier / 3;
                Bouclier = (PourcentageBouclier * degatsInfliges / 100);
                ReserveBouclier = 0;

                LabelInfo.Text = "Vous attirez une cible à votre corps à corps et lui infligez " + DegatsReelsComp2Num + " dégâts et gagnez " + (PourcentageBouclier * degatsInfliges /100) + " points de bouclier";


                CdComp2 = 4;
                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }


                affichageInitial();
                LabelInfo.FontSize = 18;
                PopUpDegatsReelsComp2.IsVisible = false;
            }

        }
        private void ButtonAnnulerDegatsComp2_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp2.IsVisible = false;
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            degatsInfliges =  Intelligence / 4 + Force / 2;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = 90 * degatsInfliges / 100;
            }

            bool comp2 = await DisplayAlert("Pusilli Ectoplasmi", "Réincarner en spectre jusqu'à 3 adversaires morts au cours du combat.\nChaque spectre inflige jusqu'à " + degatsInfliges + " dégâts et vous fait gagner jusqu'à " + (PourcentageBouclier * degatsInfliges / 100) + " points de boucliers.", "Ok", "Annuler");

            switch (comp2)
            {
                case true:

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
                string BouclierSouhaite = await DisplayPromptAsync("Bouclier souhaité", "", "Ok", "Annuler", null, -1, Keyboard.Numeric);
                int BouclierSouhaiteNum;
                if (int.TryParse(BouclierSouhaite, out BouclierSouhaiteNum) == false)
                {
                    LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
                }
                else
                {
                    if (RadioButtonComp3EchecCritique.IsChecked == true)
                    {
                        LabelInfo.Text = "La compétence échoue et vous générez le même bouclier qu'au tour précédent.";
                    }

                    if (RadioButtonComp3CoupNormal.IsChecked == true)
                    {
                        string NombreSpectre = await DisplayPromptAsync("Nombre de Spectre", "", "Ok", "Annuler", null, -1, Keyboard.Numeric);
                        int NombreSpectreNum;
                        if (int.TryParse(NombreSpectre, out NombreSpectreNum) == false)
                        {
                            LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
                        }
                        else
                        {
                            NombrePusilli = NombreSpectreNum;
                            if (NombreSpectreNum == 1)
                            {
                                LabelInfo.Text = "Votre spectre inflige " + DegatsReelsComp3Num + " dégâts et vous gagnez " + BouclierSouhaiteNum + " points de boucliers.";
                            }
                            else
                            {
                                LabelInfo.Text = "Vos " + NombreSpectreNum + " spectres infligent " + DegatsReelsComp3Num + " dégâts et vous gagnez " + BouclierSouhaiteNum + " points de boucliers.";
                            }
                        }
                    }


                    Bouclier = BouclierSouhaiteNum;

                    CdComp3 = 99;
                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }
                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }

                    affichageInitial();
                    LabelInfo.FontSize = 14;
                    PopUpDegatsReelsComp3.IsVisible = false;
                }
            }

        }
        private void ButtonAnnulerDegatsComp3_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp3.IsVisible = false;
        }
    }
}
