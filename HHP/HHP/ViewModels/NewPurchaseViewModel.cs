using HHP.Database;
using HHP.Models;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml;
using Xamarin.Forms;

namespace HHP.ViewModels
{
    class NewPurchaseViewModel : BaseViewModel
    {
        private List<User> _household_users;

        public ObservableCollection<User> HouseholdUsers { get; set; }
        public int PickerSelectedCurrentUser { get; set; }

        public ImageSource Image_ReceiptImage { get; set; }
        public String ReceiptImageLocalPath { get; set; }
        public String ReceiptImageS3Path { get; set; }

        public NewPurchaseViewModel()
        {
            Title = "Neuer Einkauf";
            HouseholdUsers = new ObservableCollection<User>();

            _household_users = FindAllHouseholdUsers();

            // Fills the listview with data
            PopulateHouseholdUsers();

            // Sets "Purchased By" picker to current user as default
            PickerSelectedCurrentUser = _household_users.FindIndex(user => user.Name == Session.User.Name);
        }

        private List<User> FindAllHouseholdUsers()
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            List<User> household_users = db.User.FindAllUserFromHousehold(Session.Household.HouseholdId);

            return household_users;
        }

        private void PopulateHouseholdUsers()
        {
            foreach (User u in _household_users)
            {
                HouseholdUsers.Add(u);
            }
        }

    }
}
