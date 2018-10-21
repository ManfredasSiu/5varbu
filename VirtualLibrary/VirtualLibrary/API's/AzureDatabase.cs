 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary 
{
    class AzureDatabase : API_s.IDataB
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public AzureDatabase()
        {
            builder.DataSource = "virlib.database.windows.net";  
            builder.UserID = "ILBooks";
            builder.Password = File.ReadAllText(Application.StartupPath + "/SQLPassword.txt");
            builder.InitialCatalog = "VirtualLib";
        }
       
        public int AddBook(Book AddThis)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                Book book = new Book();
                book.Name = AddThis.getName();
                book.Author = AddThis.getAuthor();
                book.Press = AddThis.getPressName();
                book.Barcode = AddThis.getCode();
                book.Genre = AddThis.getGenre();
                book.Pages = AddThis.getPages();
                book.Quantity = AddThis.getQuantity();
                db.Books.InsertOnSubmit(book);
                db.SubmitChanges();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }

        public int AddUser(String name, String Password, String email, int Permission)
        {
            try
            {
                string s = Convert.ToString(Permission);
                DataClasses1DataContext db = new DataClasses1DataContext();
                User user = new User();
                user.Name = name;
                user.Password = Password;
                user.Email = email;
                user.Permission = s;
                db.Users.InsertOnSubmit(user);
                db.SubmitChanges();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }

        public int SearchUser(string name)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var naudotojas = from u in db.Users
                                where u.Name == name
                                select u;
                if (naudotojas.ToArray().Length != 0) { return 2; }    
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }

        public String[] GetUser(string name)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var naudotojas = from u in db.Users
                                 where u.Name == name
                                 select u;

                User user = new User();
                foreach (var item in naudotojas)
                { 
                    user.ID = item.Id;
                    user.userName = item.Name;
                    user.passWord = item.Password;
                    user.email = item.Email;
                    user.permission = item.Permission;
                }
                //cia gal nelogiskas foreach??
                StaticData.CurrentUser = user;
                return null;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
         
        }

        public int ReturnBook (Book delThis)
        {
            //Reik sukurt lentelę, kur dėsim perskaitytas knygas - knygos ID ir reader ID
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var knyga = from u in db.UserBooks
                            where u.UserID == StaticData.CurrentUser.ID && u.BookID == delThis.ID
                            select u;
                foreach(var item in knyga)
                {
                    db.UserBooks.DeleteOnSubmit(item);
                }
                db.SubmitChanges();
                BooksRead book = new BooksRead();
                book.UserID = StaticData.CurrentUser.ID;
                book.BookID = delThis.ID;
                db.BooksReads.InsertOnSubmit(book);
                db.SubmitChanges();
                var knygos = from u in db.Books
                             where u.Id == delThis.ID
                             select u;
                foreach (var item in knygos)
                {
                    item.Quantity++;
                }
                db.SubmitChanges();

                
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }

        

        public void GetAllUserBooks()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var knygos = from u in db.UserBooks
                             where u.UserID == StaticData.CurrentUser.ID
                             select u;

                List<Book> bookIDList = new List<Book>();
                foreach (var item in knygos)
                {
                    Book book = new Book();
                    book.ID = item.Id;
                    //ar nereikia kitų parametrų?
                    bookIDList.Add(StaticData.Books.Find(x => x.ID == item.BookID));
                }
                StaticData.CurrentUser.setUserBooks(bookIDList);
                return;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return;
            }

        }

        public void GetAllBooksRead()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var knygos = from u in db.BooksReads
                             where u.UserID == StaticData.CurrentUser.ID
                             select u;

                List<Book> bookIDList = new List<Book>();
                foreach (var item in knygos)
                {
                    Book book = new Book();
                    book.ID = item.Id;
                    //ar nereikia kitų parametrų
                    bookIDList.Add(StaticData.Books.Find(x => x.ID == item.BookID));
                }
                
                StaticData.CurrentUser.setBooksRead(bookIDList);
                return;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }


        public void BorrowBook (Book addThis)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                UserBook book = new UserBook();
                book.UserID = StaticData.CurrentUser.ID;
                book.BookID = addThis.ID;
                db.UserBooks.InsertOnSubmit(book);
                db.SubmitChanges();
                var knygos = from u in db.Books
                             where u.Id == addThis.ID
                             select u;
                foreach (var item in knygos)
                {
                    item.Quantity--;
                }
                db.SubmitChanges();
            }
            catch(SqlException e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        public void GetAllBooks()
            {
                try
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    var knygos = from u in db.Books
                                 select u;
                    List<Book> templist = new List<Book>();
                    foreach (var item in knygos)
                    {
                        Book book = new Book();
                        book.ID = item.ID;
                        book.name = item.Name;
                        book.auth = item.Author;
                        book.pressName = item.Press;
                        book.code = item.Barcode;
                        book.genre = item.Genre;
                        book.pages = item.Pages;
                        book.quantity = item.Quantity;
                        templist.Add(book);
                    }
                    StaticData.Books = templist;
                return;
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
            }
        }
    

    
}
