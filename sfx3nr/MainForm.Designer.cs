/*
 * Created by SharpDevelop.
 * User: HP
 * Date: 2015.10.15.
 * Time: 12:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace sfx3nr
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label Title;
		private System.Windows.Forms.Button exit_button;
		private System.Windows.Forms.Button play_game_button;
		private System.Windows.Forms.Button highscores_button;
		private System.Windows.Forms.Button username_ok_button;
		private System.Windows.Forms.TextBox username_textBox;
		private System.Windows.Forms.Label username_label;
        internal object dataTable;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Title = new System.Windows.Forms.Label();
            this.exit_button = new System.Windows.Forms.Button();
            this.play_game_button = new System.Windows.Forms.Button();
            this.highscores_button = new System.Windows.Forms.Button();
            this.username_ok_button = new System.Windows.Forms.Button();
            this.username_textBox = new System.Windows.Forms.TextBox();
            this.username_label = new System.Windows.Forms.Label();
            this.main_decoration_pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.main_decoration_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Title.ForeColor = System.Drawing.Color.Red;
            this.Title.Location = new System.Drawing.Point(12, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(192, 33);
            this.Title.TabIndex = 0;
            this.Title.Text = "Checkers Game";
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(176, 215);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(75, 23);
            this.exit_button.TabIndex = 1;
            this.exit_button.Text = "Exit";
            this.exit_button.UseVisualStyleBackColor = true;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // play_game_button
            // 
            this.play_game_button.Location = new System.Drawing.Point(22, 70);
            this.play_game_button.Name = "play_game_button";
            this.play_game_button.Size = new System.Drawing.Size(123, 23);
            this.play_game_button.TabIndex = 2;
            this.play_game_button.Text = "Play a game";
            this.play_game_button.UseVisualStyleBackColor = true;
            this.play_game_button.Click += new System.EventHandler(this.play_game_button_Click);
            // 
            // highscores_button
            // 
            this.highscores_button.Location = new System.Drawing.Point(22, 116);
            this.highscores_button.Name = "highscores_button";
            this.highscores_button.Size = new System.Drawing.Size(123, 23);
            this.highscores_button.TabIndex = 3;
            this.highscores_button.Text = "Show Highscores";
            this.highscores_button.UseVisualStyleBackColor = true;
            this.highscores_button.Click += new System.EventHandler(this.highscores_button_Click);
            // 
            // username_ok_button
            // 
            this.username_ok_button.Location = new System.Drawing.Point(176, 160);
            this.username_ok_button.Name = "username_ok_button";
            this.username_ok_button.Size = new System.Drawing.Size(75, 23);
            this.username_ok_button.TabIndex = 6;
            this.username_ok_button.Text = "OK";
            this.username_ok_button.UseVisualStyleBackColor = true;
            this.username_ok_button.Click += new System.EventHandler(this.username_ok_button_Click);
            // 
            // username_textBox
            // 
            this.username_textBox.Location = new System.Drawing.Point(22, 160);
            this.username_textBox.Name = "username_textBox";
            this.username_textBox.Size = new System.Drawing.Size(123, 20);
            this.username_textBox.TabIndex = 7;
            this.username_textBox.Text = "Username";
            // 
            // username_label
            // 
            this.username_label.Location = new System.Drawing.Point(173, 70);
            this.username_label.Name = "username_label";
            this.username_label.Size = new System.Drawing.Size(88, 52);
            this.username_label.TabIndex = 8;
            // 
            // main_decoration_pictureBox
            // 
            this.main_decoration_pictureBox.Image = global::sfx3nr.Properties.Resources.ckeckers_main;
            this.main_decoration_pictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("main_decoration_pictureBox.InitialImage")));
            this.main_decoration_pictureBox.Location = new System.Drawing.Point(22, 210);
            this.main_decoration_pictureBox.Name = "main_decoration_pictureBox";
            this.main_decoration_pictureBox.Size = new System.Drawing.Size(28, 28);
            this.main_decoration_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.main_decoration_pictureBox.TabIndex = 4;
            this.main_decoration_pictureBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.username_label);
            this.Controls.Add(this.username_textBox);
            this.Controls.Add(this.username_ok_button);
            this.Controls.Add(this.main_decoration_pictureBox);
            this.Controls.Add(this.highscores_button);
            this.Controls.Add(this.play_game_button);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.Title);
            this.Name = "MainForm";
            this.Text = "sfx3nr";
            ((System.ComponentModel.ISupportInitialize)(this.main_decoration_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private System.Windows.Forms.PictureBox main_decoration_pictureBox;
    }
}
