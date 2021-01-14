using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Windows.UI.Xaml.Media.Animation;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon;
using System.IO;
using Windows.Storage.Pickers.Provider;
using HHP.secrets;
using Amazon.S3.Transfer;
using Amazon.CognitoIdentity;
using System.Reflection;
using System.Threading;
using HHP.Models;
using HHP.Database;

namespace HHP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public SettingsPage()
        {
            InitializeComponent();

            HaushaltsplanerDB db = new HaushaltsplanerDB();
            List<User> household_users = db.User.FindAllUserFromHousehold(Session.Household.HouseholdId);

            Label_username.Text = Session.User.Name;
            Label_household.Text = Session.User.HouseholdId;
            Label_houshold_user.Text = string.Join(", ", household_users);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            List<User> household_users = db.User.FindAllUserFromHousehold(Session.Household.HouseholdId);

            Label_username.Text = Session.User.Name;
            Label_household.Text = Session.User.HouseholdId;
            Label_houshold_user.Text = string.Join(", ", household_users);
        }

    }
}