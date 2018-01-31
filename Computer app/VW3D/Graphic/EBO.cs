using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace VW3D.Graphic
{
    class EBO
    {
        private readonly int _idEBO;
        private int _elementCount;
        public int ID { get { return _idEBO; } }
        public int ElementCount { get { return _elementCount; } }
        public int PointerSize { get; private set; }
        public static EBO Create(int[] tab)
        {
            int pointerSize = 1;
            int idBuff = GL.GenBuffer();
            //System.Diagnostics.Debug.WriteLine("EBOBuff: {0}", idBuff);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, idBuff);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(tab.Length * sizeof(int)), tab, BufferUsageHint.DynamicDraw);
            //Console.WriteLine("EboSize: {0}", tab.Length);
            return new EBO(idBuff, pointerSize, tab.Length);
        }
        

        private EBO(int id, int pointerSize, int tabLength)
        {
            _idEBO = id;
            PointerSize = pointerSize;
            _elementCount = tabLength;
        }
    }
}
