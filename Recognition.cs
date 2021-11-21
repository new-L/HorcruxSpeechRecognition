using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;

namespace Speech_Recognition
{
    class Recognition
    {
        const string path = @"D:\C#\Speech Recognition\Speech Recognition\Speech Recognition\";
        private string tag = "";
        private Grammar grammar;

        ServerTCP _server = new ServerTCP();

        private SpeechRecognitionEngine sre;

        public SpeechRecognitionEngine SRE { get => sre; }



        public void SetRecognitionSettings()
        {
            sre = new SpeechRecognitionEngine(
                new System.Globalization.CultureInfo("ru-RU")
                );
            LoadGrammarSettings();
            sre.LoadGrammar(grammar);
            sre.SetInputToDefaultAudioDevice();

            sre.SpeechRecognized += Sre_SpeechRecognized;
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }
        //Загружаем грамматику файла
        private void LoadGrammarSettings()
        {
            //Получаем поток на грамматику(создали файловый поток, куда поместить откомпилированную грамматику)
            FileStream fs = new FileStream(path + "Grammar.cfg", FileMode.Create);

            SrgsGrammarCompiler.Compile(path + "Grammar.xml", fs);
            grammar = new Grammar(path + "Grammar.cfg", "Spells");

            fs.Close();
        }

        //Вызывается при распозновании чего-либо
        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            tag = e.Result.Semantics["cmd"].Value.ToString();
            Console.WriteLine(tag);
            //_server.ReceiveMessage(tag);
            sre.RecognizeAsyncStop();
        }

    }
}
