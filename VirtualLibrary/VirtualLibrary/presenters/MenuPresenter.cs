using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    

    class MenuPresenter
    {
        private IMenu MenuView;
        static VoiceRecognition VR;

        public MenuPresenter(IMenu MenuView)
        {
            this.MenuView = MenuView;
            VR = RefClass.Instance.InitVoiceRecMenu(this);
            VR.SetBlockFlagTrue();
        }

        public void RestartVR()
        {
            VR.SetBlockFlagTrue();
        }

        public void LoginButtonPressed()
        {
            VR.SetBlockFlagFalse();
            MenuView.HideForm();
            RefClass.Instance.SaveMenuForm(MenuView);  //Issaugoma meniu forma tolesniem naudojimam
            RefClass.Instance.InitLoginForm();         //Login forma
        }

        public void RegisterButtonPressed()
        {
            VR.SetBlockFlagFalse();
            MenuView.HideForm();
            RefClass.Instance.SaveMenuForm(MenuView); //Issaugoma meniu forma tolesniem naudojimam
            RefClass.Instance.InitRegisterForm();     //Register Forma
        }
    }
}
