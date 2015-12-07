/*
 * Created by SharpDevelop.
 * User: HP
 * Date: 2015.10.15.
 * Time: 13:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace sfx3nr
{
    partial class HighscoresForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.Title = new System.Windows.Forms.Label();
            this.exit_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Palatino Linotype", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Title.Location = new System.Drawing.Point(326, 25);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(211, 50);
            this.Title.TabIndex = 2;
            this.Title.Text = "Highscores";
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(420, 516);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(74, 27);
            this.exit_button.TabIndex = 50;
            this.exit_button.Text = "Exit";
            this.exit_button.UseVisualStyleBackColor = true;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // HighscoresForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 711);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.exit_button);
            this.Name = "HighscoresForm";
            this.Text = "Highscores";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button exit_button;
    }
}
