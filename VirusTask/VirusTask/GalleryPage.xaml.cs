using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTask.Helper;
using VirusTask.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPage
    {
        private string uid = "";
        MediaFile file;

        private List<string> listImage = new List<string>();
        ObservableCollection<ImageInfo> images = new ObservableCollection<ImageInfo>();
        public ObservableCollection<ImageInfo> Images { get { return images; } }

        public GalleryPage(string uid)
        {
            InitializeComponent();
            this.uid = uid;
            btnAddGallery.IsVisible = uid == Session.shared.virus.uid;
            NavigationPage.SetHasNavigationBar(this, false);
            
            listImages.ItemsSource = Images;
            FetchImages();

            AddPhoto();
        }

        private async void ImagePickerTappedAsync()
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
                var url = await UploadStorage.UploadProfileFile(file.GetStream(), Path.GetFileName(file.Path));
                if (String.IsNullOrEmpty(url))
                {
                    CrossToastPopUp.Current.ShowToastMessage("Something do wrong...");
                } else
                {
                    await FirebaseHelper.SaveVirusPhoto(Session.shared.virus.uid, url);
                    images.Add(new ImageInfo(url));
                    CrossToastPopUp.Current.ShowToastMessage("Added!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AddPhoto()
        {
            btnAddGallery.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    ImagePickerTappedAsync();
                })
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchImages();
        }

        private async void FetchImages()
        {
            var list = await FirebaseHelper.GetVirusGallery(uid);
            listImage.Clear();
            listImage = list;
            AddImageInfo(list);
        }

        private void AddImageInfo(List<string> items)
        {
            images.Clear();
            foreach (var item in items)
            {
                images.Add(new ImageInfo(item));
            }
        }
    }
}