using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3DIL.Sources.WavefrontOBJ
{
    interface IVertexCollection
    {
        int[] GetElements(ElementType faceType);

        bool Contains(ElementType faceType);

        ElementType Type { get; }
    }
}
