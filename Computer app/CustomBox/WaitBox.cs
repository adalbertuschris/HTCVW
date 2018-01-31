using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomBox
{
    public partial class WaitBox : Form, IDialogBox
    {
        public WaitBox()
        {
            InitializeComponent();
        }       

        public string TextContent
        {
            get => lb_Text.Text;
            set => lb_Text.Text = value;
        }

        public string Caption
        {
            get => Text;
            set => Text = value;
        }        
        
        void IDialogBox.ShowDialog()
        {
            this.ShowDialog();
        }
    }
}
