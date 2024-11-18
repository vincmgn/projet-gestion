using projet_gestion.ViewModels.Dialogs;
using System.Windows;


namespace projet_gestion.Views.Dialogs
{
    public partial class AddProductDialog : Window
    {
        public AddProductDialog()
        {
            InitializeComponent();
            DataContext = new AddProductDialogViewModel();
        }

        private void PriceTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Autorise uniquement les chiffres et un seul point
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[0-9.]$");
        }
    }
}
