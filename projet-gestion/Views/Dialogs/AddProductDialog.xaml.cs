using Back.Models;
using System.Windows;


namespace projet_gestion.Views.Dialogs
{
    public partial class AddProductDialog : Window
    {
        public Product Product { get; set; }

        public AddProductDialog()
        {
            InitializeComponent();
            Product = new Product();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les informations du produit
            Product.Name = ProductNameTextBox.Text;
            Product.Quantity = int.TryParse(ProductQuantityTextBox.Text, out int quantity) ? quantity : 0;
            Product.Price = decimal.TryParse(ProductPriceTextBox.Text, out decimal price) ? price : 0;
            Product.DatePeremption = ProductExpirationDatePicker.SelectedDate ?? DateTime.MinValue;
            //Product.Category = ProductCategoryComboBox.Text;
            Product.Emplacement = ProductLocationTextBox.Text;

            DialogResult = true; // Indiquer que l'ajout a été validé
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Annulation
            Close();
        }
    }
}
