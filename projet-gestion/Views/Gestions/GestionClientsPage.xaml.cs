using projet_gestion.ViewModels;
using System.Windows.Controls;

namespace projet_gestion.Views.Gestions 
{ 
    public partial class GestionClientsPage : Page
    {
        public GestionClientsPage()
        {
            InitializeComponent();
            DataContext = new GestionClientsViewModel();
        }
    }
}
