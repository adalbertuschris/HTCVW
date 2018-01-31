using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using System.Drawing;
using AppController;

namespace MouseKeyboardController
{
    public class MouseKeyboard : IAppController
    {
        public event EventHandler<WorkModeEventArgs> WorkModeChanged;
        public event EventHandler AllPartDispModeChanged;
        public event EventHandler PartDispModeChanged;
        public event EventHandler PrevPartChoosed;
        public event EventHandler NextPartChoosed;
        public event EventHandler<OrientPosEventArgs> OrientationChanged;
        public event EventHandler<OrientPosEventArgs> PositionChanged;
        public event EventHandler<OrientPosEventArgs> ZoomChanged;
        public event EventHandler ChangeOrientPosZoomOperationCompleted;
        public event EventHandler<TestEventArgs> TestData;

        OpenTK.Input.MouseButton _mouseButton;
        Mouse _mouse;
        Keyboard _keyboard;

        int _xOffset = 0;
        int _yOffset = 0;
        bool _workMode = false;

        public bool IsConnected { get; private set; }

        public MouseKeyboard(GameWindow game)
        {
            _mouse = Mouse.CreateMouse();
            _keyboard = Keyboard.CreateKeyboard();

            _mouse.Initialize(game);
            _keyboard.Initialize(game);
        }

        public void Initialize()
        {
            _keyboard.KeyDown += MouseKeyboard_KeyDown;
            //mouseKeyboard.KeyUp += MouseKeyboard_KeyUp;
            _mouse.MouseDown += MouseKeyboard_MouseDown;
            _mouse.MouseUp += MouseKeyboard_MouseUp;
            _mouse.MouseMove += MouseKeyboard_MouseMove;
            _mouse.MouseWheel += MouseKeyboard_MouseWheel;
            //Console.WriteLine("Initialized");
        }

        public void Connect()
        {
            IsConnected = true;
        }

        protected virtual void OnWorkModeChanged(WorkModeEventArgs wmea)
        {
            if (WorkModeChanged != null)
            {
                WorkModeChanged(this, wmea);
            }
        }

        protected virtual void OnAllPartDispModeChanged(EventArgs e)
        {
            if (AllPartDispModeChanged != null)
            {
                AllPartDispModeChanged(this, e);
            }
        }

        protected virtual void OnPartDispModeChanged(EventArgs e)
        {
            if (PartDispModeChanged != null)
            {
                PartDispModeChanged(this, e);
            }
        }

        protected virtual void OnPrevPartChoosed(EventArgs e)
        {
            if (PrevPartChoosed != null)
            {
                PrevPartChoosed(this, e);
            }
        }

        protected virtual void OnChangeOrientPosZoomOperationCompleted(EventArgs e)
        {
            if (ChangeOrientPosZoomOperationCompleted != null)
            {
                ChangeOrientPosZoomOperationCompleted(this, e);
            }
        }

        protected virtual void OnNextPartChoosed(EventArgs e)
        {
            if (NextPartChoosed != null)
            {
                NextPartChoosed(this, e);
            }
        }

        protected virtual void OnOrientationChanged(OrientPosEventArgs e)
        {
            if (OrientationChanged != null)
            {
                OrientationChanged(this, e);
            }
        }

        protected virtual void OnPositionChanged(OrientPosEventArgs e)
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, e);
            }
        }

        protected virtual void OnZoomChanged(OrientPosEventArgs e)
        {
            if (ZoomChanged != null)
            {
                ZoomChanged(this, e);
            }
        }

        protected virtual void OnTestData(TestEventArgs e)
        {
            if (TestData != null)
            {
                TestData(this, e);
            }
        }

        private void MouseKeyboard_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            OnChangeOrientPosZoomOperationCompleted(new EventArgs());
        }

        private void MouseKeyboard_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            _xOffset = e.X;
            _yOffset = e.Y;
            _mouseButton = e.Button;
        }

        private void MouseKeyboard_MouseWheel(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            OnZoomChanged(new OrientPosEventArgs(e.Value * 2, 0));
        }

        private void MouseKeyboard_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            if (!_workMode && e.Mouse.IsAnyButtonDown)
            {
                int pitch = Convert.ToInt16((_yOffset - e.Y));
                int roll = Convert.ToInt16((e.X - _xOffset));
                if (_mouseButton == OpenTK.Input.MouseButton.Right)
                {
                    OnPositionChanged(new OrientPosEventArgs(pitch, roll));
                }
                else if (_mouseButton == OpenTK.Input.MouseButton.Left)
                {
                    OnOrientationChanged(new OrientPosEventArgs(pitch, roll));
                }
            }
        }        

        private void MouseKeyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Number5)
            {
                if (_workMode)
                {
                    _workMode = false;
                    OnWorkModeChanged(new WorkModeEventArgs(WorkModeState.Disabled));
                }
                else
                {
                    _workMode = true;
                    OnWorkModeChanged(new WorkModeEventArgs(WorkModeState.Enabled));
                }
            }

            if (e.Key == OpenTK.Input.Key.Number1)
            {
                OnPrevPartChoosed(new EventArgs());
            }

            if (e.Key == OpenTK.Input.Key.Number2)
            {
                OnNextPartChoosed(new EventArgs());
            }

            if (e.Key == OpenTK.Input.Key.Number3)
            {
                OnPartDispModeChanged(new EventArgs());
            }

            if (e.Key == OpenTK.Input.Key.Number4)
            {
                OnAllPartDispModeChanged(new EventArgs());
            }
        }
    }
}
