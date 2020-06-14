using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TerminalClient.Clients;
using TerminalClient.Entities;

namespace TerminalClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientProduct clientProduct = new ClientProduct();

        List<Product> allProducts = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();

            // handlers
            btn_Add.Click += Btn_Add_Click;
            btn_UploadProducts.Click += Btn_UploadProducts_ClickAsync;

        }

        private void Btn_UploadProducts_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (allProducts.Count == 0)
            {
                ShowInformation("Add products before uploading!");
                return;
            }
            UploadAllProductsAsync();
        }
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string productName = tb_Name.Text.Trim();
            if (productName.Equals(""))
            {
                ShowInformation("Input product name!");
                return;
            }
            string productAmount = tb_Amount.Text.Trim();
            if (productAmount.Equals(""))
            {
                ShowInformation("Input product amount!");
                return;
            }
            if (productName.Length >= 100)
            {
                ShowInformation("Max length of product name is 100 symbols!");
                return;
            }
            int amount = 0;
            bool res = int.TryParse(productAmount, out amount);
            if (res == false || amount <= 0)
            {
                ShowInformation("Incorrect product amount!");
                return;
            }

            Product product = new Product { Name = productName, Amount = amount };
            allProducts.Add(product);
            
            dg_Products.ItemsSource = null;
            dg_Products.ItemsSource = allProducts;
            dg_Products.IsReadOnly = true;

            tb_Name.Text = "";
            tb_Amount.Text = "";
        }
        private async void UploadAllProductsAsync()
        {
            // grouping
            var productsForUpload = allProducts.GroupBy(product => product.Name)
                .Select(group => new Product { Name = group.Key, Amount = group.Sum(p => p.Amount) })
                .ToList();

            int uploadSize = productsForUpload.Count;
            int uploadCounter = 0;
            
            // update GUI
            allProducts = new List<Product>();
            dg_Products.ItemsSource = null;
            dg_Products.ItemsSource = allProducts;
            dg_Products.IsReadOnly = true;

            // uploading
            while(productsForUpload.Count > 0)
            {
                var product = productsForUpload.Last();
                lbl_Status.Content = $"Status : Upoading products - {++uploadCounter} / {uploadSize}";
                bool? result = await clientProduct.UploadProductAsync(product);

                if (result == null || // bad internet connection
                    result == false)  // error in http-protocol from server
                {
                    allProducts.AddRange(productsForUpload);
                    dg_Products.ItemsSource = null;
                    dg_Products.ItemsSource = allProducts;
                    dg_Products.IsReadOnly = true;

                    lbl_Status.Content = "Status : Not all products were uploaded";
                    ShowInformation("Bad internet connection or server not responding. Try uploading later!");
                    return;
                }
                else // product was uploaded
                    productsForUpload.RemoveAt(productsForUpload.Count - 1);
            }

            // successefull uploading
            lbl_Status.Content = $"Status : Products were uploaded - {uploadCounter} / {uploadSize}";
        }
        private void ShowInformation(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
