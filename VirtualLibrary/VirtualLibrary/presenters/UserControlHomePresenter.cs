﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    class UserControlHomePresenter
    {
        private IDataB ADB;
        CurrentUserStatistics CUS;
        IUControlH IUCH;

        public UserControlHomePresenter(IUControlH IUCH)
        {
            this.IUCH = IUCH;
            this.ADB = RefClass.Instance.LogicC.DB;
            ADB.GetAllBooksRead();
            CUS = RefClass.Instance.InitStatistics();
            PresentStatistics();
        }

        private void PresentStatistics()
        {
            IUCH.BooksRead = CUS.BooksRead.ToString();
            IUCH.PagesRead = CUS.PagesRead.ToString();
        }

    }
}
