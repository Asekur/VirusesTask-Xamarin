using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTask.Helper;
using VirusTask.Resx;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiltersPage : ContentPage
    {
        const double stepValue = 1.0;

        private double _resultDomain = 1;
        private double _resultMortality = 0;
        private double _resultYear = 1800;

        public FiltersPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            sliderDomain.ValueChanged += OnSliderDomainValueChanged;
            sliderMortality.ValueChanged += OnSliderMortalityValueChanged;
            sliderYear.ValueChanged += OnSliderYearValueChanged;
        }

        void OnSliderYearValueChanged(object sender, ValueChangedEventArgs e)
        {
            var slider = (Slider)sender;
            _resultYear = (int)slider.Value * 2 + 1800;

            textFilterYear.Text = _resultYear.ToString();
        }

        void OnSliderMortalityValueChanged(object sender, ValueChangedEventArgs e)
        {
            var slider = (Slider)sender;
            _resultMortality = (int)slider.Value;

            textFilterMortality.Text = _resultMortality + "%";
        }

        void OnSliderDomainValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / stepValue);
            var slider = (Slider)sender;
            slider.Value = newStep * stepValue;
            _resultDomain = slider.Value;


            //change text
            textFilterDomain.Text = slider.Value == 0 ? AppResources.resBacteria : AppResources.resVirus; 
        }

        private async void ClickFilter(object sender, EventArgs e)
        {
            List<Virus> list = await FirebaseHelper.FetchViruses();

            var domain = _resultDomain.Equals(1.0) ? "Virus" : "Bacteria";
            List<Virus> resultViruses = list.FindAll((virus) =>
            {
                Int32.TryParse(virus.mortality.Replace("%", ""), out int mortality);
                Int32.TryParse(virus.year, out int year);
                return virus.domain == domain && mortality > _resultMortality && year > _resultYear;
            });

            //result
            if (resultViruses != null)
            {
                if (resultViruses.Count == 0)
                {
                    CrossToastPopUp.Current.ShowToastMessage("No results");
                    return;
                }
                else if (resultViruses.Count == 1)
                {
                    Navigation.PushModalAsync(new DetailPage(resultViruses[0]));
                }
                else
                {
                    //go to itemsPage
                     Navigation.PushModalAsync(new FiltersItemsPage(resultViruses));
                }
            } 
        }
    }
}