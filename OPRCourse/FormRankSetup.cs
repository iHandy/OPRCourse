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
    public partial class FormRankSetup : Form
    {
        private List<int> mRanks = new List<int>();
        private PopulationChooseMethods.RankMethod rankMethod;

        public FormRankSetup(PopulationChooseMethods.RankMethod rankMethod)
        {
            this.rankMethod = rankMethod;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int value;
            if (textBox1.TextLength > 0 && int.TryParse(textBox1.Text, out value))
            {
                mRanks.Add(value);
                invalidateInfo();
            }
            else { MessageBox.Show("Error! Check input values!"); }
        }

        private void invalidateInfo()
        {
            int totalPercent = 0;
            foreach (var item in mRanks)
            {
                totalPercent += item;
            }

            label3.Text = mRanks.Count.ToString();
            label5.Text = totalPercent.ToString() + "%";

            if (totalPercent > 100)
            {
                MessageBox.Show("Over 100%! Ranks clear. Setup again, please.");
                mRanks.Clear();
                invalidateInfo();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mRanks.Count > 0)
            {
                rankMethod.RankList = mRanks;
                Close();
            }
            else
            {
                MessageBox.Show("No setup ranks! Please, add ranks!");
            }
        }
    }
}
