using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using oprCourseSoloviev.PopulationCreationMethods;

namespace oprCourseSoloviev
{
    public partial class FormUserDefinePoints : Form
    {
        private UserDefined mUserDefined;
        private int N;
        private ParamBoundaries pb;

        public FormUserDefinePoints(UserDefined userDefined, int N, ParamBoundaries pb)
        {
            mUserDefined = userDefined;
            this.N = N;
            this.pb = pb;

            InitializeComponent();

            label4.Text = N.ToString();
            dataGridView1.DataSource = userDefined.getPopulation(N, pb);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0 && textBox2.TextLength > 0)
            {
                bool result = false;
                float x1, x2;
                result = float.TryParse(textBox1.Text, out x1);
                result &= float.TryParse(textBox2.Text, out x2);
                if (result)
                {
                    mUserDefined.addPoint(x1, x2);

                    dataGridView1.DataSource = mUserDefined.getPopulation(N, pb);
                    dataGridView1.Invalidate();

                    N--;
                    label4.Text = N.ToString();
                    if (N == 0)
                    {
                        Close();
                    }
                }
            }
        }
    }
}
