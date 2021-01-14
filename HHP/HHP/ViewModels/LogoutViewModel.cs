using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HHP.ViewModels
{
    public class LogoutViewModel : BaseViewModel
    {
        public LogoutViewModel()
        {
            Title = "Abmelden";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}