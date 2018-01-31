using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace VW3D.Graphic
{
    class VAO
    {
        private int _idVAO;
        private GraphicPipeline.ProgramShader _ps;

        public string Name { get; set; }

        private List<VBO> _vboIDList = new List<VBO>();
        private EBO _ebo;
        private List<IndicesCollectionPointer> _eboPointers;
        public int ID => _idVAO;

        public static VAO Create()
        {
            int idVertArray = GL.GenVertexArray();
            System.Diagnostics.Debug.WriteLine("VAO_ID: {0}", idVertArray);
            return new VAO(idVertArray);
        }

        private VAO(int id)
        {
            _idVAO = id;
        }

        public void AddVBuff(VBO vbo, int stride = 0, int offset = 0)
        {
            GL.BindVertexArray(_idVAO);
            _vboIDList.Add(vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ID);
            int attribLoc = _ps.GetAttribLocation(vbo.AttribName);
            GL.VertexAttribPointer(attribLoc, vbo.PointerSize, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(attribLoc);
            System.Diagnostics.Debug.WriteLine("VBO_ID: {0}", vbo.ID);
            //GL.DeleteBuffer(vbo.ID);
            GL.BindVertexArray(0);
        }


        void AddEBuff(EBO ebo)
        {
            //_elementCount = ebo.ElementCount;
            System.Diagnostics.Debug.WriteLine("Element count: {0}", ebo.ElementCount);
            _ebo = ebo;
            //Console.WriteLine(_elementCount);
            GL.BindVertexArray(_idVAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo.ID);
            System.Diagnostics.Debug.WriteLine("EBO_ID: {0}", ebo.ID);

            GL.BindVertexArray(0);
        }

        public void AddIndicesCollection(IndicesCollection indices)
        {
            AddEBuff(EBO.Create(indices.Indices.ToArray()));            
            _eboPointers = indices.Pointers;
        }

        public void Draw()
        {
            _ps.UseProgram();
            GL.BindVertexArray(_idVAO);

            foreach (var pointer in _eboPointers)
            {
                if (pointer.Stride == 4)
                {
                    GL.DrawElements(BeginMode.Quads, pointer.Length, DrawElementsType.UnsignedInt, sizeof(int)*pointer.Position);
                }
                else
                {
                    GL.DrawElements(BeginMode.Triangles, pointer.Length, DrawElementsType.UnsignedInt, sizeof(int) * pointer.Position);
                }
            }            

            GL.BindVertexArray(0);
            GL.Flush();
        }
         

        public void Clear()
        {            
            GL.DeleteVertexArray(_idVAO);

            if (_ebo != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _ebo.ID);
                GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                GL.DeleteBuffer(_ebo.ID);
            }
            if (_vboIDList.Count != 0)
            {
                foreach (var vbo in _vboIDList)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ID);
                    GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                    
                    GL.DeleteBuffer(vbo.ID);                    
                }
            }
        }

        public void LinkPS(GraphicPipeline.ProgramShader ps)
        {
            _ps = ps; 
        }        
    }
}
