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
        int degatsBaseArmes = 18;
        int degatsTotauxArmes;
        int CdComp1;
        int CdComp2;
        int CdComp3;
        int CdComp4;
        int PourcentageBouclier = 200;
        int ReserveBouclier = 0;
        int BouclierPrecedent;
        int NombrePusilli;
        int Degats1Pussilli;
        int DegatsTotauxPusilli;


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
            LabelCdComp4.Text = "" + CdComp4;

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

            if (CdComp4 != 0)
            {
                ButtonComp4.IsEnabled = false;
                LabelCdComp4.BackgroundColor = Color.FromRgb(171, 221, 224);
            }
            else
            {
                ButtonComp4.IsEnabled = true;
                LabelCdComp4.BackgroundColor = Color.Default;
                LabelCdComp4.Text = "";
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

            DegatsTotauxPusilli = (int)Math.Round((double)Intelligence / 4.0 + Force / 2.0, MidpointRounding.AwayFromZero) * NombrePusilli;

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
                }
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

        private void ButtonCdComp3_Click(object sender, EventArgs e)
        {
            CdComp3 = ModifStats();
            LabelInfo.Text = "Le Cd de votre compétence 3 passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonCdComp4_Click(object sender, EventArgs e)
        {
            CdComp4 = ModifStats();
            LabelInfo.Text = "Le Cd de votre compétence 4 passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonSalvos_Click(object sender, EventArgs e)
        {
            PourcentageBouclier = ModifStats();
            LabelInfo.Text = "Le pourcentage de puissance de votre relique passe à " + ModifStats() + "%\n";
            AffichageInitial();
        }

        private void ButtonReserve_Click(object sender, EventArgs e)
        {
            ReserveBouclier = ModifStats();
            LabelInfo.Text = "Votre réserve passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonPusilli_Click(object sender, EventArgs e)
        {
            NombrePusilli = ModifStats();
            LabelInfo.Text = "Le nombre de vos Pusilli passe à " + ModifStats() + "\n";
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
            AffichageInitial();
        }
        private async void ButtonFinTour_Click(object sender, EventArgs e)
        {
            bool FinTour = await DisplayAlert("Fin de tour", "Voulez vous vraiment passer votre tour ?", "Oui", "Non");

            switch (FinTour)
            {
                case true:

                    Bouclier = 0;
                    BouclierPrecedent = Bouclier;

                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }
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
            degatsInfliges += DegatsTotauxPusilli;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            string DegatsReelsAutoAttack = await DisplayPromptAsync("Dégâts réels Auto-Attack", "Infliger jusqu'à " + degatsInfliges + " et gagner " + (int)Math.Round((double)PourcentageBouclier * degatsInfliges / 100.0, MidpointRounding.AwayFromZero) + " points de boucliers par ennemi touché ?","Ok","Annuler",null,-1,Keyboard.Numeric);

            if (int.TryParse(DegatsReelsAutoAttack, out int DegatsReelsAutoAttackNum) == false)
            {
                LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
            }

            else
            {
                string BouclierSouhaite = await DisplayPromptAsync("Bouclier souhaité", "Bouclier maxiumum possible : " + (int)Math.Round((double)PourcentageBouclier * DegatsReelsAutoAttackNum / 100.0, MidpointRounding.AwayFromZero), "Ok", "Annuler", null, -1, Keyboard.Numeric);

                if (int.TryParse(BouclierSouhaite, out int BouclierSouhaiteNum) == false)
                {
                    LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
                }

                else
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts et gagnez " + BouclierSouhaiteNum + " points de bouclier";
                    Bouclier = BouclierSouhaiteNum;
                    BouclierPrecedent = Bouclier;
                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }

                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }


                    AffichageInitial();
                }
            }

        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)5 + Intelligence * 1.5 + Force * 1.5, MidpointRounding.AwayFromZero);
            degatsInfliges += DegatsTotauxPusilli;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            string DegatsReelsComp1 = await DisplayPromptAsync("Dégâts réels Compétence 1", "Cette compétence ne peut pas toucher un point faible.\n\n- Coup normal :\nDégâts = " + degatsInfliges + "\nBouclier = " + (int)Math.Round((double)PourcentageBouclier * degatsInfliges / 100.0, MidpointRounding.AwayFromZero) + "\n\n- Ennemi touché 2 fois par la compétence :\nDégâts = " + (degatsInfliges * 2) + "\nBouclier = " + (int)Math.Round((double)(PourcentageBouclier * (degatsInfliges * 2) / 100.0), MidpointRounding.AwayFromZero) + "\n\n- Coup Critique :\nDégâts +50%\n(1ère fois : " + (int)Math.Round((double)degatsInfliges * 1.5, MidpointRounding.AwayFromZero) + " / 2e fois : " + (int)Math.Round((double)degatsInfliges * 3, MidpointRounding.AwayFromZero) + ")", "Ok", "Annuler", null, -1, Keyboard.Numeric);

            if (int.TryParse(DegatsReelsComp1, out int DegatsReelsComp1Num) == false)
            {
                LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
            }

            else
            {
                string BouclierSouhaite = await DisplayPromptAsync("Bouclier souhaité", "Bouclier maxiumum possible : " + (int)Math.Round((double)PourcentageBouclier * DegatsReelsComp1Num / 100.0, MidpointRounding.AwayFromZero), "Ok", "Annuler", null, -1, Keyboard.Numeric);

                if (int.TryParse(BouclierSouhaite, out int BouclierSouhaiteNum) == false)
                {
                    LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
                }

                else
                {
                    LabelInfo.Text = "Vous infligez " + DegatsReelsComp1Num + " dégâts et gagnez " + BouclierSouhaiteNum + " points de bouclier";
                    Bouclier = BouclierSouhaiteNum;
                    BouclierPrecedent = Bouclier;

                    CdComp1 = 4;

                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }


                    AffichageInitial();
                }
            }
        }

        private async void ButtonComp2_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)ReserveBouclier * 0.5, MidpointRounding.AwayFromZero);

            if (degatsInfliges < DefDegatsTotauxArmes())
            {
                degatsInfliges = DefDegatsTotauxArmes();
            }

            degatsInfliges += DegatsTotauxPusilli;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            string DegatsReelsComp2 = await DisplayPromptAsync("Dégâts réels Compétence 1", "Vous attirez une cible à votre corps à corps et lui infligez jusqu'à " + degatsInfliges + " dégâts et gagnez jusqu'à " + (int)Math.Round((double)((PourcentageBouclier + 2 * ReserveBouclier / 3.0) * degatsInfliges / 100.0), MidpointRounding.AwayFromZero) + " points de bouclier.\n\nVotre réserve se vide.\n\nLe bouclier généré n'affaiblit pas votre relique.", "Ok", "Annuler", null, -1, Keyboard.Numeric);

            if (int.TryParse(DegatsReelsComp2, out int DegatsReelsComp2Num) == false)
            {
                LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
            }
            else
            {
                PourcentageBouclier += (int)Math.Round((double)2 * ReserveBouclier / 3.0, MidpointRounding.AwayFromZero);
                Bouclier = (int)Math.Round((double)(PourcentageBouclier * degatsInfliges / 100.0), MidpointRounding.AwayFromZero);
                ReserveBouclier = 0;
                BouclierPrecedent = Bouclier;

                LabelInfo.Text = "Vous attirez une cible à votre corps à corps et lui infligez " + DegatsReelsComp2Num + " dégâts et gagnez " + (int)Math.Round((double)(PourcentageBouclier * degatsInfliges / 100.0), MidpointRounding.AwayFromZero) + " points de bouclier";


                CdComp2 = 4;
                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }


                AffichageInitial();
                LabelInfo.FontSize = 18;
            }
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            Degats1Pussilli = (int)Math.Round((double)Intelligence / 4.0 + Force / 2.0, MidpointRounding.AwayFromZero);

            string NombreSpectre = await DisplayPromptAsync("Pusilli Ectoplasmi"," Réincarner en spectre jusqu'à 3 adversaires morts au cours du combat.\n Chaque spectre inflige jusqu'à " + Degats1Pussilli + " dégâts et vous fait gagner jusqu'à " + (int)Math.Round((double)PourcentageBouclier * Degats1Pussilli / 100.0, MidpointRounding.AwayFromZero) + " points de boucliers.\n Echec critique : La compétence échoue, vous générez le même bouclier qu'au tour précédent (" + BouclierPrecedent + " points de bouclier)\n\n     Nombre de spectres =","Ok","Annuler",null,-1,Keyboard.Numeric);

            if (int.TryParse(NombreSpectre, out int NombreSpectreNum) == false)
            {
                LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
            }
            else
            {
                NombrePusilli = NombreSpectreNum;
                DegatsTotauxPusilli = (int)Math.Round((double)Intelligence / 4.0 + Force / 2.0, MidpointRounding.AwayFromZero) * NombrePusilli;
                
                degatsInfliges = DegatsTotauxPusilli;

                if (Beni() == true && Maudit() == false)
                {
                    degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
                }

                else if (Maudit() == true && Beni() == false)
                {
                    degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
                }

                if (NombreSpectreNum == 0)
                {
                    LabelInfo.Text = "La compétence échoue et vous générez le même bouclier qu'au tour précédent (" + BouclierPrecedent + ").";
                    Bouclier = BouclierPrecedent;

                    CdComp3 = 99;
                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }
                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }

                    AffichageInitial();
                    LabelInfo.FontSize = 14;
                }
                else if (NombreSpectreNum > 0)
                {

                    string BouclierSouhaite = await DisplayPromptAsync("Bouclier souhaité", "Dégâts totaux des spectres = " + DegatsTotauxPusilli + "\n\nBouclier maxiumum possible : " + (int)Math.Round((double)PourcentageBouclier * DegatsTotauxPusilli / 100.0, MidpointRounding.AwayFromZero), "Ok", "Annuler", null, -1, Keyboard.Numeric);

                    if (int.TryParse(BouclierSouhaite, out int BouclierSouhaiteNum) == false)
                    {
                        LabelInfo.Text = "Ce n'est pas un nombre entier, rééssayez !";
                    }
                    else
                    {

                        if (NombreSpectreNum == 1)
                        {
                            LabelInfo.Text = "Votre spectre inflige " + DegatsTotauxPusilli + " dégâts et vous gagnez " + BouclierSouhaiteNum + " points de boucliers.";
                        }
                        else
                        {
                            LabelInfo.Text = "Vos " + NombreSpectreNum + " spectres infligent " + DegatsTotauxPusilli + " dégâts et vous gagnez " + BouclierSouhaiteNum + " points de boucliers.";
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

                        AffichageInitial();
                        LabelInfo.FontSize = 14;
                    }
                }
            }
 

        }

        private async void ButtonComp4_Click(object sender, EventArgs e)
        {
            bool comp4 = await DisplayAlert("La fin et votre âme ne font qu'un", "Extirper un fragment d'âme de la cible et créer une copie spectrale de son corps juste devant vous. La copie spectrale dure un tour.\n\nTous les dégâts infligés à la copie spectrale sont renvoyés à la cible concernée.\n\nLe coeur de la cible devient un point faible (réussite = 1 à 13 au dé).\n\nNe consomme pas votre tour.","Ok","Annuler");

            switch (comp4)
            {
                case true:

                    CdComp4 = 99;

                    LabelInfo.Text = "Vous créez, devant vous, une copie spectrale de la cible qui dure un tour.";

                    AffichageInitial();
                    break;

                case false:

                    break;
            }
        }
    }
}
