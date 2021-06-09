
namespace Realtime
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textboxSelectedFile = new System.Windows.Forms.TextBox();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.checkBox_wireframe = new System.Windows.Forms.CheckBox();
            this.buttonColor = new System.Windows.Forms.Button();
            this.ColorPanel = new System.Windows.Forms.Panel();
            this.shaderType_ListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.explode_bar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.explode_bar)).BeginInit();
            this.SuspendLayout();
            // 
            // textboxSelectedFile
            // 
            this.textboxSelectedFile.Location = new System.Drawing.Point(93, 46);
            this.textboxSelectedFile.Name = "textboxSelectedFile";
            this.textboxSelectedFile.ReadOnly = true;
            this.textboxSelectedFile.Size = new System.Drawing.Size(215, 23);
            this.textboxSelectedFile.TabIndex = 0;
            this.textboxSelectedFile.Text = "meshes/bunny.obj";
            this.textboxSelectedFile.TextChanged += new System.EventHandler(this.textboxSelectedFile_TextChanged);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(12, 46);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadFile.TabIndex = 1;
            this.btnLoadFile.Text = "Choose file";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.LoadFileButton_Click);
            // 
            // checkBox_wireframe
            // 
            this.checkBox_wireframe.AutoSize = true;
            this.checkBox_wireframe.Location = new System.Drawing.Point(12, 75);
            this.checkBox_wireframe.Name = "checkBox_wireframe";
            this.checkBox_wireframe.Size = new System.Drawing.Size(109, 19);
            this.checkBox_wireframe.TabIndex = 3;
            this.checkBox_wireframe.Text = "Draw wireframe";
            this.checkBox_wireframe.UseVisualStyleBackColor = true;
            this.checkBox_wireframe.CheckedChanged += new System.EventHandler(this.CheckBox_wireframe_CheckedChanged);
            // 
            // buttonColor
            // 
            this.buttonColor.Location = new System.Drawing.Point(12, 139);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(92, 23);
            this.buttonColor.TabIndex = 4;
            this.buttonColor.Text = "Choose Color";
            this.buttonColor.UseVisualStyleBackColor = true;
            this.buttonColor.Click += new System.EventHandler(this.ButtonColor_Click);
            // 
            // ColorPanel
            // 
            this.ColorPanel.BackColor = System.Drawing.Color.Red;
            this.ColorPanel.Location = new System.Drawing.Point(110, 139);
            this.ColorPanel.Name = "ColorPanel";
            this.ColorPanel.Size = new System.Drawing.Size(54, 23);
            this.ColorPanel.TabIndex = 5;
            // 
            // shaderType_ListBox
            // 
            this.shaderType_ListBox.FormattingEnabled = true;
            this.shaderType_ListBox.ItemHeight = 15;
            this.shaderType_ListBox.Items.AddRange(new object[] {
            "Default",
            "Normals"});
            this.shaderType_ListBox.Location = new System.Drawing.Point(12, 191);
            this.shaderType_ListBox.Name = "shaderType_ListBox";
            this.shaderType_ListBox.Size = new System.Drawing.Size(120, 49);
            this.shaderType_ListBox.TabIndex = 6;
            this.shaderType_ListBox.Tag = "";
            this.shaderType_ListBox.SelectedIndexChanged += new System.EventHandler(this.shaderType_ListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Shader Type";
            // 
            // explode_bar
            // 
            this.explode_bar.Location = new System.Drawing.Point(67, 246);
            this.explode_bar.Name = "explode_bar";
            this.explode_bar.Size = new System.Drawing.Size(326, 45);
            this.explode_bar.TabIndex = 8;
            this.explode_bar.Scroll += new System.EventHandler(this.explode_bar_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Explode";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 505);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.explode_bar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shaderType_ListBox);
            this.Controls.Add(this.ColorPanel);
            this.Controls.Add(this.buttonColor);
            this.Controls.Add(this.checkBox_wireframe);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.textboxSelectedFile);
            this.Name = "Form1";
            this.Text = "Realtime 3D Control Panel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.explode_bar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textboxSelectedFile;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.CheckBox checkBox_wireframe;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.Panel ColorPanel;
        private System.Windows.Forms.ListBox shaderType_ListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar explode_bar;
        private System.Windows.Forms.Label label2;
    }
}

