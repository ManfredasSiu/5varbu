﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary
{

    public partial class User
    {
        public String userName, passWord, email,  permission;
        
        public int ID;

        private List<Book> UserBooks = new List<Book>();

        private List<Book> BooksRead = new List<Book>();

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

        public User(int ID, String userName, String passWord, String email, String permission)
        {
            this.ID = ID;
            this.userName = userName;
            this.passWord = passWord;
            this.email = email;
            this.permission = permission;
        }
        public String getuserName()
        {
            return this.userName;
        }

        public List<Book> getUserBooks()
        {
            return UserBooks;
        }

        public void setBooksRead(List<Book> BooksRead)
        {
            this.BooksRead = BooksRead;
        }

        public void removeUserBook(Book removeThis)
        {
            this.UserBooks.Remove(removeThis);
        }

        public void setUserBooks(List<Book> UserBooks)
        {
            this.UserBooks = UserBooks;
        }

        public String getPermission()
        {
            return this.permission;
        }

        public void AddReadBook(Book b)
        {
            BooksRead.Add(b);
        }

        public void AddTakenBook(Book b)
        {
            UserBooks.Add(b);
        }

        public List<Book> getBooksRead()
        {
            return BooksRead;
        }

    }
}
