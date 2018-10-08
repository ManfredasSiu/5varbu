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
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace VirtualLibrary
{
    public partial class FormBorrowBook : Form
    {
        VideoCapture capture;
        Mat frame;
        Bitmap img;

        public FormBorrowBook()
        {
            InitializeComponent();
            capture = new VideoCapture(0);
            capture.Open(0);
            Application.Idle += FrameProcedure;
        }

        BarcodeDecoder Scanner;
        OpenFileDialog OD;
        SaveFileDialog SD;

        private void FrameProcedure(Object sender, EventArgs e)
        {
            frame = new Mat();
            if (capture.IsOpened())
            {
                capture.Read(frame);
                img = BitmapConverter.ToBitmap(frame);
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = img;
            }
        }

        //Mygtuko paspaudimas turetu isimti knyga is Library saraso ir ivesti i MyBooks sarasa
        private void button1_Click(object sender, EventArgs e)
        {
            //Laikinai cia kol nesumerginom brancho
            this.Dispose();
        }

        public void ScanBarcode()
        {
            //Skenavimas
            Scanner = new BarcodeDecoder();
            try
            {
                Result result = Scanner.Decode(new Bitmap(img));
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
