using projet_gestion.ModelViews;
using System.Windows.Controls;

namespace projet_gestion.Views.Gestions
{
    public partial class GestionProductPage : Page
    {
        public GestionProductPage()
        {
            InitializeComponent();
            DataContext = new GestionProductsViewModel();
        }
    }
}
