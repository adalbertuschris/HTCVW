using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3DIL
{
    [Flags]
    public enum ElementType
    {
        Vertex = 0x01,
        TextureCoord = 0x02,
        Normal = 0x04
    }
}
