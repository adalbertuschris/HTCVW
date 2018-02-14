using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using OpenTK;
using M3DIL;

namespace VW3D.Physic
{
    public class Transform
    {
        //object _lock1 = new object();
        object _lock2 = new object();

        Matrix4 _modelMatrix = Matrix4.Identity;
        Matrix4 _referenceModelMatrix = Matrix4.Identity;

        float _lastTransXAngle, _lastTransYAngle;
        float _lastRotXAngle, _lastRotYAngle;

        Matrix4 _transX = Matrix4.CreateTranslation(0, 0, 0);
        Matrix4 _transY = Matrix4.CreateTranslation(0, 0, 0);
        Matrix4 _rotX = Matrix4.CreateRotationX(0);
        Matrix4 _rotY = Matrix4.CreateRotationY(0);

        float _sensitivityRot = 0.8f;
        float _sensitivityTrans = 0.08f;

        BlockingCollection<Tuple<TransformType, int>> _xAngle;
        BlockingCollection<Tuple<TransformType, int>> _yAngle;

        Task _taskX;
        Task _taskY;

        public Transform()
        {
            _xAngle = new BlockingCollection<Tuple<TransformType, int>>();
            _yAngle = new BlockingCollection<Tuple<TransformType, int>>();

            _taskX = Task.Factory.StartNew(TransformX, TaskCreationOptions.LongRunning);
            _taskY = Task.Factory.StartNew(TransformY, TaskCreationOptions.LongRunning);
        }

        public void TransformX()
        {
            while (true)
            {
                Tuple<TransformType, int> item = _xAngle.Take();
                switch (item.Item1)
                {
                    case TransformType.Rotate:
                        RotateX(item.Item2);
                        break;
                    case TransformType.Translate:
                        TranslationX(item.Item2);
                        break;
                }
            }
        }

        public void TransformY()
        {
            while (true)
            {
                Tuple<TransformType, int> item = _yAngle.Take();
                switch (item.Item1)
                {
                    case TransformType.Rotate:
                        RotateY(item.Item2);
                        break;
                    case TransformType.Translate:
                        TranslationY(item.Item2);
                        break;
                }
            }
        }

        public Matrix4 ModelMatrix
        {
            get
            {
                lock (_lock2)
                {
                    return _modelMatrix;
                }
            }
        }

        public void SaveChanges()
        {
            _referenceModelMatrix = ModelMatrix;

            _lastTransXAngle = _lastTransYAngle = 0;
            _lastRotXAngle = _lastRotYAngle = 0;

            lock (_lock2)
            {
                _rotX = Matrix4.CreateRotationX(0);
                _rotY = Matrix4.CreateRotationY(0);
                _transX = _transY = Matrix4.CreateTranslation(0, 0, 0);
            }
        }

        public void Translate(int pitch, int roll)
        {
            _xAngle.Add(new Tuple<TransformType, int>(TransformType.Translate, roll));
            _yAngle.Add(new Tuple<TransformType, int>(TransformType.Translate, pitch));
        }

        public void Rotate(int pitch, int roll)
        {
            _xAngle.Add(new Tuple<TransformType, int>(TransformType.Rotate, pitch));
            _yAngle.Add(new Tuple<TransformType, int>(TransformType.Rotate, roll));
        }

        private void TranslationY(int transYAngle)
        {
            if (_lastTransYAngle > transYAngle)
            {
                for (double i = _lastTransYAngle * _sensitivityTrans; i > transYAngle * _sensitivityTrans; i = i - _sensitivityTrans)
                {
                    lock (_lock2)
                    {
                        _transY = Matrix4.CreateTranslation(0, (float)i, 0);
                        _modelMatrix = _referenceModelMatrix * _transX * _transY;
                    }
                }
            }
            else if (_lastTransYAngle < transYAngle)
            {
                for (double i = _lastTransYAngle * _sensitivityTrans; i < transYAngle * _sensitivityTrans; i = i + _sensitivityTrans)
                {
                    lock (_lock2)
                    {
                        _transY = Matrix4.CreateTranslation(0, (float)i, 0);
                        _modelMatrix = _referenceModelMatrix * _transX * _transY;
                    }
                }
            }
            else
            {
                lock (_lock2)
                {
                    _transY = Matrix4.CreateTranslation(0, (float)transYAngle * _sensitivityTrans, 0);
                    _modelMatrix = _referenceModelMatrix * _transX * _transY;
                }
            }
            _lastTransYAngle = transYAngle;
        }

        private void TranslationX(int transXAngle)
        {
            if (_lastTransXAngle > transXAngle)
            {
                for (double i = _lastTransXAngle * _sensitivityTrans; i > transXAngle * _sensitivityTrans; i = i - _sensitivityTrans)
                {
                    lock (_lock2)
                    {
                        _transX = Matrix4.CreateTranslation(-(float)i, 0, 0);
                        _modelMatrix = _referenceModelMatrix * _transX * _transY;
                    }
                }
            }
            else if (_lastTransXAngle < transXAngle)
            {
                for (double i = _lastTransXAngle * _sensitivityTrans; i < transXAngle * _sensitivityTrans; i = i + _sensitivityTrans)
                {
                    lock (_lock2)
                    {
                        _transX = Matrix4.CreateTranslation(-(float)i, 0, 0);
                        _modelMatrix = _referenceModelMatrix * _transX * _transY;
                    }
                }
            }
            else
            {
                lock (_lock2)
                {
                    _transX = Matrix4.CreateTranslation(-(float)transXAngle * _sensitivityTrans, 0, 0);
                    _modelMatrix = _referenceModelMatrix * _transX * _transY;
                }
            }
            _lastTransXAngle = transXAngle;
        }

        private void RotateX(int rotXAngle)
        {
            if (_lastRotXAngle > rotXAngle)
            {
                for (double i = _lastRotXAngle; i > rotXAngle; i = i - _sensitivityRot)
                {
                    lock (_lock2)
                    {
                        _rotX = Matrix4.CreateRotationX((float)(i * Math.PI / 180));
                        _modelMatrix = _referenceModelMatrix * _rotX * _rotY;
                    }
                }
            }
            else if (_lastRotXAngle < rotXAngle)
            {
                for (double i = _lastRotXAngle; i < rotXAngle; i = i + _sensitivityRot)
                {
                    lock (_lock2)
                    {
                        _rotX = Matrix4.CreateRotationX((float)(i * Math.PI / 180));
                        _modelMatrix = _referenceModelMatrix * _rotX * _rotY;
                    }
                }
            }
            else
            {
                lock (_lock2)
                {
                    _rotX = Matrix4.CreateRotationX((float)(rotXAngle * Math.PI / 180));
                    _modelMatrix = _referenceModelMatrix * _rotX * _rotY;
                }
            }
            _lastRotXAngle = rotXAngle;
        }

        private void RotateY(int rotYAngle)
        {
            if (_lastRotYAngle > rotYAngle)
            {
                for (double i = _lastRotYAngle; i > rotYAngle; i = i - _sensitivityRot)
                {
                    lock (_lock2)
                    {
                        _rotY = Matrix4.CreateRotationY((float)(i * Math.PI / 180));
                        _modelMatrix = _referenceModelMatrix * _rotX * _rotY;
                    }
                }
            }
            else if (_lastRotYAngle < rotYAngle)
            {
                for (double i = _lastRotYAngle; i < rotYAngle; i = i + _sensitivityRot)
                {
                    lock (_lock2)
                    {
                        _rotY = Matrix4.CreateRotationY((float)(i * Math.PI / 180));
                        _modelMatrix = _referenceModelMatrix * _rotX * _rotY;
                    }
                }
            }
            else
            {
                lock (_lock2)
                {
                    _rotY = Matrix4.CreateRotationY((float)(rotYAngle * Math.PI / 180));
                    _modelMatrix = _referenceModelMatrix * _rotX * _rotY;
                }
            }
            _lastRotYAngle = rotYAngle;
        }
        private enum TransformType
        {
            Rotate,
            Translate
        }
    }
}
