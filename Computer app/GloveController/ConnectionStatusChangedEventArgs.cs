using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloveController
{
    public class ConnectionStatusChangedEventArgs: EventArgs
    {
        public bool Status { get; private set; }

        public ConnectionStatusChangedEventArgs(bool status)
        {
            Status = status;
        }
    }
}
