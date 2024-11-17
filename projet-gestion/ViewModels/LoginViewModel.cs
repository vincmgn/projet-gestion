using System.Windows.Input;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using projet_gestion.Views;

namespace projet_gestion.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _httpClient = new HttpClient();
            LoginCommand = new RelayCommand(async _ => await OnLogin());
        }

        private async Task OnLogin()
        {
            var loginData = new
            {
                Username = this.Username,
                Password = this.Password
            };

            try
            {
                string jsonData = JsonConvert.SerializeObject(loginData);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:5042/api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    string token = responseObject.token;

                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var layoutWindow = new Layout();
                            layoutWindow.Show();
                            Application.Current.MainWindow.Close();
                        });
                    }
                    else
                    {
                        MessageBox.Show("Token generation failed.");
                    }
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur de connexion: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la tentative de connexion: {ex.Message}");
            }
        }
    }
}
