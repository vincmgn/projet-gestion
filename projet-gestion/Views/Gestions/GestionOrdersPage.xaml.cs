using projet_gestion.ViewModels;
using System.Windows.Controls;

namespace projet_gestion.Views.Gestions
{
    public partial class GestionOrdersPage : Page
    {
        public GestionOrdersPage()
        {
            InitializeComponent();
            DataContext = new GestionOrdersViewModel();
        }
    }
}
