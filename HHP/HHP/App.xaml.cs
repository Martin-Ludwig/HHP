using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HHP.Services;
using HHP.Views;
using HHP.Models;
using HHP.Database;
using Amazon;
using HHP.Utils;

namespace HHP
{
    public partial class App : Application
    {


        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new LoginPage());
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
