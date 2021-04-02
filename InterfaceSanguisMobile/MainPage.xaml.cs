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
        int Armure = 10;
        int ResistMagique = 10;
        int Force = 16;
        int Agilité = 16;
        int Initiative = 20;
        int Pm = 5;
        int degatsInfliges;
        int degatsArmes;

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
            LabelResistMagique.Text = "Résistance Magique : " + ResistMagique;

            LabelForce.Text = "Force : " + Force;
            LabelAgilite.Text = "Agilité : " + Agilité;
            LabelInitiative.Text = "Initiative : " + Initiative;
            LabelPm.Text = "PM : " + Pm;
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

        void autoAttack()
        {
            degatsArmes = 1 + Force + Agilité;
            degatsInfliges = degatsArmes;
            LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + degatsInfliges/2 + "PV"; // PV soignés = basés  sur dégats théoriques ou effectifs ?
            
            if ((PVactuels + degatsInfliges / 2) < PVmax)
            {
                PVactuels = PVactuels + degatsInfliges / 2;
            }
            else
            {
                PVactuels = PVmax;
            }
            affichageInitial();
            return;
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



        private void ButtonAutoAttack_Click(object sender, EventArgs e)
        {
            autoAttack();
        }

 //       private void ButtonComp1_Click(object sender, EventArgs e)
 //       {

 //           affichageInitial();
//        }

        private async void ButtonComp1_Click(object sender, EventArgs e)
        {
            var actionSheet = await DisplayActionSheet("","Annuler",null,"Echec critique","Coup normal", "Coup critique");

            switch (actionSheet)
            {
                case "Annuler":

                    break;

                case "Coup normal":

                    degatsInfliges = 3 + 3 * (Force / 2) + 3 * (Agilité / 2);
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + degatsInfliges + "PV.\nLa cible est terrifiée."; // PV soignés = basés  sur dégats théoriques ou effectifs ?
                    if ((PVactuels + degatsInfliges) < PVmax)
                    {
                        PVactuels = PVactuels + degatsInfliges;
                    }
                    else
                    {
                        PVactuels = PVmax;
                    }

                    break;

                case "Echec critique":

                    degatsInfliges = 3 + 3 * (Force / 2) + 3 * (Agilité / 2);
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts mais vous ne vous soignez pas.\nLa cible est terrifiée.";

                    break;

                case "Coup critique":

                    degatsInfliges = 3 + 3 * (Force / 2) + 3 * (Agilité / 2);
                    LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts et vous soignez de " + degatsInfliges + "PV.\nVos PVmax augmentent de " + degatsInfliges + "\nLa cible est terrifiée."; // PV soignés = basés  sur dégats théoriques ou effectifs ?
                    PVmax = PVmax + degatsInfliges;
                    if ((PVactuels + degatsInfliges) < PVmax)
                    {
                        PVactuels = PVactuels + degatsInfliges;
                    }
                    else
                    {
                        PVactuels = PVmax;
                    }

                    break;

            }
            affichageInitial();

        }

        private void ButtonComp2_Click(object sender, EventArgs e)
        {
            degatsInfliges = 1 + Force / 3 + Agilité / 3;
            LabelInfo.Text = "Vous infligez " + degatsInfliges + " dégâts à tous les ennemis et vous soignez de " + (PVmax - PVactuels) + "PV."; // PV soignés = basés  sur dégats théoriques ou effectifs ?
            PVactuels = PVmax;
            affichageInitial();
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
                    degatsArmes = 1 + Force + Agilité;
                    degatsInfliges = degatsArmes + degatsArmes / 2;

                    if (PVactuels <= PVmax / 4)
                    {
                        LabelInfo.Text = "Vous ne sacrifiez pas de PV et infligez " + degatsInfliges + " dégâts.\nVous gagnez un bouclier de " + (degatsInfliges / 2);
                        Bouclier = Bouclier + degatsInfliges / 2;
                    }

                    else
                    {
                        LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV et infligez " + degatsInfliges + " dégâts.";
                        PVactuels = PVactuels - PVsacrifiés;
                    }
                    break;

                case "Echec critique":

                    PVsacrifiés = 20 * PVactuels / 100;
                    degatsArmes = 1 + Force + Agilité;
                    degatsInfliges = degatsArmes + degatsArmes / 2;
                    LabelInfo.Text = "Vous sacrifiez " + PVsacrifiés + "PV mais la compétence échoue et vous n'infligez pas de dégâts.";
                    PVactuels = PVactuels - PVsacrifiés;


                    break;
            }
            affichageInitial();
        }
    }
}

