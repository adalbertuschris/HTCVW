using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomBox
{
    public class DialogBox
    {
        enum State
        {
            Ready,
            Initialized,
            Running,
            Terminated
        }

        static IDialogBox _dialogBox;

        static SynchronizationContext _sc;

        static State _state = State.Terminated;

        private DialogBox()
        {
        }

        public static void Show<T>(string textContent, string caption)
            where T : Form, IDialogBox, new()
        {
            if (_state == State.Terminated)
            {
                SetBox(new T());
            }

            if (_state == State.Ready)
            {
                Initialize(textContent, caption);
            }

            if (_state == State.Initialized)
            {
                Show();
            }
        }

        public static void Show<T>(string textContent)
            where T : Form, IDialogBox, new()
        {
            Show<T>(textContent, string.Empty);
        }

        static void SetBox(IDialogBox dialogBox)
        {
            _dialogBox = dialogBox;
            _state = State.Ready;
        }

        static void Initialize(string textContent, string caption)
        {
            _dialogBox.Load += Load;
            _dialogBox.Closed += Closed;
            _dialogBox.Caption = caption;
            _dialogBox.TextContent = textContent;
            _state = State.Initialized;
        }

        static void Show()
        {
            var t = new Thread(() => { _dialogBox.ShowDialog(); });
            t.IsBackground = true;
            t.Start();
            _state = State.Running;            
        }

        private static void Load(object sender, EventArgs e)
        {
            _sc = SynchronizationContext.Current;
        }

        public static void Close()
        {
            if (_state == State.Running || _state == State.Initialized)
            {
                _state = State.Terminated;
                _sc.Post((obj) => { _dialogBox.Close(); }, null);        
            }
        }

        private static void Closed(object sender, EventArgs e)
        {
            _state = State.Terminated;
        }
    }
}
