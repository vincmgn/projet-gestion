using Back.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace projet_gestion.ViewModels.Dialogs
{
    public class AddProductDialogViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;

        // Propriétés
        public int? ProductId { get; set; } 
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; } 
        public decimal ProductPrice { get; set; }
        public DateTime ProductDatePeremption { get; set; } = DateTime.Today; 
        public int? ProductCategoryId { get; set; } = null;
        public string ProductEmplacement { get; set; }

        // Commandes
        public ICommand AddProductCommand { get; }
        public ICommand CancelCommand { get; }

        // Événements
        public event Action OnProductAdded;

        // Mode édition
        public bool IsEditMode => ProductId.HasValue;

        // Titre et contenu du bouton dynamique
        public string DialogTitle => IsEditMode ? "Modifier un produit" : "Ajouter un produit";
        public string ActionButtonContent => IsEditMode ? "Modifier" : "Ajouter";

        public AddProductDialogViewModel(Product? productToEdit = null)
        {
            _httpClient = new HttpClient();

            // Commandes
            AddProductCommand = new RelayCommand(async param => await ExecuteAddOrEditProductAsync());
            CancelCommand = new RelayCommand(ExecuteCancel);

            // Mode édition : Initialiser les valeurs du produit
            if (productToEdit != null)
            {
                ProductId = productToEdit.Id;
                ProductName = productToEdit.Name;
                ProductQuantity = productToEdit.Quantity;
                ProductPrice = productToEdit.Price;
                ProductDatePeremption = productToEdit.DatePeremption;
                ProductCategoryId = productToEdit.CategoryId;
                ProductEmplacement = productToEdit.Emplacement;
            }
        }

        private async Task ExecuteAddOrEditProductAsync()
        {
            // Validations
            if (string.IsNullOrEmpty(ProductName))
            {
                MessageBox.Show("Veuillez entrer un nom pour le produit.");
                return;
            }

            if (!ProductCategoryId.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner une catégorie.");
                return;
            }

            string apiUrl = IsEditMode
                ? $"http://localhost:5042/api/Products/{ProductId}"  // API pour modifier
                : "http://localhost:5042/api/Products";               // API pour ajouter

            try
            {
                HttpResponseMessage response;

                if (IsEditMode)
                {
                    // Modification
                    var updatedProduct = new Product
                    {
                        Id = ProductId.Value,
                        Name = ProductName,
                        Quantity = ProductQuantity,
                        Price = ProductPrice,
                        DatePeremption = ProductDatePeremption,
                        CategoryId = ProductCategoryId,
                        Emplacement = ProductEmplacement,
                    };

                    response = await _httpClient.PutAsJsonAsync(apiUrl, updatedProduct);
                }
                else
                {
                    // Ajout
                    var newProduct = new Product
                    {
                        Name = ProductName,
                        Quantity = ProductQuantity,
                        Price = ProductPrice,
                        DatePeremption = ProductDatePeremption,
                        CategoryId = 1, ///////////////////////////////// REPREBDRE l'ID DE LA CATEGORIE
                        Emplacement = ProductEmplacement,
                    };

                    response = await _httpClient.PostAsJsonAsync(apiUrl, newProduct);
                }

                if (response.IsSuccessStatusCode)
                {
                    OnProductAdded?.Invoke(); // Notifier le succès
                    CloseDialog();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur : {response.StatusCode}. Détails : {content}");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout/modification du produit : {ex.Message}");
            }
        }

        private void CloseDialog()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
                window?.Close();
            });
        }

        private void ExecuteCancel(object parameter)
        {
            CloseDialog();
        }
    }
}
