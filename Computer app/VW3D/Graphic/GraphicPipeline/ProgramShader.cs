using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace VW3D.Graphic.GraphicPipeline
{
    partial class ProgramShader
    {
        private readonly int _programID;
       
        public static ProgramShader CreateProgram()
        {
            int id = GL.CreateProgram();
            return new ProgramShader(id);
        }

        private ProgramShader(int programID)
        {
            _programID = programID;
        }

        public void AttachShader(Shader shader)
        {
            GL.AttachShader(_programID, shader.ID);
            GL.LinkProgram(_programID);
            GL.DeleteShader(shader.ID);
        }

        //public void DeleteShader(Shader shader)
        //{
        //    GL.DeleteShader(shader.ID);
        //}
        //public void LoadDefaultShaders()
        //{

        //}

        public void UseProgram()
        {            
            GL.UseProgram(_programID);
        }
        
        public void DeleteProgram()
        {
            GL.DeleteProgram(_programID);
        }

        public int GetAttribLocation(string attribName)
        {
            int attribLocation = GL.GetAttribLocation(_programID, attribName);
            //Console.WriteLine(attribName + ": " + attribLocation);
            return attribLocation;
        }

        private int GetUniformLocation(string uniformName)
        {
            int uniformLocation = GL.GetUniformLocation(_programID, uniformName);
            //Console.WriteLine(uniformName + ": " + uniformLocation);
            return uniformLocation;
        }
    }
}
