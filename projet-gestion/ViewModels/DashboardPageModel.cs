using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projet_gestion.ModelViews
{
    internal class DashboardPageModel
    {
        public string BestSeller { get; set; } = "Produit A";
        public decimal TotalSales { get; set; } = 12500.50m;
        public ObservableCollection<string> LowStockNotifications { get; set; }

        public DashboardPageModel()
        {
            LowStockNotifications = new ObservableCollection<string>
        {
            "Produit B - Stock < 5",
            "Produit C - Stock < 5"
        };
        }
    }
}
