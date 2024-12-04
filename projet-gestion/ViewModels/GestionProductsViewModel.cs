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
    internal class GestionProductsViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<Product> Products { get; set; }
        public ICommand AddProductCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public GestionProductsViewModel()
        {
            _httpClient = new HttpClient();
            Products = new ObservableCollection<Product>();
            AddProductCommand = new RelayCommand(param => OpenAddProductDialog());
            EditCommand = new RelayCommand(EditProductAsync);
            DeleteCommand = new RelayCommand(DeleteProduct);

            _ = LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                string productApiUrl = "http://localhost:5042/api/products";
                string categoryApiUrl = "http://localhost:5042/api/categories";


                var products = await _httpClient.GetFromJsonAsync<List<Product>>(productApiUrl);
                var categories = await _httpClient.GetFromJsonAsync<List<Category>>(categoryApiUrl);


                if (products != null && categories != null)
                {
                    Products.Clear();
                    foreach (var product in products)
                    {
                        product.Category = categories.FirstOrDefault(c => c.Id == product.CategoryId);
                        Products.Add(product);
                    }
                }
                else
                {
                    MessageBox.Show("Aucun produit ou catégorie trouvé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des produits ou des catégories : {ex.Message}");
            }
        }

        private void OpenAddProductDialog()
        {
            var dialog = new AddProductDialog();
            var dialogViewModel = new AddProductDialogViewModel();
            dialogViewModel.OnProductAdded += async () =>
            {
                await LoadProductsAsync();
                dialog.Close();
            };
            dialog.DataContext = dialogViewModel;
            dialog.ShowDialog();
        }

        private void EditProductAsync(object parameter)
        {
            if (parameter is Product productToEdit)
            {
                var dialog = new AddProductDialog();
                var dialogViewModel = new AddProductDialogViewModel
                {
                    ProductId = productToEdit.Id,
                    ProductQuantity = productToEdit.Quantity,
                    ProductName = productToEdit.Name,
                    ProductPrice = productToEdit.Price,
                    ProductDatePeremption = productToEdit.DatePeremption,
                    ProductCategoryId = productToEdit.CategoryId,
                    ProductEmplacement = productToEdit.Emplacement
                };

                dialogViewModel.OnProductAdded += async () => await LoadProductsAsync();

                dialog.DataContext = dialogViewModel;
                dialog.ShowDialog();
            }
        }

        private async void DeleteProduct(object parameter)
        {
            if (parameter is Product product)
            {
                string apiUrl = $"http://localhost:5042/api/products/{product.Id}";

                try
                {
                    HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                    if(response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Produit supprimé avec succès.");
                        await LoadProductsAsync();
                    }
                    else
                    {
                        MessageBox.Show("Une erreur est survenue lors de la suppression du produit.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}");
                }
            }
        }
    }
}
