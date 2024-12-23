﻿using Back.Models;
using projet_gestion.ViewModels;
using System.Net.Http;
using System.Windows.Input;
using System.Windows;
using System.Net.Http.Json;

namespace projet_gestion.ViewModels.Dialogs
{
    public class AddClientDialogViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;

        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientSiret { get; set; }
        public ICommand AddClientCommand { get; }
        public ICommand CancelCommand { get; }
        public event Action OnClientAdded;
        public bool IsEditMode => ClientId.HasValue;
        public string DialogTitle => IsEditMode ? "Modifier un client" : "Ajouter un client";
        public string ActionButtonContent => IsEditMode ? "Modifier" : "Ajouter";

        public AddClientDialogViewModel()
        {   
            _httpClient = new HttpClient();
            AddClientCommand = new RelayCommand(async param => await ExecuteAddOrEditClientAsync());
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private async Task ExecuteAddOrEditClientAsync()
        {
            if (string.IsNullOrEmpty(ClientName))
            {
                MessageBox.Show("Veuillez entrer un nom pour le client.");
                return;
            }

            string apiUrl = IsEditMode
                ? $"http://localhost:5042/api/Clients/{ClientId}"
                : "http://localhost:5042/api/Clients";

            try
            {
                HttpResponseMessage response;

                if (IsEditMode)
                {
                    // Modification
                    var updatedClient = new Client
                    {
                        Id = ClientId.Value,
                        Name = ClientName,
                        Address = ClientAddress,
                        Siret = ClientSiret,
                        Orders = new List<Order>()
                    };

                    var existingClient = await _httpClient.GetFromJsonAsync<Client>($"http://localhost:5042/api/Clients/{ClientId}");

                    if (existingClient != null && existingClient.Orders != null)
                    {
                        updatedClient.Orders = existingClient.Orders;
                    }

                    response = await _httpClient.PutAsJsonAsync(apiUrl, updatedClient);
                }
                else
                {
                    // Ajout
                    var newClient = new Client
                    {
                        Name = ClientName,
                        Address = ClientAddress,
                        Siret = ClientSiret,
                        Orders = new List<Order>()

                    };
                    response = await _httpClient.PostAsJsonAsync(apiUrl, newClient);
                }

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(IsEditMode ? "Client modifié avec succès." : "Client ajouté avec succès.");
                    OnClientAdded?.Invoke();
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