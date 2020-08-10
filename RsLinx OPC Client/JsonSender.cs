using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RsLinx_OPC_Client
{
     
    public class Product
    {
        public string Id { get; set; }
        public string param_name { get; set; }
        public string param_value { get; set; }
        public string param_redundant { get; set; }
        public string param_redundant_2 { get; set; }
    }

    class JsonSender
    {
        static HttpClient client = new HttpClient();
        static bool firstEntry = true;
        public static String value = "111";
        static String pathAcknowladge = "cellPhoneParam/1";
        static String need_acknowladge = "Yes";

        /*static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.zd_name}\tPrice: " +
                $"{product.zd_location}\tCategory: {product.zd_blok_number}");
        }*/

        static async void CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "exchangeParam", product);
            response.EnsureSuccessStatusCode();

        }


        /*static async void CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "exchangeParam", product);
            response.EnsureSuccessStatusCode();

            Product productResp = null;
            HttpResponseMessage responseResp = await client.GetAsync(response.Headers.Location.PathAndQuery);
            if (responseResp.IsSuccessStatusCode)
            {
                productResp = await responseResp.Content.ReadAsAsync<Product>();
            }
            path = productResp.Id;

        }*/

        static async Task<Product> GetProductAsync(string pathLocal)
        {
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(pathLocal);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
        }

        // static async Task<Product> UpdateProductAsync(Product product)
        static async void UpdateProductAsync(Product product, String pathLocal)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"exchangeParam/" + pathLocal, product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            // product = await response.Content.ReadAsAsync<Product>();
            //return product;
        }

        static async void UpdateProductAsync(String pathGlobal, Product product, int index)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                 pathGlobal, product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            // product = await response.Content.ReadAsAsync<Product>();
            //return product;
        }

        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"exchangeParam/{id}");
            return response.StatusCode;
        }

        static public async Task updateFromCellPhone(String valueFromCellPhone)
        {
            firstScan();

            Product product = new Product
            {
                param_name = ".myValue",
                param_value = valueFromCellPhone,
                param_redundant = need_acknowladge,
                param_redundant_2 = "null",
            };

            //CreateProductAsync(product);
            UpdateProductAsync(pathAcknowladge, product, 1);
        }

        static public async Task infinityScan()
        {
            while (true)
            {
                firstScan();
                Thread.Sleep(1000);
                Product result = await GetProductAsync(pathAcknowladge);
                if (result.param_redundant == need_acknowladge)
                {
                    Form1.myForm.WriteWord(result.param_name, Convert.ToSingle(result.param_value));

                    Form1.myForm.historyList.Invoke(new EventHandler(delegate {
                        ListViewItem lvItem = new ListViewItem();
                        ListViewItem.ListViewSubItem[] lvSubItem = new ListViewItem.ListViewSubItem[1];
                        lvItem.Text = result.param_name + result.param_value;
                        lvItem.Name = result.param_name;
                        lvItem.SubItems.Add(DateTime.Now.ToString("h:mm:ss tt"));
                        Form1.myForm.historyList.Items.Add(lvItem);
                    }));

                    Thread.Sleep(1000);
                    Product product = new Product
                    {
                        param_name = "null",
                        param_value = "null",
                        param_redundant = "null",
                        param_redundant_2 = "null",

                    };
                    UpdateProductAsync(pathAcknowladge, product, 0);
                }
            }
        }
        static private void firstScan()
        {
            if (firstEntry)
            {
                client.BaseAddress = new Uri("http://192.168.43.238:4000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                firstEntry = false;
            }
        }
        static public async Task RunAsync(string index, string name, string value)
        {
            // Update port # in the following line.
            /*while (true)
            { }*/
            firstScan();

                if (Form1.tryToExit) {
                for (int i = (Convert.ToInt16(Form1.itemIndexForJson)+1); i >= 1;i--) 
                { var statusCode = await DeleteProductAsync(i.ToString()); }
                return;
                }


                if (value == "null")
                {// Create a new product
                    Product product = new Product
                    {
                        param_name = name,
                        param_value = value,
                        param_redundant = index,
                        param_redundant_2 = "null",

                    };
                    CreateProductAsync(product);
                return;
                    //Thread.Sleep(5000); 
                }


                try
                {
                
                // Create a new product
                Product product = new Product
                    {
                        param_name = name,
                        param_value = value,
                        param_redundant = index,
                        param_redundant_2 = "null",
                    };
                    int my_path = Convert.ToInt16(index) + 1;
                    //CreateProductAsync(product);
                    UpdateProductAsync(product,my_path.ToString());
                   // Thread.Sleep(5000);

                    //Console.WriteLine($"Created at {url}");

                    // Get the product
                    //product = await GetProductAsync(url.PathAndQuery);
                    //ShowProduct(product);

                    // Update the product
                    //Console.WriteLine("Updating price...");
                    //product.zd_blok_number = "777";
                    //await UpdateProductAsync(product);

                    // Get the updated product
                    //product = await GetProductAsync(url.PathAndQuery);
                    //ShowProduct(product);

                    // Delete the product
                    // var statusCode = await DeleteProductAsync(product.Id);
                    //  Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
                }

                //Console.ReadLine();
            
        }
    }
}