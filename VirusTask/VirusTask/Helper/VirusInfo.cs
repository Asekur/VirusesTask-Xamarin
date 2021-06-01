using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using VirusTask.Helper;
using Xamarin.Forms;

namespace VirusTask.Models
{
    public class VirusInfo : INotifyPropertyChanged
    {
        private Virus virus;

        public Virus Virus
        {
            get
            {
                return virus;
            }
        }

        public string Name
        {
            get { return virus.fullName; }
        }

        public string DomainAndYear
        {
            get { return virus.domain + ", " + virus.year; }
        }

        public ImageSource imageSource
        {
            get { return ImageSource.FromUri(new Uri(virus.photo_link)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public VirusInfo(Virus _virus)
        {
            virus = _virus;
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
