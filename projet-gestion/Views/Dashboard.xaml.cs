using System.Windows.Controls;


namespace projet_gestion.Views
{
    /// <summary>
    /// Logique d'interaction pour DashboardPage.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            DataContext = new ModelViews.DashboardPageModel();
        }
    }
}
