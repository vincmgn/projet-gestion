﻿using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace projet_gestion.ModelViews
{
    internal class DashboardPageModel : INotifyPropertyChanged
    {
        private decimal _totalSales;
        private Bestseller _bestSeller;
        private ObservableCollection<ColumnSeries> _top3ChartValues;

        // Propriétés liées à l'UI
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

        public ObservableCollection<string> LowStockNotifications { get; set; }

        private readonly HttpClient _httpClient;

        public DashboardPageModel()
        {
            _httpClient = new HttpClient();
            LowStockNotifications = new ObservableCollection<string>();
            Top3ChartValues = new ObservableCollection<ColumnSeries>();

            // Chargement des données au démarrage
            _ = LoadLowStockNotifications();
            _ = LoadBestSeller();
            _ = LoadTotalSales();
            _ = LoadTop3();
        }

        // Méthode pour charger les notifications de stock faible
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

                // Mise à jour de la collection dans le thread principal
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

        // Méthode pour charger le meilleur vendeur
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

        // Méthode pour charger le total des ventes
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

        // Méthode pour charger les top 3 produits
        

        // Implémentation de l'interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Classe représentant un produit best-seller
    public class Bestseller
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    // Classe représentant un produit dans le Top 3
    public class TopProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
