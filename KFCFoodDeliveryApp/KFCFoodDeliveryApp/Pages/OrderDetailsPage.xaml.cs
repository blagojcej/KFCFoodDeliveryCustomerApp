using KFCFoodDeliveryApp.Models;
using KFCFoodDeliveryApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KFCFoodDeliveryApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        public ObservableCollection<OrderDetail> OrderDetailCollection;
        public OrderDetailsPage(int orderId)
        {
            InitializeComponent();
            OrderDetailCollection = new ObservableCollection<OrderDetail>();
            GetOrderDetail(orderId);
        }
        private async void GetOrderDetail(int orderId)
        {
            var orders = await APIService.GetOrderDetails(orderId);
            var orderDetails = orders[0].orderDetails;
            foreach (var item in orderDetails)
            {
                OrderDetailCollection.Add(item);
            }

            LvOrderDetail.ItemsSource = OrderDetailCollection;

            LblTotalPrice.Text = orders[0].orderTotal + " $ ";
        }

        private async void TapBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}