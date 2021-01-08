using KFCFoodDeliveryApp.Models;
using KFCFoodDeliveryApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KFCFoodDeliveryApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceOrderPage : ContentPage
    {
        private readonly double totalPrice;

        public PlaceOrderPage(double totalPrice)
        {
            InitializeComponent();
            this.totalPrice = totalPrice;
        }

        private async void BtnPlaceOrder_Clicked(object sender, EventArgs e)
        {
            var order = new Order();
            order.fullName = EntName.Text;
            order.address = EntAddress.Text;
            order.phone = EntPhone.Text;
            order.userId = Preferences.Get("user_id", 0);
            order.orderTotal = totalPrice;

            var response = await APIService.PlaceOrder(order);
            if(response != null)
            {
                await DisplayAlert("", "Your order number is "+response.orderId, "OK");
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }

        private async void TapBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}