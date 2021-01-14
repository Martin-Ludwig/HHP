using HHP.Database;
using HHP.Models;
using HHP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HHP.ViewModels
{
    public class PurchaseViewModel : BaseViewModel
    {

        public ObservableCollection<Purchase> Purchases { get; set; }
        public PurchaseViewModel()
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            Session.Household = db.Household.FindByHouseholdId(Session.Household.HouseholdId);

            Title = "Einkäufe";
            Purchases = new ObservableCollection<Purchase>();

            PopulatePurchases();

            // Creates a subscriber that waits for new added purchases from New Purchase Page
            MessagingCenter.Subscribe<NewPurchasePage, Purchase>(this, "AddPurchase", (sender, arg) =>
            {
                // insert new purchase to listview
                Purchases.Insert(0, arg);
            });
        }

        /// <summary>
        /// Retrieves purchases from session household
        /// </summary>
        public void PopulatePurchases()
        {
            Purchases.Clear();
            List<Purchase> _purchases = Session.Household.Purchases;
            foreach (Purchase p in _purchases)
            {
                Purchases.Insert(0, p);
            }
        }





    }
}