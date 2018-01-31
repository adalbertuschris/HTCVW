using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using VW3D.Graphic.GraphicPipeline;
using VW3D.Text;
using VW3D.Extensions;
using VW3D.Graphic;
using GloveController;
using CustomBox;
using AppController;
using MouseKeyboardController;
using M3DIL;
using System.ComponentModel;

namespace VW3D
{
    class Game : GameWindow
    {
        public event EventHandler ComponentInitialized;

        TextGenerator _text;        
        GameObject _gameObject;        
        SceneRenderer _scene;
        Scene.Camera _camera;

        float fovy;
        float lastFovy = 0f;
        float initialFovyVal = 20;

        public Game() : base(512, 512, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8), "HTCVW Model")
        {
            //GL.Enable(EnableCap.Texture2D);            
        }       

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
           
            InitializeComponent();

            OnComponentLoaded(new EventArgs());            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);          
            
            _scene.UpdateModelViewProjMatrix();
            Thread.Sleep(1);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            
            if (SceneRenderer.WorkMode)
            {
                float xPosText = (-1) * (ClientSize.Width / 2) + 6;
                float yPosText = (ClientSize.Height / 2) - 16;
                _text.RenderText(@"Display mode:", xPosText, yPosText, new Vector3(0.0f, 0.0f, 0.0f));
                _text.RenderText(string.Format("All parts: {0}", _gameObject.Model.DisplayMode.ToString()), xPosText, yPosText - 15, new Vector3(0.0f, 0.0f, 0.0f));
                if (_gameObject.SelectedPart != null)
                {
                    _text.RenderText(string.Format("Choosed part: {0}", _gameObject.SelectedPart.DisplayMode), xPosText, yPosText - 30, new Vector3(0.0f, 0.0f, 0.0f));
                }
            }

            _scene.Draw();
            SwapBuffers();
        }

        public void InitializeComponent()
        {
            GL.ClearColor(Color.LightGray);
            GL.PointSize(5f);

            fovy = initialFovyVal;

            _text = new TextGenerator("fonts/arialbd.ttf", "arialB");
            //TextGenerator.ProjMatrix = Matrix4.CreateOrthographic(512, 512, -10, 10);
            _camera = new Scene.Camera();
            _camera.Position = new Vector3(0, 0, -50);
            _scene = new SceneRenderer();
            _scene.AddLight(new Scene.Light());
            _scene.AddGameObject(_gameObject);
            _scene.AddCamera(_camera);
            _scene.SetProjectionMatrix(fovy, (float)ClientSize.Width, (float)ClientSize.Height, 20.0f, 500.0f);
        }

        public void LoadModel(Model model)
        {
            if (model != null)
            {
                _gameObject = new GameObject(model);
            }
            else
            {
                throw new NullReferenceException("Model cant load");
            }
        }

        public void Clear()
        {
            if (_scene != null)
            {
                _scene.Clear();
                SceneRenderer.WorkMode = false;
            }

            if (_text != null)
            {
                _text.Clear();
            }            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _scene.UpdateScreenSize((float)ClientSize.Width, (float)ClientSize.Height);           
            _text.ProjMatrix = Matrix4.CreateOrthographic((float)ClientSize.Width, (float)ClientSize.Height, -10, 10);
        }     
        
        //bool beginReceive = true;
        
        

        public void WorkModeChanged(object sender, WorkModeEventArgs e)
        {
            if (_scene != null)
            {
                bool state = false;

                if (e.State == WorkModeState.Enabled)
                {
                    state = true;
                }
                
                SceneRenderer.WorkMode = Convert.ToBoolean(state);                
            }
        }

        public void PrevPartChoosed(object sender, EventArgs e)
        {
            if (_gameObject != null && SceneRenderer.WorkMode)
            {
                _gameObject.SelectPrevPart();
            }
        }

        public void NextPartChoosed(object sender, EventArgs e)
        {
            if (_gameObject.Model != null && SceneRenderer.WorkMode)
            {
                _gameObject.SelectNextPart();
            }
        }

        public void PartDispModeChanged(object sender, EventArgs e)
        {
            if (_gameObject.Model != null && SceneRenderer.WorkMode)
            {
                if (_gameObject.SelectedPart != null)
                {
                    _gameObject.SelectedPart.DisplayMode = GetNextStateDisplayMode(_gameObject.SelectedPart.DisplayMode);                    
                }
            }
        }

        public void AllPartDispModeChanged(object sender, EventArgs e)
        {
            if (_gameObject.Model != null && SceneRenderer.WorkMode)
            {
                _gameObject.Model.DisplayMode = GetNextStateDisplayMode(_gameObject.Model.DisplayMode);
            }
        }

        private DispMode GetNextStateDisplayMode(DispMode currentDispMode)
        {
            DispMode dispMode = currentDispMode;

            switch (currentDispMode)
            {
                case DispMode.normal:
                    dispMode = DispMode.transparent;
                    break;
                case DispMode.hide:
                    dispMode = DispMode.normal;
                    break;
                case DispMode.transparent:
                    dispMode = DispMode.hide;
                    break;
            }

            return dispMode;
        }

        public void ChangeOrientPosZoomOperationCompleted(object sender, EventArgs e)
        {
            _gameObject.Transform.SaveChanges();            
        }
        
        public void OrientationChanged(object sender, OrientPosEventArgs e)
        {
            _gameObject.Transform.Rotate(e.Pitch, e.Roll);
        }

        public void PositionChanged(object sender,OrientPosEventArgs e)
        {
            _gameObject.Transform.Translate(e.Pitch, e.Roll);            
        }

        public void ZoomChanged(object sender, OrientPosEventArgs e)
        {
            if (e.Pitch == 0)
            {
                lastFovy = e.Pitch;
            }

            float sensitivity = 0.2f;

            float angle = e.Pitch - lastFovy ;
            float tmpFovy = fovy + angle * sensitivity;

            if (tmpFovy > 1 && tmpFovy < 179)
            {
                fovy = tmpFovy;
                _scene.UpdateFovy(tmpFovy);
            }
            lastFovy = e.Pitch;
        }

        public void TestData(object sender, TestEventArgs e)
        {
            Console.WriteLine("Roll: {0}", e.Data);
        }

        void OnComponentLoaded(EventArgs e)
        {
            if (ComponentInitialized != null)
            {
                ComponentInitialized(this, e);
            }
        }
    }
}
