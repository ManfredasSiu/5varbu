namespace VirtualLibrary
{
    partial class LoginWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.Camera = new Emgu.CV.UI.ImageBox();
            this.buttonShutDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Camera)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label1.Location = new System.Drawing.Point(256, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 31);
            this.label1.TabIndex = 3;
            this.label1.Text = "Look at the Camera";
            // 
            // Camera
            // 
            this.Camera.Location = new System.Drawing.Point(12, 59);
            this.Camera.Name = "Camera";
            this.Camera.Size = new System.Drawing.Size(776, 379);
            this.Camera.TabIndex = 2;
            this.Camera.TabStop = false;
            // 
            // buttonShutDown
            // 
            this.buttonShutDown.FlatAppearance.BorderSize = 0;
            this.buttonShutDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShutDown.Image = global::VirtualLibrary.Properties.Resources.icons8_shutdown_321;
            this.buttonShutDown.Location = new System.Drawing.Point(760, 0);
            this.buttonShutDown.Name = "buttonShutDown";
            this.buttonShutDown.Size = new System.Drawing.Size(40, 40);
            this.buttonShutDown.TabIndex = 4;
            this.buttonShutDown.UseVisualStyleBackColor = true;
            this.buttonShutDown.Click += new System.EventHandler(this.buttonShutDown_Click);
            // 
            // LoginWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonShutDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Camera);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginWindow";
            this.Text = "LoginWindow";
            ((System.ComponentModel.ISupportInitialize)(this.Camera)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox Camera;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonShutDown;
    }
}