using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using TerminalClientAdmin.Clients;
using TerminalClientAdmin.Entities;

namespace TerminalClientAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientAdminProduct clientAdminProduct = new ClientAdminProduct();

        List<Product> allProductsTemp = new List<Product>();
        public MainWindow()
        {
            InitializeComponent();

            // handlers
            btn_LoadProducts.Click += (o, e) => { UpdateAllProductsAsync(); };
            btn_DeleteProduct.Click += (o, e) => { DeleteProductsAsync(); };
        }


        private async void DeleteProductsAsync()
        {
            var products = dg_Products.SelectedItems;
            if (products == null || products.Count == 0)
            {
                ShowInformation("Select products for deletion!");
                return;
            }

            List<Product> productsForDelete = new List<Product>();
            foreach (var item in products)
            {
                Product product = item as Product;
                productsForDelete.Add(product);
            }

            while(productsForDelete.Count > 0)
            {
                var product = productsForDelete[productsForDelete.Count - 1];
                
                try
                {
                    await clientAdminProduct.DeleteProductAsync(product.Name);
                }
                catch (Exception)
                {
                    lbl_Status.Content = "Status : Not all products were deleted";
                    ShowInformation("Bad internet connection or server not responding. Try deletion later!");
                    return;
                }

                allProductsTemp.Remove(product);
                dg_Products.ItemsSource = null;
                dg_Products.ItemsSource = allProductsTemp;
                dg_Products.IsReadOnly = true;

                productsForDelete.RemoveAt(productsForDelete.Count - 1);
            }

            lbl_Status.Content = "Status : Selected products were deleted";
        }
        private async void UpdateAllProductsAsync()
        {
            lbl_Status.Content = "Status : Loading products..";
            List<Product> products = null;

            try
            {
                products = await clientAdminProduct.GetAllProductsAsync();
            }
            catch (Exception)
            {
                lbl_Status.Content = "Status :";
                ShowInformation("Bad internet connection or server not responding. Try loading later!");
                return;
            }
            
            products = products.OrderBy(p => p.Name).ToList();
            allProductsTemp = products;

            dg_Products.ItemsSource = null;
            dg_Products.ItemsSource = products;
            dg_Products.IsReadOnly = true;
            lbl_Status.Content = "Status : Products were loaded";
        }
        private void ShowInformation(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
