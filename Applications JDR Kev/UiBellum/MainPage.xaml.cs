using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UiBellum
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ------------- Définition des variables ------------- 
        int PVmax = 150;
        int PVactuels = 150;
        int Bouclier = 0;
        int ArmureInitiale = 20;
        int Armure = 19;
        int ResistMagiqueInitiale = 17;
        int ResistMagique = 16;
        int Intelligence = 2;
        int Force = 21;
        int Agilité = 2;
        int Initiative = 12;
        int Pm = 4;
        int degatsInfliges;
        int degatsBaseArmes = 4;
        int degatsTotauxArmes;
        int CdComp1;
        int CdComp2;
        int CdComp3;
        bool DestinataireBouclierComp2;
        int DureeBonusComp2;
        bool inebranlable;

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


            if (DureeBonusComp2 == 0 && DestinataireBouclierComp2 == true)
            {
                Bouclier -= Force * 2;
                DestinataireBouclierComp2 = false;
            }
            LabelBouclier.Text = "Bouclier : " + Bouclier;


            if (CdComp1 == 3)
            {
                Armure = ArmureInitiale + Force / 3;
                ResistMagique += PVmax / 50;
            }
            if (DureeBonusComp2 == 2)
            {
                Armure  = ArmureInitiale + Force;
            }
            else if (CdComp1 < 3 && DureeBonusComp2 < 2)
            {
                Armure = ArmureInitiale;
                ResistMagique = ResistMagiqueInitiale;
            }
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
            LabelCdComp3.Text = "" + CdComp3;

            if (CdComp1 != 0)
            {
                ButtonComp1.IsEnabled = false;
                LabelCdComp1.BackgroundColor = Color.Maroon;
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
                LabelCdComp2.BackgroundColor = Color.Maroon;
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
                LabelCdComp3.BackgroundColor = Color.Maroon;
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

            if (CdComp2 == 4 && CdComp3 != 4)
            {
                LabelEtat.Text = "Stable";
            }
            else if (CdComp2 != 4 && CdComp3 == 4 && inebranlable == true)
            {
                LabelEtat.Text = "Inébranlable";
            }
            else if (CdComp2 == 4 && CdComp3 ==4)
            {
                LabelEtat.Text = "Stable et inébranlable";
            }
            else
            {
                LabelEtat.Text = "";
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

        async void comp2()
        {
            bool Comp2 = await DisplayAlert("Stabilisation", "Vous gagnerez " + Force + " Armure pendant un tour et " + (Force * 2) + " points de bouclier pendant 2 tours.\n\nVous serez insensible aux effets de déplacement.\n\n- Si une unité se tient à votre corps à corps, vous pourrez la repousser d'une case.\n\n- Si vous chosissez de transférer vos points de bouclier à un allié : le CD de Stabilisation sera réduit de 1 en cas de destruction par un ennemi.", "Ok", "Annuler");

            switch (Comp2)
            {
                case true:

                    String Comp2Bouclier = await DisplayActionSheet ("Destinataire des points de bouclier","Annuler",null,"Vous","Un allié");

                        switch (Comp2Bouclier)
                    {
                        case "Vous":
                            {
                                DestinataireBouclierComp2 = true;
                                DureeBonusComp2 = 2;
                                Bouclier += Force * 2;
                                LabelInfo.Text = "Vous gagnez " + Force + " Armure pendant un tour et " + (Force * 2) + " points de bouclier pendant 2 tours.\nVous êtes insensible aux effets de déplacement.\nVous pouvez repousser une unité à votre corps à corps d'une case.";
                                break;
                            }

                        case "Un allié":
                            {
                                DestinataireBouclierComp2 = false;
                                LabelInfo.Text = "Vous gagnez " + Force + " Armure pendant un tour et octroyez " + (Force * 2) + " points de bouclier à un allié pendant 2 tours.\nVous êtes insensible aux effets de déplacement.\nVous pouvez repousser une unité à votre corps à corps d'une case.";
                                break;
                            }
                    }
                    CdComp2 = 4;
                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }
                    if (CdComp3 > 0)
                    {
                        CdComp3 -= 1;
                    }

                    affichageInitial();
                    LabelInfo.FontSize = 15;
                    break;

                case false:

                    break;
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
            ArmureInitiale = modifStats();
            LabelInfo.Text = "Votre armure passe à " + modifStats() + "\n";
            affichageInitial();
        }

        private void ButtonResistMagique_Click(object sender, EventArgs e)
        {
            ResistMagique = modifStats();
            ResistMagiqueInitiale = modifStats();
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
                    if (DureeBonusComp2 > 0)
                    {
                        DureeBonusComp2 -= 1;
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

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = 90 * degatsInfliges / 100;
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

                if (CdComp1 > 0)
                {
                    CdComp1 -= 1;
                }
                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }
                if (DureeBonusComp2 > 0)
                {
                    DureeBonusComp2 -= 1;
                }
                if (CdComp3 > 0)
                {
                    CdComp3 -= 1;
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
            degatsInfliges = 7 + Force/2;
            int degatsInfligesCritique = degatsInfliges + 3 + Force / 2;

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges += degatsInfliges / 10;
                degatsInfligesCritique += degatsInfligesCritique / 10;
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = 90 * degatsInfliges / 100;
                degatsInfligesCritique = 90 * degatsInfligesCritique / 100;
            }

                bool comp1 = await DisplayAlert("Charge héroïque", "Vous chargerez une cible et arriverez à son corps à corps.\nDégâts = " + degatsInfliges + "\nArmure = +" + (Force/3) + "\nRM = +" + (PVmax/50) + "\n\n- Echec critique : la cible ne devient pas vulnérable\n\n- Coup critique :\nDégats = " + degatsInfligesCritique + "\nLa cible est étourdie", "Ok", "Annuler");

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
                    LabelInfo.Text = "Vous chargez la cible et lui infligez " + DegatsReelsComp1 + " dégâts.\nVous gagnez " + (Force / 3) + " Armure et " + (PVmax / 50) + " RM pendant 1 tour.";
                }

                else if (RadioButtonComp1CoupNormal.IsChecked == true)
                {
                    LabelInfo.Text = "Vous chargez la cible et lui infligez " + DegatsReelsComp1 + " dégâts.\nVous gagnez " + (Force / 3) + " Armure et " + (PVmax / 50) + " RM pendant 1 tour.\nLa cible devient vulnérable et subit 25% de dégâts supplémentaires pendant 1 tour.";
                }

                else if (RadioButtonComp1CoupCritique.IsChecked == true)
                {
                    LabelInfo.Text = "Vous chargez la cible et lui infligez " + DegatsReelsComp1 + " dégâts.\nVous gagnez " + (Force / 3) + " Armure et " + (PVmax / 50) + " RM pendant 1 tour.\nLa cible est étourdie, devient vulnérable et subit 25% de dégâts supplémentaires pendant 1 tour.";
                }

                CdComp1 = 3;

                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }
                if (CdComp3 > 0)
                {
                    CdComp3 -= 1;
                }
                if (DureeBonusComp2 > 0)
                {
                    DureeBonusComp2 -= 1;
                }


                affichageInitial();
                LabelInfo.FontSize = 17;
                PopUpDegatsReelsComp1.IsVisible = false;
            }
        }
        private void ButtonAnnulerDegatsComp1_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp1.IsVisible = false;
        }

        private void ButtonComp2_Click(object sender, EventArgs e)
        {
            comp2();
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {

            string comp3 = await DisplayActionSheet("Inébranlable","Annuler",null, "Toutes les attaques adverses sont dirigées contre vous pendant 1 tour", "Echec critique : La compétence échoue");

            switch (comp3)
            {
                case "Toutes les attaques adverses sont dirigées contre vous pendant 1 tour":

                    if (CdComp2 == 0)
                    {
                        bool comp3et2 = await DisplayAlert("Activer Stabilisation ?", null, "oui", "non");

                        switch (comp3et2)
                        {
                            case true:
                                inebranlable = true;
                                CdComp3 = 5;
                                comp2();

                                break;

                            case false:
                                inebranlable = true;
                                CdComp3 = 4;
                                LabelInfo.Text = "Toutes les attaques adverses sont dirigées contre vous pendant 1 tour";

                                break;
                        }
                    }

                    else
                    {
                        inebranlable = true;
                        CdComp3 = 4;
                        LabelInfo.Text = "Toutes les attaques adverses sont dirigées contre vous pendant 1 tour";
                    }

                    break;

                case "Echec critique : La compétence échoue" :
                    inebranlable = false;
                    CdComp3 = 4;
                    LabelInfo.Text = "La compétence échoue";

                    break;
            }

            if (CdComp1 > 0)
            {
                CdComp1 -= 1;
            }
            if (CdComp2 > 0)
            {
                CdComp2 -= 1;
            }
            if (DureeBonusComp2 > 0)
            {
                DureeBonusComp2 -= 1;
            }

            affichageInitial();
        }
    }
}
