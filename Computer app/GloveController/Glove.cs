using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppController;

namespace GloveController
{
    public class Glove : IAppController
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

        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;

        //public event EventHandler

        Bluetooth bluetooth;

        public bool IsPaired { get; private set; }

        public bool IsConnected { get; private set; }

        protected virtual void OnConnectionStatusChanged(ConnectionStatusChangedEventArgs e)
        {
            if (ConnectionStatusChanged != null)
            {
                ConnectionStatusChanged(this, e);
            }
        }

        public void Initialize()
        {
            bluetooth = new Bluetooth();
            bluetooth.DataReceived += Bluetooth_DataReceived;
            bluetooth.ConnectionStatusChanged += Bluetooth_ConnectionStatusChanged;
        }

        public void Connect()
        {            
            bluetooth.ConnectDevice();  
        }

        private void Bluetooth_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e)
        {
            IsConnected = e.Status;
            OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(e.Status));
        }

        private void Bluetooth_DataReceived(object sender, DataReceivedEventArgs e)
        {
            //System.Threading.Thread.Sleep(1);
            //StringBuilder sb = new StringBuilder();
            //foreach (var item in e.Data)
            //{
            //    sb.AppendFormat("{0:X2} ", item);
            //}
            //System.Diagnostics.Debug.WriteLine(sb.ToString());
            switch (e.Data[3])
            {
                case 0x01:
                    WorkModeState state = WorkModeState.Disabled;
                    if (e.Data[4] == 1)
                    {
                        state = WorkModeState.Enabled;
                    }
                    OnWorkModeChanged(new WorkModeEventArgs(state));
                    break;
                case 0x02:
                    PartChoosed(e.Data[4]);
                    break;
                case 0x03:
                    DisplayModeChanged(e.Data[4]);
                    break;
                case 0x04:
                    OrientPosZoomChanged(e.Data.Skip(3).ToArray());
                    break;
                case 0x05:
                    OrientPosZoomChanged(e.Data.Skip(3).ToArray());
                    //Console.WriteLine(dataRead.Length);
                    break;
                case 0x06:
                    OrientPosZoomChanged(e.Data.Skip(3).ToArray());
                    break;
                case 0xFE:
                    OnEndReceive(new EventArgs());
                    break;
                case 0xFD:
                    int tmpRoll = ConvertUnsignedToSigned(e.Data[4], e.Data[5]);
                    OnTest(new TestEventArgs(tmpRoll));
                    break;

                default:
                    break;
            }
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

        protected virtual void OnEndReceive(EventArgs e)
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

        protected virtual void OnTest(TestEventArgs e)
        {
            if (TestData != null)
            {
                TestData(this, e);
            }
        }

        private void PartChoosed(byte operation)
        {
            if (operation == 0x01)
            {
                OnPrevPartChoosed(new EventArgs());
            }
            else if (operation == 0x02)
            {
                OnNextPartChoosed(new EventArgs());
            }
        }

        private void DisplayModeChanged(byte count)
        {
            if (count == 0x01)
            {
                OnPartDispModeChanged(new EventArgs());
            }
            else if (count == 0x02)
            {
                OnAllPartDispModeChanged(new EventArgs());
            }
        }

        private void OrientPosZoomChanged(byte[] data)
        {
            int pitch = ConvertUnsignedToSigned(data[1], data[2]);
            int roll = ConvertUnsignedToSigned(data[3], data[4]);

            switch (data[0])
            {
                case 0x04:
                    OnPositionChanged(new OrientPosEventArgs(pitch, roll));
                    break;
                case 0x05:
                    OnOrientationChanged(new OrientPosEventArgs(pitch, roll));
                    break;
                case 0x06:
                    OnZoomChanged(new OrientPosEventArgs(pitch, roll));
                    break;
            }
        }

        private int ConvertUnsignedToSigned(byte sign, byte number)
        {
            int numb = 0;
            if (sign == 0x00)
            {
                numb = Convert.ToInt32(number);
            }
            else if (sign == 0x01)
            {
                numb = (-1) * Convert.ToInt32(number);
                //Console.WriteLine(numb);

            }
            return numb;
        }
    }
}
