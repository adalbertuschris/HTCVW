using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Threading;

namespace M3DIL
{
    public class Model
    {
        public Geometry Geometry { get; set; }

        public Dictionary<string, Material> Materials = new Dictionary<string, Material>();
       
        public Dictionary<string, Part> Parts = new Dictionary<string, Part>();

        private DispMode dispMode = DispMode.normal;

        public DispMode DisplayMode
        {
            get => dispMode;
            set
            {
                foreach (var part in Parts.Values)
                {
                    part.DisplayMode = value;
                }
                dispMode = value;
            }
        }

        public static Model Load(Sources.SourceType sourceType, string path)
        {
            switch (sourceType)
            {
                case Sources.SourceType.OBJ:
                    return new M3DIL.Sources.WavefrontOBJ.Parser().LoadModel(path);                    
            }
            return null;
        }  
    }
}