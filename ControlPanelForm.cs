using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Realtime
{
    public partial class Form1 : Form
    {

        Thread windowThread;
        Window window;
        Simulation simulation;
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("Starting...");

            Mesh mesh = new Mesh(textboxSelectedFile.Text);
            Camera cam = new Camera(new Vector3(0, 0, 3), new Vector3(0, 0, 0), 
                new Vector3(0, 1, 0), MathHelper.PiOver4, .01f, 100.0f, Height / Width);

            Simulation sim = new Simulation(mesh, cam);

            this.simulation = sim;

            windowThread = new Thread(() =>
            {
                window = new Window(800, 600, "Main Window", sim);
                window.object_color = new Vector3(ColorPanel.BackColor.R/255, ColorPanel.BackColor.G/255, ColorPanel.BackColor.B/255);
                window.Run(60.0);

           });
            windowThread.Start();
                       

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Exiting control panel");
            window.Exit();
        }


        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory() + "/meshes",
                Title = "Select a Mesh",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "obj",
                Filter = "obj files (*.obj)|*.obj",
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly =true
            };
            if (dialog.ShowDialog()  == DialogResult.OK)
            {
                string filename = dialog.FileName;
                textboxSelectedFile.Text = dialog.SafeFileName;
                System.Diagnostics.Debug.WriteLine(filename);
                simulation.mesh.LoadMesh(filename);
            }
        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textboxSelectedFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox_wireframe_CheckedChanged(object sender, EventArgs e)
        {
            window.draw_wireframe = checkBox_wireframe.Checked;
        }

        private void ButtonColor_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = true;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            MyDialog.Color = ColorPanel.ForeColor;

            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK) {
                ColorPanel.BackColor = MyDialog.Color;
                window.object_color = window.object_color = new Vector3(ColorPanel.BackColor.R / 255.0f,
                                                            ColorPanel.BackColor.G / 255.0f, ColorPanel.BackColor.B / 255.0f);
            } 
        }

        private void shaderType_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (shaderType_ListBox.SelectedIndex)
            {
                case 0:
                    window.shaderType = AvailableShaders.DEFAULT;
                    break;
                case 1:
                    window.shaderType = AvailableShaders.NORMAL;
                    break;
                default:
                    break;
            }
            window.shader_changed = true;

        }

        float last_explode_val = 0;
        private void explode_bar_Scroll(object sender, EventArgs e)
        {
            float delta =  explode_bar.Value-last_explode_val;
            last_explode_val = explode_bar.Value;
            simulation.mesh.Explode(delta / 10.0f);
        }
    }


}


