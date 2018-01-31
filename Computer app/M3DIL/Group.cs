using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3DIL;
using OpenTK;
using M3DIL.Sources.WavefrontOBJ;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace M3DIL
{
    public class Group
    {
        public Material Material { get; set; }

        public bool Selected { get; set; }

        public bool Visible { get; set; } = true;

        public bool Transparent { get; set; }


        public List<Face> Faces = new List<Face>();        
    }
}
