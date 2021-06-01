using VirusTask.Themes;
using Xamarin.Forms;
using static VirusTask.App;

[assembly: Dependency(typeof(VirusTask.Droid.ThemeHelper))]
namespace VirusTask.Droid
{
    public class ThemeHelper : IAppTheme
    {
        public void SetAppTheme(App.Theme theme)
        {

            SetTheme(theme);
        }
        void SetTheme(Theme mode)
        {
            if (mode == Theme.Dark)
            {
                if (App.AppTheme == Theme.Dark)
                    return;
                App.Current.Resources = new DarkTheme();
            }
            else
            {
                if (App.AppTheme != Theme.Dark)
                    return;
                App.Current.Resources = new LightTheme();
            }
            App.AppTheme = mode;
        }
    }
}