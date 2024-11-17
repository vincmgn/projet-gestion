using System;
using System.Windows;

namespace projet_gestion.Views.Dialogs
{
    public partial class AddClientDialog : Window
    {
        public string ClientName { get; private set; }
        public string ClientAddress { get; private set; }
        public string ClientSiret { get; private set; }

        public AddClientDialog()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les informations du client
            ClientName = ClientNameTextBox.Text;
            ClientAddress = ClientAddressTextBox.Text;
            ClientSiret = ClientSiretTextBox.Text;

            DialogResult = true; // Indiquer que l'ajout a été validé
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Annulation
            Close();
        }
    }
}
