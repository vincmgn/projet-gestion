using System.Windows;
using System.Windows.Controls;
using projet_gestion.ViewModels;

namespace projet_gestion
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Event handler for PasswordChanged to update the Password in the ViewModel
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                // Update the ViewModel's Password property
                var viewModel = this.DataContext as LoginViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = passwordBox.Password;
                }
            }
        }
    }
}
