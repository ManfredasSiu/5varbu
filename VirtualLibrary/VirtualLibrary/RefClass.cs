using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public LogicController LogicC = new LogicController();

        public IMenu menuForm = null;

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
            var mainW = new MainTry(LogicC, LogicC.getDB());
            mainW.Show();
        }
    }
}
