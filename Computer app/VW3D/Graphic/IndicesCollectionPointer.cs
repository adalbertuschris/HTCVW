using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VW3D.Graphic
{
    struct IndicesCollectionPointer
    {
        int _position;
        int _length;
        int _stride;

        public int Position => _position;

        public int Length => _length;

        public int Stride => _stride;

        public IndicesCollectionPointer(int position, int length, int stride)
        {
            _position = position;
            _length = length;
            _stride = stride;
        }
    }
}
