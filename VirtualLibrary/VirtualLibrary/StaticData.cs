using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary
{
    static class StaticData  //Visi uzkrauti duomenys
    {
        public static List<Image<Gray, byte>> training = new List<Image<Gray, byte>>();
        public static List<string> labels = new List<string>();
        public static int numLablels;
        public static User CurrentUser;
        public static List<Book> Books = new List<Book>();
        //labas
    }
}
