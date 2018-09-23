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

        private Book userBooks;


        public User(String userName, String passWord)
        {
            this.userName = userName;
            this.passWord = passWord;
        }

    }
}
