using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using M3DIL.Sources.WavefrontOBJ;
using System.IO;
using System.Globalization;

namespace M3DIL
{
    public class Geometry
    {
        //public static double minZ = 0;
        //public static double maxZ = 0;

        //public static double minX = 0;
        //public static double maxX = 0;

        //public static double minY = 0;
        //public static double maxY = 0;

        public List<Vector3> VertCoords = new List<Vector3>();
        public List<Vector3> Normals = new List<Vector3>();
        public List<Vector2> TexCoords = new List<Vector2>();

        Dictionary<int, string> vertCoordsDamaged = new Dictionary<int, string>();
        Dictionary<int, string> normalsDamaged = new Dictionary<int, string>();
        Dictionary<int, string> texCoordsDamaged = new Dictionary<int, string>();

        public void AddVertCoords(IEnumerable<string> vertices)
        {
            string[] tmpVertex = vertices.ToArray();

            try
            {
                if (tmpVertex.Length != 3)
                {
                    throw new ArgumentException();
                }
                Vector3 vert3 = VertexParser.Parse(tmpVertex[0], tmpVertex[1], tmpVertex[2]);
                VertCoords.Add(vert3);
            }
            catch
            {
                vertCoordsDamaged.Add(VertCoords.Count, String.Join(" ", vertices));
                //Console.WriteLine(String.Join(" ", vertexes));
                VertCoords.Add(Vector3.Zero);
            }
            
        }

        public void AddNormals(IEnumerable<string> vertices)
        {
            string[] tmpVertex = vertices.ToArray();

            try
            {
                if (tmpVertex.Length != 3)
                {
                    throw new ArgumentException();
                }
                Vector3 vert3 = VertexParser.Parse(tmpVertex[0], tmpVertex[1], tmpVertex[2]);
                Normals.Add(vert3);
            }
            catch
            {
                normalsDamaged.Add(Normals.Count, String.Join(" ", vertices));
                //Console.WriteLine(String.Join(" ", vertexes));
                Normals.Add(Vector3.Zero);
            }
        }

        public void AddTextCoords(IEnumerable<string> vertices)
        {
            string[] tmpVertex = vertices.ToArray();

            try
            {
                if (tmpVertex.Length != 2 && tmpVertex.Length != 3)
                {
                    throw new ArgumentException();
                }
                Vector2 vert2 = VertexParser.Parse(tmpVertex[0], tmpVertex[1]);
                TexCoords.Add(vert2);
            }
            catch
            {
                texCoordsDamaged.Add(TexCoords.Count, String.Join(" ", vertices));
                //Console.WriteLine(String.Join(" ", vertexes));
                TexCoords.Add(Vector2.Zero);
            }
        }        

        public List<Vector3> GetVertices(Face face)
        {
            if (face.Contains(ElementType.Vertex))
            {
                List<Vector3> tmpList = new List<Vector3>();

                int[] indexes = face.GetElements(ElementType.Vertex);

                for (int i = 0; i < face.MeshSize; i++)
                {
                    tmpList.Add(VertCoords[indexes[i] - 1]);
                }
                return tmpList;
            }
            else
            {
                return null;
            }
        }

        public List<Vector3> GetVertices(IEnumerable<Face> faces)
        {
            List<Vector3> tmpList = new List<Vector3>();

            foreach (var face in faces)
            {
                tmpList.AddRange(GetVertices(face));
            }
            return tmpList;
        }
        
        public List<Vector3> GetNormals(Face face)
        {
            if (face.Contains(ElementType.Normal))
            {
                List<Vector3> tmpList = new List<Vector3>();

                int[] indexes = face.GetElements(ElementType.Normal);

                for (int i = 0; i < face.MeshSize; i++)
                {
                    tmpList.Add(Normals[indexes[i] - 1]);
                }
                return tmpList;
            }
            else
            {
                return null;
            }
        }

        public List<Vector3> GetNormals(IEnumerable<Face> faces)
        {
            List<Vector3> tmpList = new List<Vector3>();

            foreach (var face in faces)
            {
                tmpList.AddRange(GetNormals(face));
            }
            return tmpList;
        }

        public List<Vector2> GetTextCoords(Face face)
        {
            if (face.Contains(ElementType.TextureCoord))
            {
                List<Vector2> tmpList = new List<Vector2>();

                int[] indexes = face.GetElements(ElementType.TextureCoord);

                for (int i = 0; i < face.MeshSize; i++)
                {
                    tmpList.Add(TexCoords[indexes[i]-1]);
                }
                return tmpList;
            }
            else
            {
                return null;
            }
        }

        public List<Vector2> GetTextCoords(IEnumerable<Face> faces)
        {
            List<Vector2> tmpList = new List<Vector2>();

            foreach (var face in faces)
            {
                tmpList.AddRange(GetTextCoords(face));
            }

            return tmpList;
        }
    }
}
