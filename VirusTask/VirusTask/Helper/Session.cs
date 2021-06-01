using System;
using System.Collections.Generic;
using System.Text;

namespace VirusTask.Helper
{
    public class Session
    {
        public Virus virus = new Virus();
        public static Session shared = new Session();
    }
}
