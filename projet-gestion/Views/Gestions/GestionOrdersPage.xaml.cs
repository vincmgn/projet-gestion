using projet_gestion.Views.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace projet_gestion.Views.Gestions
{
    public partial class GestionOrdersPage : Page
    {
        public GestionOrdersPage()
        {
            InitializeComponent();
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var addOrderDialog = new AddOrderDialog()
            {
                Owner = Window.GetWindow(this)
            };

            if (addOrderDialog.ShowDialog() == true)
            {
                // Code pour rafraîchir les données si la commande est ajoutée
            }
        }
    }
}
