using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;

namespace projet_gestion.ModelViews
{
    internal class DashboardPageModel : INotifyPropertyChanged
    {
        private decimal _totalSales;
        private Bestseller _bestSeller;
        private ObservableCollection<ColumnSeries> _top3ChartValues;
        public decimal TotalSales
        {
            get => _totalSales;
            set
            {
                _totalSales = value;
                OnPropertyChanged();
            }
        }

        public Bestseller BestSeller
        {
            get => _bestSeller;
            set
            {
                _bestSeller = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ColumnSeries> Top3ChartValues
        {
            get => _top3ChartValues;
            set
            {
                _top3ChartValues = value;
                OnPropertyChanged();
            }
        }

        private readonly HttpClient _httpClient;
        public ObservableCollection<string> LowStockNotifications { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public ObservableCollection<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


        public DashboardPageModel()
        {
            _httpClient = new HttpClient();
            LowStockNotifications = new ObservableCollection<string>();
            Top3ChartValues = new ObservableCollection<ColumnSeries>();
            Labels = new ObservableCollection<string>();

            _ = LoadLowStockNotifications();
            _ = LoadBestSeller();
            _ = LoadTotalSales();
            _ = LoadTop3Chart();
        }

        private async Task LoadLowStockNotifications()
        {
            string apiUrl = "http://localhost:5042/api/products/lowstock";

            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                var notifications = JsonConvert.DeserializeObject<List<string>>(response);

                if (notifications == null || notifications.Count == 0)
                {
                    notifications = new List<string> { "Aucune notification de stock bas." };
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    LowStockNotifications.Clear();
                    foreach (var notification in notifications)
                    {
                        LowStockNotifications.Add(notification);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des notifications de stock bas : {ex.Message}");
            }
        }

        private async Task LoadBestSeller()
        {
            string apiUrl = "http://localhost:5042/api/orders/bestseller";

            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    BestSeller = JsonConvert.DeserializeObject<Bestseller>(response);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du meilleur vendeur : {ex.Message}");
            }
        }

        private async Task LoadTotalSales()
        {
            string apiUrl = "http://localhost:5042/api/orders/total";

            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TotalSales = JsonConvert.DeserializeObject<decimal>(response);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du total des ventes : {ex.Message}");
            }
        }

        private async Task LoadTop3Chart()
        {
            string apiUrl = "http://localhost:5042/api/orders/top3";

            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                var top3 = JsonConvert.DeserializeObject<List<TopProduct>>(response);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    SeriesCollection = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Top 3",
                            Values = new ChartValues<int>()
                        }
                    };

                    Labels.Clear();
                    foreach (var product in top3)
                    {
                        SeriesCollection[0].Values.Add(product.Quantity);
                        Labels.Add(product.ProductName);
                    }

                    Formatter = value => value.ToString("N");

                    OnPropertyChanged(nameof(SeriesCollection));
                    OnPropertyChanged(nameof(Labels));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du Top 3 des produits : {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Bestseller
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
    public class TopProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
