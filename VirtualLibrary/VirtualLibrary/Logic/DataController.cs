﻿using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.API_s;

namespace VirtualLibrary
{
    public class DataController
    {

        public IDataB DB = new AzureDatabase();

        //Laikinosios direktorijos kontroleris. Sukurta direktorija skirta paruosti veidams sistemos treniravimui
        //Direktorija istrinama baigus registracijos procesa.
        public int TempDirectoryController(string action, string name, Bitmap face, int iterator)
        {
            if (action == "Create")
            {
                try
                {
                    Directory.CreateDirectory(Application.StartupPath + "/" + name + "Temp");
                    face.Save(Application.StartupPath + "/" + name + "Temp" + "/" + name + "" + iterator + ".jpg");
                }
                catch 
                {
                    return 1;
                }
            }
            else if(action == "Delete")
            {
                try
                {   
                    Directory.Delete(Application.StartupPath + "/" + name + "Temp", true);
                }
                catch
                {
                    return 1;
                }
            }
            return 0;
        }


    }
}
