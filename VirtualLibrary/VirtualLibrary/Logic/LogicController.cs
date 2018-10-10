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

namespace VirtualLibrary
{
    public class LogicController//TODO:Reikia metodu failu su duomenim sukurimui ir loadinimui i kazkokius tai listus, parametras - prisijungimo vardas
    {//TODO: duomenis krauti i StaticData klase, sukurti ten reikiamus duomenu tipus
        public LogicController() //TODO: LOAD USER DATA; LOAD ALL BOOKS ;; SAVE USER DATA; SAVE ALL BOOKS
        {
          
        }

        public int TempDirectoryController(string action, string name, Bitmap face, int iterator)
        {
            if (action == "Create")
            {
                try
                {
                    Directory.CreateDirectory(Application.StartupPath + "/" + name + "Temp");
                    face.Save(Application.StartupPath + "/" + name + "Temp" + "/" + name + "" + iterator + ".jpg");

                }
                catch (Exception e)
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
                catch(Exception e)
                {
                    return 1;
                }
            }
            return 0;
        }


    }
}