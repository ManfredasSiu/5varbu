using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
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
            LoadFaceData(); 
        }

        public void LoadFaceData()//Loadina veidus
        {
            try
            {
                string labelsInf = File.ReadAllText(Application.StartupPath + "/faces/faces.txt");
                string[] Labels = labelsInf.Split(',');

                int.TryParse(Labels[0], out StaticData.numLablels);
                string FacesLoad;
                for (int i = 1; i <= StaticData.numLablels; i++)
                {
                    FacesLoad = "face" + i + ".bmp";
                    StaticData.training.Add(new Image<Gray, byte>(Application.StartupPath + $"/faces/{FacesLoad}"));
                    StaticData.labels.Add(Labels[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nothing in the database");
            }
        }

        public void SaveFaceData()
        {
            File.WriteAllText(Application.StartupPath + "/faces/faces.txt", StaticData.training.ToArray().Length + ",");
            for (int i = 1; i <= StaticData.numLablels; i++)
            {
                StaticData.training.ToArray()[i - 1].Save(Application.StartupPath + "/faces/face" + i + ".bmp");
                File.AppendAllText(Application.StartupPath + "/faces/faces.txt", StaticData.labels.ToArray()[i - 1] + ",");
            }
        }

        public void LoadBooksData()
        {
            string labelsInfo = File.ReadAllText(Application.StartupPath + "/books/books.txt");
            string[] Labels = labelsInfo.Split(',');
            for (int x = 0; x < Labels.Count(); x = +4)
            {
                StaticData.Books.Add(new Book(Labels[x], Labels[x+1], Labels[x+2], Labels[x+3]));
            }
        }


    }
}
