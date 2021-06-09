using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Realtime
{

    public enum AvailableShaders
    {
        DEFAULT,
        NORMAL,

    }
    public class Shader
    {
        int Handle;

        public Shader( string vertex_filename, string fragment_filename)
        {
            int vs;
            int fs;

            string VertexShaderSource;

            using (StreamReader reader = new StreamReader(vertex_filename, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;

            using (StreamReader reader = new StreamReader(fragment_filename, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            vs = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vs, VertexShaderSource);
            fs = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fs, FragmentShaderSource);

            GL.CompileShader(vs);
            string infoLogVert = GL.GetShaderInfoLog(vs);
            if (infoLogVert != System.String.Empty)
                System.Diagnostics.Debug.WriteLine(infoLogVert);

            GL.CompileShader(fs);
            string infoLogFrag = GL.GetShaderInfoLog(fs);
            if (infoLogFrag != System.String.Empty)
                System.Diagnostics.Debug.WriteLine(infoLogFrag);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vs);
            GL.AttachShader(Handle, fs);

            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, vs);
            GL.DetachShader(Handle, fs);
            GL.DeleteShader(vs);
            GL.DeleteShader(fs);

        }
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetHandle()
        {
            return Handle;
        }


        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

   
}
