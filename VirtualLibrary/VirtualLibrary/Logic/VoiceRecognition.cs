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

        SpeechRecognitionEngine SRecEng = new SpeechRecognitionEngine();
        MenuPresenter MP;
        public bool block { set; get; }

        public VoiceRecognition(MenuPresenter MP)
        {
            
            this.MP = MP;
            SRecEng = new SpeechRecognitionEngine();
            Choices commands = new Choices();
            commands.Add(new string[] { "Register", "Connect", "Exit" });
            GrammarBuilder gBuild = new GrammarBuilder();
            gBuild.Culture = new System.Globalization.CultureInfo("en-GB");
            gBuild.Append(commands);Console.WriteLine(SpeechRecognitionEngine.InstalledRecognizers());
            Grammar gram = new Grammar(gBuild);
            SRecEng.LoadGrammarAsync(gram);
            SRecEng.SetInputToDefaultAudioDevice();
            SRecEng.RecognizeAsync(RecognizeMode.Multiple);
            Console.WriteLine("Info::::" + SRecEng.RecognizerInfo);
            SRecEng.SpeechRecognized += VoiceRecBehaviourOnStart;

        }

        private void VoiceRecBehaviourOnStart(Object sender, SpeechRecognizedEventArgs e)
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
    }
}
