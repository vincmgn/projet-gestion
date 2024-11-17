using System;
using System.Windows;

namespace projet_gestion.Views.Dialogs
{
    public partial class AddClientDialog : Window
    {
        public AddClientDialog()
        {
            InitializeComponent();
            DataContext = new AddClientDialogViewModel();
        }
    }
}
