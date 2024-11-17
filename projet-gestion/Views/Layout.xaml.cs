using projet_gestion.Views.Gestions;
using System.Windows;
using System.Windows.Controls;

namespace projet_gestion.Views
{
    public partial class Layout : Window
    {
        public Layout()
        {
            InitializeComponent();
            ContentFrame.Navigate(new Dashboard());
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pageName = button?.Tag.ToString();

            switch (pageName)
            {
                case "Dashboard":
                    ContentFrame.Navigate(new Dashboard());
                    break;

                case "Products":
                    ContentFrame.Navigate(new GestionProductPage());
                    break;

                case "Categories":
                    ContentFrame.Navigate(new GestionCategoriesPage());
                    break;

                case "Clients":
                    ContentFrame.Navigate(new GestionClientsPage());
                    break;

                case "Orders":
                    ContentFrame.Navigate(new GestionOrdersPage());
                    break;

                case "Deconnection":
                    HandleLogout();
                    break;

                default:
                    break;
            }
        }

        private void HandleLogout()
        {
            MessageBox.Show("Déconnexion réussie !");
            this.Close();
        }
    }
}
