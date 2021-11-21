using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Runtime.InteropServices;
using Microsoft.Speech.Recognition.SrgsGrammar;


namespace Speech_Recognition
{
    
    class Program
    {
        #region Global Hotkey
            public static bool isHotKeyPressed = false;
        #endregion

        public ServerUDP _server = new ServerUDP();

        private const string PATH = @"D:\C#\Speech Recognition\Speech Recognition\Speech Recognition\";
        private string tag = "";
        public SpeechRecognitionEngine sre;

        #region IfLowLevelHookDoesntWork
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Привет, эта прога для распознавания речи. Пожалуйста, произнеси следующее:\n-Авада Кедавра (целиком);\n-Экспел;\n-Ку\nПримечание: пока на экране не появится слово на английском, пожалуйста, не произноси дальше. Микро должен быть включен!\n\n");
        //    Program program = new Program();

        //    Console.ReadLine();
        //}
        #endregion

        public Program()
        {
            // Create a server base settings
            _server.ServerCreate();
            // Speech Recognize Engine for russian language
            sre = new SpeechRecognitionEngine(
                new System.Globalization.CultureInfo("ru-RU")
                );

            // Get a stream for grammar(создали файловый поток, куда поместили откомпилированную грамматику)
            FileStream fs = new FileStream(PATH + "Grammar.cfg", FileMode.Create);

            SrgsGrammarCompiler.Compile(PATH + "Grammar.xml", fs);
            Grammar grammar = new Grammar(PATH + "Grammar.cfg", "Spells");
            fs.Close();
            // Load custom grammar from .xml
            sre.LoadGrammar(grammar);
            // Set Default microphone from device
            sre.SetInputToDefaultAudioDevice();
            // Will work when speech is recognizedd
            sre.SpeechRecognized += Sre_SpeechRecognized;

            #region IfLowLevelHookDoesntWork
            //// Read global hotKey (Key is 'G') without modifier (Alt, Ctrl, etc.)
            //HotKeyManager.RegisterHotKey(Keys.G, KeyModifiers.NoRepeat);
            //HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
            #endregion
        }

        #region IfLowLevelHookDoesntWork
        // On global hotkey pressed
        //private void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        //{
        //    if (!isHotKeyPressed)
        //    {
        //        sre.RecognizeAsync(RecognizeMode.Single);
        //        isHotKeyPressed = true;
        //    }
        //}
        #endregion

        // Will work when speech is recognizedd
        public void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        { 
            tag = e.Result.Semantics["cmd"].Value.ToString();
            Console.WriteLine(tag);
            _server.ReceiveMeesage(tag);
        }    
        
    }
}
