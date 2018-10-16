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
using VirtualLibrary.API_s;
using VirtualLibrary.presenters;
using VirtualLibrary.Views;

namespace VirtualLibrary
{
    public partial class FormBorrowBooks : Form, IBorrow
    {
        
        private BorrowBookPresenter BBP;

        public FormBorrowBooks(String procedure)
        {
            InitializeComponent();
            BBP = new BorrowBookPresenter(procedure, this);
        }

        public Image NewFrame { get => pictureBox2.Image; set => pictureBox2.Image = value; }

        public string barcodeText { get => textBox1.Text.Replace(" ", ""); set => textBox1.Text = value; }

        private void button3_Click(object sender, EventArgs e)
        {
            BBP.ScanBarcode();
        }
        
        private void buttonShutDown_Click(object sender, EventArgs e)
        {
            BBP.ExitScanner();
        }

        public void ShowAsDialog()
        {
            this.ShowDialog();
        }

        public void CloseForm()
        {
            this.Close();
        }
    }
}
