using HHP.Models;
using HHP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HHP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseDetailPage : ContentPage
    {
        PurchaseDetailViewModel viewModel;

        public PurchaseDetailPage(Purchase purchase)
        {
            InitializeComponent();
            BindingContext = viewModel = new PurchaseDetailViewModel(purchase);

        }
    }
}