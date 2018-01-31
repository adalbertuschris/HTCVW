using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VW3D.Graphic
{
    partial class ObjectRenderer
    {
        private partial class ComponentRenderer
        {
            //Dictionary<string, Dictionary<string, Texture>> textures = new Dictionary<string, Dictionary<string, Texture>>();
            //private void SetTexture(Material material)
            //{
            //    //if (_ps.GetUniformLocation("texture.map_Ka") != -1)
            //    //{
            //    //    if (textures[material.Name]["map_Ka"] != null)
            //    //    {
            //    //        GL.Uniform1(_ps.GetUniformLocation("texture.map_Ka"), textures[material.Name]["map_Ka"].ID);
            //    //    }                
            //    //}
            //    //if (_ps.GetUniformLocation("texture.map_Kd") != -1 && textures[material.Name]["map_Kd"] != null)
            //    //{
            //    //    GL.Uniform1(_ps.GetUniformLocation("texture.map_Kd"), textures[material.Name]["map_Kd"].ID);
            //    //}
            //    //if (_ps.GetUniformLocation("texture.map_Ks") != -1 && textures[material.Name]["map_Ks"] != null)
            //    //{
            //    //    GL.Uniform1(_ps.GetUniformLocation("texture.map_Ks"), textures[material.Name]["map_Ks"].ID);
            //    //}
            //    //if (_ps.GetUniformLocation("texture.map_Ns") != -1 && textures[material.Name]["map_Ns"] != null)
            //    //{
            //    //    GL.Uniform1(_ps.GetUniformLocation("texture.map_NS"), textures[material.Name]["map_Ns"].ID);
            //    //}
            //    //if (_ps.GetUniformLocation("texture.map_d") != -1 && textures[material.Name]["map_d"] != null)
            //    //{
            //    //    GL.Uniform1(_ps.GetUniformLocation("texture.map_d"), textures[material.Name]["map_d"].ID);
            //    //}
            //}

            //void LoadTextures(Dictionary<string, Material> materials)
            //{
            //    //foreach (var material in materials)
            //    //{
            //    //    if (material.Value.HasTextures)
            //    //    {
            //    //        textures.Add(material.Key, new Dictionary<string, Texture>()
            //    //        {
            //    //            { "map_Ka", null },
            //    //            { "map_Kd", null },
            //    //            { "map_Ks", null },
            //    //            { "map_Ns", null },
            //    //            { "map_d", null }
            //    //        });

            //    //        if (material.Value.AmbientMap != null)
            //    //        {
            //    //            textures[material.Key]["map_Ka"] = Texture.CreateTexture(material.Value.AmbientMap);
            //    //        }
            //    //        if (material.Value.DiffuseMap != null)
            //    //        {
            //    //            textures[material.Key]["map_Kd"] = Texture.CreateTexture(material.Value.DiffuseMap);
            //    //        }
            //    //        if (material.Value.SpecularMap != null)
            //    //        {
            //    //            textures[material.Key]["map_Ks"] = Texture.CreateTexture(material.Value.SpecularMap);
            //    //        }
            //    //        if (material.Value.SpecularExpMap != null)
            //    //        {
            //    //            textures[material.Key]["map_Ns"] = Texture.CreateTexture(material.Value.SpecularExpMap);
            //    //        }
            //    //        if (material.Value.DissolveMap != null)
            //    //        {
            //    //            textures[material.Key]["map_d"] = Texture.CreateTexture(material.Value.DissolveMap);
            //    //        }
            //    //    }
            //    //}
            //}
        }
    }
}
