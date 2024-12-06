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
        private void OnStatusSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedStatus = selectedItem.Content.ToString();

                var viewModel = DataContext as AddOrderDialogViewModel;
                if (viewModel != null)
                {
                    viewModel.OrderStatus = selectedStatus;
                }
            }
        }

    }
}
