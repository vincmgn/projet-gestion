using System;
using System.Windows;

namespace projet_gestion.Views.Dialogs
{
    public partial class AddOrderDialog : Window
    {
        public AddOrderDialog()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Code pour ajouter la commande (à implémenter selon la logique de votre application)

            // Affichage d'un message temporaire
            MessageBox.Show("Commande ajoutée avec succès !");
            this.DialogResult = true; // Ferme la boîte de dialogue avec succès
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Ferme la boîte de dialogue sans effectuer de modifications
        }
    }
}
