using KFCFoodDeliveryApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnixTimeStamp;
using Xamarin.Essentials;

namespace KFCFoodDeliveryApp.Services
{
    public static class APIService
    {
        public static async Task<bool> RegisterUser(string name, string email, string password)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var register = new Register()
                {
                    Name = name,
                    Email = email,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(register);
                using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync(string.Format("{0}{1}", new object[] { AppSettings.API_URL, "Accounts/Register" }), content);
                    if (!response.IsSuccessStatusCode) return false;

                    return true;
                }
            }
        }

        public static async Task<bool> Login(string email, string password)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var login = new Login()
                {
                    Email = email,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(login);
                using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync(string.Format("{0}{1}", new object[] { AppSettings.API_URL, "Accounts/Login" }), content);
                    if (!response.IsSuccessStatusCode) return false;

                    try
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Token>(jsonResult);
                        Preferences.Set("access_token", result.access_token);
                        Preferences.Set("user_id", result.user_Id);
                        Preferences.Set("user_name", result.user_name);
                        Preferences.Set("token_exp_time", result.expiration_Time);
                        Preferences.Set("current_time", UnixTime.GetCurrentTime());
                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }                    
                }
            }
        }

        public static async Task<List<Category>> GetCategories()
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}", new object[] { AppSettings.API_URL, "Categories" }));
                return JsonConvert.DeserializeObject<List<Category>>(response);
            }
        }

        public static async Task<Product> GetProductById(int productId)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}/{2}", new object[] { AppSettings.API_URL, "Products", productId }));
                return JsonConvert.DeserializeObject<Product>(response);
            }
        }

        public static async Task<List<ProductByCategory>> GetProductByCategory(int categoryId)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}/{2}", new object[] { AppSettings.API_URL, "Products/ProductsByCategory", categoryId }));
                return JsonConvert.DeserializeObject<List<ProductByCategory>>(response);
            }
        }

        public static async Task<List<PopularProduct>> GetPopularProducts()
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}", new object[] { AppSettings.API_URL, "Products/PopularProducts" }));
                return JsonConvert.DeserializeObject<List<PopularProduct>>(response);
            }
        }

        public static async Task<bool> AddItemsToCart(AddToCart addToCart)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var json = JsonConvert.SerializeObject(addToCart);
                using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync(string.Format("{0}{1}", new object[] { AppSettings.API_URL, "ShoppingCartItems" }), content);
                    if (!response.IsSuccessStatusCode) return false;

                    return true;
                }
            }
        }

        public static async Task<CartSubTotal> GetCartSubTotal(int userId)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}/{2}", new object[] { AppSettings.API_URL, "ShoppingCartItems/SubTotal", userId }));
                return JsonConvert.DeserializeObject<CartSubTotal>(response);
            }
        }

        public static async Task<List<ShoppingCartItem>> GetShoppingCartItems(int userId)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}/{2}", new object[] { AppSettings.API_URL, "ShoppingCartItems", userId }));
                return JsonConvert.DeserializeObject<List<ShoppingCartItem>>(response);
            }
        }

        public static async Task<TotalCartItem> GetTotalCartItems(int userId)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}/{2}", new object[] { AppSettings.API_URL, "ShoppingCartItems/TotalItems", userId }));
                return JsonConvert.DeserializeObject<TotalCartItem>(response);
            }
        }

        public static async Task<bool> ClearShoppingCart(int userId)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.DeleteAsync(string.Format("{0}{1}/{2}", new object[] { AppSettings.API_URL, "ShoppingCartItems", userId }));
                if (!response.IsSuccessStatusCode) return false;

                return true;
            }
        }

        public static async Task<OrderResponse> PlaceOrder(Order order)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var json = JsonConvert.SerializeObject(order);
                using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync(string.Format("{0}{1}", new object[] { AppSettings.API_URL, "Orders" }), content);

                    try
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<OrderResponse>(jsonResult);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public static async Task<List<Order>> GetOrderDetails(int orderId)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}/{2}", new object[] { AppSettings.API_URL, "Orders/OrderDetails", orderId }));
                return JsonConvert.DeserializeObject<List<Order>>(response);
            }
        }

        public static async Task<List<OrderByUser>> GetOrdersByUser(int userId)
        {
            await TokenValidator.CheckTokenValidity();
            using (HttpClient httpClient = new HttpClient())
            {
                var token = Preferences.Get("access_token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var response = await httpClient.GetStringAsync(string.Format("{0}{1}/{2}", new object[] { AppSettings.API_URL, "Orders/OrdersByUser", userId }));
                return JsonConvert.DeserializeObject<List<OrderByUser>>(response);
            }
        }
    }

    public static class TokenValidator
    {
        public static async Task CheckTokenValidity()
        {
            var token_exp_time = Preferences.Get("token_exp_time", 0);
            var current_time = Preferences.Get("current_time", 0);

            if(token_exp_time < current_time)
            {
                var email = Preferences.Get("email", string.Empty);
                var password = Preferences.Get("password", string.Empty);
                await APIService.Login(email, password);
            }
        }
    }
}
