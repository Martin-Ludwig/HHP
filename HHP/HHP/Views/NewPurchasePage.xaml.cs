using HHP.Database;
using HHP.Models;
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
using HHP.Utils;
using Windows.System;
using System.Collections.ObjectModel;
using User = HHP.Models.User;
using HHP.ViewModels;

/* Haushaltsplaner
 * 
 * Page to add a new purchase receipt
 * 
 * 
 */

namespace HHP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPurchasePage : ContentPage
    {
        NewPurchaseViewModel viewModel;

        public NewPurchasePage()
        {
            InitializeComponent();
            BindingContext = this.viewModel = new NewPurchaseViewModel();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CollectionView_purchase_involved_users.SelectedItem = null;
            CollectionView_purchase_involved_users.SelectedItems = null;
        }

        public async void InsertPurchase(object sender, EventArgs e)
        {

            if (!IsFormValid())
            {
                await DisplayAlert("Fehler", "Bitte geben Sie ein Titel und ein gültigen Preis an.", "Ok");
                return;
            }


            // create username list from selected users in collection view CollectionView_purchase_involved_users
            List<String> _usersInvolved = new List<String>();
            foreach (var item in CollectionView_purchase_involved_users.SelectedItems)
            {
                User selectedUser = (User)item;
                _usersInvolved.Add(selectedUser.Name);
            }

            // add payed users to involved
            //if (!_usersInvolved.Contains(((User)Picker_purchase_payed_by.SelectedItem).Name))
            //{
            //    _usersInvolved.Add(((User)Picker_purchase_payed_by.SelectedItem).Name);
            //}

            // add payed users to payed list
            List<string> _usersInvolvedPayed = new List<string>();
            _usersInvolvedPayed.Add(((User)Picker_purchase_payed_by.SelectedItem).Name);

            // get image
            Receipt receipt = new Receipt();
            try
            {
                ImageSource img = Image_purchase_receipt.Source;
                if ((viewModel.ReceiptImageLocalPath != null || viewModel.ReceiptImageLocalPath != "" ) &&
                    (viewModel.Image_ReceiptImage != null))
                {
                    viewModel.ReceiptImageS3Path = await S3.Upload(viewModel.ReceiptImageLocalPath, viewModel.Image_ReceiptImage.Id.ToString());
                    receipt.ImageUrl = $"{viewModel.ReceiptImageS3Path}";
                }
            }
            catch (Exception)
            {

            }


            Purchase newPurchase = new Purchase()
            {
                Title = Entry_purchase_title.Text,
                Price = double.Parse(Entry_purchase_price.Text),
                Date = DatePicker_purchase_day.Date,
                UserPayed = ((User)Picker_purchase_payed_by.SelectedItem).Name,
                UsersInvolved = _usersInvolved,
                UsersInvolvedPayed = _usersInvolvedPayed,
                Receipt = receipt
            };
            HaushaltsplanerDB db = new HaushaltsplanerDB();

            // sync session with database
            Session.Household = db.Household.FindByHouseholdId(Session.Household.HouseholdId);
            // add new purchase to session
            Session.Household.AddPurchase(newPurchase);
            // update database with session
            db.Household.Update(Session.Household);

            // Send message to Purchase Page
            // updates listview with new purchase
            MessagingCenter.Send(this, "AddPurchase", newPurchase);

            CollectionView_purchase_involved_users.SelectedItem = null;
            CollectionView_purchase_involved_users.SelectedItems = null;
            Navigation.PopAsync();
        }

        /// <summary>
        /// Tests if every Form value has valid values.
        /// </summary>
        public bool IsFormValid()
        {

            if (Entry_purchase_title.Text == null || Entry_purchase_title.Text == "")
            {
                return false;
            }

            // Check for Red Textcolor
            // PriceValidationBehavior sets Textcolor
            if (Entry_purchase_price.TextColor == HHP_Colors.Error)
            {
                return false;
            }

            return true;
        }

        public async void CancelPage(object sender, EventArgs e)
        {
            CollectionView_purchase_involved_users.SelectedItem = null;
            CollectionView_purchase_involved_users.SelectedItems = null;
            await Navigation.PopAsync();
        }

        private async void TakePhotoButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                String imgpath = await Camera.TakePhoto();
                if (imgpath != null)
                {
                    viewModel.ReceiptImageLocalPath = imgpath;
                    viewModel.Image_ReceiptImage = Camera.LoadPhoto(imgpath);
                    Image_purchase_receipt.Source = viewModel.Image_ReceiptImage;
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Fehler", "Keine Kamera gefunden.", "Ok");
            } 


        }

    }
}