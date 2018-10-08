using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary
{
    class Book
    {
        private String name, auth, or, genre, pressName;
        private int pages, quantity, code;

        public int ID;

        public Book (String name, String auth, String code, String or)
        {
            this.name = name;
            this.auth = auth;
            this.or = or;
        }

        public Book(String name, String auth, int code, string genre, int quantity, int pages, string pressName, int ID)
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
