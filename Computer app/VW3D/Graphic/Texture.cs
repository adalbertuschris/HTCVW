using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace VW3D.Graphic
{
    class Texture
    {
        public int ID { get; private set; }

        private Texture(int width, int height, PixelInternalFormat pixelInternalFormat, OpenTK.Graphics.OpenGL.PixelFormat pixelFormat, IntPtr data)
        {
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);

            //BitmapData data = image.LockBits(
            //    new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
            //    ImageLockMode.ReadOnly, 
            //    System.Drawing.Imaging.PixelFormat.Format32bppArgb
            //);

            GL.TexImage2D(
                TextureTarget.Texture2D, 
                0, 
                pixelInternalFormat, 
                width, 
                height, 
                0,
                pixelFormat, 
                PixelType.UnsignedByte, 
                data
                );

            //image.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureParameterName.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureParameterName.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public static void SetTexture(int idTexture)
        {
            GL.BindTexture(TextureTarget.Texture2D, idTexture);
        }

        public static void Clear()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public static Texture CreateTexture(int width, int height, PixelInternalFormat pixelInternalFormat, OpenTK.Graphics.OpenGL.PixelFormat pixelFormat, IntPtr data)
        {
            //Bitmap image = new Bitmap(fileName);
            Texture texture = new Texture(width, height, pixelInternalFormat, pixelFormat, data);
            return texture;
        }
    }
}
