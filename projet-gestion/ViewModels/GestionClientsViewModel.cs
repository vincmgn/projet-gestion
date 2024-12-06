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
    public class GestionClientsViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<Client> Clients { get; set; }
        public ICommand AddClientCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ExportToCSVCommand { get; }
        public ICommand ExportToJSONCommand { get; }

        public GestionClientsViewModel()
        {
            _httpClient = new HttpClient();
            Clients = new ObservableCollection<Client>();
            AddClientCommand = new RelayCommand(param => OpenAddClientDialog(null));
            EditCommand = new RelayCommand(EditClientAsync);
            DeleteCommand = new RelayCommand(DeleteClient);
            ExportToCSVCommand = new RelayCommand(_ => ExportToCSV());
            ExportToJSONCommand = new RelayCommand(_ => ExportToJSON());

            _ = LoadClientsAsync();
        }

        private async Task LoadClientsAsync()
        {
            try
            {
                string apiUrl = "http://localhost:5042/api/clients";

                var clients = await _httpClient.GetFromJsonAsync<List<Client>>(apiUrl);

                if (clients != null)
                {
                    Clients.Clear();
                    foreach (var client in clients)
                    {
                        Clients.Add(client);
                    }
                }
                else
                {
                    MessageBox.Show("Aucun client trouvé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des clients : {ex.Message}");
            }
        }

        private void OpenAddClientDialog(object parameter)
        {
            var dialog = new AddClientDialog();
            var dialogViewModel = new AddClientDialogViewModel();
            dialogViewModel.OnClientAdded += async () =>
            {
                await LoadClientsAsync();
                dialog.Close();
            };
            dialog.DataContext = dialogViewModel;
            dialog.ShowDialog();
        }

        private void EditClientAsync(object parameter)
        {
            if(parameter is Client clientToEdit)
            {
                var dialog = new AddClientDialog();
                var dialogViewModel = new AddClientDialogViewModel
                {
                    ClientId = clientToEdit.Id,
                    ClientName = clientToEdit.Name,
                    ClientAddress = clientToEdit.Address,
                    ClientSiret = clientToEdit.Siret,
                };

                dialogViewModel.OnClientAdded += async () => await LoadClientsAsync();

                dialog.DataContext = dialogViewModel;
                dialog.ShowDialog();
            }
        }

        private async void DeleteClient(object parameter)
        {
            if (parameter is Client client)
            {
                string apiUrl = $"http://localhost:5042/api/clients/{client.Id}";

                try
                {
                    HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Client supprimé avec succès.");
                        await LoadClientsAsync();
                    }
                    else
                    {
                        MessageBox.Show("Une erreur est survenue lors de la suppression du client.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}");
                }
            }
        }

        private void ExportToCSV()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Fichiers CSV (*.csv)|*.csv",
                FileName = "clients.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var csvContent = new StringBuilder();
                    csvContent.AppendLine("ID;Nom;Adresse;SIRET");

                    foreach (var client in Clients)
                    {
                        csvContent.AppendLine($"{client.Id};{client.Name};{client.Address};{client.Siret}");
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
                    FileName = "clients.json",
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var jsonContent = JsonConvert.SerializeObject(Clients, Formatting.Indented);

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
