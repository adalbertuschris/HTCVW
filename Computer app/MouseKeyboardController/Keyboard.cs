using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace MouseKeyboardController
{
    class Keyboard
    {
        public event EventHandler<KeyboardKeyEventArgs> KeyUp;
        public event EventHandler<KeyboardKeyEventArgs> KeyDown;

        private static Keyboard _keyboard;

        private Keyboard()
        {
        }

        public static Keyboard CreateKeyboard()
        {
            if (_keyboard == null)
            {
                _keyboard = new Keyboard();
            }
            return _keyboard;
        }        

        protected virtual void OnKeyUp(KeyboardKeyEventArgs e)
        {
            if (KeyUp != null)
            {
                KeyUp(this, e);
            }
        }

        protected virtual void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (KeyDown != null)
            {
                KeyDown(this, e);
            }
        }

        private void Game_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            OnKeyUp(e);
        }

        private void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            OnKeyDown(e);
        }

        public void Initialize(GameWindow game)
        {
            game.KeyDown += Game_KeyDown;
            game.KeyUp += Game_KeyUp;            
        }
    }
}
