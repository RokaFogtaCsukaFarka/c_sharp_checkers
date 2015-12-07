/*
 * Created by SharpDevelop.
 * User: HP
 * Date: 2015.10.15.
 * Time: 13:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace sfx3nr
{
    /// <summary>
    /// Description of FormHighscores.
    /// </summary>
    public partial class HighscoresForm : Form
    {
        Label[] labelArray;
        DataTable userTable;

        public HighscoresForm(DataTable userTable)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            //var dbEnum = ((MainForm)ParentForm).database.GetEnumerator();
            //dbEnum.MoveNext();
            this.userTable = userTable;
            int userQuantity = this.userTable.Rows.Count;

            labelArray = new Label[userQuantity];
            DataAusschrieben();
            //this.Refresh();
        }

        public void addArrayToForm()
        {
            //Add to panel the members of labelArray[]           
            foreach(Label element in labelArray)
                this.Controls.Add(element);
        }

       public void addLabelsToArray(int i, DataRow data)
        {
            // 
            // labelArray[] filling up
            // 
            labelArray[i] = new Label();
            labelArray[i].AutoSize = true;
            labelArray[i].Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(238)));
            labelArray[i].Location = new Point(this.Size.Width / 2 - 105, 60+20*(i+1));
            labelArray[i].Name = "label" + i;
            labelArray[i].Size = new Size(172, 33);
            labelArray[i].TabIndex = 0;
            labelArray[i].Text = data["Name"] + "   " + data["Highscore"];

        }

        public void DataAusschrieben()
       {            
            int i = 0;
            foreach (DataRow row in this.userTable.Rows)
            {
                addLabelsToArray(i, row);
                i++;
            }
            addArrayToForm();
       }

        /*private void HighscoresForm_Load(object sender, EventArgs e)
        {
            DataAusschrieben();
            this.Refresh();
        }*/

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
