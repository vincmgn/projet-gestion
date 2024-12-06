using Back.Models;
using Newtonsoft.Json;
using projet_gestion.ViewModels.Dialogs;
using projet_gestion.Views.Dialogs;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
        public ICommand ExportToCSVCommand { get; }
        public ICommand ExportToJSONCommand { get; }

        public GestionCategoriesViewModel()
        {
            _httpClient = new HttpClient();
            Categories = new ObservableCollection<Category>();
            AddCategoryCommand = new RelayCommand(param => OpenAddCategoryDialog(null));
            EditCommand = new RelayCommand(EditCategory);
            DeleteCommand = new RelayCommand(DeleteCategory);
            ExportToCSVCommand = new RelayCommand(_ => ExportToCSV());
            ExportToJSONCommand = new RelayCommand(_ => ExportToJSON());

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

        private void ExportToCSV()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Fichiers CSV (*.csv)|*.csv",
                FileName = "categories.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var csvContent = new StringBuilder();
                    csvContent.AppendLine("ID;Nom");

                    foreach (var category in Categories)
                    {
                        csvContent.AppendLine($"{category.Id};{category.Name}");
                    }

                    File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                    MessageBox.Show($"Données exportées avec succès en CSV : {saveFileDialog.FileName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'exportation en CSV : {ex.Message}");
                }
            }
        }


        private void ExportToJSON()
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Fichiers JSON (*.json)|*.json", 
                    FileName = "categories.json",
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var jsonContent = JsonConvert.SerializeObject(Categories, Formatting.Indented);

                    File.WriteAllText(saveFileDialog.FileName, jsonContent);

                    MessageBox.Show($"Données exportées avec succès en JSON : {saveFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exportation en JSON : {ex.Message}");
            }
        }

    }
}
