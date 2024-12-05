using System.Windows.Controls;


namespace projet_gestion.Views
{
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            DataContext = new ModelViews.DashboardPageModel();
        }
    }
}
