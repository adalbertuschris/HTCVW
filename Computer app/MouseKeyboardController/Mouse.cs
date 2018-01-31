using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace MouseKeyboardController
{
    class Mouse
    {
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseMoveEventArgs> MouseMove;
        public event EventHandler<MouseWheelEventArgs> MouseWheel;

        private static Mouse _mouse;

        private Mouse()
        {
        }

        public static Mouse CreateMouse()
        {
            if (_mouse == null)
            {
                _mouse = new Mouse();
            }
            return _mouse;
        }

        protected virtual void OnMouseUp(MouseButtonEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, e);
            }
        }

        protected virtual void OnMouseDown(MouseButtonEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
            }
        }

        protected virtual void OnMouseMove(MouseMoveEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, e);
            }
        }

        protected virtual void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (MouseWheel != null)
            {
                MouseWheel(this, e);
            }
        }

        private void Game_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseUp(e);
        }

        private void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseDown(e);
        }

        private void Game_MouseMove(object sender, MouseMoveEventArgs e)
        {
            OnMouseMove(e);
        }

        private void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(e);
        }

        public void Initialize(GameWindow game)
        {
            game.MouseDown += Game_MouseDown;
            game.MouseUp += Game_MouseUp;
            game.MouseMove += Game_MouseMove;
            game.MouseWheel += Game_MouseWheel;
        }
    }
}
