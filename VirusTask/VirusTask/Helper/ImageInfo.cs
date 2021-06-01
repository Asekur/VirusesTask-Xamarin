using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace VirusTask.Helper
{
    public class ImageInfo : INotifyPropertyChanged
    {

        private string image;

        public string Image
        {
            get
            {
                return image;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageInfo(string _image)
        {
            image = _image;
        }

        public ImageSource imageSourceGallery
        {
            get { return ImageSource.FromUri(new Uri(image)); }
        }

        private void RaisePropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
