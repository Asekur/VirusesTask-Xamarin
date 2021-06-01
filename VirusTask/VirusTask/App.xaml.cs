using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirusTask
{
    public partial class App : Application
    {
        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }
        public static Theme AppTheme { get; set; }

        public enum Theme
        {
            Light,
            Dark
        }
        public App()
        {
            InitializeComponent();
            AppTheme = Theme.Light;
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
