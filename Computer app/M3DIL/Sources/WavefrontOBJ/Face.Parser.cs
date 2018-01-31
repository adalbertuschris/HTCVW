using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3DIL.Sources.WavefrontOBJ
{
    partial class Face
    {
        private class Parser
        {
            List<int> vertIndices;
            List<int> normalIndices;
            List<int> textCoordIndices;

            public static Face CreateFace(IEnumerable<string> face)
            {
                Parser parser = new Parser();
                ElementType faceType = SetType(face);

                if (faceType.HasFlag(ElementType.Vertex))
                {
                    parser.vertIndices = new List<int>();
                }

                if (faceType.HasFlag(ElementType.TextureCoord))
                {
                    parser.textCoordIndices = new List<int>();
                }

                if (faceType.HasFlag(ElementType.Normal))
                {
                    parser.normalIndices = new List<int>();
                }

                foreach (var item in face)
                {
                    parser.ParseFaceItem(item, faceType);
                }

                return new Face(faceType, parser.vertIndices, parser.textCoordIndices, parser.normalIndices);
            }
            
            private void ParseFaceItem(string faceItem, ElementType faceType)
            {
                int position;
                int textCoord;
                int normal;
                bool success;

                string[] partsOfFaceItem = faceItem.Split(new char[] { '/', ' ' });

                if (faceType.HasFlag(ElementType.Vertex))
                {
                    success = int.TryParse(partsOfFaceItem[0], out position);
                    if (success)
                    {
                        vertIndices.Add(position);
                    }
                }

                if (faceType.HasFlag(ElementType.TextureCoord))
                {
                    success = int.TryParse(partsOfFaceItem[1], out textCoord);                    
                    if (success)
                    {
                        textCoordIndices.Add(textCoord);
                    }
                }

                if (faceType.HasFlag(ElementType.Normal))
                {
                    success = int.TryParse(partsOfFaceItem[2], out normal);
                    if (success)
                    {
                        normalIndices.Add(normal);
                    }
                }
            }

            private static ElementType SetType(IEnumerable<string> face)
            {
                ElementType faceType = ElementType.Vertex;

                //Check face
                var faceItemGroups = face.GroupBy((item) => item.Where(c => c == '/').Count());

                
                if (faceItemGroups.Count() == 1)//else - face damaged
                {
                    //Set type
                    IGrouping<int, string> faceGroup = faceItemGroups.Single();

                    switch (faceGroup.Key)
                    {
                        case 1: 
                            faceType |= ElementType.TextureCoord;// vi1/vti1 vi2/vti2 vi3/vti3 ...
                            break;
                        case 2:
                            faceType |= ElementType.Normal;// vi1//vni1 vi2//vni2 vi3//vni3 ...

                            if (!faceGroup.First().Contains("//")) 
                            {
                                faceType |= ElementType.TextureCoord;// vi1/vti1/vni1 vi2/vti2/vni2 vi3/vti3/vni3 ...
                            }                            
                            break;
                    }
                }
                else
                {
                    throw new ArgumentException("Face damaged");
                }
                return faceType;
            }
        }
    }
}
