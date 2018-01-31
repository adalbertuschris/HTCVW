using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;

namespace M3DIL
{
    public class Material
    {
        public string Name { get; set; }
        public Vector3 Ambient;                             //Ka
        public Vector3 Diffuse;                             //Kd
        public Vector3 Specular;                            //Ks
        public float SpecularExponent { get; set; }         //Ns
        public float Dissolve { get; set; }                  //d
        public int IllumModel { get; set; }                 //illum
        public Vector3 TransmissionFilter;                  //Tf

        public bool HasTextures { get; set; }
        //public Dictionary<string, >
        public String AmbientMap;
        public String DiffuseMap;
        public String SpecularMap;
        public String DissolveMap;
        public String SpecularExpMap;

        public Material()
        {
        }

        public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 transFilter, float specularExp = 1.0f, float dissolve = 1.0f, int illum = 0)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            SpecularExponent = specularExp;
            TransmissionFilter = transFilter;
            Dissolve = dissolve;
            IllumModel = illum;
        }        
    }
}
