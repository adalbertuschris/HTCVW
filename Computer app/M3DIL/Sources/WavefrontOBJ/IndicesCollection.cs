using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3DIL.Sources.WavefrontOBJ
{
    class IndicesCollection : IVertexCollection
    {
        IVertexCollection _collection;
        int[] items;

        public ElementType Type { get; private set; }

        public IndicesCollection(ElementType collectionType, IEnumerable<int> indices)
        {
            Type = collectionType;
            items = indices.ToArray();
        }

        public bool Contains(ElementType faceType)
        {
            if (faceType == Type)
            {
                return true;
            }
            else
            {
                if (_collection != null)
                {
                    return _collection.Contains(faceType);
                }
            }
            return false;
        }

        public int[] GetElements(ElementType faceType)
        {
            if (faceType == Type)
            {
                return items;
            }
            else
            {
                if (_collection != null)
                {
                    return _collection.GetElements(faceType);
                }
            }
            return null;
        }

        public void Extend(IVertexCollection collection)
        {
            _collection = collection;
        }
    }
}
