using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.presenters;
using VirtualLibrary.Views;

namespace VirtualLibrary
{
    class RefClass
    {
        //*****IFN Singleton
        private static RefClass instance = null;
        private static readonly object padlock = new object();

        public static RefClass Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RefClass();
                    }
                    return instance;
                }
            }
        }
        //*****ENDOFSINGLETON
    
        //Logic classes

        public CurrentUserStatistics InitStatistics()
        {
            return new CurrentUserStatistics();
        }

        public LogicController LogicC = new LogicController();

        //Forms

        public IMenu menuForm = null;

        public IUControlMB MBControl = null;

        public IUControlL LControl = null;

        public IAddBook IABook = null;

        public void SaveMenuForm(IMenu menuForm)
        {
            this.menuForm = menuForm;
        }

        public void InitLoginForm()
        {
            var LoginW = new LoginWindow();
            LoginW.Show();
        }

        public void InitRegisterForm()
        {
            Form2 registerFaceWindow = new Form2();
            registerFaceWindow.Show();
        }

        public void InitMainForm()
        {
            var mainW = new MainTry();
            mainW.Show();
        }

        public void InitBorrowForm(String procedure)
        {
            var BorrBook = new FormBorrowBooks(procedure);
            BorrBook.ShowDialog();
        }

        public void InitAddBookForm()
        {
            IABook = new FormAdminAddBook();
            ((Form)IABook).ShowDialog();
        }

        //User Controls***

        public IUControlH InitHomeControl()
        {
            IUControlH IUCH = new UserControlHome();
            return IUCH;
        }

        public IUControlL InitLibControl()
        {
            return LControl = new UserControlLibrary();
        }

        public IUControlMB InitMBControl()
        {
            return MBControl = new UserControlMyBooks();
        }

        public IUControlR InitRecomControl()
        {
            return new UserControlRecom();
        }
    }
}
