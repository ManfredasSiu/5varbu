using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary
{
    class User
    {
        private String userName, passWord, email;

        private List<Book> UserBooks;

        private List<Book> BooksRead;

        public User(String userName, String passWord)
        {
            this.userName = userName;
            this.passWord = passWord;
        }

        public User(String userName,String passWord, List<Book> UserBooks, List<Book> BooksRead)
        {
            this.userName = userName;
            this.passWord = passWord;
            this.UserBooks = UserBooks;
            this.BooksRead = BooksRead;
        }

        public String getuserName()
        {
            return this.userName;
        }

    }
}
