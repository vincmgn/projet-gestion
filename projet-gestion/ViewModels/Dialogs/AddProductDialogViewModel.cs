using Back.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Input;

namespace projet_gestion.ViewModels.Dialogs
{
    public class AddProductDialogViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();


        public int? ProductId { get; set; } 
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; } 
        public decimal ProductPrice { get; set; }
        public DateTime ProductDatePeremption { get; set; } = DateTime.Now;
        public int? ProductCategoryId { get; set; } = null;
        public string ProductEmplacement { get; set; }
        public ICommand AddProductCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action OnProductAdded;
        public bool IsEditMode => ProductId.HasValue;
        public string DialogTitle => IsEditMode ? "Modifier un produit" : "Ajouter un produit";
        public string ActionButtonContent => IsEditMode ? "Modifier" : "Ajouter";

        public AddProductDialogViewModel(Product? productToEdit = null)
        {
            _httpClient = new HttpClient();
            LoadCategoriesAsync();

            AddProductCommand = new RelayCommand(async param => await ExecuteAddOrEditProductAsync());
            CancelCommand = new RelayCommand(ExecuteCancel);

            // Mode édition
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
        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _httpClient.GetFromJsonAsync<List<Category>>("http://localhost:5042/api/Categories");
                if (categories != null)
                {
                    Categories.Clear();
                    foreach (var category in categories)
                    {
                        Categories.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des catégories : {ex.Message}");
            }
        }

        private async Task ExecuteAddOrEditProductAsync()
        {
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
                ? $"http://localhost:5042/api/Products/{ProductId}"
                : "http://localhost:5042/api/Products";

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
                        CategoryId = ProductCategoryId,
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
