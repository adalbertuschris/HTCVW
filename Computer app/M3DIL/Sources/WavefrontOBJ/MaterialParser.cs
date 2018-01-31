using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Globalization;

namespace M3DIL.Sources.WavefrontOBJ
{
    class MaterialParser
    {
        private Dictionary<String, Material> _materials = new Dictionary<String, Material>();

        private static NumberFormatInfo _numbFormatInfo = new NumberFormatInfo();        

        static MaterialParser()
        {
            _numbFormatInfo.NumberDecimalSeparator = ".";
        }

        private MaterialParser()
        {
        }

        public static Dictionary<String, Material> LoadMaterials(string fileName)
        {
            MaterialParser parser = new MaterialParser();
            Material tmpMaterial = null;

            try
            {
                foreach (string line in File.ReadLines(fileName))
                {
                    string[] readLineItems = line.Trim().Split(' ');

                    switch (readLineItems[0])
                    {
                        case "newmtl":
                            tmpMaterial = new Material();
                            tmpMaterial.Name = string.Join(" ", readLineItems.Skip(1));
                            parser._materials.Add(tmpMaterial.Name, tmpMaterial);
                            break;
                        case "Ns":
                            tmpMaterial.SpecularExponent = (float)Convert.ToDouble(readLineItems[1], _numbFormatInfo);
                            break;
                        case "d":
                            tmpMaterial.Dissolve = (float)Convert.ToDouble(readLineItems[1], _numbFormatInfo);
                            break;
                        case "Tf":
                            tmpMaterial.TransmissionFilter = GetVec3FromTabStr(readLineItems.Skip(1).ToArray(), _numbFormatInfo);
                            break;
                        case "illum":
                            tmpMaterial.IllumModel = Convert.ToInt32(readLineItems[1]);
                            break;
                        case "Ka":
                            tmpMaterial.Ambient = GetVec3FromTabStr(readLineItems.Skip(1).ToArray(), _numbFormatInfo);
                            break;
                        case "Kd":
                            tmpMaterial.Diffuse = GetVec3FromTabStr(readLineItems.Skip(1).ToArray(), _numbFormatInfo);
                            break;
                        case "Ks":
                            tmpMaterial.Specular = GetVec3FromTabStr(readLineItems.Skip(1).ToArray(), _numbFormatInfo);
                            break;
                            //case "map_Ka":
                            //    if (!material.HasTextures)
                            //    {
                            //        material.HasTextures = true;
                            //    }
                            //    material.AmbientMap = currentLine[1];
                            //    break;
                            //case "map_Kd":
                            //    if (!material.HasTextures)
                            //    {
                            //        material.HasTextures = true;
                            //    }
                            //    material.DiffuseMap = currentLine[1];
                            //    break;
                            //case "map_Ks":
                            //    if (!material.HasTextures)
                            //    {
                            //        material.HasTextures = true;
                            //    }
                            //    material.SpecularMap = currentLine[1];
                            //    break;
                            //case "map_Ns":
                            //    if (!material.HasTextures)
                            //    {
                            //        material.HasTextures = true;
                            //    }
                            //    material.SpecularExpMap = currentLine[1];
                            //    break;
                            //case "map_d":
                            //    if (!material.HasTextures)
                            //    {
                            //        material.HasTextures = true;
                            //    }
                            //    material.DissolveMap = currentLine[1];
                            //    break;
                    }
                }
            }
            catch (FileNotFoundException f)
            {
                throw new FileNotFoundException("MTL file not exist", f);
            }
            catch (ArgumentException ae)
            {
                throw new ArgumentException("MTL file is damaged", ae);
            }
            catch (Exception)
            {
                throw;
            }

            return parser._materials;
        }

        private static Vector3 GetVec3FromTabStr(string[] vecStr, IFormatProvider provider)
        {
            if (vecStr.Length != 3)
            {
                throw new ArgumentException("Invalid data in MTL file");
            }

            var tmpVector3 = new Vector3();

            tmpVector3.X = float.Parse(vecStr[0], provider);
            tmpVector3.Y = float.Parse(vecStr[1], provider);
            tmpVector3.Z = float.Parse(vecStr[2], provider);

            return tmpVector3;
        }
    }
}
