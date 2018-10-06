using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingToolkit.Barcode;

namespace VirtualLibrary
{
    public partial class ScannerWindow : Form
    {
        public ScannerWindow()
        {
            InitializeComponent();
        }

        BarcodeDecoder Scanner;
        OpenFileDialog OD;
        SaveFileDialog SD;

        //TODO: Įdėti mygtuką, grąžinantį į MainWindow

        //Tik administratoriaus profilyje
        //Išsaugojimo f-cija
        /*
        private void saveButtonClick(object sender, EventArgs e)
        {
            SD = new SaveFileDialog();
            SD.Filter = "PNG File|*.png";
            if (SD.ShowDialog() == DialogResult.OK)
                pictureBox.Image.Save(SD.FileName, System.Drawing.Imaging.ImageFormat.Png);
        }
        */

        private void scanButtonClick(object sender, EventArgs e)
        {
            OD = new OpenFileDialog();
            OD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (OD.ShowDialog() == DialogResult.OK)
                pictureBox.Load(OD.FileName);

            //Skenavimas
            Scanner = new BarcodeDecoder();
            Result result = Scanner.Decode(new Bitmap(pictureBox.Image));
            //Atskirai parodo nuskenuoto barkodo skaičius
            MessageBox.Show(result.Text); 
        }
    }
}
