using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTask.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        Virus virus;
        public DetailPage(Virus virus)
        {
            InitializeComponent();
            this.virus = virus;
            NavigationPage.SetHasNavigationBar(this, false);
            SetupFields();
        }

        private void SetupFields()
        {
            imageDetail.Source = ImageSource.FromUri(new Uri(virus.photo_link));
            videoDetail.Source = virus.video_link;
            detailFullName.Text = virus.fullName + ": " + virus.domain;
            detailLocation.Text = virus.continent + " , " + virus.country;
            detailCharacter.Text = virus.year + " , " + virus.mortality;
        }

        private void ShowGallery(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new GalleryPage(virus.uid));
        }

        
    }
}