using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TerminalClientAdmin.Entities;

namespace TerminalClientAdmin.Clients
{
    class ClientAdminProduct
    {
        string baseUrl = @"http://localhost:53169/api/";
        string baseUrlProduct => baseUrl + "product";

        HttpClient client;

        public ClientAdminProduct()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> products = null;
            HttpResponseMessage response = await client.GetAsync(baseUrlProduct);
            if (response.IsSuccessStatusCode)
                products = await response.Content.ReadAsAsync<List<Product>>();

            return products;
        }
        public async Task<Product> DeleteProductAsync(string productName)
        {
            string url = baseUrlProduct + "/" + productName;
            HttpResponseMessage response = await client.DeleteAsync(url);
            Product product = await response.Content.ReadAsAsync<Product>();

            return product;
        }
    }
}
