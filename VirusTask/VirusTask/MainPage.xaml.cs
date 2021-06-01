using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTask.Helper;
using Xamarin.Forms;

namespace VirusTask
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ClickRegister();
        }

        private void ClickRegister()
        {
            lableRegister.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushModalAsync(new SignPage());
                })
            });
        }

        private async void ClickAuthorize(object sender, EventArgs e)
        {
            string fullName = entryFullname.Text;
            string password = entryPassword.Text;

            //check not null
            if (String.IsNullOrEmpty(fullName) || String.IsNullOrEmpty(password))
            {
                CrossToastPopUp.Current.ShowToastMessage("Enter password and name!");
                return;
            }

            //check exist
            var exist = await FirebaseHelper.GetVirus(fullName);
            if (exist == null)
            {
                CrossToastPopUp.Current.ShowToastMessage("User doesn't exist!");
                return;
            }

            //check password
            if (exist.password != password)
            {
                CrossToastPopUp.Current.ShowToastMessage("Incorrect password!");
                return;
            }

            Session.shared.virus = exist;
            Application.Current.MainPage = new NavigationPage(new TabbedVirusPage());
        }
    }
}
