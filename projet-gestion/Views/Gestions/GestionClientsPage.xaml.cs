using projet_gestion.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projet_gestion.Views.Gestions
{
    /// <summary>
    /// Logique d'interaction pour GestionClientsPage.xaml
    /// </summary>
    public partial class GestionClientsPage : Page
    {
        public GestionClientsPage()
        {
            InitializeComponent();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            // Crée et affiche la boîte de dialogue pour ajouter un client
            var addClientDialog = new AddClientDialog
            {
                Owner = Window.GetWindow(this) // Définit le propriétaire de la fenêtre modale
            };

            if (addClientDialog.ShowDialog() == true) // Si l'utilisateur a validé l'ajout
            {
                string clientName = addClientDialog.ClientName;
                string clientAddress = addClientDialog.ClientAddress;
                string clientSiret = addClientDialog.ClientSiret;

                // Ajouter ici la logique pour insérer le client dans la base de données
                MessageBox.Show($"Client '{clientName}' ajouté avec succès !", "Succès");
            }
        }
    }
}
