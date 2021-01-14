using HHP.Models;
using HHP.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace HHP.ViewModels
{
    class PurchaseDetailViewModel : BaseViewModel
    {
        public ObservableCollection<String> PurchaseUsersInvolved { get; set; }
        public String PurchaseTitle { get; set; }
        public String PurchaseDate { get; set; }
        public ImageSource Image_ReceiptImage { get; set; }
        public String PurchasePrice { get; set; }
        public String PurchaseUserPayed { get; set; }


        public PurchaseDetailViewModel(Purchase purchase)
        {
            Title = $"Einkauf: {purchase.Title}";

            PurchaseTitle = purchase.ToString();

            PurchasePrice = $"{purchase.Price}€";
            PurchaseUserPayed = purchase.UserPayed;

            PurchaseUsersInvolved = new ObservableCollection<string>();
            if (purchase.UsersInvolved != null)
            {
                foreach (String username in purchase.UsersInvolved)
                {
                    PurchaseUsersInvolved.Add(username);
                }
            }

            if (purchase.Receipt != null)
            {
                if (purchase.Receipt.ImageUrl != null || purchase.Receipt.ImageUrl != "")
                {
                    try
                    {
                        Image_ReceiptImage = ImageSource.FromUri(new Uri(purchase.Receipt.ImageUrl));
                    }
                    catch (Exception)
                    {
                    }
                }
            }


        }


    }
}
