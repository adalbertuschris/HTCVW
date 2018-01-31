using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpFont;
using VW3D.Graphic.GraphicPipeline;
using VW3D.Graphic;

namespace VW3D.Text
{
    class TextGenerator
    {
        ProgramShader _textPs;
        Dictionary<char, Character> _characters = new Dictionary<char, Character>();
        //private List<int> _texturesID = new List<int>();
        float[] vertices = new float[16];
        VAO vao;
        VBO vbo;
        //EBO ebo;
        IndicesCollection indices;

        public Matrix4 ProjMatrix { get; set; }
        public string FontName { get; private set; }
        public uint FontSize { get; private set; }
        
        void LoadProgramShader()
        {
            _textPs = ProgramShader.CreateProgram();
            Shader vsText = Shader.CreateShader(Graphic.GraphicPipeline.ShaderType.VertexShader, @"Shaders\vs_text.glsl");
            Shader fsText = Shader.CreateShader(Graphic.GraphicPipeline.ShaderType.FragmentShader, @"Shaders\fs_text.glsl");
            _textPs.AttachShader(vsText);
            _textPs.AttachShader(fsText);
        }

        public TextGenerator(string path, string fontName, uint fontSize = 12)
        {
            if (_textPs == null)
            {
                LoadProgramShader();
            }

            FontName = fontName;
            FontSize = fontSize;

            SharpFont.Library fontLib = new SharpFont.Library();
            SharpFont.Face fontFace = new SharpFont.Face(fontLib, path);
            fontFace.SetPixelSizes(0, FontSize);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            for (char c = (char)0x20; c <= (char)0x7E; c++)
            {
                fontFace.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);

                Texture texture = Texture.CreateTexture(
                    fontFace.Glyph.Bitmap.Width, 
                    fontFace.Glyph.Bitmap.Rows, 
                    PixelInternalFormat.R8, 
                    PixelFormat.Red, 
                    fontFace.Glyph.Bitmap.Buffer);
                
                Character character = new Character(
                    texture.ID,
                    new Vector2(fontFace.Glyph.Bitmap.Width, fontFace.Glyph.Bitmap.Rows),
                    new Vector2(fontFace.Glyph.BitmapLeft, fontFace.Glyph.BitmapTop),
                    (int)fontFace.Glyph.Advance.X);

                _characters.Add(c, character);                
            }

            fontFace.Dispose();
            fontLib.Dispose();

            vao = VAO.Create();
            vao.LinkPS(_textPs);

            vbo = VBO.Create<float>(vertices.Length, 4, "vertex");
            indices = new IndicesCollection();
            indices.Add(4, vertices.Length);
            vao.AddVBuff(vbo);
            vao.AddIndicesCollection(indices);
        }               

        public void RenderText(string text, float x, float y, Vector3 color)
        {
            float tmpX = x;
            float tmpY = y;

            _textPs.UseProgram();
            _textPs.SetVariable("projection", ProjMatrix);
            _textPs.SetVariable("textColor", color);            

            foreach (var c in text)
            {    
                Character ch = _characters[c];

                float xpos = tmpX + ch.Bearing.X;
                float ypos = tmpY + ch.Bearing.Y - ch.Size.Y;

                float w = ch.Size.X;
                float h = ch.Size.Y;

                float[] vertices = new float[]
                {
                    xpos,     ypos + h,   0.0f, 0.0f,
                    xpos,     ypos,       0.0f, 1.0f,
                    xpos + w, ypos,       1.0f, 1.0f,
                    xpos + w, ypos + h,   1.0f, 0.0f
                };

                tmpX += ch.Advance + 1; // Bitshift by 6 to get value in pixels (2^6 = 64
                //Console.WriteLine(ch.Advance >> 6);

                Texture.SetTexture(ch.TextureID);
                vbo.ChangeData(vertices);                
                vao.Draw();
                Texture.Clear();                
            }            
        }

        public void Clear()
        {
            vao.Clear();
            foreach (var character in _characters)
            {
                GL.DeleteTexture(character.Value.TextureID);
            }
            _textPs.DeleteProgram();                    
        }
    }
}
