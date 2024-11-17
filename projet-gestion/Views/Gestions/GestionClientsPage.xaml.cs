using projet_gestion.ViewModels;
using projet_gestion.Views.Dialogs;
using System.Windows;
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
