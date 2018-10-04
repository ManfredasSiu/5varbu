using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary
{
    class Book
    {
        private String name, auth, code, or, type, pages;

        public Book (String name, String auth, String code, String or)
        {
            this.name = name;
            this.auth = auth;
            this.code = code;
            this.or = or;
        }
    }
}
