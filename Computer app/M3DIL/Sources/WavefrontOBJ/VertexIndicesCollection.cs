using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3DIL.Sources.WavefrontOBJ
{
    class VertexIndicesCollection : IVertexCollection
    {
        int[] items;

        public VertexIndicesCollection(IEnumerable<int> vertexes)
        {
            items = vertexes.ToArray();
        }

        public ElementType Type => ElementType.Vertex;

        public bool Contains(ElementType faceType)
        {
            if (faceType == Type)
            {
                return true;
            }
            return false;
        }

        public int[] GetElements(ElementType faceType)
        {
            if (faceType == Type)
            {
                return items;
            }
            return null;            
        }
    }
}
