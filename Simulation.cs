using System;
using System.Collections.Generic;
using System.Text;

namespace Realtime
{
    public class Simulation
    {

        public Mesh mesh;
        public Camera camera;

        double time;

        public Simulation(Mesh mesh, Camera camera){
            this.mesh = mesh;
            this.camera = camera;
        }

        public void Step()
        {

        }


    }

}
