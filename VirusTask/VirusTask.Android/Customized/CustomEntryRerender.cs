using Android.Content;
using VirusTask.Customized;
using VirusTask.Droid.Customized;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRerender))]
namespace VirusTask.Droid.Customized
{
    public class CustomEntryRerender : EntryRenderer
    {
        public CustomEntryRerender(Context context) : base (context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}