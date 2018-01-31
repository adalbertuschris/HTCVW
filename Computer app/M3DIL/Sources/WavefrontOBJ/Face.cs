using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3DIL.Sources.WavefrontOBJ
{
    public partial class Face
    {
        IVertexCollection collection;

        public int MeshSize { get; set; }

        public ElementType Type { get; private set; }

        public static Face Parse(IEnumerable<string> face)
        {
            return Parser.CreateFace(face);
        }

        private Face(ElementType faceType, IEnumerable<int> vertexes, IEnumerable<int> textCoords, IEnumerable<int> normals)
        {
            MeshSize = vertexes.Count();

            if (faceType.HasFlag(ElementType.Vertex))
            {
                Type = ElementType.Vertex;
                collection = new VertexIndicesCollection(vertexes);                
            }

            if (faceType.HasFlag(ElementType.Normal))
            {
                Type |= ElementType.Normal;
                var normal = new IndicesCollection(ElementType.Normal, normals);

                if (collection != null)
                {
                    normal.Extend(collection);
                }

                collection = normal;
            }

            if (faceType.HasFlag(ElementType.TextureCoord))
            {
                Type |= ElementType.TextureCoord;
                var textCoord = new IndicesCollection(ElementType.TextureCoord, textCoords);

                if (collection != null)
                {
                    textCoord.Extend(collection);
                }

                collection = textCoord;
            }
        }        

        public int[] GetElements(ElementType faceType)
        {
            return collection.GetElements(faceType);
        }

        public bool Contains(ElementType faceType)
        {
            return collection.Contains(faceType);
        }
    }
}
