using projet_gestion.ViewModels.Dialogs;
using System.Windows;

namespace projet_gestion.Views.Dialogs
{
    public partial class AddCategoryDialog : Window
    {
        public AddCategoryDialog()
        {
            InitializeComponent();
            DataContext = new AddCategoryDialogViewModel();
        }
    }
}
