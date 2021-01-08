using KFCFoodDeliveryApp.Models;
using KFCFoodDeliveryApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KFCFoodDeliveryApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        private ObservableCollection<ShoppingCartItem> ShoppingCartCollection;
        public CartPage()
        {
            InitializeComponent();
            ShoppingCartCollection = new ObservableCollection<ShoppingCartItem>();
            GetShoppingCartItems();
            CalculateTotalPrice();
        }

        private async void CalculateTotalPrice()
        {
            var userId = Preferences.Get("user_id", 0);
            var totalPrice = await APIService.GetCartSubTotal(userId);
            LblTotalPrice.Text = totalPrice.subTotal.ToString();
        }

        private async void GetShoppingCartItems()
        {
            var userId = Preferences.Get("user_id", 0);
            var shoppingCartItems = await APIService.GetShoppingCartItems(userId);

            ShoppingCartCollection.Clear();
            foreach (var shippingCartItem in shoppingCartItems)
            {
                ShoppingCartCollection.Add(shippingCartItem);
            }
            LvShoppingCart.ItemsSource = ShoppingCartCollection;
        }

        private async void TapBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void TapClearCart_Tapped(object sender, EventArgs e)
        {
            var userId = Preferences.Get("user_id", 0);
            var response = await APIService.ClearShoppingCart(userId);
            if (response)
            {
                LvShoppingCart.ItemsSource = null;
                LblTotalPrice.Text = "0";
                await DisplayAlert("", "Your cart has been cleared", "OK");
            }
            else
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }

        private async void BtnProceed_Clicked(object sender, EventArgs e)
        {
            double orderTotal = 0;
            double.TryParse(LblTotalPrice.Text, out orderTotal);
            await Navigation.PushModalAsync(new PlaceOrderPage(orderTotal));
        }
    }
}