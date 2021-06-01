using System;
using System.Collections.Generic;
using System.Text;

namespace VirusTask.Helper
{
    public class Virus
    {
        public string uid;
        public string fullName;
        public string country;
        public string continent;
        public string year;
        public string photo_link;
        public string mortality;
        public string domain;       
        public string password;
        public string video_link;

        public Virus (string uid = "", string fullName = "", string country = "", string continent = "",
            string year = "", string photo_link = "", string mortality = "",
            string domain = "", string password = "", string video_link = "https://archive.org/download/ElephantsDream/ed_1024.mp4")
        {
            this.uid = uid;
            this.fullName = fullName;
            this.country = country;
            this.continent = continent;
            this.year = year;
            this.photo_link = photo_link;
            this.mortality = mortality;
            this.domain = domain;
            this.password = password;
            this.video_link = video_link;
        }
    }
}
