using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TerminalClient.Entities;

namespace TerminalClient.Clients
{
    class ClientProduct
    {
        string baseUrl = @"http://localhost:53169/api/";
        string baseUrlProduct => baseUrl + "product";

        HttpClient client;

        public ClientProduct()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<bool?> UploadProductAsync(Product product)
        {
            try
            {
                HttpResponseMessage responsePost = await client.PostAsJsonAsync(baseUrlProduct, product);
                if (responsePost.IsSuccessStatusCode)
                    return true;
                else
                {
                    string url = baseUrlProduct + "/" + product.Name;
                    HttpResponseMessage responsePut = await client.PutAsJsonAsync(url, product);
                    if (responsePut.IsSuccessStatusCode)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
