using System;
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
        int Force = 2;
        int Agilité = 15;
        int Initiative = 29;
        int Pm = 3;
        int degatsInfliges;
        int degatsBaseArmes = 7;
        int degatsTotauxArmes;
        int degatsRuneA;
        int degatsRuneB;
        int CdComp1;
        int CdComp2;
        int CdComp3;
        int multDegats;
        int degatsStockes;

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
                if (CdComp2 == 4)
                {
                    degatsStockes += degatsRecusNum;
                    LabelInfo.Text = "Vous ne subissez aucun dégâts et stockez " + degatsRecusNum + " dégâts";
                }
                else
                {
                    int degatsReduits = (int)Math.Round((double)degatsRecusNum * 0.1, MidpointRounding.AwayFromZero);
                    degatsStockes += degatsReduits;
                    degatsRecusNum -= (int)Math.Round((double)degatsReduits, MidpointRounding.AwayFromZero);
                    LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts et stockez " + degatsReduits + " dégats";
                }
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

        private void ButtonDegatsStockes_Click(object sender, EventArgs e)
        {
            degatsStockes = ModifStats();
            LabelInfo.Text = "Vos dégâts stockés passent à " + ModifStats() + "\n";
            AffichageInitial();
        }

        private void ButtonDegatsAugmentes_Click(object sender, EventArgs e)
        {
            multDegats = ModifStats();
            LabelInfo.Text = "Vos dégâts augmentés passent à " + ModifStats() + "%\n";
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

            if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            degatsInfliges += (int)Math.Round((double)multDegats * degatsInfliges / 100.0, MidpointRounding.AwayFromZero);
            degatsInfliges += (int)Math.Round((double)degatsStockes, MidpointRounding.AwayFromZero);

            string AutoAttack = await DisplayActionSheet("Auto-Attack","Annuler",null,"- Echec critique :\n- Dégâts = 0", "- Coup normal :\n- Dégâts = " + degatsInfliges,"- Coup critique :\n- Dégâts = " + degatsInfliges + "\n- Vous pouvez rejouer un tour");
            if (AutoAttack == "Annuler" || AutoAttack == null)
            {
                return;
            }
            else
            {
                if (AutoAttack == "- Echec critique :\n- Dégâts = 0")
                {
                    LabelInfo.Text = "L'attaque échoue, vous n'infligez pas de dégâts";
                }

                if (AutoAttack == "- Coup normal :\n- Dégâts = " + degatsInfliges)
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts";
                    multDegats = 0;
                    degatsStockes = 0;
                }

                if (AutoAttack == "- Coup critique :\n- Dégâts = " + degatsInfliges + "\n- Vous pouvez rejouer un tour")
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et pouver jouer immédiatement un second tour";
                    multDegats = 0;
                    degatsStockes = 0;
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

                AffichageInitial();
            }
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsRuneA = (int)Math.Round((double)4 + (Intelligence * 0.5) + (Agilité * 0.5), MidpointRounding.AwayFromZero);

            degatsRuneB = (int)Math.Round((double)8 + 2 * Intelligence + 2 * Agilité, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsRuneA = (int)Math.Round((double)degatsRuneA * 1.1, MidpointRounding.AwayFromZero);
                degatsRuneB = (int)Math.Round((double)degatsRuneB * 1.1, MidpointRounding.AwayFromZero);
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsRuneA = (int)Math.Round((double)degatsRuneA * 0.9, MidpointRounding.AwayFromZero);
                degatsRuneB = (int)Math.Round((double)degatsRuneB * 0.9, MidpointRounding.AwayFromZero);
            }

            degatsRuneA += (int)Math.Round((double)multDegats * degatsRuneA / 100.0, MidpointRounding.AwayFromZero);
            degatsRuneB += (int)Math.Round((double)multDegats * degatsRuneB / 100.0, MidpointRounding.AwayFromZero);
            degatsRuneA += (int)Math.Round((double)degatsStockes, MidpointRounding.AwayFromZero);
            degatsRuneB += (int)Math.Round((double)degatsStockes, MidpointRounding.AwayFromZero);

            string comp1 = await DisplayActionSheet("Annihilation runique", "Annuler",null, "- Echec critique :\nDégâts = " + degatsRuneA + " à votre cible et à vous-même","- RuneA :\nDégâts = " + degatsRuneA, "- Rune B :\nDégâts = " + degatsRuneB, "- Coup critique :\nDégâts = " + (degatsRuneA + degatsRuneB - degatsStockes));
            if (comp1 == "Annuler" || comp1 == null)
            {
                return;
            }
            else
            {
                if (comp1 == "- Echec critique :\nDégâts = " + degatsRuneA + " à votre cible et à vous-même")
                {
                    LabelInfo.Text = "Votre rune A inflige " + degatsRuneA + " dégâts à votre cible et à vous même";
                }

                else if(comp1 == "- RuneA :\nDégâts = " + degatsRuneA)
                {
                    LabelInfo.Text = "Votre rune A inflige " + degatsRuneA + " dégâts";
                }

                else if(comp1== "- Rune B :\nDégâts = " + degatsRuneB)
                {
                    LabelInfo.Text = "Votre rune B inflige " + degatsRuneB + " dégâts";
                }

                else if(comp1 == "- Coup critique :\nDégâts = " + (degatsRuneA + degatsRuneB - degatsStockes))
                {
                    LabelInfo.Text = "Vos runes A et B infligent " + (degatsRuneA + degatsRuneB - degatsStockes) + " dégâts";
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

                multDegats = 0;
                degatsStockes = 0;

                AffichageInitial();
            }
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
                    AffichageInitial();
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
                    AffichageInitial();
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
                    AffichageInitial();
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
                        AffichageInitial();
                        LabelInfo.FontSize = 14;
                    }
                    break;
            }
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)2 + (Intelligence / 3.0) + (Agilité / 3.0), MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            degatsInfliges += (int)Math.Round((double)multDegats * degatsInfliges / 100.0, MidpointRounding.AwayFromZero);
            degatsInfliges += (int)Math.Round((double)degatsStockes, MidpointRounding.AwayFromZero);

            string Comp3EchecCritique1 = "Echec critique :\n - Dégâts = 0\n - Vous pouvez:\n->dévier la prochaine attaque que\n vous recevez vers un allié au\n hasard \n\n Ou\n";
            string Comp3EchecCritique2 = "\n->réinitialiser le temps de recharge\n de la compétence";
            string Comp3CoupCritique = "\nCoup critique :\n- Chaque rune réussie inflige " + degatsInfliges + "\n  dégâts";

            string comp3 = await DisplayActionSheet("Rafale runique","Annuler",null, Comp3EchecCritique1, Comp3EchecCritique2, Comp3CoupCritique);

            if (comp3 == "Annuler" || comp3 == null)
            {
                return;
            }
            else
            {
                if (comp3 == Comp3EchecCritique1)
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
                }
                else if (comp3 == Comp3EchecCritique2)
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
                }
                else if (comp3 == Comp3CoupCritique)
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

                    LabelInfo.Text = "Vos runes infligent " + degatsInfliges + " dégâts.";
                    multDegats = 0;
                    degatsStockes = 0;
                }

                AffichageInitial();
            }
        }
    }
}

