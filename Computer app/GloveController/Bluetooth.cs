using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using System.Windows.Forms;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Runtime.InteropServices.WindowsRuntime;

namespace GloveController
{ 
    class Bluetooth
    {
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;

        private Windows.Devices.Bluetooth.BluetoothLEDevice _device;
        private GattDeviceService _service;
        private GattCharacteristic _characteristic;
        private DeviceWatcher _watcher;
        private List<DeviceInformation> _bluetoothDevices = new List<DeviceInformation>();        

        public bool IsDeviceDriverInstaled { get; private set; }
        public bool IsPaired { get; set; }

        public Bluetooth()
        {
            string aqs = Windows.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelector();
            _watcher = DeviceInformation.CreateWatcher(aqs);
            _watcher.Added += Watcher_Added;
            _watcher.EnumerationCompleted += Watcher_EnumerationCompleted;
        }

        public void ConnectDevice()
        {
            _watcher.Start();
        }

        protected virtual void OnConnectionStatusChanged(ConnectionStatusChangedEventArgs e)
        {
            if (ConnectionStatusChanged != null)
            {
                ConnectionStatusChanged(this, e);
            }
        }

        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            if (DataReceived != null)
            {
                DataReceived(this, e);
            }
        }

        bool CheckFrame(byte[] data)
        {
            if (data[data.Length - 1] == 0xFF)
            {
                if (data.Length == data[0])
                {
                    return true;
                }
            }
            return false;
        }    

        private async Task Connect()
        {
            try
            {
                _device = await Windows.Devices.Bluetooth.BluetoothLEDevice.FromBluetoothAddressAsync(220989373023087);
                _device.ConnectionStatusChanged += Device_ConnectionStatusChanged;
                _service = _device.GattServices.Where(s => s.Uuid == GattDeviceService.ConvertShortIdToUuid(0xFFE0)).SingleOrDefault();
                if (_service != null)
                {
                    _characteristic = _service.GetCharacteristics(GattCharacteristic.ConvertShortIdToUuid(0xFFE1)).Single();
                    _characteristic.ProtectionLevel = GattProtectionLevel.Plain;
                    _characteristic.ValueChanged += Characteristic_ValueChanged;
                    bool status = false;
                    switch (_device.ConnectionStatus)
                    {
                        case Windows.Devices.Bluetooth.BluetoothConnectionStatus.Disconnected:
                            status = false;
                            break;
                        case Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected:
                            status = true;
                            break;
                    }
                    OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(status));
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Nie można nawiązać łączności z urządzeniem sterującym. Możliwe przyczyny: \n\r- Brak odbiornika Bluetooth\n\r- Urządzenie nie jest sparowane\n\r- Brak sterowników Bluetooth.\n\rBez podłączonego urządzenia sterującego korzystanie z aplikacji nie jest możliwe.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //switch ((uint)e.HResult)
                //{
                //    case 0x80070490:
                //        MessageBox.Show("Wykonaj parowanie urządzenia sterującego z komputerem. Informacje dotyczące parowania znajdziesz w zakładce Help", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        break;
                //}
            }
        }


        private async void Watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            var bluetoothDev = _bluetoothDevices.Where((dev => dev.Name == "HTCVW")).SingleOrDefault();
            if (bluetoothDev != null)
            {                
                await Connect();
            }
            else
            {
                IsPaired = false;
                MessageBox.Show("Wykonaj parowanie urządzenia sterującego z komputerem. Informacje dotyczące parowania znajdziesz w zakładce Help. Po wykonaniu parowania uruchom ponownie aplikację.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Watcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            _bluetoothDevices.Add(args);
        }

        private void Device_ConnectionStatusChanged(Windows.Devices.Bluetooth.BluetoothLEDevice sender, object args)
        {
            bool status = false;
            switch (sender.ConnectionStatus)
            {
                case Windows.Devices.Bluetooth.BluetoothConnectionStatus.Disconnected:
                    status = false;
                    break;
                case Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected:
                    status = true;
                    break;
            }
            OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(status));
        }

        List<byte> buffor = new List<byte>();
        List<List<byte>> frames = new List<List<byte>>();

        void ExtractFrameFromBuffor(List<byte> buff)
        {
            int frameLength = buffor.IndexOf(0xFF) + 1;

            if (buffor[0] == frameLength)
            {
                frames.Add(new List<byte>(buffor.Take(frameLength)));
            }

            if (frameLength != 0)
            {
                //Console.WriteLine("Frames");
                buff.RemoveRange(0, frameLength);
                if (buff.Count > 0)
                {
                    ExtractFrameFromBuffor(buff);                    
                }
            }
            
        }

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            try
            {
                buffor.AddRange(args.CharacteristicValue.ToArray());
                if (buffor.Count > 0)
                {
                    ExtractFrameFromBuffor(buffor);
                }

                foreach (var data in frames)
                {
                    OnDataReceived(new DataReceivedEventArgs(data.ToArray()));
                }

                frames.RemoveAll((f) => f != null);
            }
            catch (Exception)
            {
                frames = new List<List<byte>>();
                buffor = new List<byte>();
            }
             
        }
    }

    public class DataReceivedEventArgs
    {
        private readonly byte[] _data;

        public DataReceivedEventArgs(byte[] data)
        {
            _data = data;
        }

        public byte[] Data { get { return _data; } }
    }
}
