using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary.API_s
{
    public interface IDataB
    {
        int AddBook(Book AddThis);

        int AddUser(String name, String Password, String email, int Permission);

        int SearchUser(string name);

        User GetUser(string name);

        int ReturnBook(Book delThis, User user);

        List<Book> GetAllBooks();

        List<Book> GetAllUserBooks(User user);

        List<Book> GetAllBooksRead(User user);

        int BorrowBook(Book addThis, User user);
    }
}
