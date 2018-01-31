using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using VW3D.Graphic.GraphicPipeline;
using System.ComponentModel;
using M3DIL;
using M3DIL.Sources.WavefrontOBJ;

namespace VW3D.Graphic
{
    partial class ObjectRenderer
    {
        //private Model _model;
        private ProgramShader _ps;
        private List<ComponentRenderer> componentRendererCollection;
       
        static ObjectRenderer()
        {
            SelectedPartMaterial = new Material(
                new Vector3(0.5137f, 0.3451f, 0.7725f),
                new Vector3(0.5137f, 0.3451f, 0.7725f),
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(1, 1, 1),
                300
                );
        }

        public static Material SelectedPartMaterial { get; set; }

        public bool Selected { get; set; }

        public bool Visible { get; set; } = true;

        public bool EnableTransparent { get; set; }

        public static ObjectRenderer CreateObject(Model model, ProgramShader ps)
        {
            ObjectRenderer objectRenderer = new ObjectRenderer();
            //objectRenderer._model = model;
            objectRenderer._ps = ps;

            objectRenderer.componentRendererCollection = new List<ComponentRenderer>();
            
            foreach (var part in model.Parts.Values)
            {
                foreach (var group in part.Groups)
                {
                    List<Vector3> verts = new List<Vector3>();
                    //List<int> inds = new List<int>();
                    List<Vector2> texCoords = new List<Vector2>();
                    List<Vector3> normals = new List<Vector3>();
                    IndicesCollection indices = new IndicesCollection();

                    foreach (var face in group.Faces)
                    {                        
                        try
                        {
                            if (face.Contains(ElementType.Vertex))
                            {
                                List<Vector3> tmpList = model.Geometry.GetVertices(face);
                                verts.AddRange(tmpList);
                                indices.Add(face.MeshSize, tmpList.Count);
                            }

                            if (face.Contains(ElementType.Normal))
                            {
                                normals.AddRange(model.Geometry.GetNormals(face));
                            }

                            if (face.Contains(ElementType.TextureCoord))
                            {
                                texCoords.AddRange(model.Geometry.GetTextCoords(face));
                            }

                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    

                    VAO vao = VAO.Create();
                    //EBO eboIndices = EBO.Create(inds.ToArray());
                    VBO vboCoords = VBO.Create(verts.ToArray(), "position");
                    VBO vboNormals = VBO.Create(normals.ToArray(), "normal");
                    VBO vboTexCoords = VBO.Create(texCoords.ToArray(), "texCoords");
                    vao.LinkPS(objectRenderer._ps);
                    vao.AddIndicesCollection(indices);
                    //vao.AddEBuff(eboIndices);
                    vao.AddVBuff(vboCoords);
                    vao.AddVBuff(vboNormals);
                    vao.AddVBuff(vboTexCoords);
                    ComponentRenderer cr = new ComponentRenderer(group, vao, objectRenderer._ps);
                    objectRenderer.componentRendererCollection.Add(cr);
                }
            }
            return objectRenderer;
        }        

        public void Draw()
        {
            foreach (var item in componentRendererCollection)
            {
                item.Draw();
            }                
        }

        public void Clear()
        {
            foreach (var item in componentRendererCollection)
            {
                item.Clear();
            }
        }
    }
}
