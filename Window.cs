using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;

namespace Realtime
{    
    public class Window : GameWindow {

        Simulation simulation;

        Shader shader;
        Stopwatch stopwatch;


        // buffer hadnles

        
        int VertexBufferObject;
        int VertexArrayObject;
        int NormalBufferObject;

        // uniforms
        int viewUniform;
        int modelUniform;
        int projectionUniform;
        int colorUniform;
        int camPosUniform;

        // control variables
        public bool draw_wireframe = false;
        public Vector3 object_color;
        public AvailableShaders shaderType = AvailableShaders.FLAT;
        public bool shader_changed = true;


        public Window(int width, int height, string title, Simulation simulation) : base(width, height, GraphicsMode.Default, title) {

            this.simulation = simulation;
            object_color = new Vector3(1, 0, 0);
        }


        private Vector2 drag_pos = new Vector2(0, 0);
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            if (dragging)
            {
                MouseState mouse = Mouse.GetCursorState();
                drag_pos.X = mouse.X;
                drag_pos.Y = mouse.Y;
               
                drag_pos = drag_pos - start_drag;               

                // compute change in position, normalized 
                drag_pos.X = drag_pos.X * Width / Height / Width;
                drag_pos.Y = drag_pos.Y/Height;
                float scale = -5;
                // construct some rotation matrices               
                Matrix3 x_rot_mat = Matrix3.CreateRotationY(scale * drag_pos.X);
                Vector3 axis = Vector3.Cross(new Vector3(0, 1, 0), simulation.camera.Eye - simulation.camera.At );
                Matrix3 y_rot_mat = Matrix3.CreateFromAxisAngle(axis, scale * drag_pos.Y);

                // update camera pos
                simulation.camera.Eye = Vector3.Transform(x_rot_mat, simulation.camera.Eye);
                simulation.camera.Eye = Vector3.Transform(y_rot_mat, simulation.camera.Eye);
                
                // update for next frame
                start_drag.X = mouse.X;
                start_drag.Y = mouse.Y;
            }                        
            base.OnUpdateFrame(e);            
        }


        bool dragging = false;
        private Vector2 start_drag = new Vector2(0, 0);       
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            { 
                dragging = true;
                MouseState mouse = Mouse.GetCursorState();
                start_drag.X = mouse.X;
                start_drag.Y = mouse.Y;
            }
            base.OnMouseDown(e);
        }       

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                dragging = false;
            }
            base.OnMouseUp(e);
        }

        private float scroll_scale = .5f;
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.DeltaPrecise > 0)
                simulation.camera.Eye += scroll_scale * (simulation.camera.At - simulation.camera.Eye).Normalized();
            else
                simulation.camera.Eye -= scroll_scale * (simulation.camera.At - simulation.camera.Eye).Normalized();
            base.OnMouseWheel(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            
            GL.ClearColor(.2f, .3f, .3f, 1.0f);

            BindArrays();
            shader = new Shader("shaders/flat.vs", "shaders/flat.fs");
            shader.Use();

            // get uniforms
            modelUniform = GL.GetUniformLocation(shader.GetHandle(), "model");
            viewUniform = GL.GetUniformLocation(shader.GetHandle(), "view");
            projectionUniform = GL.GetUniformLocation(shader.GetHandle(), "projection");
            colorUniform = GL.GetUniformLocation(shader.GetHandle(), "color");
            camPosUniform = GL.GetUniformLocation(shader.GetHandle(), "camPos");

            GL.Enable(EnableCap.DepthTest);
            stopwatch = new Stopwatch();
            stopwatch.Start();
            base.OnLoad(e);
        }

        private void BindArrays()
        {
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, simulation.mesh.GetVerticesArray().Length * sizeof(float), 
                simulation.mesh.GetVerticesArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
                       

            NormalBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, simulation.mesh.GetVertexNormalsArray().Length * sizeof(float), 
                simulation.mesh.GetVertexNormalsArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (simulation.mesh.update_buffers) { // need to rebind buffers because the mesh changed
                BindArrays();
                simulation.mesh.update_buffers = false;
            }

            if (shader_changed)
            {
                shader.Dispose();
                switch (shaderType)
                {                    
                    case AvailableShaders.FLAT:
                        shader = new Shader("shaders/flat.vs", "shaders/flat.fs");
                        break;
                    case AvailableShaders.NORMAL:
                        shader = new Shader("shaders/normal.vs", "shaders/normal.fs");
                        break;
                    case AvailableShaders.POINT_LIGHT:
                        shader = new Shader("shaders/point_light.vs", "shaders/point_light.fs");
                        break;
                    default:
                        break;
                }                       
                shader.Use();

                // get uniforms
                modelUniform = GL.GetUniformLocation(shader.GetHandle(), "model");
                viewUniform = GL.GetUniformLocation(shader.GetHandle(), "view");
                projectionUniform = GL.GetUniformLocation(shader.GetHandle(), "projection");
                colorUniform = GL.GetUniformLocation(shader.GetHandle(), "color");
                camPosUniform = GL.GetUniformLocation(shader.GetHandle(), "camPos");

                shader_changed = false;
            }

            shader.Use();            
          
            // update uniforms
            Matrix4 model = Matrix4.Identity;
            GL.UniformMatrix4(modelUniform, false, ref model);     

            Matrix4 view = simulation.camera.View;
            GL.UniformMatrix4(viewUniform, false, ref view);
            
            Matrix4 projection = simulation.camera.Projection;
            GL.UniformMatrix4(projectionUniform, false, ref projection);

            GL.Uniform3(colorUniform, object_color);

            GL.Uniform3(camPosUniform, simulation.camera.Eye);
                     
            if (draw_wireframe)
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            else
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, simulation.mesh.GetVerticesArray().Length);
            GL.BindVertexArray(0);

            // GL.DrawElements(PrimitiveType.Triangles, simulation.mesh.GetFacesArray().Length, DrawElementsType.UnsignedInt, 0);
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }


        protected override void OnUnload(EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Unloading window");
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            shader.Dispose();
            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            simulation.camera.Aspect = Width / Height;
            base.OnResize(e);
        }


    }
   
}
