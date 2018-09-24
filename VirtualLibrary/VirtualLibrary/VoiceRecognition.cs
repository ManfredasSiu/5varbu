using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Windows.Forms;

namespace VirtualLibrary
{
    class VoiceRecognition // neveikia kolkas
    {

        SpeechRecognitionEngine SRecEng = new SpeechRecognitionEngine();
        Form1 f;

        public VoiceRecognition(Form1 f)
        {
            this.f = f;
            SRecEng = new SpeechRecognitionEngine();
            Choices commands = new Choices();
            commands.Add(new string[] { "Register", "Connect", "Exit" });
            GrammarBuilder gBuild = new GrammarBuilder();
            gBuild.Append(commands);Console.WriteLine(SpeechRecognitionEngine.InstalledRecognizers());
            Grammar gram = new Grammar(gBuild);
            SRecEng.LoadGrammarAsync(gram);
            SRecEng.SetInputToDefaultAudioDevice();
            SRecEng.RecognizeAsync(RecognizeMode.Single);

            SRecEng.SpeechRecognized += VoiceRecBehaviourOnStart;
        }

        private void VoiceRecBehaviourOnStart(Object sender, SpeechRecognizedEventArgs e)
        {
            if(e.Result.Text.Equals("Register"))
            {
                f.Register();
            }
            if(e.Result.Text.Equals("Connect"))
            {
                f.Login();
            }
            if(e.Result.Text.Equals("Exit"))
            {
                Application.Exit();
            }
        }
    }
}
