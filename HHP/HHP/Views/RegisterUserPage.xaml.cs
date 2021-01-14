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

namespace HHP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUserPage : ContentPage
    {
        public RegisterUserPage()
        {
            InitializeComponent();
        }

        async void CreateUserButtonOnClick(object sender, EventArgs args)
        {
            if (string.IsNullOrWhiteSpace(RegisterUsername.Text) || string.IsNullOrWhiteSpace(RegisterPassword.Text))
            {
                await DisplayAlert("Ungültige Eingabe", "Geben Sie einen Benutzernamen und ein Passwort ein.", "Zurück zur Registrierung");
            }
            else
            {
                HaushaltsplanerDB db = new HaushaltsplanerDB();
                //User tmp = db.User.FindByName(RegisterUsername.Text);
                if (db.User.Exists(new User() { Name = RegisterUsername.Text }))
                {
                    await DisplayAlert("Benutzername bereits vergeben", "Dieser Benutzername ist bereits vergeben. Bitte geben Sie einen anderen Namen ein.", "Zurück zur Registrierung");
                }
                else
                {
                    await Navigation.PushAsync(new RegisterHouseholdPage() { Username = RegisterUsername.Text, HashedPassword = Crypto.ComputeSHA256(RegisterPassword.Text) });
                }
            }
        }
    }
}