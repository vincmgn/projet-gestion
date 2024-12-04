using Back.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using System.Net.Http.Json;
using System.Windows;
using projet_gestion.Views.Dialogs;
using projet_gestion.ViewModels.Dialogs;

namespace projet_gestion.ViewModels
{
    internal class GestionOrdersViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<Order> Orders { get; set; }
        public ICommand AddOrderCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public GestionOrdersViewModel()
        {
            _httpClient = new HttpClient();
            Orders = new ObservableCollection<Order>();
            AddOrderCommand = new RelayCommand(param => OpenAddOrderDialog());
            EditCommand = new RelayCommand(EditOrderAsync);
            DeleteCommand = new RelayCommand(DeleteOrder);

            _ = LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                string orderApiUrl = "http://localhost:5042/api/orders";
                string clientApiUrl = "http://localhost:5042/api/clients";
                string productApiUrl = "http://localhost:5042/api/products";

                var orders = await _httpClient.GetFromJsonAsync<List<Order>>(orderApiUrl);
                var clients = await _httpClient.GetFromJsonAsync<List<Client>>(clientApiUrl);
                var products = await _httpClient.GetFromJsonAsync<List<Product>>(productApiUrl);

                if (orders != null && clients != null && products != null)
                {
                    Orders.Clear();
                    foreach (var order in orders)
                    {
                        order.Client = clients.FirstOrDefault(c => c.Id == order.ClientId);
                        order.Product = products.FirstOrDefault(p => p.Id == order.ProductId);
                        Orders.Add(order);
                    }
                }
                else
                {
                    MessageBox.Show("Aucune commande, client ou produit trouvé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des commandes, clients ou produits : {ex.Message}");
            }
        }

        private void OpenAddOrderDialog()
        {
            var dialog = new AddOrderDialog();
            var dialogViewModel = new AddOrderDialogViewModel();
            dialogViewModel.OnOrderAdded += async () =>
            {
                await LoadOrdersAsync();
                dialog.Close();
            };
            dialog.DataContext = dialogViewModel;
            dialog.ShowDialog();
        }

        private void EditOrderAsync(object parameter)
        {
            if (parameter is Order order)
            {
                var dialog = new AddOrderDialog();
                var dialogViewModel = new AddOrderDialogViewModel
                {
                    OrderClientId = order.ClientId,
                    OrderProductId = order.ProductId,
                    OrderQuantity = order.Quantity,
                    OrderDateCommande = order.DateCommande,
                    OrderStatus = order.Statut,
                    OrderId = order.Id
                };

                dialogViewModel.OnOrderAdded += async () => await LoadOrdersAsync();

                dialog.DataContext = dialogViewModel;
                dialog.ShowDialog();

            }
        }

        private async void DeleteOrder(object parameter)
        {
            if (parameter is Order order)
            {
                string apiUrl = $"http://localhost:5042/api/orders/{order.Id}";
                
                try
                {
                    HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Produit supprimé avec succès.");
                        Orders.Remove(order);
                        await LoadOrdersAsync();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la commande.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression de la commande : {ex.Message}");
                }
            }
        }
    }
}
