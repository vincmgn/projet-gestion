using projet_gestion.ViewModels.Dialogs;
using System.Windows;
using System.Text.RegularExpressions;

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
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9\,\.]");
        }

        private void QuantityTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        private void PriceTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string text = (sender as System.Windows.Controls.TextBox).Text;

            if (!Regex.IsMatch(text, @"^\d+([\,\.]\d{1,2})?$"))
            {
                MessageBox.Show("Veuillez entrer un prix valide (ex : 100,50 ou 100.50).");
            }
        }
    }
}
