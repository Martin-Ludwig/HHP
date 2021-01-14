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

namespace HHP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LoginViewModel();
        }

        async void RegisterButtonOnClick(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RegisterUserPage());
        }
        
        /// <summary>
        /// creates a new session when username and password are correct
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LoginButtonOnClick(object sender, EventArgs args)
        {
            // username and password required for login
            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text)) 
            {
                // error message
                await DisplayAlert("Fehlende Eingabe", "Bitte geben Sie Benutzernamen und Passwort ein.", "Zurück zum Login");
            }
            else
            {
                // MongoDB Connection
                HaushaltsplanerDB db = new HaushaltsplanerDB();
                string hashed_pwd = Crypto.ComputeSHA256(password.Text);  // password hashed (SHA256)
                User user = db.User.FindByName(username.Text); // instanciates user object from database
                //if user does not exist
                if (user == null)
                {
                    // error message
                    await DisplayAlert("Falsche Eingabe", "Falscher Benutzername oder Passwort.", "Zurück zum Login");
                    hashed_pwd = String.Empty;
                }
                else // if user exists
                {
                    // instanciates household object from database
                    Household household = db.Household.FindByHouseholdId(user.HouseholdId);
                    // if password and username are correct
                    if (user.Password == hashed_pwd)
                    {
                        // create a new session that can be accessed later on
                        Session Session = new Session(user, household);
                        App.Current.MainPage = new MainPage();
                    }
                    else // wrong password
                    {
                        // error message
                        await DisplayAlert("Falsche Eingabe", "Falscher Benutzername oder Passwort.", "Zurück zum Login");
                        hashed_pwd = String.Empty;
                        user = null;
                    }
                }
            }
        }
    }
}