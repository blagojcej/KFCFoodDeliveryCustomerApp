using KFCFoodDeliveryApp.Pages;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KFCFoodDeliveryApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var access_token = Preferences.Get("access_token", string.Empty);
            if(string.IsNullOrEmpty(access_token))
            {
                MainPage = new NavigationPage(new SignupPage());
            }
            else
            {
                MainPage = new NavigationPage(new HomePage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
