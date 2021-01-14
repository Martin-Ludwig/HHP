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
//using System.Random;

namespace HHP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterHouseholdPage : ContentPage
    {
        public string Username { set; get; }
        public string HashedPassword { set; get; }
        public RegisterHouseholdPage()
        {
            InitializeComponent();
        }

        async private void CreateHouseholdButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RegisterHousholdEntry.Text))
            {
                await DisplayAlert("Falsche Eingabe", "Bitte geben Sie einen Namen ein.", "Zurück zum Haushalt");
            }
            else
            {
                HaushaltsplanerDB db = new HaushaltsplanerDB();
                Random rnd = new Random();
                int _randomId, _idLength;
                string _finalId;
                Household household = new Household();
                do
                {
                    _randomId = rnd.Next(1, 9999);
                    _idLength = _randomId.ToString().Length;
                    _finalId = String.Empty;
                    for (int i = 0; i < (4 - _idLength); i++)
                    {
                        _finalId = _finalId + "0";
                    }
                    _finalId = _finalId + _randomId.ToString();

                    household = new Household()
                    {
                        Name = RegisterHousholdEntry.Text,
                        HouseholdId = RegisterHousholdEntry.Text + "#" + _finalId
                    };
                } while (db.Household.Exists(household));


                User user = new User()
                {
                    Name = Username,
                    Password = HashedPassword,
                    HouseholdId = household.HouseholdId
                };


                db.Household.AddIfNotExists(household);
                db.User.AddIfNotExists(user);
                App.Current.MainPage = new LoginPage();
            }
            
        }

        async private void JoinHouseholdButton_Clicked(object sender, EventArgs e)
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();

            // Household tmp = db.Household.FindByHouseholdId(JoinHouseholdEntry.Text);
            if (db.Household.Exists(new Household() { HouseholdId = JoinHouseholdEntry.Text }))
            {
                User user = new User()
                {
                    Name = Username,
                    Password = HashedPassword,
                    HouseholdId = JoinHouseholdEntry.Text
                };
                db.User.AddIfNotExists(user);
                App.Current.MainPage = new LoginPage();
            }
            else
            {
                //tmp = null;
                await DisplayAlert("Falsche Eingabe", "Dieser Haushalt existiert nicht.", "Zurück zum Haushalt");

            }

            
        }
    }
}