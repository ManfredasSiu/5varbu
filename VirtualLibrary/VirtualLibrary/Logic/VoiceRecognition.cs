using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Windows.Forms;
using VirtualLibrary.presenters;

namespace VirtualLibrary
{
    class VoiceRecognition // neveikia kolkas
    {
        //-----For Menu Presenter
        SpeechRecognitionEngine SRecEng = new SpeechRecognitionEngine();
        MenuPresenter MP;
        public bool block { set; get; }

        public VoiceRecognition(MenuPresenter MP)
        {
            this.MP = MP;
            
            Choices commands = new Choices();
            commands.Add(new string[] { "Register", "Connect", "Exit" });
            UniversalInit(commands);
            SRecEng.SpeechRecognized += MenuVoiceRec;

        }

        private void MenuVoiceRec(Object sender, SpeechRecognizedEventArgs e)
        {
            if (block)
            {
                if (e.Result.Text.Equals("Register"))
                {
                    MP.RegisterButtonPressed();
                }
                if (e.Result.Text.Equals("Connect"))
                {
                    MP.LoginButtonPressed();
                }
                if (e.Result.Text.Equals("Exit"))
                {
                    Application.Exit();
                }
            }
        }
        //-----

        //-----For Main
        MainPresenter Main;
        public VoiceRecognition(MainPresenter Main)
        {
            this.Main = Main;
            Choices commands = new Choices();
            commands.Add(new string[] { "Home", "My Books", "Library", "Recommended" , "Exit" });
            UniversalInit(commands);
            SRecEng.SpeechRecognized += MainVoiceRec;
        }

        private void MainVoiceRec(Object sender, SpeechRecognizedEventArgs e)
        {
            if (block)
            {
                if (e.Result.Text.Equals("Home"))
                {
                    Main.HButtonBehaviour();
                }
                else if (e.Result.Text.Equals("My Books"))
                {
                    Main.MBButtonBehaviour();
                }
                else if (e.Result.Text.Equals("Library"))
                {
                    Main.LButtonBehaviour();
                }
                else if (e.Result.Text.Equals("Recommended"))
                {
                    Main.RButtonBehaviour();
                }
                else if (e.Result.Text.Equals("Exit"))
                {
                    Application.Exit();
                }
            }
        }

        //-----

        //-----Universal
        private void UniversalInit(Choices commands)
        {
            SRecEng = new SpeechRecognitionEngine();
            GrammarBuilder gBuild = new GrammarBuilder();
            gBuild.Culture = new System.Globalization.CultureInfo("en-GB");
            gBuild.Append(commands);
            Console.WriteLine(SpeechRecognitionEngine.InstalledRecognizers());
            Grammar gram = new Grammar(gBuild);
            SRecEng.LoadGrammarAsync(gram);
            SRecEng.SetInputToDefaultAudioDevice();
            SRecEng.RecognizeAsync(RecognizeMode.Multiple);
            Console.WriteLine("Info::::" + SRecEng.RecognizerInfo);
        }
        //-----
    }
}
