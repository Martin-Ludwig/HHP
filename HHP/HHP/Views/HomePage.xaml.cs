using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HHP.Models;
using HHP.Database;
using HHP.Utils;
using HHP.ViewModels;
using UserClaim = HHP.ViewModels.HomeViewModel.UserClaim;
using User = HHP.Models.User;
using System.Threading;

namespace HHP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;
        HaushaltsplanerDB db;
        public HomePage()
        {
            db = new HaushaltsplanerDB();
            InitializeComponent();

            BindingContext = viewModel = new HomeViewModel();
        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            BindingContext = viewModel = new HomeViewModel();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CollectionView_purchase_involved_users.SelectedItem = null;
            CollectionView_purchase_involved_users.SelectedItems = null;
        }

        /// <summary>
        /// Geld erhalten Button
        /// Fügt alle ausgewählten User bei allen beteiligten Einkäufen zur Bezahlt Liste hinzu
        /// </summary>
        private void Button_Clicked(object sender, EventArgs e)
        {
            List<String> _usersInvolved = new List<String>();
            foreach (var item in CollectionView_purchase_involved_users.SelectedItems)
            {
                UserClaim usercost = (UserClaim)item;
                foreach (Purchase purchase in Session.Household.Purchases)
                {
                    if (purchase.UserPayed.Contains(Session.User.Name))
                    {
                        if (!purchase.UsersInvolvedPayed.Contains(usercost.Name))
                        {
                            purchase.UsersInvolvedPayed.Add(usercost.Name);
                        }
                    }
                }
            }
            db.Household.Update(Session.Household);
            viewModel.CostCalculation();
            App.Current.MainPage = new MainPage();

        }
    }
}