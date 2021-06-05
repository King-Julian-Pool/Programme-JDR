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
        int PVmax = 108;
        int PVactuels = 108;
        int Bouclier = 0;
        int Armure = 3;
        int ResistMagique = 13;
        int Intelligence = 38;
        int Force = 2;
        int Agilité = 2;
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

            if (CdComp2 != 0)
            {
                ButtonComp2.IsEnabled = false;
                LabelCdComp2.BackgroundColor = Color.FromRgb(38, 170, 51);
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

            degatsTotauxArmes = (int) Math.Round(degatsBaseArmes + varForce + varAgilite + varIntelligence + varPVmax, MidpointRounding.AwayFromZero);

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
            CdPassifComp3 = CdComp3 + 1;
            LabelInfo.Text = "Le Cd de votre compétence 3 passe à " + ModifStats() + "\n";
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
                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }
                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }
                    if (CdComp2 == 3)
                    {
                        Bouclier -= BouclierComp2;
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
            int quartDegatsInfliges = (int) Math.Round((double)degatsInfliges / 4.0, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
                quartDegatsInfliges = (int)Math.Round((double)quartDegatsInfliges / 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
                quartDegatsInfliges = (int)Math.Round((double)quartDegatsInfliges*0.9, MidpointRounding.AwayFromZero);
            }

            String AutoAttack = await DisplayActionSheet("Auto-Attack", "Annuler", null, "- Cible unique : " + (degatsInfliges + quartDegatsInfliges) + " dégâts", "- Cibles multiples : " + degatsInfliges + " dégâts à une cible et " + quartDegatsInfliges + " dégâts à jusqu'à 3 autres cibles");

            if (AutoAttack == "Annuler" || AutoAttack == null)
            {
                return;
            }
            else
            {
                if (AutoAttack == "- Cible unique : " + (degatsInfliges + quartDegatsInfliges) + " dégâts")
                {
                    LabelInfo.Text = "Vous infligez " + (degatsInfliges + quartDegatsInfliges) + " dégâts à une seule cible";
                }
                else if (AutoAttack == "- Cibles multiples : " + degatsInfliges + " dégâts à un cible et " + quartDegatsInfliges + " dégâts à jusqu'à 3 autres cibles")
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts à une cible et " + quartDegatsInfliges + " aux autres cibles";
                }
                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }
                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }
                if (CdComp2 == 3)
                {
                    Bouclier -= BouclierComp2;
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

                AffichageInitial();
            }
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)2 + Intelligence * 0.66, MidpointRounding.AwayFromZero);
            int degatsInfliges1Cible = (int)Math.Round((double)2 + Intelligence / 2.0, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
                degatsInfliges1Cible = (int)Math.Round((double)degatsInfliges1Cible *1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
                degatsInfliges1Cible = (int)Math.Round((double)degatsInfliges1Cible * 0.9, MidpointRounding.AwayFromZero);
            }

            string comp1 = await DisplayActionSheet("Auto-Attack", "Annuler", null, "- Cible unique : " + degatsInfliges1Cible + " dégâts", "- Cibles multiples : " + degatsInfliges + " dégâts à toutes les cibles + 1 par 2 ennemis touchés");

            if (comp1 == "Annuler" || comp1 == null)
            {
                return;
            }
            else
            {
                if (comp1 == "- Cible unique : " + degatsInfliges1Cible + " dégâts")
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges1Cible + " dégâts à une cible unique";
                }
                else if (comp1 == "- Cibles multiples : " + degatsInfliges + " dégâts à toutes les cibles + 1 par 2 ennemis touchés")
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts à tous les ennemis + 1 par 2 ennemis touchés";
                }
                CdComp1 = 2;

                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }
                if (CdComp2 == 3)
                {
                    Bouclier -= BouclierComp2;
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


                AffichageInitial();
            }
        }

        private async void ButtonComp2_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)2 + ResistMagique / 2.0, MidpointRounding.AwayFromZero);


            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            BouclierComp2 = degatsInfliges * 4;

            string comp2 = await DisplayActionSheet("Décharge lumineuse", "Annuler", null, "- Echec critique :\nDégâts = " + degatsInfliges + "\nBouclier = +" + BouclierComp2 + " pour vous et votre cible pendant 1 tour\nLa boule n'emmagasine aucun dégât\n", "- Coup normal :\nDégâts = " + degatsInfliges + "\nBouclier = + " + BouclierComp2 + " pendant 1 tour\nLa boule emmagasine 33% des dégâts subis pendant un tour\n", "- Coup critique :\nDégâts = " + degatsInfliges + " à deux cibles\nBouclier = + " + (BouclierComp2 * 2) + " pendant 1 tour\nLes 2 boules emmagasinent 33% des dégâts subis pendant un tour");
            if (comp2 == "Annuler" || comp2 == null)
            {
                return;
            }
            else
            {
                if (comp2 == "- Echec critique :\nDégâts = " + degatsInfliges + "\nBouclier = +" + BouclierComp2 + " pour vous et votre cible pendant 1 tour\nLa boule n'emmagasine aucun dégât\n")
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts.\nVotre cible et vous recevez " + BouclierComp2 + " points de bouclier pour 1 tour.\nLa boule n'emmagasine aucun dégât.";
                }
                else if (comp2 == "- Coup normal :\nDégâts = " + degatsInfliges + "\nBouclier = + " + BouclierComp2 + " pendant 1 tour\nLa boule emmagasine 33% des dégâts subis pendant un tour\n")
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts.\nVous recevez " + BouclierComp2 + " points de bouclier pour 1 tour.\nLa boule emmagasine 33% des dégâts subis pendant un tour.";
                    LabelBouleLumière.Text = "1 Boule de lumière activée";
                }
                else if (comp2 == "- Coup critique :\nDégâts = " + degatsInfliges + " à deux cibles\nBouclier = + " + (BouclierComp2 * 2) + " pendant 1 tour\nLes 2 boules emmagasinent 33% des dégâts subis pendant un tour")
                {
                    BouclierComp2 *= 2;
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts à deux ennemis.\nVous recevez " + BouclierComp2 + " points de bouclier pour 1 tour.\nLes 2 boules emmagasinent 33% des dégâts subis pendant un tour.";
                    LabelBouleLumière.Text = "2 Boules de lumière activées";
                }
                Bouclier += BouclierComp2;
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

                AffichageInitial();
                LabelInfo.FontSize = 12;
            }
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)3 + Intelligence / 2.0, MidpointRounding.AwayFromZero);

            int degatsEnnemiA = degatsInfliges;
            int degatsEnnemiB = (int)Math.Round((double)degatsInfliges * 1.25, MidpointRounding.AwayFromZero);
            int degatsEnnemiC = (int)Math.Round((double)degatsInfliges * 1.5, MidpointRounding.AwayFromZero);
            int degatsEnnemiD = (int)Math.Round((double)6 + Intelligence, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
                degatsEnnemiA = (int)Math.Round((double)degatsEnnemiA * 1.1, MidpointRounding.AwayFromZero);
                degatsEnnemiB = (int)Math.Round((double)degatsEnnemiB * 1.1, MidpointRounding.AwayFromZero);
                degatsEnnemiC = (int)Math.Round((double)degatsEnnemiC * 1.1, MidpointRounding.AwayFromZero);
                degatsEnnemiD = (int)Math.Round((double)degatsEnnemiD * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
                degatsEnnemiA = (int)Math.Round((double)degatsEnnemiA * 0.9, MidpointRounding.AwayFromZero);
                degatsEnnemiB = (int)Math.Round((double)degatsEnnemiB * 0.9, MidpointRounding.AwayFromZero);
                degatsEnnemiC = (int)Math.Round((double)degatsEnnemiC * 0.9, MidpointRounding.AwayFromZero);
                degatsEnnemiD = (int)Math.Round((double)degatsEnnemiD * 0.9, MidpointRounding.AwayFromZero);
            }

            string comp3 = await DisplayActionSheet("Vous ne devriez pas exister", "Annuler", null , "- Echec critique : La compétence échoue","- Dégâts cible 1 = " + degatsEnnemiA + "\n  Dégâts passif = " + degatsEnnemiA, "- Dégâts cible 2 = " + degatsEnnemiB + "\n  Dégâts passif = " + (degatsEnnemiA + degatsEnnemiB), "- Dégâts cible 3 = " + degatsEnnemiC + "\n  Dégâts passif = " + (degatsEnnemiA + degatsEnnemiB + degatsEnnemiC), "- Dégâts cible 4 = " + degatsEnnemiD + "\n  Dégâts passif = " + (degatsEnnemiA + degatsEnnemiB + degatsEnnemiC + degatsEnnemiD));

            if (comp3 == "Annuler" || comp3 == null)
            {
                return;
            }
            else
            {
                if (comp3 == "- Echec critique : La compétence échoue")
                {
                    LabelInfo.Text = "La compétence échoue\n";
                    degatsPassifComp3 = 0;
                }
                if (comp3 == "- Dégâts cible 1 = " + degatsEnnemiA + "\n  Dégâts passif = " + degatsEnnemiA)
                {
                    degatsPassifComp3 = degatsEnnemiA;
                    LabelInfo.Text = "Vous infligez " + degatsEnnemiA + " dégâts à la cible 1";
                }
                if (comp3 == "- Dégâts cible 2 = " + degatsEnnemiB + "\n  Dégâts passif = " + (degatsEnnemiA + degatsEnnemiB))
                {
                    degatsPassifComp3 = degatsEnnemiA + degatsEnnemiB;
                    LabelInfo.Text = "Vous infligez " + degatsEnnemiA + " dégâts à la cible 1 et " + degatsEnnemiB + " dégâts à la cible 2";
                }
                if (comp3 == "- Dégâts cible 3 = " + degatsEnnemiC + "\n  Dégâts passif = " + (degatsEnnemiA + degatsEnnemiB + degatsEnnemiC))
                {
                    degatsPassifComp3 = degatsEnnemiA + degatsEnnemiB + degatsEnnemiC;
                    LabelInfo.Text = "Vous infligez " + degatsEnnemiA + " dégâts à la cible 1, " + degatsEnnemiB + " dégâts à la cible 2 et " + degatsEnnemiC + " dégâts à a cible 3";
                }
                if (comp3 == "- Dégâts cible 4 = " + degatsEnnemiD + "\n  Dégâts passif = " + (degatsEnnemiA + degatsEnnemiB + degatsEnnemiC + degatsEnnemiD))
                {
                    degatsPassifComp3 = degatsEnnemiA + degatsEnnemiB + degatsEnnemiC + degatsEnnemiD;
                    LabelInfo.Text = "Vous infligez " + degatsEnnemiA + " dégâts à la cible 1, " + degatsEnnemiB + " dégâts à la cible 2, " + degatsEnnemiC + " dégâts à a cible 3 et " + degatsEnnemiD + " dégâts à la cible 4";
                }

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
                if (CdComp2 == 3)
                {
                    Bouclier -= BouclierComp2;
                }
                if (CdBouleLumiere > 0)
                {
                    CdBouleLumiere -= 1;
                }

                AffichageInitial();
                LabelInfo.FontSize = 14;
            }

        }
    }
}
