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
    public class AzureDatabase : API_s.IDataB
    {
        DataClasses1DataContext db;
        public AzureDatabase()
        {
            this.db = CreateDC();
        }
       

        public DataClasses1DataContext CreateDC()
        {
            return new DataClasses1DataContext();
        }
        public int AddBook(Book AddThis)
        {
            try
            {
                
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
                return 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }
        }

        public int AddUser(String name, String Password, String email, int Permission)
        {
            try
            {
                string s = Convert.ToString(Permission);
                User user = new User();
                user.Name = name;
                user.Password = Password;
                user.Email = email;
                user.Permission = s;
                db.Users.InsertOnSubmit(user);
                db.SubmitChanges();
                return 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return 1;
            } 
        }

        public int SearchUser(string name)
        {
            try
            {
                var naudotojas = from u in db.Users
                                where u.Name == name
                                select u;
                if (naudotojas.ToArray().Length != 0) { return 2; }
                return 0;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
        }

        public User GetUser(string name)
        {
            try
            {
                var naudotojas = from u in db.Users
                                 where u.Name == name
                                 select u;
               // User[] naud = naudotojas.ToArray();
                User user = new User();
                foreach (var item in naudotojas)
                {
                    user.ID = item.Id;
                    user.userName = item.Name;
                    user.passWord = item.Password;
                    user.email = item.Email;
                    user.permission = item.Permission;
                }
                //StaticData.CurrentUser = user;
                return user; //????
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
         
        }

        public int ReturnBook (Book delThis, User user)
        {
            //Reik sukurt lentelę, kur dėsim perskaitytas knygas - knygos ID ir reader ID
            try
            {
                var knyga = from u in db.UserBooks
                            where u.UserID == user.ID && u.BookID == delThis.ID
                            select u;
                foreach(var item in knyga)
                {
                    db.UserBooks.DeleteOnSubmit(item);
                }
                db.SubmitChanges();
                BooksRead book = new BooksRead();
                book.UserID = user.ID;
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
                return 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }
        }

        public List<Book> GetAllUserBooks(User user)
        {
            try
            {
                var knygos = from u in db.UserBooks
                             join g in db.Books on u.BookID equals g.ID
                             where u.UserID == user.ID
                             select g;

                List<Book> bookIDList = new List<Book>();

                //Book book = new Book();
                foreach (var item in knygos)
                {
                    Book book = new Book();
                    book.ID = item.Id;
                    book.name = item.Name;
                    book.auth = item.Author;
                    book.pressName = item.Press;
                    book.code = item.Barcode;
                    book.genre = item.Genre;
                    book.pages = item.Pages;
                    book.quantity = item.Quantity;
                    bookIDList.Add(book);
                    //bookIDList.Add(StaticData.Books.Find(x => x.ID == item.BookID));
                }
                //StaticData.CurrentUser.setUserBooks(bookIDList);
                return bookIDList;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }

        }

        public List<Book> GetAllBooksRead()
        {
            try
            {
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
                
               // StaticData.CurrentUser.setBooksRead(bookIDList);
                return bookIDList;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }


        public int BorrowBook (Book addThis)
        {
            try
            {
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
                return 0;
            }
            catch(SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
        }

        public List<Book> GetAllBooks()
            {
                try
                {
                    var knygos = from u in db.Books
                                 select u;
                    List<Book> templist = new List<Book>();
                    foreach (var item in knygos)
                    {
                        Book book = new Book();
                        book.ID = item.Id;
                        book.name = item.Name;
                        book.auth = item.Author;
                        book.pressName = item.Press;
                        book.code = item.Barcode;
                        book.genre = item.Genre;
                        book.pages = item.Pages;
                        book.quantity = item.Quantity;
                        templist.Add(book);
                    }
                    return templist;
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
            }
        }
    

    
}
