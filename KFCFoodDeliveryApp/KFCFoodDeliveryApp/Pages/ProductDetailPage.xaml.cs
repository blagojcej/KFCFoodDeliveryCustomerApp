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
    public partial class ProductDetailPage : ContentPage
    {
        private readonly int _productId;

        public ProductDetailPage(int productId)
        {
            InitializeComponent();
            GetProductDetails(productId);
            this._productId = productId;
        }

        private async void GetProductDetails(int productId)
        {
            var product = await APIService.GetProductById(productId);
            if (product != null)
            {
                LblName.Text = product.name;
                LblDetail.Text = product.detail;
                ImgProduct.Source = product.FullImageUrl;
                LblPrice.Text = product.price.ToString();
                LblTotalPrice.Text = product.price.ToString();
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

        private void TapDecrement_Tapped(object sender, EventArgs e)
        {
            int qty;
            decimal price;
            int.TryParse(LblQty.Text, out qty);
            decimal.TryParse(LblPrice.Text, out price);
            qty--;
            if (qty < 1)
            {
                return;
            }
            LblQty.Text = qty.ToString();
            LblTotalPrice.Text = (qty * price).ToString();
        }

        private void TapIncrement_Tapped(object sender, EventArgs e)
        {
            int qty;
            decimal price;
            int.TryParse(LblQty.Text, out qty);
            decimal.TryParse(LblPrice.Text, out price);
            qty++;
            LblQty.Text = qty.ToString();
            LblTotalPrice.Text = (qty * price).ToString();
        }

        private async void BtnAddToCart_Clicked(object sender, EventArgs e)
        {
            var addToCart = new AddToCart();
            addToCart.CustomerId = Preferences.Get("user_id", 0);
            addToCart.Qty = LblQty.Text;
            addToCart.Price = LblPrice.Text;
            addToCart.TotalAmount = LblTotalPrice.Text;
            addToCart.ProductId = _productId;

            var response = await APIService.AddItemsToCart(addToCart);
            if(response)
            {
                await DisplayAlert("", "Your items has been added to the cart", "OK");
            }
            else
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }
    }
}