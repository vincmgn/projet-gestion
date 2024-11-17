using System.Windows.Controls;
using projet_gestion.ViewModels;

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
