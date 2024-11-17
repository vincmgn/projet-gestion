using Back.Models;
using projet_gestion.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace projet_gestion.ViewModels
{
    public class GestionCategoriesViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand AddCategoryCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }


        public GestionCategoriesViewModel()
        {
            _httpClient = new HttpClient();
            Categories = new ObservableCollection<Category>();
            AddCategoryCommand = new RelayCommand(param => OpenAddCategoryDialog(null));
            EditCommand = new RelayCommand(EditCategory);
            DeleteCommand = new RelayCommand(DeleteCategory);

            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                string apiUrl = "http://localhost:5042/api/categories";

                var categories = await _httpClient.GetFromJsonAsync<List<Category>>(apiUrl);

                if (categories != null)
                {
                    Categories.Clear();
                    foreach (var category in categories)
                    {
                        Categories.Add(category);
                    }
                }
                else
                {
                    MessageBox.Show("Aucune catégorie trouvée.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des catégories : {ex.Message}");
            }
        }
        private void OpenAddCategoryDialog(object parameter)
        {
            var dialog = new AddCategoryDialog();
            var dialogViewModel = new AddCategoryDialogViewModel();

            dialogViewModel.OnCategoryAdded += async () => await LoadCategoriesAsync();

            dialog.DataContext = dialogViewModel;
            dialog.ShowDialog();
        }


        private void EditCategory(object parameter)
        {
            if (parameter is Category categoryToEdit)
            {
                var dialog = new AddCategoryDialog();
                var dialogViewModel = new AddCategoryDialogViewModel
                {
                    CategoryId = categoryToEdit.Id,
                    CategoryName = categoryToEdit.Name
                };

                dialogViewModel.OnCategoryAdded += async () => await LoadCategoriesAsync();

                dialog.DataContext = dialogViewModel;
                dialog.ShowDialog();
            }
        }



        private async void DeleteCategory(object parameter)
        {
            if (parameter is Category categoryToDelete)
            {
                var result = MessageBox.Show(
                    $"Voulez-vous vraiment supprimer la catégorie '{categoryToDelete.Name}' ?",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        string apiUrl = $"http://localhost:5042/api/categories/{categoryToDelete.Id}";

                        var response = await _httpClient.DeleteAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Catégorie supprimée avec succès.");
                            await LoadCategoriesAsync();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression de la catégorie.");
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
}
