using Back.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace projet_gestion.ViewModels.Dialogs
{
    internal class AddOrderDialogViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public int? OrderId { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime OrderDateCommande { get; set; } = DateTime.Now;
        public int? OrderClientId { get; set; } = null;
        public int? OrderProductId { get; set; } = null;
        public String? OrderStatus { get; set; } = null;
        public ICommand AddOrderCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action OnOrderAdded;
        public bool IsEditMode => OrderId.HasValue;
        public string DialogTitle => IsEditMode ? "Modifier une commande" : "Ajouter une commande";
        public string ActionButtonContent => IsEditMode ? "Modifier" : "Ajouter";

        public AddOrderDialogViewModel(Order? orderToEdit = null)
        {
            _httpClient = new HttpClient();
            LoadClientsAndProductsAsync();

            AddOrderCommand = new RelayCommand(async param => await ExecuteAddOrEditOrderAsync());
            CancelCommand = new RelayCommand(ExecuteCancel);

            if (orderToEdit != null)
            {
                OrderId = orderToEdit.Id;
                OrderQuantity = orderToEdit.Quantity;
                OrderDateCommande = orderToEdit.DateCommande;
                OrderClientId = orderToEdit.ClientId;
                OrderProductId = orderToEdit.ProductId;
                OrderStatus = orderToEdit.Statut;
            }
        }

        private async Task LoadClientsAndProductsAsync()
        {
            try
            {
                Clients.Clear();
                Products.Clear();
                var clients = await _httpClient.GetFromJsonAsync<List<Client>>("http://localhost:5042/api/clients");
                var products = await _httpClient.GetFromJsonAsync<List<Product>>("http://localhost:5042/api/products");
                foreach (var client in clients)
                {
                    Clients.Add(client);
                }
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des clients et produits : {ex.Message}");
            }
        }

        private async Task ExecuteAddOrEditOrderAsync()
        {
            if (OrderQuantity <= 0)
            {
                MessageBox.Show("La quantité doit être supérieure à 0.");
                return;
            }
            if (!OrderClientId.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner un client.");
                return;
            }
            if (!OrderProductId.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner un produit.");
                return;
            }
            if (string.IsNullOrEmpty(OrderStatus))
            {
                MessageBox.Show("Veuillez entrer un statut pour la commande.");
                return;
            }

            string apiUrl = IsEditMode
                ? $"http://localhost:5042/api/orders/{OrderId}"
                : "http://localhost:5042/api/orders";

            try
            {
                HttpResponseMessage response;

                if (IsEditMode)
                {
                    // Modification
                    var updatedOrder = new Order
                    {
                        Id = OrderId.Value,
                        Quantity = OrderQuantity,
                        DateCommande = OrderDateCommande,
                        ClientId = (int)OrderClientId,
                        ProductId = (int)OrderProductId,
                        Statut = OrderStatus,
                    };

                    response = await _httpClient.PutAsJsonAsync(apiUrl, updatedOrder);
                }
                else
                {
                    // Ajout
                    var newOrder = new Order
                    {
                        Quantity = OrderQuantity,
                        DateCommande = OrderDateCommande,
                        ClientId = (int)OrderClientId,
                        ProductId = (int)OrderProductId,
                        Statut = OrderStatus,
                    };

                    response = await _httpClient.PostAsJsonAsync(apiUrl, newOrder);
                }

                if (response.IsSuccessStatusCode)
                {
                    OnOrderAdded?.Invoke();
                    CloseDialog();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout ou de la modification de la commande.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout ou de la modification de la commande : {ex.Message}");
            }
        }


        private void CloseDialog()
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private void ExecuteCancel(object parameter)
        {
            CloseDialog();
        }
    }
}