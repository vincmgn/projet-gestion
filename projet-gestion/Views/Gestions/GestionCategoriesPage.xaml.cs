using System.Windows;
using System.Windows.Controls;
using projet_gestion.ViewModels;
using projet_gestion.Views.Dialogs;

namespace projet_gestion.Views.Gestions
{
    public partial class GestionCategoriesPage : Page
    {
        public GestionCategoriesPage()
        {
            InitializeComponent();
            DataContext = new GestionCategoriesViewModel();
        }
    }
}
