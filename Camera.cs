using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace Realtime
{
    public class Camera
    {

        private Vector3 at;
        private Vector3 eye;
        private Vector3 up;

        private float fovy;
        private float near;
        private float far;
        private float aspect;

        private Matrix4 view;
        private Matrix4 projection;

        public Camera(Vector3 eye, Vector3 at, Vector3 up, float fovy, float near, float far, float aspect)
        {
            this.at = at;
            this.eye = eye;
            this.up = up;

            this.fovy = fovy;
            this.near = near;
            this.far = far;
            this.aspect = aspect;

            updateProjectionMatrix();
            updateViewMatrix();
        }

        public Vector3 At { get => at; set { at = value; updateViewMatrix(); } }
        public Vector3 Eye { get => eye; set { eye = value; updateViewMatrix(); } }
        public Vector3 Up { get => up; set { up = value; updateViewMatrix(); } }

        public float Fovy { get => fovy; set { fovy = value; updateProjectionMatrix(); } }
        public float Near { get => near; set { near = value; updateProjectionMatrix(); } }
        public float Far { get => far; set { far = value; updateProjectionMatrix(); } }
        public float Aspect { get => aspect; set { aspect = value; updateProjectionMatrix(); } }

        public Matrix4 View { get => view; }
        public Matrix4 Projection { get => projection; }

        private void updateViewMatrix()
        {
            view = Matrix4.LookAt(eye, at, up);
        }

        private void updateProjectionMatrix()
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, near, far);
        }
    }
}
