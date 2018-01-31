using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Globalization;

namespace M3DIL.Sources.WavefrontOBJ
{
    class VertexParser
    {
        static NumberFormatInfo _numbFormatInfo = new NumberFormatInfo();

        static VertexParser()
        {
            _numbFormatInfo.NumberDecimalSeparator = ".";
        }

        internal static Vector2 Parse(string x, string y)
        {
            Vector2 vert = new Vector2();
            vert.X = float.Parse(x, _numbFormatInfo);
            vert.Y = float.Parse(y, _numbFormatInfo);
            return vert;
        }

        internal static Vector3 Parse(string x, string y, string z)
        {
            Vector3 vert = new Vector3(Parse(x, y));
            vert.Z = float.Parse(z, _numbFormatInfo);

            return vert;
        }
    }
}
