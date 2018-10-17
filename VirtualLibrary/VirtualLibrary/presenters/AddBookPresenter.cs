using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    class AddBookPresenter
    {
        IAddBook AB;
        IDataB ADB;

        public AddBookPresenter(IAddBook AB)
        {
            this.ADB = RefClass.Instance.LogicC.DB;
            this.AB = AB;
        }

        public void AddBookButton()
        {
            if (CheckTBs() == 1)
            {
                MessageBox.Show("Nevisi laukai uzpildyti arba uzpildyti nelegaliai\nUzpildykite laukus pries tesdami");
                return;
            }
            Book bookToAdd = new Book(AB.NameField, AB.AuthorField, AB.BarcodeField, AB.GenreField, int.Parse(AB.QuantityField), int.Parse(AB.Pagesfield), AB.PressField, 0);
            ADB.AddBook(bookToAdd);
            ADB.GetAllBooks();
            RefClass.Instance.LControl.UpdateTable();
            AB.CloseForm();
        }

        public int CheckTBs()
        {
            String[] TBs = { AB.NameField, AB.AuthorField, AB.PressField, AB.Pagesfield, AB.GenreField, AB.QuantityField, AB.BarcodeField };
            foreach (string tb in TBs)
            {
                if (tb.Replace(" ", "") == "")
                    return 1;
            }
            try
            {
                int.Parse(AB.QuantityField);
                int.Parse(AB.Pagesfield);
            }
            catch
            {
                return 1;
            }
            return 0;
        }
    }
}
