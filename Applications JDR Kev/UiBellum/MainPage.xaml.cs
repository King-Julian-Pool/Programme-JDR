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
        int ArmureInitiale = 19;
        int Armure = 19;
        int ResistMagiqueInitiale = 16;
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

        int PVmaxPet = 150;
        int PVactuelsPet = 150;
        int BouclierPet = 0;

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

        private async void BarPvPet(ProgressBar ProgressBarPvPet)
        {
            await ProgressBarPvPet.ProgressTo(Math.Round((double)PVactuelsPet / PVmaxPet, 1), 1000, Easing.CubicInOut);

            if (PVactuelsPet <= PVmaxPet / 4)
            {
                ProgressBarPvPet.ProgressColor = Color.Red;
            }
            else if (PVactuelsPet <= PVmaxPet / 2)
            {
                ProgressBarPvPet.ProgressColor = Color.Orange;
            }
            else
            {
                ProgressBarPvPet.ProgressColor = Color.Green;
            }
        }

        void AffichageInitial()
        {
            LabelPv.Text = "PV : " + PVactuels + "/" + PVmax;
            BarPv(ProgressBarPv);

            LabelPvPet.Text = "PV : " + PVactuelsPet + "/" + PVmaxPet;
            BarPvPet(ProgressBarPvPet);


            if (DureeBonusComp2 == 0 && DestinataireBouclierComp2 == true)
            {
                Bouclier -= Force * 2;
                DestinataireBouclierComp2 = false;
            }
            LabelBouclier.Text = "Bouclier : " + Bouclier;

            LabelBouclierPet.Text = "Bouclier : " + BouclierPet;


            if (CdComp1 == 3)
            {
                Armure = ArmureInitiale + (int)Math.Round((double)Force / 3.0, MidpointRounding.AwayFromZero);
                ResistMagique += (int)Math.Round((double)PVmax / 50.0, MidpointRounding.AwayFromZero);
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

        void Heal()
        {
            string PVsoignes = EntryDegatsRecus.Text;

            if (int.TryParse(PVsoignes, out int PVsoignesNum) == false)
            {
                EntryDegatsRecus.Text = "E";
            }
            else
            {
                LabelInfo.Text = "Vous vous soignez de " + PVsoignesNum + " PV";
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


        void HealPet()
        {
            string PVsoignesPet = EntryDegatsRecusPet.Text;

            if (int.TryParse(PVsoignesPet, out int PVsoignesPetNum) == false)
            {
                EntryDegatsRecusPet.Text = "E";
            }
            else
            {
                LabelInfoPet.Text = "Sonne se soigne de " + PVsoignesPetNum + " PV";
            }

            if ((PVactuelsPet + PVsoignesPetNum) < PVmaxPet)
            {
                PVactuelsPet += PVsoignesPetNum;
            }
            else
            {
                PVactuelsPet = PVmaxPet;
            }
            AffichageInitial();
        }

        void GainBouclierPet()
        {
            string GainBouclierPet = EntryDegatsRecusPet.Text;

            if (int.TryParse(GainBouclierPet, out int GainBouclierPetNum) == false)
            {
                EntryDegatsRecusPet.Text = "E";
            }
            else
            {
                LabelInfoPet.Text = "Sonne gagne " + GainBouclierPetNum + " points de bouclier";
            }
            BouclierPet += GainBouclierPetNum;
            AffichageInitial();
        }

        void ReceptionDegatsPet()
        {
            string degatsRecusPet = EntryDegatsRecusPet.Text;

            if (int.TryParse(degatsRecusPet, out int degatsRecusPetNum) == false)
            {
                EntryDegatsRecusPet.Text = "E";
            }
            else
            {
                LabelInfoPet.Text = "Sonne subit " + degatsRecusPetNum + " dégâts";
            }

            if (BouclierPet == 0)
            {
                PVactuelsPet -= degatsRecusPetNum;
            }
            else if (BouclierPet >= degatsRecusPetNum)
            {
                BouclierPet -= degatsRecusPetNum;
            }
            else
            {
                degatsRecusPetNum -= BouclierPet;
                BouclierPet -= BouclierPet;
                PVactuelsPet -= degatsRecusPetNum;
            }

            if (PVactuelsPet < 0)
            {
                PVactuelsPet = 0;
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

        async void Comp2()
        {
            bool Comp2 = await DisplayAlert("Stabilisation", "Vous gagnerez " + Force + " Armure pendant un tour et " + (Force * 2) + " points de bouclier pendant 2 tours.\n\nVous serez insensible aux effets de déplacement.\n\n- Si une unité se tient à votre corps à corps, vous pourrez la repousser d'une case.\n\n- Si vous chosissez de transférer vos points de bouclier à un allié : le CD de Stabilisation sera réduit de 1 en cas de destruction par un ennemi.", "Ok", "Annuler");

            switch (Comp2)
            {
                case true:

                    String Comp2Bouclier = await DisplayActionSheet ("Destinataire des points de bouclier","Annuler",null,"Vous","Un allié");

                    if (Comp2Bouclier == "Annuler" || Comp2Bouclier == null)
                    {
                        return;
                    }

                    else
                    {
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

                        AffichageInitial();
                        LabelInfo.FontSize = 15;
                        break;
                    }
                case false:

                    break;
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
            ArmureInitiale = ModifStats();
            LabelInfo.Text = "Votre armure passe à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonResistMagique_Click(object sender, EventArgs e)
        {
            ResistMagique = ModifStats();
            ResistMagiqueInitiale = ModifStats();
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


        private void ButtonPet_Click(object sender, EventArgs e)
        {
            if (PopUpDegatsSoinsPet.IsVisible == false)
            {
                ButtonPet.BackgroundColor = Color.White;
                ButtonPet.TextColor = Color.Maroon;
                PopUpDegatsSoinsPet.IsVisible = true;
                PopUpModifStats.IsVisible = false;
                PopUpModifArme.IsVisible = false;
            }
            else
            {
                ButtonPet.BackgroundColor = Color.Maroon;
                ButtonPet.TextColor = Color.White;
                PopUpDegatsSoinsPet.IsVisible = false;
            }
        }

        private void ButtonSubirDegatsPet_Click(object sender, EventArgs e)
        {
            ReceptionDegatsPet();
        }

        private void ButtonHealPet_Click(object sender, EventArgs e)
        {
            HealPet();
        }

        private void ButtonGainBouclierPet_Click(object sender, EventArgs e)
        {
            GainBouclierPet();
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
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            bool AutoAttack = await DisplayAlert("Auto-Attack", "Voulez-vous infliger jusqu'à " + degatsInfliges + " dégâts?", "Ok", "Annuler");

            switch (AutoAttack)
            {
                case true:

                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts";

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

                    AffichageInitial();

                    break;

                case false:

                    break;
            }
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)7 + Force / 2.0, MidpointRounding.AwayFromZero);
            int degatsInfligesCritique = (int)Math.Round((double)degatsInfliges + 3 + Force / 2.0, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
                degatsInfligesCritique = (int)Math.Round((double)degatsInfligesCritique * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
                degatsInfligesCritique = (int)Math.Round((double)degatsInfligesCritique * 0.9, MidpointRounding.AwayFromZero);
            }

            string Comp1EchecCritique = "- Echec critique : la cible ne devient pas vulnérable";
            string Comp1CoupNormal = "Vous chargerez une cible et arriverez à son corps à corps.\nDégâts = " + degatsInfliges + "\nArmure = +" + (int)Math.Round((double)Force / 3.0, MidpointRounding.AwayFromZero) + "\nRM = +" + (int)Math.Round((double)PVmax / 50.0, MidpointRounding.AwayFromZero) + "\nLa cible devient vulnérable et subit 25% de dégâts supplémentaires pendant un tour";
            string Comp1CoupCritique = "- Coup critique :\nDégats = " + degatsInfligesCritique + "\nLa cible est étourdie";


            string comp1 = await DisplayActionSheet("Charge héroïque","Annuler",null, Comp1CoupNormal, Comp1EchecCritique, Comp1CoupCritique);

            if (comp1 ==  "Annuler" || comp1 == null)
            {
                return;
            }
            else
            {
                if (comp1 == Comp1CoupNormal)
                {
                    LabelInfo.Text = "Vous chargez la cible et lui infligez " + degatsInfliges + " dégâts.\nVous gagnez " + (int)Math.Round((double)Force / 3.0, MidpointRounding.AwayFromZero) + " Armure et " + (int)Math.Round((double)PVmax / 50.0, MidpointRounding.AwayFromZero) + " RM pendant 1 tour.\nLa cible devient vulnérable et subit 25% de dégâts supplémentaires pendant 1 tour.";
                }
                if (comp1 == Comp1EchecCritique)
                {
                    LabelInfo.Text = "Vous chargez la cible et lui infligez " + degatsInfliges + " dégâts.\nVous gagnez " + (int)Math.Round((double)Force / 3.0, MidpointRounding.AwayFromZero) + " Armure et " + (int)Math.Round((double)PVmax / 50.0, MidpointRounding.AwayFromZero) + " RM pendant 1 tour.";
                }
                if (comp1 == Comp1CoupCritique)
                {
                    LabelInfo.Text = "Vous chargez la cible et lui infligez " + degatsInfliges + " dégâts.\nVous gagnez " + (int)Math.Round((double)Force / 3.0, MidpointRounding.AwayFromZero) + " Armure et " + (int)Math.Round((double)PVmax / 50.0, MidpointRounding.AwayFromZero) + " RM pendant 1 tour.\nLa cible est étourdie, devient vulnérable et subit 25% de dégâts supplémentaires pendant 1 tour.";
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


                AffichageInitial();
                LabelInfo.FontSize = 17;
            }

        }

        private void ButtonComp2_Click(object sender, EventArgs e)
        {
            Comp2();
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
                                Comp2();

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

            AffichageInitial();
        }

        private async void ButtonComp4_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)8 + Force * 0.66, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            bool comp4 = await DisplayAlert("Six pieds sous terre", "Voulez-vous enfoncer une cible dans le sol en lui infligeant jusqu'à " + degatsInfliges + " dégâts?\n\nPendant un tour, la cible sera piégée et ne pourra pas se déplacer ou être déplacée, se téléporter ni tacler.\n\n- Une utilisation par cible\n- Partiellement efficace contre les boss\n- Inutilisable sur les cibles trop imposantes", "Ok", "Annuler");

            switch (comp4)
            {

                case false:
                    break;

                case true:

                    LabelInfo.Text = "Vous enfoncez la cible dans le sol pendant un tour et lui infligez " + degatsInfliges + " dégâts.";

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

                    AffichageInitial();

                    break;
            }
        }
    }
}
