using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using System.Collections.Generic;


namespace Realtime
{
   public class Mesh
    {

        private float[] vertices_array;
        private uint[] faces_array;
        private float[] vertex_normals_array;

        private List<Vertex> vertex_objects;
        private List<Face> face_objects;

        public bool update_buffers = true;
        public Mesh()
        { 
            vertices_array = new float[] { };
            faces_array = new uint[]  { };
            vertex_normals_array = new float[] { };
        }

        public Mesh(string filename)
        {
            this.LoadMesh(filename);
        }

        public void LoadMesh(string filename)
        {
           
            update_buffers = true;
            System.IO.StreamReader f = new System.IO.StreamReader(filename);
            string line;
            int v_counter = 0;
            int f_counter = 0;
            List<float> vertices_list = new List<float>();
            List<uint> faces_v_list = new List<uint>();

            List<float> vertex_normals_list = new List<float>();
            List<uint> faces_vn_list = new List<uint>();

            vertex_objects = new List<Vertex>();
            face_objects = new List<Face>();
           

            float x_ave = 0;
            float y_ave = 0;
            float z_ave = 0;

            Vector2 x_extremes = new Vector2(float.MaxValue, float.MinValue);
            Vector2 y_extremes = new Vector2(float.MaxValue, float.MinValue);
            Vector2 z_extremes = new Vector2(float.MaxValue, float.MinValue);

            while ((line = f.ReadLine()) != null)
            {
                if (line.StartsWith("v "))
                {
                    string[] split = line.Split(" ");
                    float x = float.Parse(split[1]);
                    float y = float.Parse(split[2]);
                    float z = float.Parse(split[3]);
                    vertices_list.Add(x);
                    vertices_list.Add(y);
                    vertices_list.Add(z);

                    x_ave += x;
                    y_ave += y;
                    z_ave += z;

                    x_extremes.X = Math.Min(x_extremes.X, x);
                    y_extremes.X = Math.Min(y_extremes.X, y);
                    z_extremes.X = Math.Min(z_extremes.X, z);

                    x_extremes.Y = Math.Max(x_extremes.Y, x);
                    y_extremes.Y = Math.Max(y_extremes.Y, y);
                    z_extremes.Y = Math.Max(z_extremes.Y, z);

                    v_counter++;
                } else if (line.StartsWith('f'))
                {
                    string[] split = line.Split(" ");

                    for (int i = 1; i <= 3; i++)
                    {
                        string data = split[i];
                        if (data.Contains("//")) { // just vertex and normal
                            string[] d = data.Split("//");
                            faces_v_list.Add(uint.Parse(d[0]) - 1);
                            faces_vn_list.Add(uint.Parse(d[1]) - 1);

                        }
                        else if (data.Contains('/'))
                        {
                            string[] d = data.Split("/");
                            if (d.Length == 3) // vertex, uv, normal
                            {
                                faces_v_list.Add(uint.Parse(d[0]) - 1);
                                // TODO: parse for uv info
                                faces_vn_list.Add(uint.Parse(d[2]) - 1);
                            }
                            else // vertex, uv
                            {
                                faces_v_list.Add(uint.Parse(d[0]) - 1);
                                // TODO: parse for uv info
                            }
                        } else
                        {
                            faces_v_list.Add(uint.Parse(split[i]) - 1);
                        }
                        
                    }
                  
                    f_counter++;
                }  
                else if (line.StartsWith("vn "))
                {
                    string[] split = line.Split(" ");
                    vertex_normals_list.Add(float.Parse(split[1]));
                    vertex_normals_list.Add(float.Parse(split[2]));
                    vertex_normals_list.Add(float.Parse(split[3]));
                   
                }
            }

            f.Close();

            System.Diagnostics.Debug.WriteLine("Built object with " + v_counter + " vertices and " + f_counter + " faces");
            vertices_array = vertices_list.ToArray();
            faces_array = faces_v_list.ToArray();  
            

            float scale = Math.Max(x_extremes.Y - x_extremes.X, y_extremes.Y - y_extremes.X);
            scale = Math.Max(scale, z_extremes.Y - z_extremes.X);

            x_ave /= v_counter;
            y_ave /= v_counter;
            z_ave /= v_counter;

            System.Diagnostics.Debug.WriteLine("Corrected object with scale " + scale
                + " and offset (" + x_ave + ", " + y_ave + ", " + z_ave + ")");

            for (int i= 0; i < vertices_array.Length/3; i++)
            {
                vertices_array[3 * i] = (vertices_array[3 * i] - x_ave) / scale;
                vertices_array[3 * i + 1] = (vertices_array[3 * i + 1] - y_ave) / scale;
                vertices_array[3 * i + 2] = (vertices_array[3 * i + 2] - z_ave) / scale;

            }

           
            for (int i = 0; i < faces_array.Length/3; i++)
            {

                Vertex[] vertices = new Vertex[3];
                for (int j = 0; j < 3; j++)
                {
                    // get index of vertex for the face
                    int v_ind = (int)faces_v_list[3 * i+j];
                    // get vec3 pos of vertex
                    Vector3 pos = new Vector3(vertices_array[v_ind * 3], vertices_array[v_ind * 3 + 1], vertices_array[v_ind * 3 + 2]);
                    Vector3 vn = Vector3.Zero;
                    if (vertex_normals_list.Count > 0)
                    {
                        int vn_ind = (int)faces_vn_list[3 * i + j];
                        vn = new Vector3(vertex_normals_list[vn_ind * 3], vertex_normals_list[vn_ind * 3 + 1], vertex_normals_list[vn_ind * 3 + 2]);
                    }

                    if (vn != Vector3.Zero)
                    {
                        vertices[j] = new Vertex(pos, vn);
                    }
                    else
                    {
                        vertices[j] = new Vertex(pos);
                    }
                }                             

                Face face = new Face(vertices[0], vertices[1], vertices[2]);
                              
                face_objects.Add(face);
            }
            UpdateArraysFromFaces();
        }

        public void Explode(float s)
        {
            System.Diagnostics.Debug.Print("Explode: " + s);
            update_buffers = true;

            for (int i = 0; i < face_objects.Count; i++)
            {
                Face f = face_objects[i];
                Vector3 n = f.getNormal();
                n = Vector3.Multiply(n,s);
                f.V1.Position += n;
                f.V2.Position += n;
                f.V3.Position += n;
            }
             UpdateArraysFromFaces();
        }

        private void UpdateArraysFromFaces()
        {
            List<float> final_vertices_array = new List<float>();
            List<float> final_vn_array = new List<float>();
            for (int i = 0; i < face_objects.Count; i++)
            {
                Face face = face_objects[i];
                // vertex pos array
                final_vertices_array.Add(face.V1.Position.X);
                final_vertices_array.Add(face.V1.Position.Y);
                final_vertices_array.Add(face.V1.Position.Z);

                final_vertices_array.Add(face.V2.Position.X);
                final_vertices_array.Add(face.V2.Position.Y);
                final_vertices_array.Add(face.V2.Position.Z);

                final_vertices_array.Add(face.V3.Position.X);
                final_vertices_array.Add(face.V3.Position.Y);
                final_vertices_array.Add(face.V3.Position.Z);


                if (face.V1.Normal == Vector3.Zero) // no vn provided in obj, use face normal
                {
                    Vector3 n = face.getNormal();
                    face.V1.Normal = n;
                    face.V2.Normal = n;
                    face.V3.Normal = n;
                }
                // vertex normal array
                final_vn_array.Add(face.V1.Normal.X);
                final_vn_array.Add(face.V1.Normal.Y);
                final_vn_array.Add(face.V1.Normal.Z);

                final_vn_array.Add(face.V2.Normal.X);
                final_vn_array.Add(face.V2.Normal.Y);
                final_vn_array.Add(face.V2.Normal.Z);

                final_vn_array.Add(face.V3.Normal.X);
                final_vn_array.Add(face.V3.Normal.Y);
                final_vn_array.Add(face.V3.Normal.Z);
            }

            vertices_array = final_vertices_array.ToArray();
            vertex_normals_array = final_vn_array.ToArray();

        }

        public Vector3 GetVertex(int i)
        {
            Vector3 x = new Vector3(vertices_array[3 * i], vertices_array[3 * i + 1], vertices_array[3 * i + 2]);
            return x;
        }

        public void SetVertex(int i, Vector3 v)
        {
            vertices_array[3 * i] = v.X;
            vertices_array[3 * i + 1] = v.Y;
            vertices_array[3 * i + 2] = v.Z;
        }

        public float[] GetVerticesArray()
        {
            return vertices_array;
        }

        public uint[] GetFacesArray()
        {
            return faces_array;
        }

        public float[] GetVertexNormalsArray()
        {
            return vertex_normals_array;
        }

    }

    public class Vertex
    {
        public List<Face> adjacent_faces;
        Vector3 normal;
        Vector3 position;
        Vector2 uv;

        public Vector3 Position { get => position; set => position = value; }

        public Vector3 Normal { get => normal; set => normal = value; }

        public Vertex(Vector3 p) { 
            this.position = new Vector3(p);
            this.normal = Vector3.Zero;
            adjacent_faces = new List<Face>(); 
        }

        public Vertex(Vector3 p, Vector3 vn) { 
            this.position = new Vector3(p);
            this.normal = new Vector3(vn);
            adjacent_faces = new List<Face>(); 
        }

        public bool Equals(Vertex v)
        {
            return v.position == this.position;
        }
        
    }

    public class Face
    {
        Vertex v1;
        Vertex v2;
        Vertex v3;

        public Face(Vertex v1, Vertex v2, Vertex v3) 
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public Vertex V1 { get => v1; set => v1 = value; }
        public Vertex V2 { get => v2; set => v2 = value; }
        public Vertex V3 { get => v3; set => v3 = value; }

        public Vector3 getNormal()
        {
            Vector3 e1 = v2.Position - v1.Position;
            Vector3 e2 = v3.Position - v1.Position;
            return Vector3.Cross(e1, e2).Normalized();
        }
    }
}


