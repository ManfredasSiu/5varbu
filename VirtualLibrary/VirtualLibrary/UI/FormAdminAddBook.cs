using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.API_s;
using VirtualLibrary.presenters;
using VirtualLibrary.Views;

namespace VirtualLibrary
{
    public partial class FormAdminAddBook : Form, IAddBook
    {
        public string BarcodeField { get => textBox7.Text; set => textBox7.Text = value; }

        public string NameField => textBox1.Text;

        public string PressField => textBox3.Text;

        public string Pagesfield => textBox4.Text;

        public string QuantityField => textBox6.Text;

        public string GenreField => textBox5.Text;

        public string AuthorField => textBox2.Text;

        public FormAdminAddBook()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AddBookPresenter(this).AddBookButton();
        }

        private void buttonScanner_Click(object sender, EventArgs e)
        {
            RefClass.Instance.InitScannerForm("Add");
        }

        private void buttonShutdown_Click(object sender, EventArgs e)
        {

            if (RefClass.Instance.VR != null)
                RefClass.Instance.VR.SetBlockFlagTrue();
            this.Close();
        }

        public void CloseForm()
        {
            this.Close();
        }
    }
}
