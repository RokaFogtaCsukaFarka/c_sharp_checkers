using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace sfx3nr
{
	/// <summary>
	/// On MainForm first you have to give in your user name, then the GameForm appears.
	/// You can load the Game itself, the Highscores, and the Exit button. 
	/// </summary>
	public partial class MainForm : Form
	{
        public Dictionary<string, int> database;

        private bool loginState;

        public Form childForm;

        public CheckersDataSet.UsersDataTable userData;

        public CheckersDataSetTableAdapters.UsersTableAdapter dataAdapter;

        private System.Data.DataRow[] user;

        public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

            //
            // TODO: Add a database read aand program initialization.
            //
            dataAdapter = new CheckersDataSetTableAdapters.UsersTableAdapter();
            userData = dataAdapter.GetData();
            user = null;
            loginState = false;
		}
		
		void play_game_button_Click(object sender, EventArgs e)
		{
            if (loginState)
            {
                childForm = new GameForm(userData, dataAdapter, user[0]);
                childForm.ShowDialog();
            }
            else
                this.username_label.Text = "Please log in first!";
            this.Refresh();
		}
		
		void highscores_button_Click(object sender, EventArgs e)
		{
            childForm = new HighscoresForm(userData);
			childForm.ShowDialog();
		}
		
		void exit_button_Click(object sender, EventArgs e)
		{
            this.Close();
            //Application.Shutdown();
		}

        private void username_ok_button_Click(object sender, EventArgs e)
        {
            try
            {
                user = userData.Select("Name = " + this.username_textBox.Text);
            }
            catch (EvaluateException exc)
            {
                userData.AddUsersRow(this.username_textBox.Text, 0);
                user = userData.Select("Name = '" + this.username_textBox.Text +"'");
            }

            username_label.Text = this.username_textBox.Text;
            loginState = true;
        }
    }
}
