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
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<PopularProduct> ProductsCollection;
        public ObservableCollection<Category> CategoriesCollection;
        public HomePage()
        {
            InitializeComponent();
            ProductsCollection = new ObservableCollection<PopularProduct>();
            CategoriesCollection = new ObservableCollection<Category>();
            GetPopularProducts();
            GetCategories();
            LblUserName.Text = Preferences.Get("user_name", string.Empty);
        }

        //Everytime we come to this page OnAppearing methods is calling, constructor is colled just one time
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var user_id = Preferences.Get("user_id", 0);
            var response = await APIService.GetTotalCartItems(user_id);
            LblTotalItems.Text = response.totalItems.ToString();
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            await CloseHamburgerMenu();
        }

        private async Task CloseHamburgerMenu()
        {
            //400 miliseconds to close side menu
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }

        private async void GetCategories()
        {
            CategoriesCollection.Clear();
            var categories = await APIService.GetCategories();
            foreach (var category in categories)
            {
                CategoriesCollection.Add(category);
            }
            CvCategories.ItemsSource = CategoriesCollection;
        }

        private async void GetPopularProducts()
        {
            ProductsCollection.Clear();
            var products = await APIService.GetPopularProducts();
            foreach (var product in products)
            {
                ProductsCollection.Add(product);
            }
            CvProducts.ItemsSource = ProductsCollection;
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            //400 miliseconds to open side menu
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }

        private async void TapCloseMenu_Tapped(object sender, EventArgs e)
        {
            await CloseHamburgerMenu();
        }

        private async void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current_selection = e.CurrentSelection.FirstOrDefault() as Category;
            if (current_selection == null) return;
            await Navigation.PushModalAsync(new ProductsListPage(current_selection.id, current_selection.name));
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current_selection = e.CurrentSelection.FirstOrDefault() as PopularProduct;
            if (current_selection == null) return;

            await Navigation.PushModalAsync(new ProductDetailPage(current_selection.id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void TapCartIcon_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CartPage());
        }

        private async void TapOrders_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new OrdersPage());
        }

        private async void TapContact_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ContactPage());
        }

        private async void TapCart_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CartPage());
        }

        private void TapLogout_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("access_token", string.Empty);
            Preferences.Set("token_exp_time", 0);
            Application.Current.MainPage = new NavigationPage(new SignupPage());
        }
    }
}