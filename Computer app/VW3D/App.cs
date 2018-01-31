using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CustomBox;
using AppController;
using GloveController;
using MouseKeyboardController;
using M3DIL;
using M3DIL.Sources;


namespace VW3D
{
    public class App
    {        
        public static bool IsRunning { get; private set; }
        static App _app;
        static bool _initialized;

        public static event EventHandler ComponentInitialized;

        Input.Controller _controller;
        //Model model;
        Game _game;        
        
        public bool IsExiting { get; private set; }
        
        public static void Initialize(IAppController appController)
        {
            _app = new App();            
            //app.ComponentInitialized += ComponentLoaded;            
            _app._game = new Game();
            _app._game.ComponentInitialized += (sender, e) => { DialogBox.Close(); };
            _app._game.Closing += _app.Game_Closing;
            _app.SetInput(appController);            
            
            _initialized = true;
        }

        public static void Run(M3DIL.Sources.SourceType fileExtension, string modelFileName)
        {
            DialogBox.Show<WaitBox>("Loading model, please wait", "Loading Model");
            if (!_initialized)
            {
                Initialize(null);
            }
            try
            {
                Model model = Model.Load(fileExtension, modelFileName);
                _app._game.LoadModel(model);

                _app._game.Run(100, 70);
            }
            catch (Exception e)
            {
                DialogBox.Close();
                StringBuilder sb = new StringBuilder();
                GetExceptionList(sb, e);
                MessageBox.Show(sb.ToString(),"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        static void GetExceptionList(StringBuilder str, Exception e)
        {
            str.AppendLine(e.Message);
            if (e.InnerException != null)
            {
                GetExceptionList(str, e.InnerException);
            }
        }
        
        private void SetInput(IAppController controller)
        {
            if (controller == null)
            {
                controller = new MouseKeyboardController.MouseKeyboard(_app._game);
            }
            _app._controller = Input.Controller.CreateController(controller);
            _app._controller.Initialize(_app._game);
            _app._controller.Connect();
        }
        
        protected virtual void OnComponentInitialized(EventArgs e)
        {
            if (ComponentInitialized != null)
            {
                ComponentInitialized(this, e);
            }
        }

        private void Game_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {            
            _game.Clear();
            _app.IsExiting = true;
            _controller.Delete();
            _initialized = false;
        }

        public static void Close()
        {
            _app.IsExiting = true;
            _app._controller.Delete();
            _initialized = false;

            if (_app._game != null)
            {
                _app._game.Clear();
                _app._game.Close();
            }
        }
    }
}