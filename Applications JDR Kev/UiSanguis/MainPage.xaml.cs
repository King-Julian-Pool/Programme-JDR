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
        int Intelligence = 2;
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
            LabelPv.Text = "PV : " + PVactuels + "/" + PVmax + "(" + PVmaxInitiaux + ")";
            BarPv(ProgressBarPv);
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

            ButtonComp2.BackgroundColor = Color.Default;
            if (CdComp2 != 0)
            {
                ButtonComp2.IsEnabled = false;
                LabelCdComp2.BackgroundColor = Color.Red;
                if (CdComp2 <= 4)
                {
                    ButtonComp2.BackgroundColor = Color.Red;
                }
            }
            else
            {
                ButtonComp2.IsEnabled = true;
                LabelCdComp2.BackgroundColor = Color.Default;
                LabelCdComp2.Text = "";
            }


            if (DegatsAugmentes() == true)
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
            int PVmaxPrecedents = PVmax;

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
                PVmax = PVmaxInitiaux - (PVperdus / 2);
                LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts et vos PVmax sont diminués de " + (PVmaxPrecedents - PVmax);

            }
            else
            {
                PVmax = PVmaxInitiaux / 2;
                LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts et vos PVmax sont diminués de " + (PVmaxPrecedents - PVmax);

                PVperdus += (PVmax - (PVmaxInitiaux / 2));
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

        bool DegatsAugmentes()
        {
            if (PVactuels < (int)Math.Round((double)PVmax / 2.0, MidpointRounding.AwayFromZero))
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

            PVperdus = (PVmaxInitiaux - PVmax) * 2; // problème ! (si PVmax prend valeur 200 --> PVperdus = 200....

            AffichageInitial();
        }

        private void ButtonPVmaxInitiaux_Click(object sender, EventArgs e)
        {
            PVmaxInitiaux = ModifStats();
            LabelInfo.Text = "Vos PV maximum initiaux passent à " + ModifStats() + "\n";
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

            if (DegatsAugmentes() == true)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.25, MidpointRounding.AwayFromZero);
            }

            int degatsSoignés = (int)Math.Round((double)degatsInfliges / 2.0, MidpointRounding.AwayFromZero);

            bool AutoAttack = await DisplayAlert("Auto-Attack", "Voulez-vous infliger jusqu'à " + degatsInfliges + " dégâts et vous soigner jusqu'à " + degatsInfliges / 2 + "PV?", "Oui", "Non");

            switch (AutoAttack)
            {
                case true:

                    if ((int)Math.Round((double)(PVactuels + degatsInfliges / 2.0), MidpointRounding.AwayFromZero) < PVmax)
                    {
                        LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + degatsSoignés + "PV";
                        PVactuels += degatsSoignés;
                    }
                    else
                    {
                        LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + (PVmax - PVactuels) + "PV";
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

                    AffichageInitial();

                    break;

                case false:

                    break;
            }
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)3 + Force * 1.5 + Agilité * 1.5, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            if (DegatsAugmentes() == true)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.25, MidpointRounding.AwayFromZero);
            }

            string Comp1EchecCritique = "Echec critique :\n- Dégâts = " + degatsInfliges + "\n- Soin = 0\n- Cible terrifiée";
            string Comp1CoupNormal = "Coup normal:\n - Dégâts = " + degatsInfliges + "\n - Soin = " + degatsInfliges + "\n - Cible terrifiée";
            string Comp1CoupCritique = "Coup critique :\n- Dégâts = " + degatsInfliges + "\n- Soin = " + degatsInfliges + "\n- PVmax = +" + degatsInfliges + "\n- Cible terrifiée";


            string comp1 = await DisplayActionSheet("Cela ne t'est pas si vital","Annuler",null,Comp1EchecCritique,Comp1CoupNormal,Comp1CoupCritique);

            if (comp1 == "Annuler" || comp1 == null)
            {
                return;
            }
            else
            {
                if (comp1 == Comp1EchecCritique)
                {
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts mais vous ne vous soignez pas.\nLa cible est terrifiée.";
                }
                else if (comp1 == Comp1CoupNormal)
                {
                    if (PVactuels + degatsInfliges < PVmax)
                    {
                        PVactuels += degatsInfliges;
                        LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + degatsInfliges + "PV.\nLa cible est terrifiée.";
                    }
                    else
                    {
                        LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + (PVmax - PVactuels) + "PV.\nLa cible est terrifiée.";
                        PVactuels = PVmax;
                    }
                }
                else if (comp1 == Comp1CoupCritique)
                {
                    PVmax += degatsInfliges;

                    if (PVactuels + degatsInfliges < PVmax)
                    {
                        PVactuels += degatsInfliges;
                        LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + degatsInfliges + "PV.\nVos PVmax augmentent de " + degatsInfliges + ".\nLa cible est terrifiée.";
                    }
                    else
                    {
                        LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + (PVmax - PVactuels) + "PV.\nVos PVmax augmentent de " + degatsInfliges + ".\nLa cible est terrifiée.";
                        PVactuels = PVmax;
                    }
                }

                CdComp1 = 2;

                if (CdComp2 > 0)
                {
                    CdComp2 -= 1;
                }
                AffichageInitial();
            }

        }

        private async void ButtonComp2_Click(object sender, EventArgs e)
        {
            degatsInfliges = (int)Math.Round((double)1 + Force / 3.0 + Agilité / 3.0, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }


            if (DegatsAugmentes() == true)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.25, MidpointRounding.AwayFromZero);
            }

            bool Comp2 = await DisplayAlert("Pluie de sang", "Voulez-vous infliger jusqu'à " + degatsInfliges + " dégâts à tous les ennemis et vous soigner de " + (PVmax - PVactuels) + "PV?", "Oui", "Non");

            switch (Comp2)
            {
                case true:

                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts à tous les ennemis et vous soignez de " + (PVmax - PVactuels) + "PV.";
                    PVactuels = PVmax;
                    CdComp2 = 6;

                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }

                    AffichageInitial();

                    break;

                case false:

                    break;
            }
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            PVsacrifiés = (int)Math.Round((double)PVactuels * 0.2, MidpointRounding.AwayFromZero);
            degatsInfliges = (int)Math.Round((double)DefDegatsTotauxArmes() * 1.5, MidpointRounding.AwayFromZero);

            if (Beni() == true && Maudit() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.1, MidpointRounding.AwayFromZero);
            }

            else if (Maudit() == true && Beni() == false)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 0.9, MidpointRounding.AwayFromZero);
            }

            if (DegatsAugmentes() == true)
            {
                degatsInfliges = (int)Math.Round((double)degatsInfliges * 1.25, MidpointRounding.AwayFromZero);
            }

            string Comp3EchecCritique = "Echec critique :\n- PVsacrifiés = " + PVsacrifiés + "\n- Dégâts = 0";
            string Comp3CoupNormal = "Coup normal :\n- PVsacrifiés = " + PVsacrifiés + "\n- Dégâts = " + degatsInfliges + "\n\nSi PV< 20% des PV max :\n- PV soignés = PV sacrifiés\n- Bouclier = +" + (int)Math.Round((double)degatsInfliges / 2.0, MidpointRounding.AwayFromZero);

            string comp3 = await DisplayActionSheet("Sous l'armure il y a une victime", "Annuler", null, Comp3EchecCritique, Comp3CoupNormal);

            if (comp3 == "Annuler" || comp3 == null)
            {
                return;
            }
            else
            {
                if (comp3 == Comp3EchecCritique)
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


                    PVactuels -= PVsacrifiés;

                    if (CdComp2 > 0 && CdComp2 <= 4)
                    {
                        CdComp2 -= 1;
                    }
                }

                else if (comp3 == Comp3CoupNormal)
                {
                    if (PVactuels <= (int)Math.Round((double)PVmax * 0.25, MidpointRounding.AwayFromZero))
                    {


                        if ((PVmax + PVsacrifiés) < PVmaxInitiaux)
                        {
                            LabelInfo.Text = "Vous ne sacrifiez pas de PV et infligez " + degatsInfliges + " dégâts.\nVous gagnez " + PVsacrifiés + " PVmax et un bouclier de " + (int)Math.Round((double)degatsInfliges / 2.0, MidpointRounding.AwayFromZero)+ ".";
                            PVmax += PVsacrifiés;
                            PVperdus -= PVsacrifiés * 2;
                        }
                        else
                        {
                            LabelInfo.Text = "Vous ne sacrifiez pas de PV et infligez " + degatsInfliges + " dégâts.\nVous gagnez " + (PVmaxInitiaux - PVmax) + " PVmax et un bouclier de " + (int)Math.Round((double)degatsInfliges / 2.0, MidpointRounding.AwayFromZero) + ".";
                            PVmax = PVmaxInitiaux;
                            PVperdus = 0;
                        }


                        Bouclier += (int)Math.Round((double)degatsInfliges / 2.0, MidpointRounding.AwayFromZero);

                        if (CdComp2 > 0 && CdComp2 <= 4)
                        {
                            CdComp2 -= 1;
                        }
                    }

                    else
                    {

                        if ((PVmax + PVsacrifiés) < PVmaxInitiaux)
                        {
                            LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV et infligez " + degatsInfliges + " dégâts. Vous gagnez " + PVsacrifiés + " PVmax";
                            PVmax += PVsacrifiés;
                            PVperdus -= PVsacrifiés * 2;
                        }
                        else
                        {
                            LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV et infligez " + degatsInfliges + " dégâts. Vous gagnez " + (PVmaxInitiaux - PVmax) + " PVmax";
                            PVmax = PVmaxInitiaux;
                            PVperdus = 0;
                        }


                        PVactuels -= PVsacrifiés;

                        if (CdComp2 > 0 && CdComp2 <= 4)
                        {
                            CdComp2 -= 1;
                        }

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
            AffichageInitial();
        }
    }
}

