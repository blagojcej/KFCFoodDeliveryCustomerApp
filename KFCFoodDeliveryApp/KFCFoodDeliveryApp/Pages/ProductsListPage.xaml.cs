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
    public partial class ProductsListPage : ContentPage
    {
        public ObservableCollection<ProductByCategory> ProductByCategoryCollection;
        public ProductsListPage(int categoryId, string categoryName)
        {
            InitializeComponent();
            ProductByCategoryCollection = new ObservableCollection<ProductByCategory>();
            LblCategoryName.Text = categoryName;
            GetProducts(categoryId);
        }

        private async void GetProducts(int categoryId)
        {
            ProductByCategoryCollection.Clear();
            var products = await APIService.GetProductByCategory(categoryId);
            foreach (var product in products)
            {
                ProductByCategoryCollection.Add(product);
            }
            CvProducts.ItemsSource = ProductByCategoryCollection;
        }

        private async void TapBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current_selection = e.CurrentSelection.FirstOrDefault() as ProductByCategory;
            if (current_selection == null) return;

            Navigation.PushModalAsync(new ProductDetailPage(current_selection.id));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}