using Firebase.Database;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTask.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        MediaFile file;
        private bool imageChanged = false;
        public EditPage()
        {
            InitializeComponent();
            GetValues();
            ClickSave();
        }

        private void GetValues()
        {
            editFullName.Text = Session.shared.virus.fullName;
            editCountry.Text = Session.shared.virus.country;
            editContinent.Text = Session.shared.virus.continent;
            editYear.Text = Session.shared.virus.year;
            editDomain.Text = Session.shared.virus.domain;
            editMortality.Text = Session.shared.virus.mortality;
            editVideolik.Text = Session.shared.virus.video_link;
            editPassword.Text = Session.shared.virus.password;
            virusImageEdit.Source = ImageSource.FromUri(new Uri(Session.shared.virus.photo_link));
        }

        private async void ImagePickerEdit(object sender, EventArgs e)
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
                virusImageEdit.Source = ImageSource.FromStream(() =>
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

        private void ClickSave()
        {
            btnSave.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    string fullName = editFullName.Text;
                    string password = editPassword.Text;
                    Console.WriteLine("[EditPage] Fullname: " + fullName + " Password: " + password);

                    //check not null
                    if (String.IsNullOrEmpty(fullName) || String.IsNullOrEmpty(password))
                    {
                        CrossToastPopUp.Current.ShowToastMessage("Enter password and name!");
                        return;
                    }

                    var virusFr = await FirebaseHelper.GetVirus(fullName);
                    //check exist fullname
                    if (virusFr != null && virusFr.fullName != Session.shared.virus.fullName)
                    {
                        CrossToastPopUp.Current.ShowToastMessage("User already exists!");
                        return;
                    }

                    string continent = String.IsNullOrEmpty(editContinent.Text) ? "Continent" : editContinent.Text;
                    string country = String.IsNullOrEmpty(editCountry.Text) ? "Country" : editCountry.Text;
                    string year = String.IsNullOrEmpty(editYear.Text) ? "Year" : editYear.Text;
                    string domain = String.IsNullOrEmpty(editDomain.Text) ? "Domain" : editDomain.Text;
                    string mortality = String.IsNullOrEmpty(editMortality.Text) ? "Mortality" : editMortality.Text;
                    string videolink = String.IsNullOrEmpty(editVideolik.Text) ? "https://archive.org/download/ElephantsDream/ed_1024.mp4" : editVideolik.Text;

                    //save virus to firebase
                    var virus = new Virus(Session.shared.virus.uid, fullName, country, continent, year, Session.shared.virus.photo_link, mortality, domain, password, videolink);
                    var databaseRef = new FirebaseClient(Constants.databaseUrl);
                    if (imageChanged)
                    {
                        var url = await UploadStorage.UploadProfileFile(file.GetStream(), Path.GetFileName(file.Path));
                        virus.photo_link = url;
                    }
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(virus);
                    await databaseRef.Child("Viruses/" + Session.shared.virus.uid).PutAsync(json);
                    Session.shared.virus = virus;
                    CrossToastPopUp.Current.ShowToastMessage("Success!");
                })
            });
        }
    }
}