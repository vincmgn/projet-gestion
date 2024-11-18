using Back.Models;
using projet_gestion.ViewModels;
using System.Net.Http;
using System.Windows.Input;
using System.Windows;
using System.Net.Http.Json;

namespace projet_gestion.ViewModels.Dialogs
{
    public class AddCategoryDialogViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;

        // Propriétés existantes
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICommand AddCategoryCommand { get; }
        public ICommand CancelCommand { get; }
        public event Action OnCategoryAdded;

        // Nouvelle propriété pour gérer le mode
        public bool IsEditMode => CategoryId.HasValue;

        // Propriétés dynamiques pour le titre et le bouton
        public string DialogTitle => IsEditMode ? "Modifier une catégorie" : "Ajouter une catégorie";
        public string ActionButtonContent => IsEditMode ? "Modifier" : "Ajouter";

        public AddCategoryDialogViewModel()
        {
            _httpClient = new HttpClient();
            AddCategoryCommand = new RelayCommand(async param => await ExecuteAddOrEditCategoryAsync());
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private async Task ExecuteAddOrEditCategoryAsync()
        {
            if (string.IsNullOrEmpty(CategoryName))
            {
                MessageBox.Show("Veuillez entrer un nom pour la catégorie.");
                return;
            }

            string apiUrl = IsEditMode
                ? $"http://localhost:5042/api/categories/{CategoryId}"  // API pour modifier
                : "http://localhost:5042/api/categories";               // API pour ajouter

            try
            {
                HttpResponseMessage response;

                if (IsEditMode)
                {
                    // Modification
                    var updatedCategory = new Category
                    {
                        Id = CategoryId.Value,
                        Name = CategoryName
                    };

                    response = await _httpClient.PutAsJsonAsync(apiUrl, updatedCategory);
                }
                else
                {
                    // Ajout
                    var newCategory = new Category { Name = CategoryName };
                    response = await _httpClient.PostAsJsonAsync(apiUrl, newCategory);
                }

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(IsEditMode ? "Catégorie modifiée avec succès." : "Catégorie ajoutée avec succès.");
                    OnCategoryAdded?.Invoke();
                    CloseDialog();
                }
                else
                {
                    MessageBox.Show("Une erreur est survenue lors de l'envoi des données.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
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