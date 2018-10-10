using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary
{
    public class Book
    {
        private String name, auth, or, genre, pressName, code;
        private int pages, quantity;

        public int ID;

        public Book (String name, String auth, String code, String or)
        {
            this.name = name;
            this.auth = auth;
            this.or = or;
        }

        public Book(String name, String auth, string code, string genre, int quantity, int pages, string pressName, int ID)
        {
            this.ID = ID;
            this.name = name;
            this.auth = auth;
            this.code = code;
            this.genre = genre;
            this.pages = pages;
            this.quantity = quantity;
            this.pressName = pressName;
        }

        public string getName()
        {
            return this.name;
        }

        public string getAuthor()
        {
            return this.auth;
        }

        public string getPressName()
        {
            return pressName;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setAuth(string auth)
        {
            this.auth = auth;
        }

        public void setCode(string code)
        {
            this.code = code;
        }

        public void setGenre(String genre)
        {
            this.genre = genre;
        }

        public void setPages(int pages)
        {
            this.pages = pages;
        }

        public string getCode()
        {
            return this.code;
        }

        public string getGenre()
        {
            return this.genre;
        }

        public int getPages()
        {
            return this.pages;
        }

        public int getQuantity()
        {
            return this.quantity;
        }

        public void setQuantityPlius()
        {
            this.quantity = this.quantity + 1;
        }

        public void setQuantityMinus()
        {
            this.quantity = this.quantity - 1;
        }


    }
}
