using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace oprCourseSoloviev
{
    public partial class FormTournamentSettings : Form
    {
        private PopulationChooseMethods.TournamentMethod tournamentMethod;

        public FormTournamentSettings(PopulationChooseMethods.TournamentMethod tournamentMethod)
        {
            this.tournamentMethod = tournamentMethod;

            InitializeComponent();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool result = true;

            if (textBox1.TextLength > 0 && textBox2.TextLength > 0)
            {
                int val = 0;
                result &= int.TryParse(textBox1.Text, out val);
                if (result)
                {
                    tournamentMethod.IndividN = val;
                }

                val = 0;
                result &= int.TryParse(textBox2.Text, out val);
                if (result)
                {
                    tournamentMethod.IndividForSelectN = val;
                }

                tournamentMethod.SubWay = comboBox1.SelectedIndex;
                tournamentMethod.SelectWay = comboBox2.SelectedIndex;
            }
            else
            {
                result = false;
            }

            if (result)
            {
                Close();
            }
            else
            {
                MessageBox.Show("Error! Check input values!");
            }
        }
    }
}
