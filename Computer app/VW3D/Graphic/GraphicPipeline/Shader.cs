using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace VW3D.Graphic.GraphicPipeline
{
    enum ShaderType
    {
        VertexShader,
        FragmentShader
    }
    class Shader
    {
        private readonly int _shaderID;

        private Shader(int id)
        {
            _shaderID = id;
        }

        public int ID { get { return _shaderID; } }

        public static Shader CreateShader(ShaderType shaderType, string path)
        {            
            int shaderID = 0;
            try
            {
                LoadShaderFromFile(shaderType, path, out shaderID);
            }
            catch (FileNotFoundException f)
            {
                throw new FileNotFoundException($"Shader not found: {f.FileName}", f);
            }
            
            //obsługa gdy 0, czyli operacja wczytania shadera się nie powiodła;

            Shader shader = new Shader(shaderID);

            return shader;
        }

        private static void LoadShaderFromFile(ShaderType shaderType, string path, out int shaderID)
        {
            shaderID = 0;
            string shaderCode = File.ReadAllText(path);

            switch (shaderType)
            {
                case ShaderType.VertexShader:
                    shaderID = LoadShader(shaderCode, OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
                    break;
                case ShaderType.FragmentShader:
                    shaderID = LoadShader(shaderCode, OpenTK.Graphics.OpenGL.ShaderType.FragmentShader);
                    break;
            }       
        }

        private static int LoadShader(string code, OpenTK.Graphics.OpenGL.ShaderType type)
        {
            int id = GL.CreateShader(type);
            GL.ShaderSource(id, code);
            GL.CompileShader(id);
            return id;            
        }
    }
}
