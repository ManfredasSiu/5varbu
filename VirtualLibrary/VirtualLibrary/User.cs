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

        public int ID;

        private List<Book> UserBooks;


        public User(String userName, String passWord)
        {
            this.userName = userName;
            this.passWord = passWord;
        }

        public String getuserName()
        {
            return this.userName;
        }

    }
}
