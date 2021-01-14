using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using HHP.Database;
using HHP.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using HHP.Views;

namespace HHP.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private List<User> _household_users;
        private Dictionary<string, double> _claim_dictionary;
        private Dictionary<string, double> _debt_dictionary;
        public ObservableCollection<User> HouseholdUsers { get; set; }
        public ObservableCollection<UserClaim> Claim { get; set; }
        public ObservableCollection<UserDebt> Debt { get; set; }
        public double Netto { get; set; }
        public HomeViewModel()
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            Session.Household = db.Household.FindByHouseholdId(Session.Household.HouseholdId);

            Title = "Startseite";

            HouseholdUsers = new ObservableCollection<User>();
            Claim = new ObservableCollection<UserClaim>();
            Debt = new ObservableCollection<UserDebt>();
            _claim_dictionary = new Dictionary<string, double>();
            _debt_dictionary = new Dictionary<string, double>();
            Netto = 0;

            _household_users = FindAllHouseholdUsers();

            // Fills the listview with data
            PopulateHouseholdUsers();

            CostCalculation();

        }

        /// <summary>
        /// Returns a list of users that joined the household
        /// </summary>
        /// <returns></returns>
        private List<User> FindAllHouseholdUsers()
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            List<User> household_users = db.User.FindAllUserFromHousehold(Session.Household.HouseholdId);
            return household_users;
        }

        /// <summary>
        /// fills the householdUsers with data
        /// </summary>
        private void PopulateHouseholdUsers()
        {
            foreach (User u in _household_users)
            {
                HouseholdUsers.Add(u);
            }
        }


        /// <summary>
        /// Calculates the costs of claim and debt
        /// </summary>
        public void CostCalculation()
        {
            // CLAIM
            Claim.Clear();
            foreach (Purchase purchase in Session.Household.Purchases)
            {
               if (purchase.UserPayed.Contains(Session.User.Name))
                {
                    foreach(string user in purchase.UsersInvolved)
                    {
                        if (user != Session.User.Name)
                        {
                            if (!purchase.UsersInvolvedPayed.Contains(user))
                            {
                                if (!_claim_dictionary.ContainsKey(user))
                                {
                                    _claim_dictionary.Add(user, purchase.Price / purchase.UsersInvolved.Count);
                                }
                                else
                                {
                                    _claim_dictionary[user] += (purchase.Price / purchase.UsersInvolved.Count);
                                }
                            }
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, double> entry in _claim_dictionary)
            {
                Claim.Add(new UserClaim() { Name = entry.Key, Total = entry.Value });
                Netto += entry.Value;
            }

            // DEBT
            Debt.Clear();
            foreach (Purchase purchase in Session.Household.Purchases)
            {
                if (purchase.UsersInvolved.Contains(Session.User.Name) && !purchase.UsersInvolvedPayed.Contains(Session.User.Name))
                {
                    if (!_debt_dictionary.ContainsKey(purchase.UserPayed))
                    {
                        _debt_dictionary.Add(purchase.UserPayed, purchase.Price / purchase.UsersInvolved.Count);
                    }
                    else
                    {
                        _debt_dictionary[purchase.UserPayed] += (purchase.Price / purchase.UsersInvolved.Count);
                    }
                }
            }
            foreach (KeyValuePair<string, double> entry in _debt_dictionary)
            {
                Debt.Add(new UserDebt() { Name = entry.Key, Total = entry.Value });
                Netto -= entry.Value;
            }
        }
        public class UserClaim
        {
            public string Name { get; set; }
            public double Total { get; set; }
            public Purchase purchase { get;set; }

            public override string ToString()
            {
                return $"{Name} schuldet dir {Total:F2}€";
            }
        }

        public class UserDebt
        {
            public string Name { get; set; }
            public double Total { get; set; }
            public Purchase purchase { get; set; }

            public override string ToString()
            {
                return $"Du schuldest {Name} noch {Total:F2}€";
            }
        }
    }
}