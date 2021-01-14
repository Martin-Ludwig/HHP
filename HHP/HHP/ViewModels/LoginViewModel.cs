using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace HHP.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public LoginViewModel()
        {
            Title = "Anmelden";
        }
    }
}