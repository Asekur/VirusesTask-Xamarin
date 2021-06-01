using Firebase.Database;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toast;
using System;
using System.IO;
using VirusTask.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignPage : ContentPage
    {
        MediaFile file;
        private bool imageChanged = false;
        public SignPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void ImagePickerTappedAsync(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                    return;
                imageChanged = true;
                virusImageButton.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void RegisterVirus(object sender, EventArgs e)
        {
            string fullName = registerFullname.Text;
            string password = registerPassword.Text;
            Console.WriteLine("[SignPage] Fullname: " + fullName + " Password: " + password);

            //check not null
            if (String.IsNullOrEmpty(fullName) || String.IsNullOrEmpty(password))
            {
                CrossToastPopUp.Current.ShowToastMessage("Enter password and name!");
                return;
            }


            var virusFr = await FirebaseHelper.GetVirus(fullName);
            //check exist fullname
            if (virusFr != null)
            {
                CrossToastPopUp.Current.ShowToastMessage("User already exists!");
                return;
            }

            string continent = String.IsNullOrEmpty(registerContinent.Text) ? "Continent" : registerContinent.Text;
            string country = String.IsNullOrEmpty(registerCountry.Text) ? "Country" : registerCountry.Text;
            string year = String.IsNullOrEmpty(registerYear.Text) ? "Year" : registerYear.Text;
            string domain = String.IsNullOrEmpty(registerDomain.Text) ? "Domain" : registerDomain.Text;
            string mortality = String.IsNullOrEmpty(registerMortality.Text) ? "Mortality" : registerMortality.Text;

            //save virus to firebase
            var virus = new Virus(Guid.NewGuid().ToString(), fullName, country, continent, year, Constants.defaultImage, mortality, domain, password);
            var databaseRef = new FirebaseClient(Constants.databaseUrl);
            if (imageChanged)
            {
                var url = await UploadStorage.UploadProfileFile(file.GetStream(), Path.GetFileName(file.Path));
                virus.photo_link = url;
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(virus);
            await databaseRef.Child("Viruses/" + virus.uid).PutAsync(json);
            CrossToastPopUp.Current.ShowToastMessage("Created!");
            return;
        }
    }
}