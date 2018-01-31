using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppController
{
    public class WorkModeEventArgs : EventArgs
    {
        public WorkModeEventArgs(WorkModeState state)
        {
            State = state;
        }

        public WorkModeState State { get; private set; }
    }
}
