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

        // Validateur pour le champ "Prix" (valide l'entrée avec un ou deux chiffres après le point)
        private void PriceTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Permet un seul point, puis autorise des chiffres avant ou après
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9\.]");
        }

        // Validateur pour la quantité : uniquement des chiffres
        private void QuantityTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        // Valider la saisie complète du prix après modification
        private void PriceTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string text = (sender as System.Windows.Controls.TextBox).Text;

            // Permet uniquement un seul point dans la chaîne et vérifie que c'est une valeur valide
            if (!Regex.IsMatch(text, @"^\d+(\.\d{1,2})?$"))
            {
                // Si la saisie n'est pas valide, annule la modification (ou corrige selon votre logique)
                MessageBox.Show("Veuillez entrer un prix valide (ex : 100.50).");
            }
        }
    }
}
