using projet_gestion.Views.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace projet_gestion.Views.Gestions
{
    /// <summary>
    /// Logique d'interaction pour GestionPage.xaml
    /// </summary>
    public partial class GestionProductPage : Page
    {
        public GestionProductPage()
        {
            InitializeComponent();

        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Crée et affiche la boîte de dialogue pour ajouter un produit
            var addProductDialog = new AddProductDialog()
            {
                Owner = Window.GetWindow(this) // Définit le propriétaire de la fenêtre modale
            };

            if (addProductDialog.ShowDialog() == true) // Si l'utilisateur a validé l'ajout
            {
                //string productName = addProductDialog.ProductName;
                //int productQuantity = addProductDialog.ProductQuantity;
                //decimal productPrice = (decimal)addProductDialog.ProductPrice;
                //DateTime productExpirationDate = addProductDialog.ProductExpirationDate;
                //string productCategory = addProductDialog.ProductCategory;
                //string productLocation = addProductDialog.ProductLocation;

                // Ajouter ici la logique pour insérer le produit dans la base de données
                //MessageBox.Show($"Produit '{productName}' ajouté avec succès !", "Succès");
            }
        }
    }
}
