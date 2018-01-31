using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace VW3D.Graphic
{
    class VBO
    {
        private readonly int _idVBO;
        private readonly string _attribName;

        public int ID { get => _idVBO; }

        public int PointerSize { get; private set; }

        public string AttribName { get => _attribName; }


        public static VBO Create(Vector2 [] tab, string attribName)
        {
            int pointerSize = 2;
            int idBuff = GL.GenBuffer();
            //System.Diagnostics.Debug.WriteLine("VBOBuff: {0}", idBuff);
            GL.BindBuffer(BufferTarget.ArrayBuffer, idBuff);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(tab.Length * Vector2.SizeInBytes), tab, BufferUsageHint.StaticDraw);
            
            return new VBO(idBuff, pointerSize, attribName);
        }

        public static VBO Create(Vector3 [] tab, string attribName)
        {
            //GL.BindVertexArray(1);
            int idBuff = GL.GenBuffer();
            //System.Diagnostics.Debug.WriteLine("VBOBuff: {0}", idBuff);
            GL.BindBuffer(BufferTarget.ArrayBuffer, idBuff);
            int pointerSize = 3;
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(tab.Length * Vector3.SizeInBytes), tab, BufferUsageHint.DynamicDraw);
            
            return new VBO(idBuff, pointerSize, attribName);
        }

        public static VBO Create<T>(int bufforLength, int numberOfComponents, string attribName)
        {
            //GL.BindVertexArray(1);
            int idBuff = GL.GenBuffer();
            //System.Diagnostics.Debug.WriteLine("VBOBuff: {0}", idBuff);
            GL.BindBuffer(BufferTarget.ArrayBuffer, idBuff);
            GL.BufferData(BufferTarget.ArrayBuffer, bufforLength * Marshal.SizeOf(typeof(T)), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            return new VBO(idBuff, numberOfComponents, attribName);
        }

        public void ChangeData<T>(T[] data) where T: struct
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _idVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Marshal.SizeOf(typeof(T))), data, BufferUsageHint.DynamicDraw);
        }

        private VBO(int id, int pointerSize, string attribName)
        {
            _idVBO = id;
            PointerSize = pointerSize;
            _attribName = attribName;
        }
    }
}
