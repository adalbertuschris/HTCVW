using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppController;
using OpenTK;

namespace VW3D.Input
{
    class Controller
    {
        private static Controller _controller;

        private IAppController _appController;

        private Game _game;

        private Controller()
        {
        }

        public static Controller CreateController(IAppController appController)
        {
            if (_controller == null)
            {
                _controller = new Controller();
            }
            _controller.SetController(appController);
            return _controller;
        }

        private void SetController(IAppController appController)
        {
            _appController = appController;
        }            

        public void Initialize(Game game)
        {
            _game = game;
            _appController.AllPartDispModeChanged += game.AllPartDispModeChanged;
            _appController.ChangeOrientPosZoomOperationCompleted += game.ChangeOrientPosZoomOperationCompleted;
            _appController.NextPartChoosed += game.NextPartChoosed;
            _appController.OrientationChanged += game.OrientationChanged;
            _appController.PartDispModeChanged += game.PartDispModeChanged;
            _appController.PositionChanged += game.PositionChanged;
            _appController.PrevPartChoosed += game.PrevPartChoosed;
            _appController.TestData += game.TestData;
            _appController.WorkModeChanged += game.WorkModeChanged;
            _appController.ZoomChanged += game.ZoomChanged;

            if (!_appController.IsConnected)
            {
                _appController.Initialize();
            }            
        }

        public void Connect()
        {
            if (!_appController.IsConnected)
            {
                _appController.Connect();
            }           
        }

        public void Delete()
        {
            _appController.AllPartDispModeChanged -= _game.AllPartDispModeChanged;
            _appController.ChangeOrientPosZoomOperationCompleted -= _game.ChangeOrientPosZoomOperationCompleted;
            _appController.NextPartChoosed -= _game.NextPartChoosed;
            _appController.OrientationChanged -= _game.OrientationChanged;
            _appController.PartDispModeChanged -= _game.PartDispModeChanged;
            _appController.PositionChanged -= _game.PositionChanged;
            _appController.PrevPartChoosed -= _game.PrevPartChoosed;
            _appController.TestData -= _game.TestData;
            _appController.WorkModeChanged -= _game.WorkModeChanged;
            _appController.ZoomChanged -= _game.ZoomChanged;
        }
    }
}
