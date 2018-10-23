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
    public class AddBookPresenter
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
            if (CheckTBs(AB.NameField, AB.AuthorField, AB.PressField, AB.Pagesfield, AB.GenreField, AB.QuantityField, AB.BarcodeField) == 1)
            {
                MessageBox.Show("Nevisi laukai uzpildyti arba uzpildyti nelegaliai\nUzpildykite laukus pries tesdami");
                return;
            }
            Book bookToAdd = new Book(AB.NameField, AB.AuthorField, AB.BarcodeField, AB.GenreField,
                                      int.Parse(AB.QuantityField), int.Parse(AB.Pagesfield), AB.PressField, 0); //Knyga pridedama i laikinaja saugykla
            ADB.AddBook(bookToAdd);  //Knyga irasoma i DB
            StaticData.Books = ADB.GetAllBooks();  
            RefClass.Instance.LControl.UpdateTable();  //Atnaujinama Library lentele
            AB.CloseForm();
        }

        public int CheckTBs(string NameField, string AuthorField, string PressField, string Pagesfield, string GenreField, string  QuantityField, string BarcodeField)  //Tikrinama ar texfieldai visi uzpildyti ir ar uzpildyti legaliai
        {
            String[] TBs = { NameField, AuthorField, PressField, Pagesfield, GenreField, QuantityField, BarcodeField };
            var noSpecials = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9]*$");
            foreach (string tb in TBs)
            {
                if (tb.Replace(" ", "") == "")
                    return 1;
                else if (!noSpecials.IsMatch(tb.Replace(" ", "")))
                    return 3;
            }
            try
            {
                int.Parse(QuantityField);
                int.Parse(Pagesfield);
            }
            catch
            {
                return 2;
            }
            return 0;
        }
    }
}
