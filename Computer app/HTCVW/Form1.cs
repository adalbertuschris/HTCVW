using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using GloveController;
using AppController;
using CustomBox;

namespace HTCVW
{
    public partial class Form1 : Form
    {
        IAppController appController;
        Manual.Form1 manual;

        public Form1()
        {
            InitializeComponent();
            
            panel1.BackgroundImage = Images.Disconnected;

            appController = new Glove();
            appController.Initialize();
            appController.Connect();
            //glove.Connect();
            //glove.ConnectionStatusChanged += Glove_ConnectionStatusChanged;
        }

        
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "OBJ (*.obj)|*.obj";
            openFileDialog1.FileName = "";            

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                VW3D.App.Run(M3DIL.Sources.SourceType.OBJ, openFileDialog1.FileName);                          
            }
        }       

        private void Glove_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e)
        {
            if (e.Status)
            {
                panel1.BackgroundImage = Images.Connected;
                //sc.Post((obj) => { openToolStripMenuItem.Enabled = true; }, null);
            }
            else
            {
                panel1.BackgroundImage = Images.Disconnected;
                //sc.Post((obj) => { openToolStripMenuItem.Enabled = false; }, null);
            }
        }

        private void deviceInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Device name: HTCVW");
            sb.Append("PIN: 592471");
            MessageBox.Show(sb.ToString(), "Device info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (VW3D.App.IsRunning)
            {
                VW3D.App.Close();
            }            
        }
                
        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (manual == null)
            {
                manual = new Manual.Form1();
                manual.FormClosing += Manual_FormClosing;
                manual.Show();
            }
        }

        private void Manual_FormClosing(object sender, FormClosingEventArgs e)
        {
            manual = null;
        }
    }
}
