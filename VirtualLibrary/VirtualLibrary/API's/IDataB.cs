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

        String[] GetUser(string name);

        int ReturnBook(Book delThis);

        void GetAllBooks();

        void GetAllUserBooks();

        void GetAllBooksRead();

        void BorrowBook(Book addThis);
    }
}
