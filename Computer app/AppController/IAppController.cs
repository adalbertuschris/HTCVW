using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppController
{
    public interface IAppController
    {
        event EventHandler<WorkModeEventArgs> WorkModeChanged;
        event EventHandler AllPartDispModeChanged;
        event EventHandler PartDispModeChanged;
        event EventHandler PrevPartChoosed;
        event EventHandler NextPartChoosed;
        event EventHandler<OrientPosEventArgs> OrientationChanged;
        event EventHandler<OrientPosEventArgs> PositionChanged;
        event EventHandler<OrientPosEventArgs> ZoomChanged;
        event EventHandler ChangeOrientPosZoomOperationCompleted;
        event EventHandler<TestEventArgs> TestData;

        void Initialize();

        void Connect();        

        bool IsConnected { get; }
    }
}
