using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    

    class MenuPresenter
    {
        private IMenu MenuView;

        public MenuPresenter(IMenu MenuView)
        {
            this.MenuView = MenuView;
        }

        public void LoginButtonPressed()
        {
            MenuView.HideForm();
            RefClass.Instance.SaveMenuForm(MenuView);  //Issaugoma meniu forma tolesniem naudojimam
            RefClass.Instance.InitLoginForm();         //Login forma
        }

        public void RegisterButtonPressed()
        {
            MenuView.HideForm();
            RefClass.Instance.SaveMenuForm(MenuView); //Issaugoma meniu forma tolesniem naudojimam
            RefClass.Instance.InitRegisterForm();     //Register Forma
        }
    }
}
