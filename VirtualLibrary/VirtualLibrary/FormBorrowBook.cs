using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using MessagingToolkit.Barcode;

namespace VirtualLibrary
{
    public partial class FormBorrowBook : Form
    {
        Capture cam;

        public FormBorrowBook()
        {
            InitializeComponent();
            cam = new Capture();
            Application.Idle += FrameProcedure;
        }

        BarcodeDecoder Scanner;
        OpenFileDialog OD;
        SaveFileDialog SD;

        private void FrameProcedure(Object sender, EventArgs e)
        {
            imageBox1.Image = cam.QueryFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
        }

        //Mygtuko paspaudimas turetu isimti knyga is Library saraso ir ivesti i MyBooks sarasa
        private void button1_Click(object sender, EventArgs e)
        {
            //Laikinai cia kol nesumerginom brancho
            this.Dispose();
        }

        public void ScanBarcode()
        {
            Image<Bgr, byte> img = cam.QueryFrame();
            //Skenavimas
            Scanner = new BarcodeDecoder();
            try
            {
                Result result = Scanner.Decode(new Bitmap((Image)imageBox1.Image));
                //Atskirai parodo nuskenuoto barkodo skaičius
                MessageBox.Show(result.Text);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ScanBarcode();
        }
    }
}
