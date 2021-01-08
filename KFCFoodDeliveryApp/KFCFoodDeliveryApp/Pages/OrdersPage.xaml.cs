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
    public partial class OrdersPage : ContentPage
    {
        private ObservableCollection<OrderByUser> OrdersCollection;
        public OrdersPage()
        {
            InitializeComponent();
            OrdersCollection = new ObservableCollection<OrderByUser>();
            GetOrderItems();
        }

        private async void GetOrderItems()
        {
            OrdersCollection.Clear();
            var userId = Preferences.Get("user_id", 0);
            var orders = await APIService.GetOrdersByUser(userId);
            foreach (var order in orders)
            {
                OrdersCollection.Add(order);
            }

            LvOrders.ItemsSource = OrdersCollection;
        }

        private async void TapBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void LvOrders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedOrder = e.SelectedItem as OrderByUser;
            if (selectedOrder != null)
            {
                await Navigation.PushModalAsync(new OrderDetailsPage(selectedOrder.id));
            }
            ((ListView)sender).SelectedItem = null;
        }
    }
}