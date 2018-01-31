using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace VW3D.Text
{
    struct Character
    {
        public int TextureID { get; private set; }  
        public Vector2 Size { get; private set; }     
        public Vector2 Bearing { get; private set; }    // Offset from baseline to left/top of glyph
        public int Advance { get; private set; }

        public Character(int textureID, Vector2 size, Vector2 bearing, int advance)
        {
            TextureID = textureID;
            Size = size;
            Bearing = bearing;
            Advance = advance;
        }
    }
}
