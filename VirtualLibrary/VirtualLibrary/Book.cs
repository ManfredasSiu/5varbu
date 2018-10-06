using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary
{
    class Book
    {
        private String name, auth, code, or, genre, pressName;
        private int pages;

        public int ID;

        public Book (String name, String auth, String code, String or)
        {
            this.name = name;
            this.auth = auth;
            this.code = code;
            this.or = or;
        }

        public Book(String name, String auth, String code, String or, string genre, int pages, string pressName)
        {
            this.name = name;
            this.auth = auth;
            this.code = code;
            this.genre = genre;
            this.pages = pages;
            this.pressName = pressName;
        }

        public string getGenre()
        {
            return this.genre;
        }

        public int getPages()
        {
            return this.pages;
        }
    }
}
