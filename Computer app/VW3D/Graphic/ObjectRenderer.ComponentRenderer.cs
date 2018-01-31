using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3DIL;
using M3DIL.Sources.WavefrontOBJ;
using VW3D.Graphic.GraphicPipeline;

namespace VW3D.Graphic
{
    partial class ObjectRenderer
    {
        private partial class ComponentRenderer
        {            
            private ProgramShader _ps;
            private VAO _vao;
            private Group _group;

            //bool Transparent
            //{
            //    get
            //}
            public ComponentRenderer(Group group, VAO vao, ProgramShader ps)
            {
                _ps = ps;
                _vao = vao;
                _group = group;
            }
            
            public void Draw()
            {
                SetOpacity(1);
                if (SceneRenderer.WorkMode)
                {
                    SetOpacity(SceneRenderer.Opacity);
                    
                    if (_group.Selected)
                    {
                        SetOpacity(0.8f);
                        SetMaterial(ObjectRenderer.SelectedPartMaterial);                        
                    }
                    else
                    {
                        SetMaterial(_group.Material);
                    }

                    _vao.Draw();
                }
                else
                {
                    if (_group.Visible)
                    {
                        if (_group.Transparent)
                        {
                            SetOpacity(SceneRenderer.Opacity);
                        }

                        SetMaterial(_group.Material);
                        _vao.Draw();
                    }
                }
            }

            private void SetMaterial(Material material)
            {
                _ps.SetVariable("material.ambient", material.Ambient);
                _ps.SetVariable("material.diffuse", material.Diffuse);
                _ps.SetVariable("material.specular", material.Specular);
                _ps.SetVariable("material.shininess", material.SpecularExponent);
            }

            private void SetOpacity(float opacity)
            {
                _ps.SetVariable("opacity", opacity);
            }

            public void Clear()
            {
                _vao.Clear();
            }
        }
    }
}
