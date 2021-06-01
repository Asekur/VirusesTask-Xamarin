using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedVirusPage : TabbedPage
    {
        public TabbedVirusPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}