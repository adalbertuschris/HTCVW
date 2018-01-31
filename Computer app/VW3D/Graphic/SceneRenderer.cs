using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using VW3D.Graphic.GraphicPipeline;
using M3DIL;

namespace VW3D.Graphic
{
    class SceneRenderer
    {
        private Scene.Light _light;
        private ProgramShader _ps;
        private Scene.Camera _camera;
        private ViewParameter viewParam;
        private ObjectRenderer _objRenderer;
        private GameObject _gameObj;
        Matrix4 _modelViewProjectionMatrix = Matrix4.Identity;

        public static bool WorkMode { get; set; }

        public static float Opacity { get; set; } = 0.2f;

        Matrix4 ProjectionMatrix
        {
            get => Matrix4.CreatePerspectiveFieldOfView(DegToRad(viewParam.Fovy), viewParam.Width / viewParam.Height, viewParam.Near, viewParam.Far);
        }     

        public void AddLight(Scene.Light light)
        {
            _light = light;
        }

        public void AddGameObject(GameObject gameObj)
        {
            _gameObj = gameObj;
            _objRenderer = ObjectRenderer.CreateObject(gameObj.Model, _ps);
        }

        public void AddCamera(Scene.Camera camera)
        {
            _camera = camera;
        }

        public void UpdateScreenSize(float width, float height)
        {
            viewParam.Width = width;
            viewParam.Height = height;
        }

        public void SetProjectionMatrix(float fovy, float width, float height, float near, float far)
        {
            UpdateViewParameter(width, height, near, far);
            UpdateFovy(fovy);
        }

        public void UpdateViewParameter(float width, float height, float near, float far)
        {
            viewParam.Width = width;
            viewParam.Height = height;
            viewParam.Near = near;
            viewParam.Far = far;
        }

        public void UpdateFovy(float fovy)
        {
            viewParam.Fovy = fovy;
        }

        public SceneRenderer()
        {
            LoadProgramShader();
            viewParam = new ViewParameter();
        }        

        void LoadProgramShader()
        {
            _ps = ProgramShader.CreateProgram();
            Shader vs = Shader.CreateShader(Graphic.GraphicPipeline.ShaderType.VertexShader, @"Shaders\vs.glsl");
            Shader fs = Shader.CreateShader(Graphic.GraphicPipeline.ShaderType.FragmentShader, @"Shaders\fs.glsl");
            _ps.AttachShader(vs);
            _ps.AttachShader(fs);
            //ps.DeleteShader(vs);
            //ps.DeleteShader(fs);
        }

        private void SetLight()
        {
            _ps.SetVariable("light.direction", _light.Direction);
            _ps.SetVariable("light.diffuse", _light.Diffuse);
            _ps.SetVariable("light.ambient", _light.Ambient);
            _ps.SetVariable("light.specular", _light.Specular);
        }

        public void UpdateModelViewProjMatrix()
        {
            _modelViewProjectionMatrix = _gameObj.Transform.ModelMatrix * _camera.ViewMatrix * ProjectionMatrix;
        }

        public void SetModelViewProjMatrix(Matrix4 modViewProj)
        {
            _ps.SetVariable("modelViewProj", modViewProj);
        }

        void SetCamera()
        {
            _ps.SetVariable("viewPos", _camera.Position);
        }

        public void Draw()
        {
            _ps.UseProgram();
            SetLight();
            SetCamera();
            SetModelViewProjMatrix(_modelViewProjectionMatrix);
            if (_objRenderer != null)
            {
                _objRenderer.Draw();
            }                             
        }

        class ViewParameter
        {
            public float Width { get; set; }
            public float Height { get; set; }
            public float Fovy { get; set; }
            public float Near { get; set; }
            public float Far { get; set; }
        }

        private float DegToRad(float angle)
        {
            return (float)(angle * Math.PI / 180);
        }

        public void Clear()
        {
            if (_objRenderer != null)
            {
                _objRenderer.Clear();
            }

            if (_ps != null)
            {
                _ps.DeleteProgram();
            }                  
        }
    }
}
