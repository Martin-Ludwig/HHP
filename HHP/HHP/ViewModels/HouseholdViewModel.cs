using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using HHP.Models;
using System.Collections.Generic;
using Microcharts;
using HHP.Utils.Diagram;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml;
using System.Diagnostics;
using System.Collections.ObjectModel;
using HHP.Utils;
using HHP.Database;

namespace HHP.ViewModels
{
    public class HouseholdViewModel : BaseViewModel
    {
        private double _total;
        private string _currentMonth;
        private string _currentYear;
        public string CurrentMonth
        {
            get { return _currentMonth; }
            set
            {
                _currentMonth = value;
                OnPropertyChanged("CurrentMonth"); // Notify that there was a change on this property
            }
        }
        public string CurrentYear
        {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                OnPropertyChanged("CurrentYear"); // Notify that there was a change on this property
            }
        }


        public Chart CostChart { get; set; }
        public Chart YearChart { get; set; }
        public User User { get; set; }
        public Household Household { get; set; }
        public double Total 
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Total"); // Notify that there was a change on this property
            }
        }
        public Dictionary<String, Double> UsersTotal { get; set; }

        private List<User> _householdUsers;

        public HouseholdViewModel()
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            Session.Household = db.Household.FindByHouseholdId(Session.Household.HouseholdId);

            Title = $"Haushalt {Session.Household.Name}";
            _householdUsers = Session.HouseholdUsers;
            Total = 0;
            ShowPieChart(DateTime.Now);
            ShowPointChart(DateTime.Now.Year);
        }

        private Dictionary<String, Double> CalculateUserTotal(int year, int month)
        {
            Dictionary<String, Double> _usersTotal = new Dictionary<String, Double>();
            List <Purchase> _purchases = Session.Household.Purchases;

            foreach (Purchase purchase in _purchases)
            {
                if (purchase.Date.Year == year && purchase.Date.Month == month)
                {
                    foreach (String user in purchase.UsersInvolved)
                    {
                        if (_usersTotal.ContainsKey(user))
                        {
                            _usersTotal[user] += purchase.Price / purchase.UsersInvolved.Count;
                        }
                        else
                        {
                            _usersTotal[user] = purchase.Price / purchase.UsersInvolved.Count;
                        }
                    }
                }
            }

            return _usersTotal;
        }

        /// <summary>
        /// Returns a piechart with the costs of the month
        /// </summary>
        public Chart ShowPieChart(DateTime date)
        {
            CurrentYear = date.Year.ToString();
            CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);

            Total = 0;
            int _month = date.Month;
            int _year = date.Year;
            UsersTotal = CalculateUserTotal(_year, _month);
            List<Tuple<String, Double>> _usersTotal = new List<Tuple<String, Double>>();
            foreach (KeyValuePair<String, Double> usertotal in UsersTotal)
            {
                _usersTotal.Add(Tuple.Create(usertotal.Key, usertotal.Value));
                Total += usertotal.Value;
            }
            return CostChart = Diagram.CreatePieChart(_usersTotal);
        }




        /// <summary>
        /// Returns a List of User and Cost
        /// </summary>
        private Dictionary<String, Double> MonthlyCostsInYear(int year)
        {
            CurrentYear = year.ToString();

            Dictionary<String, Double> _result = new Dictionary<String, Double>();
            for (int i = 1; i <= 12; i++)
            {
                string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                _result[month] = 0;
            }

            

            List<Purchase> _purchases = Session.Household.Purchases;

            foreach (Purchase purchase in _purchases)
            {
                if (purchase.Date.Year == year)
                {
                    string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(purchase.Date.Month);

                    Double val = _result[month] + purchase.Price;
                    _result[month] = val;
                }
            }

            return _result;
        }

        /// <summary>
        /// Point Chart for yearly cost overview each month
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Chart ShowPointChart(int year)
        {
            Dictionary<String, Double> monthlyCostsInYear = MonthlyCostsInYear(year);
            
            List<Tuple<String, Double>> _data = new List<Tuple<String, Double>>();
            foreach (KeyValuePair<String, Double> month in monthlyCostsInYear)
            {
                _data.Add(Tuple.Create(month.Key, month.Value));
            }
            return YearChart = Diagram.YearChart(_data);
        }




    }
}