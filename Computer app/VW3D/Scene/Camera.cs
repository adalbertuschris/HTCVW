using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace VW3D.Scene
{
    class Camera
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Orientation { get; set; } = new Vector3(0f, 0f, 0f);        

        public Matrix4 ViewMatrix
        {
            get
            {
                return Matrix4.LookAt(Position, Orientation, Vector3.UnitY);
            }
        }
    }
}
