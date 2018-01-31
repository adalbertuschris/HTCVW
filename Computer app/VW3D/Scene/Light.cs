using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace VW3D.Scene
{
    enum AttenuationType
    {
        Constant,
        Linear,
        Quadratic,
        None
    }

    enum LightType
    {
        Point,
        Spot,
        Directional
    }

    class Light
    {
        

        public Light()
        {
            //Position = position;
            //Color = color;

            //DiffuseIntensity = diffuseintensity;
            //AmbientIntensity = ambientintensity;
        }


        //public Vector3 Color = new Vector3();
        //public float DiffuseIntensity = 1.0f;
        //public float AmbientIntensity = 0.1f;
        //public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 Ambient { get; set; } = new Vector3(0.2f, 0.2f, 0.2f);
        public Vector3 Diffuse { get; set; } = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 Specular { get; set; } = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 Direction { get; set; } = new Vector3(0, 0, 200);
        public Vector3 SpotDirection { get; set; } = new Vector3(0, 0, -1);

        //public Vector3 ambient = new Vector3(0.4f, 0.4f, 0.4f);
        //public Vector3 diffuse = new Vector3(0.1f, 0.1f, 0.1f);
        //public Vector3 specular = new Vector3(0.6f, 0.6f, 0.6f);
        //public Vector3 direction = new Vector3(0, 0, 200);
        //public Vector3 spotDirection = new Vector3(0, 0, -1);
        public int SpotExponent { get; set; } = 0;
        public int SpotCutoff { get; set; } = 180;
    }
}
