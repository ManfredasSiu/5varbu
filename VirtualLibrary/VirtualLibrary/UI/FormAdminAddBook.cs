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
using VirtualLibrary.Views;

namespace VirtualLibrary
{
    public partial class FormAdminAddBook : Form, IAddBook
    {
        public String Barcode { set; get; }
        private IDataB ADB;

        public string BarcodeField { set => textBox7.Text = value; }

        UserControlLibrary UCL = null;


        public FormAdminAddBook()
        {
            InitializeComponent();
            this.ADB = ADB;
            this.UCL = UCL;
        }

        public void setBarcodeTB(string barcode)
        {
            textBox7.Text = barcode;
        }

        //Mygtuko paspaudimas turetu isimti knyga is MyBooks saraso ir Grazinti i library sarasa
        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckTBs() == 1)
            {
                MessageBox.Show("Nevisi laukai uzpildyti arba uzpildyti nelegaliai\nUzpildykite laukus pries tesdami");
                return;
            }
            Book bookToAdd = new Book(textBox1.Text, textBox2.Text, textBox7.Text, textBox5.Text, int.Parse(textBox6.Text), int.Parse(textBox4.Text), textBox3.Text, 0);
            ADB.AddBook(bookToAdd);
            ADB.GetAllBooks();
            UCL.UpdateTable();
            this.Close();
        }

        public int CheckTBs()
        {
            TextBox[] TBs = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7 };
            foreach (TextBox tb in TBs)
            {
                if (tb.Text.Replace(" ", "") == "")
                    return 1;
            }
            try
            {
                int.Parse(textBox6.Text);
                int.Parse(textBox4.Text);
            }
            catch
            {
                return 1;
            }
            return 0;
        }

        private void buttonScanner_Click(object sender, EventArgs e)
        {
            var fbb = RefClass.Instance.InitBorrowForm("Add");
            fbb.ShowAsDialog();
            
        }

        public void ShowAsDialog()
        {
            this.ShowAsDialog();
        }
    }
}
