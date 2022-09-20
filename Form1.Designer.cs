
namespace Mankala
{
    partial class menuForm
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
            this.buttonAI = new System.Windows.Forms.Button();
            this.button1vs1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAI
            // 
            this.buttonAI.Location = new System.Drawing.Point(351, 249);
            this.buttonAI.Name = "buttonAI";
            this.buttonAI.Size = new System.Drawing.Size(124, 42);
            this.buttonAI.TabIndex = 0;
            this.buttonAI.Text = "vs AI";
            this.buttonAI.UseVisualStyleBackColor = true;
            this.buttonAI.Click += new System.EventHandler(this.buttonAI_Click);
            // 
            // button1vs1
            // 
            this.button1vs1.Location = new System.Drawing.Point(351, 370);
            this.button1vs1.Name = "button1vs1";
            this.button1vs1.Size = new System.Drawing.Size(124, 36);
            this.button1vs1.TabIndex = 1;
            this.button1vs1.Text = "1vs1";
            this.button1vs1.UseVisualStyleBackColor = true;
            this.button1vs1.Click += new System.EventHandler(this.button1vs1_Click);
            // 
            // menuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Mankala.Properties.Resources.Mankala;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1vs1);
            this.Controls.Add(this.buttonAI);
            this.Name = "menuForm";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.menuForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAI;
        private System.Windows.Forms.Button button1vs1;
    }
}

