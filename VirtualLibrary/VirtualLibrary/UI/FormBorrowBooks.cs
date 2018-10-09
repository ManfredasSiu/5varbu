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
    public partial class FormBorrowBooks : Form
    {
        VideoCapture capture;
        Mat frame;
        Bitmap img;
        FormAdminAddBook FAdd = null;
        UserControlLibrary UCL = null;
        AzureDatabase ADB = new AzureDatabase();

        public FormBorrowBooks(UserControlLibrary UCL)
        {
            InitializeComponent();
            this.UCL = UCL;
            capture = new VideoCapture(0);
            capture.Open(0);
            Application.Idle += FrameProcedure;
        }

        public FormBorrowBooks(UserControlMyBooks UCMB)
        {
            InitializeComponent();
            capture = new VideoCapture(0);
            capture.Open(0);
            Application.Idle += FrameProcedure;
        }

        public FormBorrowBooks(FormAdminAddBook FAdd)
        {
            InitializeComponent();
            this.FAdd = FAdd;
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
                if (pictureBox2.Image != null)
                {
                    pictureBox2.Image.Dispose();
                }
                pictureBox2.Image = img;
            }
        }

        //Mygtuko paspaudimas turetu isimti knyga is Library saraso ir ivesti i MyBooks sarasa
        

        public void ScanBarcode()
        {
            //Skenavimas
            Scanner = new BarcodeDecoder();
            try
            {
                Result result = Scanner.Decode(new Bitmap(img));
                textBox1.Text = result.Text;
                
                /**
                 * Reikia atrasti knyga staticData.books liste,sumazinti quantity, prideti knyga i
                 * userbooks lista ir prideti knyga i [dbo].[UserBooks] lentele
                **/
                if (FAdd != null)
                {
                    FAdd.setBarcodeTB(textBox1.Text);
                    return;
                }
                var book = StaticData.Books.Find(x => x.getCode() == textBox1.Text);
                if (book != null)
                {
                    if(book.getQuantity() > 0)
                    {
                        book.setQuantityMinus();
                        StaticData.CurrentUser.AddTakenBook(book);
                        ADB.BorrowBook(book);
                        UCL.UpdateTable();
                    }
                    MessageBox.Show(textBox1.Text);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nera knygos tokiu barkodu");
                }
            } 
            catch(Exception e)
            {
                if(textBox1.Text.Replace(" ", "") == "")
                    MessageBox.Show("Nuskanuoti nepavyko, iveskite barkoda ranka\nArba bandykite dar karta");
                else
                {
                    if (FAdd != null)
                    {
                        FAdd.setBarcodeTB(textBox1.Text);
                        Close();
                        return;
                    }
                    var book = StaticData.Books.Find(x => x.getCode() == textBox1.Text);
                    if (book != null)
                    {
                        if(book.getQuantity() > 0)
                        {
                            book.setQuantityMinus();
                            StaticData.CurrentUser.AddTakenBook(book);
                            ADB.BorrowBook(book);
                            UCL.UpdateTable();
                        }
                        MessageBox.Show(textBox1.Text);
                        this.Close();
                    }
                    else
                        MessageBox.Show("Nera knygos tokiu barkodu");
                    
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ScanBarcode();
        }
        
        private void buttonShutDown_Click(object sender, EventArgs e)
        {
            capture.Dispose();
            Application.Idle -= FrameProcedure;
            this.Close();
        }
        
    }
}
