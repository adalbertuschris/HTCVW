using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomBox
{
    public interface IDialogBox
    {
        string TextContent { get; set; }

        string Caption { get; set; }

        event EventHandler Load;

        event EventHandler Closed;

        void ShowDialog();

        void Close();
    }
}
