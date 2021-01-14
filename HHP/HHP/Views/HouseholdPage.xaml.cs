using HHP.Models;
using HHP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HHP.Views
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HouseholdPage : ContentPage
    {
        public HouseholdViewModel viewModel;
        public HouseholdPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HouseholdViewModel();

            if (Device.RuntimePlatform == Device.UWP)
            {
                // windows
                RowDefition_Chart1.Height = 250;
                RowDefition_Chart2.Height = 250;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //android
                RowDefition_Chart1.Height = 200;
                RowDefition_Chart2.Height = 200;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DatePicker.Date = DateTime.Now;
            BindingContext = viewModel = new HouseholdViewModel();
        }


        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            Chart1.Chart = viewModel.ShowPieChart(DatePicker.Date);

            Chart2.Chart = viewModel.ShowPointChart(DatePicker.Date.Year);
        }
    }
}