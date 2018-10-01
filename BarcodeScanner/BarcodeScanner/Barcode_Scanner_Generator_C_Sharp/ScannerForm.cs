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

namespace BarcodeScanner
{
    public partial class ScannerForm : Form
    {
        public ScannerForm()
        {
            InitializeComponent();
        }
        Result result;
        BarcodeDecoder Scanner;
        OpenFileDialog OD;
        SaveFileDialog SD;

        //Barkodo išsaugojimo f-cija administratoriui
        private void saveButtonClick(object sender, EventArgs e)
        {
            SD = new SaveFileDialog();
            SD.Filter = "PNG File|*.png";
            if (SD.ShowDialog() == DialogResult.OK)
                pictureBox.Image.Save(SD.FileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        //Kol kas reikia įkelti barkodo foto iš folderio (Barcode images) 
        private void scanButtonClick(object sender, EventArgs e)
        {
            OD = new OpenFileDialog();
            OD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if(OD.ShowDialog()==DialogResult.OK)
                pictureBox.Load(OD.FileName);
            Scanner = new BarcodeDecoder();
            result = Scanner.Decode(new Bitmap(pictureBox.Image)); //Išsaugoma barkodo informacija
            MessageBox.Show(result.Text); //Atskirai parodo nuskenuoto barkodo skaičius
        }
    }
}
