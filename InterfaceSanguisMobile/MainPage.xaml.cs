using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InterfaceSanguisMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ------------- Définition des variables ------------- 
        int PVmax = 200;
        int PVactuels = 200;
        int Bouclier = 10;
        int Armure = 6;
        int ResistMagique = -3;
        int Intelligence = 10;
        int Force = 18;
        int Agilité = 18;
        int Initiative = 20;
        int Pm = 5;
        int degatsInfliges;
        int degatsBaseArmes = 12;
        int degatsTotauxArmes;
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
        }

        void heal()
        {
            string PVsoignes = EntryDegatsRecus.Text;
            int PVsoignesNum;

            if (int.TryParse(PVsoignes, out PVsoignesNum) == false)
            {
                LabelInfo.Text = "Ce n'est pas un nombre. Try again";
            }
            else
            {
                LabelInfo.Text = "Vous vous soignez de " + PVsoignesNum + " PV";
            }

            if ((PVactuels +PVsoignesNum) < PVmax)
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
                LabelInfo.Text = "Ce n'est pas un nombre. Try again";
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
                LabelInfo.Text = "Ce n'est pas un nombre. Try again";
            }
            else
            {
                LabelInfo.Text = "Vous subissez " + degatsRecusNum + " dégâts et vos PVmax sont diminués de " + degatsRecusNum/2;
            }

            if (Bouclier == 0)
            {
                PVactuels = PVactuels - degatsRecusNum;
                PVmax = PVmax - (degatsRecusNum / 2); // PV max réduit si 2PV perdu d'un coup ou tous les 2 PV perdus (ex : Si 2 attaques de 1 dégats, PVmax réduits ou pas ?)
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
                PVmax = PVmax - (degatsRecusNum / 2); // PV max réduit si 2PV perdu d'un coup ou tous les 2 PV perdus (ex : Si 2 attaques de 1 dégats, PVmax réduits ou pas ?)
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
                LabelInfo.Text = "Ce n'est pas un nombre. Try again";
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
            int varAgilite;

            if (SwitchForce.IsToggled == true)
            {
                string ArmeForce = EntryArmeForce.Text;
                int ArmeForceNum;
                if (int.TryParse(ArmeForce, out ArmeForceNum) == false)
                {
                    EntryArmeForce.Text = "E";
                }
                varForce = Force / ArmeForceNum ;
            }
            else
            {
                varForce = 0;
            }
            if (SwitchAgilite.IsToggled == true)
            {
                string ArmeAgilite = EntryArmeAgilite.Text;
                int ArmeAgiliteNum;
                if (int.TryParse(ArmeAgilite, out ArmeAgiliteNum) == false)
                {
                    EntryArmeAgilite.Text = "E";
                }


                varAgilite = Agilité / ArmeAgiliteNum;
            }
            else
            {
                varAgilite = 0;
            }

            degatsTotauxArmes = degatsBaseArmes + varForce + varAgilite; 

            return degatsTotauxArmes;
        }

        // ------------- Assignation des contrôles ------------- 
        private void Page_Appearing(object sender, EventArgs e)
        {
            affichageInitial();
        }



        private void ButtonModifStats_Click(object sender, EventArgs e)
        {
            if (PopUpModifStats.IsVisible == false)
            {
                PopUpModifStats.IsVisible = true;
                PopUpDegatsSoins.IsVisible = false;
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
            LabelInfo.Text = "Votre armure passe à " + modifStats();
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
            LabelInfo.Text = "Votre force passe à " + modifStats();
            affichageInitial();
        }

        private void ButtonAgilite_Click(object sender, EventArgs e)
        {
            Agilité = modifStats();
            LabelInfo.Text = "Votre agilité passe à " + modifStats();
            affichageInitial();
        }

        private void ButtonInitiative_Click(object sender, EventArgs e)
        {
            Initiative = modifStats();
            LabelInfo.Text = "Votre initiative passe à " + modifStats();
            affichageInitial();
        }

        private void ButtonPm_Click(object sender, EventArgs e)
        {
            Pm = modifStats();
            LabelInfo.Text = "Vos PM passent à " + modifStats();
            affichageInitial();
        }

        private void ButtonPVactuels_Click(object sender, EventArgs e)
        {
            PVactuels = modifStats();
            LabelInfo.Text = "Vos PV actuels passent à " + modifStats();
            affichageInitial();
        }

        private void ButtonPVmax_Click(object sender, EventArgs e)
        {
            PVmax = modifStats();
            LabelInfo.Text = "Vos PV maximum passent à " + modifStats();
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
            LabelInfo.Text = "Votre intelligence passe à " + modifStats();
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



        private void ButtonDegatsSoins_Click(object sender, EventArgs e)
        {
            if (PopUpDegatsSoins.IsVisible == false)
            {
                PopUpDegatsSoins.IsVisible = true;
                PopUpModifStats.IsVisible = false;
            }
            else
            {
                PopUpDegatsSoins.IsVisible = false;
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



        private async void ButtonAutoAttack_Click(object sender, EventArgs e)
        {
            degatsInfliges = defDegatsTotauxArmes();

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

            LabelInfo.Text = "Vous infligez " + DegatsReelsAutoAttackNum + " dégâts et vous soignez de " + DegatsReelsAutoAttackNum / 2 + "PV";

            if ((PVactuels + DegatsReelsAutoAttackNum / 2) < PVmax)
            {
                PVactuels = PVactuels + DegatsReelsAutoAttackNum / 2;
            }
            else
            {
                PVactuels = PVmax;
            }
            affichageInitial();

            PopUpDegatsReelsAutoAttack.IsVisible = false;

            return;
        }
        private void ButtonAnnulerDegatsAutoAttack_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsAutoAttack.IsVisible = false;
        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp1.IsVisible = true;

            var actionSheet = await DisplayActionSheet("","Annuler",null,"Echec critique","Coup normal", "Coup critique");

            switch (actionSheet)
            {
                case "Annuler":

                    break;

                case "Coup normal":

                    degatsInfliges = 3 + 3 * (Force / 2) + 3 * (Agilité / 2);


                    bool Comp1a = await DisplayAlert("Cela ne t'est pas si vital", "Voulez-vous infliger " + degatsInfliges + " dégâts et vous soigner de " + degatsInfliges + "PV?\nLa cible sera terrifiée.", "Oui", "Non");

                    switch (Comp1a)
                    {
                        case true:

                            LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + degatsInfliges + "PV.\nLa cible est terrifiée."; // PV soignés = basés  sur dégats théoriques ou effectifs ?
                            if ((PVactuels + degatsInfliges) < PVmax)
                            {
                                PVactuels = PVactuels + degatsInfliges;
                            }
                            else
                            {
                                PVactuels = PVmax;
                            }

                            CdComp1 = 2;

                            break;

                        case false:

                            break;
                    }

                    break;

                case "Echec critique":

                    degatsInfliges = 3 + 3 * (Force / 2) + 3 * (Agilité / 2);


                    bool Comp1b = await DisplayAlert("Cela ne t'est pas si vital", "Voulez-vous infliger " + degatsInfliges + " dégâts sans vous soigner?\nLa cible sera terrifiée.", "Oui", "Non");

                    switch (Comp1b)
                    {
                        case true:

                            LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts mais vous ne vous soignez pas.\nLa cible est terrifiée.";

                            CdComp1 = 2;

                            break;

                        case false:

                            break;
                    }

                    break;

                case "Coup critique":

                    degatsInfliges = 3 + 3 * (Force / 2) + 3 * (Agilité / 2);

                    bool Comp1c = await DisplayAlert("Cela ne t'est pas si vital", "Voulez-vous infliger " + degatsInfliges + " dégâts et vous soigner de " + degatsInfliges + "PV?\nVos PVmax augmenteront de " + degatsInfliges + ".\nLa cible sera terrifiée.", "Oui", "Non");

                    switch (Comp1c)
                    {
                        case true:

                            LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + degatsInfliges + "PV.\nVos PVmax augmentent de " + degatsInfliges + ".\nLa cible est terrifiée."; // PV soignés = basés  sur dégats théoriques ou effectifs ?
                            PVmax = PVmax + degatsInfliges;
                            if ((PVactuels + degatsInfliges) < PVmax)
                            {
                                PVactuels = PVactuels + degatsInfliges;
                            }
                            else
                            {
                                PVactuels = PVmax;
                            }

                            CdComp1 = 2;

                            break;

                        case false:

                            break;
                    }

                    break;

            }
            affichageInitial();

        }
        private void ButtonValiderDegatsComp1_Click(object sender, EventArgs e)
        {

        }
        private void ButtonAnnulerDegatsComp1_Click(object sender, EventArgs e)
        {
            PopUpDegatsReelsComp1.IsVisible = false;
        }

        private async void ButtonComp2_Click(object sender, EventArgs e)
        {
            degatsInfliges = 1 + Force / 3 + Agilité / 3;
            bool Comp2 = await DisplayAlert("Pluie de sang", "Voulez-vous infliger " + degatsInfliges + " dégâts à tous les ennemis et vous soigner de " + (PVmax - PVactuels) + "PV?", "Oui", "Non");

            switch (Comp2)
            {
                case true:

                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts à tous les ennemis et vous soignez de " + (PVmax - PVactuels) + "PV."; // PV soignés = basés  sur dégats théoriques ou effectifs ?
                    PVactuels = PVmax;
                    CdComp2 = 6;
                    affichageInitial();

                    break;

                case false:

                    break;
            }
        }

        private async void ButtonComp3_Click(object sender, EventArgs e)
        {
            var actionSheet = await DisplayActionSheet("","Annuler",null,"Echec critique","Coup normal");
            int PVsacrifiés;

            switch (actionSheet)
            {
                case "Annuler":

                    break;

                case "Coup normal":

                    PVsacrifiés = 20 * PVactuels / 100;
                    degatsInfliges = defDegatsTotauxArmes() + defDegatsTotauxArmes() /2;

                    if (PVactuels <= PVmax / 4)
                    {
                        bool Comp3aa = await DisplayAlert("Sous l'armure il y a une victime", "Voulez-vous ne pas sacrifier de PV et infliger " + degatsInfliges + " dégâts?\nVous gagnerez un bouclier de " + (degatsInfliges / 2) + ".", "Oui", "Non");

                        switch (Comp3aa)
                        {
                            case true:

                                LabelInfo.Text = "Vous ne sacrifiez pas de PV et infligez " + degatsInfliges + " dégâts.\nVous gagnez un bouclier de " + (degatsInfliges / 2) + ".";
                                Bouclier = Bouclier + degatsInfliges / 2;

                                if (CdComp2 > 0 && CdComp2 <= 4)
                                {
                                    CdComp2 -= 1;
                                }

                                break;

                            case false:

                                break;
                        }
                    }

                    else
                    {


                        bool Comp3ab = await DisplayAlert("Sous l'armure il y a une victime", "Voulez-vous sacrifier " + PVsacrifiés + "PV et infliger " + degatsInfliges + " dégâts?", "Oui", "Non");

                        switch (Comp3ab)
                        {
                            case true:

                                LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV et infligez " + degatsInfliges + " dégâts.";
                                PVactuels = PVactuels - PVsacrifiés;

                                if (CdComp2 > 0 && CdComp2 <= 4)
                                {
                                    CdComp2 -= 1;
                                }

                                break;

                            case false:

                                break;
                        }

                    }

                    break;

                case "Echec critique":

                    PVsacrifiés = 20 * PVactuels / 100;
                    degatsInfliges = defDegatsTotauxArmes() + defDegatsTotauxArmes() / 2;

                    bool Comp3b = await DisplayAlert("Sous l'armure il y a une victime", "Voulez-vous sacrifier " + PVsacrifiés + "PV mais la compétence échouera et vous n'infligerez pas de dégâts?", "Oui", "Non");

                    switch (Comp3b)
                    {
                        case true:

                            LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV mais la compétence échoue et vous n'infligez pas de dégâts.";
                            PVactuels = PVactuels - PVsacrifiés;

                            if (CdComp2 > 0 && CdComp2 <= 4)
                            {
                                CdComp2 -= 1;
                            }

                            break;

                        case false:

                            break;
                    }

                    break;
            }
            affichageInitial();
        }

        private async void ButtonFinTour_Click(object sender, EventArgs e)
        {
            bool FinTour = await DisplayAlert("Fin de tour", "Voulez vous vraiment mettre fin à votre tour ?", "Oui", "Non");

            switch (FinTour)
            {
                case true :

                    if (CdComp1 > 0)
                    {
                        CdComp1 -= 1;
                    }

                    if (CdComp2 > 0)
                    {
                        CdComp2 -= 1;
                    }
                    affichageInitial();
                    LabelInfo.Text = "Fin de tour";

                    break;

                case false :

                    break;
            }


        }

    }
}

