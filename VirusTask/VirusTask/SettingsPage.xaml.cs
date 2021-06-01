using Plugin.Toast;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static VirusTask.App;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        
        const double stepValue = 1.0;
        const int initialSize = 16;

        public SettingsPage()
        {
            InitializeComponent();
            sliderMode.Value = App.AppTheme == Theme.Dark ? 0 : 1;
            sliderMode.ValueChanged += OnSliderValueModeChanged;
            sliderFont.ValueChanged += OnSliderValueFontChanged;
        }

        void OnSliderValueModeChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / stepValue);
            var slider = (Slider)sender;
            slider.Value = newStep * stepValue;
            if (slider.Value == 1)
            {
                ChangeTheme(Theme.Light);
                return;
            } else
            {
                ChangeTheme(Theme.Dark);
               return;
            }
        }

        void ChangeLanguage(object sender, EventArgs e)
        {
            CultureInfo.CurrentUICulture = CultureInfo.CurrentUICulture.Name == "ru" ? new CultureInfo("en-US") : new CultureInfo("ru");
            Application.Current.MainPage = new NavigationPage(new TabbedVirusPage());
        }

        void OnSliderValueFontChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / stepValue);
            var slider = (Slider)sender;
            slider.Value = newStep * stepValue;

            btnEnglish.FontSize = initialSize + slider.Value;
            btnRussia.FontSize = initialSize + slider.Value;
            btnLogout.FontSize = initialSize + slider.Value;
            labelDarkmode.FontSize = initialSize + slider.Value;
            labelFont.FontSize = initialSize + slider.Value;
        }

        private void ChangeTheme(Theme themeRequested)
        {
            try
            {
                DependencyService.Get<IAppTheme>().SetAppTheme(themeRequested);
                Application.Current.MainPage = new NavigationPage(new TabbedVirusPage());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void LogoutVirus(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}