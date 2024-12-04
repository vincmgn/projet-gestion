using projet_gestion.ViewModels.Dialogs;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace projet_gestion.Views.Dialogs
{
    public partial class AddOrderDialog : Window
    {
        public AddOrderDialog()
        {
            InitializeComponent();
            DataContext = new AddOrderDialogViewModel();
        }

        // Gestionnaire d'événements pour SelectionChanged
        private void OnStatusSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                // Récupérer uniquement le contenu texte de l'élément sélectionné (pas l'objet ComboBoxItem)
                var selectedStatus = selectedItem.Content.ToString();

                // Mettre à jour la propriété OrderStatus dans le ViewModel
                var viewModel = DataContext as AddOrderDialogViewModel;
                if (viewModel != null)
                {
                    viewModel.OrderStatus = selectedStatus;
                }
            }
        }

    }
}
