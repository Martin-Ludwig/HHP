using HHP.Database;
using HHP.Models;
using HHP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


/* Haushaltsplaner
 * 
 * Page to view all purchase receipts from household
 * 
 */

namespace HHP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchasePage : ContentPage
    {
        PurchaseViewModel viewModel;
        
        public PurchasePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new PurchaseViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = viewModel = new PurchaseViewModel();
        }

        /// <summary>
        /// Shows a formular to add a new purchase
        /// Opens a new page
        /// </summary>
        public async void OpenNewPurchasePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPurchasePage());
        }

        /// <summary>
        /// Shows details of selected purchase
        /// Opens a new page
        /// </summary>
        public async void OpenPurchaseDetailsPage(object sender, EventArgs e)
        {
            Purchase clickedElement = (Purchase)Listview_purchases.SelectedItem;
            await Navigation.PushAsync(new PurchaseDetailPage(clickedElement));
        }


    }
}