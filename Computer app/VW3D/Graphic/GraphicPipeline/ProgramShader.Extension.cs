using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace VW3D.Graphic.GraphicPipeline
{
    partial class ProgramShader
    {
        public void SetVariable(string name, int value)
        {
            if (GetUniformLocation(name) != -1)
            {
                GL.Uniform1(GetUniformLocation(name), value);
            }
            else
            {
                //todo throw exception
            }
        }

        public void SetVariable(string name, float value)
        {
            if (GetUniformLocation(name) != -1)
            {
                GL.Uniform1(GetUniformLocation(name), value);
            }
            else
            {
                //todo throw exception
            }
        }

        public void SetVariable(string name, double value)
        {
            if (GetUniformLocation(name) != -1)
            {
                GL.Uniform1(GetUniformLocation(name), value);
            }
            else
            {
                //todo throw exception
            }
        }

        public void SetVariable(string name, Vector3 value)
        {
            if (GetUniformLocation(name) != -1)
            {
                GL.Uniform3(GetUniformLocation(name), value);
            }
            else
            {
                //todo throw exception
            }
        }

        //public void SetVariable(string name, Vector2 value)
        //{
        //    if (GetUniformLocation(name) != -1)
        //    {
        //        GL.Uniform2(GetUniformLocation(name), value);
        //    }
        //    else
        //    {
        //        //todo throw exception
        //    }
        //}

        public void SetVariable(string name, Matrix4 value)
        {
            if (GetUniformLocation(name) != -1)
            {
                GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
            }
            else
            {
                //todo throw exception
            }
        }
    }
}
