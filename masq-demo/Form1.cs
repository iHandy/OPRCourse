using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using NCalc;

namespace oprCourseSoloviev
{
    public partial class Form1 : Form
    {
        func[] f = new func[2];// = new func();
        int Z = 5;
        int N = 19;//19; //must be odd

        double[][,] arr = new double[2][,];// = new double[N,N];
        MarchingSquare masq;
        int z0;


        double a = 0, b = 0, c = 0; //alpha, beta, gamma
        //double k = 1; //param
        bool onmove = false;
        Point startpos;
        Color[] cl = { Color.LightBlue, Color.LightGreen, Color.SandyBrown };
        delegate double func(double x, double y);

        Expression expression1 = null, expression2 = null;

        FormTotalResult formResult = new FormTotalResult();

        public Form1()
        {
            InitializeComponent();
            //textBox1.Text = k.ToString();
            textBox2.Text = Z.ToString();

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Interval = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

            f[0] = f1;
            f[1] = f2;
            /*f[2] = f3;*/

            update();
            init();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 2;

            functionControl1.FunctionNumber = FUNCTION_NUMBER.FIRST;
            functionControl2.FunctionNumber = FUNCTION_NUMBER.SECOND;
            functionControl1.groupBox1.Text = "Function 1 (C# style):";
            functionControl2.groupBox1.Text = "Function 2 (C# style):";

            initForm2();
        }

        void update()
        {
            chart1.ChartAreas[0].AxisX.Minimum = -N / 2;
            chart1.ChartAreas[0].AxisY.Minimum = -N / 2;
            chart1.ChartAreas[0].AxisX.Maximum = N / 2;
            chart1.ChartAreas[0].AxisY.Maximum = N / 2;
        }
        void init()
        {
            for (int i = 0; i < 2; i++)
            {
                arr[i] = new double[N, N];
                for (int x = 0; x < N; x++)
                    for (int y = 0; y < N; y++)
                        arr[i][x, y] = f[i](x - N / 2, y - N / 2);
            }

            masq = new MarchingSquare(N);
            z0 = 2 * N * 3 + 3;

            for (int i = 0; i < 2 * N * 3; i++)
            {
                chart1.Series.Add("s" + i.ToString());
                chart1.Series[i].ChartType = SeriesChartType.Line;
            }
            for (int i = 0; i < 2 * N; i++) chart1.Series[i].Color = cl[0];
            for (int i = 2 * N; i < 2 * N * 2; i++) chart1.Series[i].Color = cl[1];
            for (int i = 2 * N * 2; i < 2 * N * 3; i++) chart1.Series[i].Color = cl[2];

            for (int i = 2 * N * 3; i < 2 * N * 3 + 3; i++)
            {
                chart1.Series.Add("a" + i.ToString());
                chart1.Series[i].ChartType = SeriesChartType.Line;
                chart1.Series[i].Color = Color.Black;
            }

            for (int i = z0; i < z0 + Z * 3; i++)
            {
                chart1.Series.Add("z" + i.ToString());
                chart1.Series[i].Color = cl[0];

                /*if (i % 2 == 0)
                {
                    chart1.Series.Add("z" + i.ToString());
                    chart1.Series[chart1.Series.Count - 1].Color = cl[0];
                }
                else
                {
                    //chart1.Series[chart1.Series.Count - 1].Color = Color.Transparent;
                }*/
            }
            switch (comboBox1.SelectedIndex)
            {
                case 0: for (int i = z0; i < z0 + Z * 2; i++) chart1.Series[i].MarkerStyle = MarkerStyle.None; break;
                //case 1: for (int i = z0; i < z0 + Z * 3; i++) chart1.Series[i].MarkerStyle = MarkerStyle.Circle; break;
                //case 2: for (int i = z0; i < z0 + Z * 3; i++) chart1.Series[i].MarkerStyle = MarkerStyle.Square; break;
            }
            switch (comboBox2.SelectedIndex)
            {
                //case 0:
                //case 1: for (int i = z0; i < z0 + Z * 3; i++) chart1.Series[i].ChartType = SeriesChartType.Line; break;
                case 2: for (int i = z0; i < z0 + Z * 2; i++) chart1.Series[i].ChartType = SeriesChartType.Spline; break;
            }


            for (int n = 0; n < 2; n++)
                for (int i = 0; i < Z; i++)
                    chart1.Series[z0 + n * Z + i/*z0 + (n * Z + i) / 2*/].Color = Color.FromArgb(255, 255 - i * 255 / Z, 0, i * 255 / Z);


            drawscene();

        }

        double f1(double x, double y)
        {
            if (functionControl1.textBoxFunction.TextLength == 0 || expression1 == null)
            {
                return 0;
            }


            evaluateParams(expression1, x, y);
            return (double)expression1.Evaluate();
        }

        static void evaluateParams(Expression expr, double x, double y)
        {
            expr.EvaluateParameter += delegate(string name, ParameterArgs args1)
            {
                if (name == "x1")
                {
                    args1.Result = x;
                }
                if (name == "x2")
                {
                    args1.Result = y;
                }
            };
        }

        double f2(double x, double y)
        {
            if (functionControl2.textBoxFunction.TextLength == 0 || expression2 == null)
            {
                return 0;
            }


            evaluateParams(expression2, x, y);
            return (double)expression2.Evaluate();
        }
        /*double f3(double x, double y)
        {
            return (100 * Math.Pow((y - Math.Pow(x, 2)), 2)) + (Math.Pow((1 - x), 2));//Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2);//(100 * Math.Pow((y - Math.Pow(x, 2)), 2)) + (Math.Pow((1 - x), 2));// k*(x * x + y * y);
        }*/
        void drawscene()
        {
            clear();
            drawxyz();

            draw(0);
            draw(1);

            /*
            if (checkBox1.Checked) draw(0);
            if (checkBox2.Checked) draw(1);
            if (checkBox3.Checked) draw(2);*/
        }
        void draw(int tn)
        {
            bool ip = false;
            if (comboBox2.SelectedIndex > 0) ip = true;

            double[,] a = arr[tn];
            int n = tn * 2 * N;

            double X, Y;
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    X = l1() * (x - N / 2) + l2() * (y - N / 2) + l3() * a[x, y];
                    Y = m1() * (x - N / 2) + m2() * (y - N / 2) + m3() * a[x, y];
                    chart1.Series[n].Points.AddXY(X, Y);

                    if (checkBox4.Checked)
                        chart1.Series[n].Points[chart1.Series[n].Points.Count - 1].Label =
                            (x - N / 2).ToString() + ";" + (y - N / 2).ToString() + ";" + a[x, y].ToString("0.00");
                }
                n++;
            }

            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    X = l1() * (x - N / 2) + l2() * (y - N / 2) + l3() * a[x, y];
                    Y = m1() * (x - N / 2) + m2() * (y - N / 2) + m3() * a[x, y];
                    chart1.Series[n].Points.AddXY(X, Y);
                }
                n++;
            }

            //isolines
            for (int i = 1; i <= Z; i++)
            {
                //int tn = 0;
                PointF[] pa = masq.get(a, i, ip);
                for (int j = 0; j < pa.Length; j++)
                {
                    chart1.Series[z0 + tn * Z + i - 1].Points.AddXY(pa[j].X * l1() + pa[j].Y * l2() + i * l3(), pa[j].X * m1() + pa[j].Y * m2() + i * m3());


                    if (checkBox4.Checked)
                        chart1.Series[z0 + tn * Z + i - 1].Points[chart1.Series[z0 + tn * Z + i - 1].Points.Count - 1].Label =
                            chart1.Series[z0 + tn * Z + i - 1].Points[chart1.Series[z0 + tn * Z + i - 1].Points.Count - 1].XValue.ToString("0.0") +
                            ";" + chart1.Series[z0 + tn * Z + i - 1].Points[chart1.Series[z0 + tn * Z + i - 1].Points.Count - 1].YValues[0].ToString("0.0");

                }
            }

        }
        #region sin,cos,l1,l2,l3,m1,m2,m3,n1,n2,n3,clear,drawxyz
        double sin(double x)
        {
            return Math.Sin(x * Math.PI / 180);
        }
        double cos(double x)
        {
            return Math.Cos(x * Math.PI / 180);
        }
        double l1()
        {
            return cos(a) * cos(c) - cos(b) * sin(a) * sin(c);
        }
        double m1()
        {
            return sin(a) * cos(c) + cos(b) * cos(a) * sin(c);
        }
        double n1()
        {
            return sin(b) * sin(c);
        }
        double l2()
        {
            return -cos(a) * sin(c) + cos(b) * sin(a) * cos(c);
        }
        double m2()
        {
            return -sin(a) * sin(c) + cos(b) * cos(a) * cos(c);
        }
        double n2()
        {
            return sin(b) * cos(c);
        }
        double l3()
        {
            return sin(b) * sin(a);
        }
        double m3()
        {
            return -sin(b) * cos(a);
        }
        double n3()
        {
            return cos(b);
        }
        void drawxyz()
        {
            double L = N / 2; //axis length

            //z
            chart1.Series[2 * N * 3].Points.AddXY(0, 0);
            chart1.Series[2 * N * 3].Points.AddXY(l3() * L, m3() * L);
            chart1.Series[2 * N * 3].Points[1].Label = "Z";
            //chart1.Series[2 * N * 3].Points[0].Label = "0";
            //x
            chart1.Series[2 * N * 3 + 1].Points.AddXY(0, 0);
            chart1.Series[2 * N * 3 + 1].Points.AddXY(l1() * L, m1() * L);
            chart1.Series[2 * N * 3 + 1].Points[1].Label = "X";
            //y
            chart1.Series[2 * N * 3 + 2].Points.AddXY(0, 0);
            chart1.Series[2 * N * 3 + 2].Points.AddXY(l2() * L, m2() * L);
            chart1.Series[2 * N * 3 + 2].Points[1].Label = "Y";
        }
        void clear()
        {
            for (int i = 0; i < chart1.Series.Count; i++) chart1.Series[i].Points.Clear();
        }
        #endregion
        #region buttons,mouse move,controls
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (checkBox5.Checked)
                {
                    onmove = true;
                    startpos = e.Location;
                }
            }
        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (onmove)
            {
                if ((startpos.Y - e.Y) < 0) b--;
                if ((startpos.Y - e.Y) > 0) b++;
                if ((startpos.X - e.X) < 0) c--;
                if ((startpos.X - e.X) > 0) c++;

                if (b > 359) b = 0;
                if (c > 359) c = 0;
                if (b < 0) b = 359;
                if (c < 0) c = 359;

                drawscene();

                this.Text = "a=" + a.ToString("0.00") + " b=" + b.ToString("0.00") + " c=" + c.ToString("0.00") + " N=" + N.ToString();
            }
        }
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) onmove = false;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int tZ = Z;
                if (!int.TryParse(textBox2.Text, out tZ))
                {
                    textBox2.Text = Z.ToString();
                    return;
                }
                if (tZ < 1 || tZ > 19)
                {
                    textBox2.Text = Z.ToString();
                    return;
                }
                Z = tZ;
                chart1.Series.Clear();
                init();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (cd.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < 2 * N; i++) chart1.Series[i].Color = cd.Color;
                cl[0] = cd.Color;
            }
        }

        //-
        private void button4_Click(object sender, EventArgs e)
        {
            if (N < 51)
            {
                N += 2;

                Z = N;// / 3;
                textBox2.Text = Z.ToString();

                chart1.Series.Clear();
                update();
                init();
            }
        }
        //+
        private void button5_Click(object sender, EventArgs e)
        {
            if (N > 7)
            {
                N -= 2;

                Z = N * 3;// / 3;
                textBox2.Text = Z.ToString();

                chart1.Series.Clear();
                update();
                init();
            }
        }
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            drawscene();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: for (int i = z0; i < z0 + (Z * 3) / 2; i++) chart1.Series[i].MarkerStyle = MarkerStyle.None; break;
                case 1: for (int i = z0; i < z0 + Z * 3; i++) chart1.Series[i].MarkerStyle = MarkerStyle.Circle; break;
                case 2: for (int i = z0; i < z0 + Z * 3; i++) chart1.Series[i].MarkerStyle = MarkerStyle.Square; break;
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                case 1: for (int i = z0; i < z0 + Z * 3; i++) chart1.Series[i].ChartType = SeriesChartType.Line; break;
                case 2: for (int i = z0; i < z0 + (Z * 3) / 2; i++) chart1.Series[i].ChartType = SeriesChartType.Spline; break;
            }
            drawscene();
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            VEGA vega = new VEGA(functionControl1, functionControl2, parametersControl1);
            vega.startSolution();

            if (functionControl1.textBoxFunction.TextLength > 0)
            {
                expression1 = new Expression(functionControl1.textBoxFunction.Text);
            }
            else
            {
                MessageBox.Show("Enter function!");
            }
            if (functionControl2.textBoxFunction.TextLength > 0)
            {
                expression2 = new Expression(functionControl2.textBoxFunction.Text);
            }
            else
            {
                MessageBox.Show("Enter function!");
                return;
            }
            chart1.Series.Clear();
            update();
            init();

            fillDataGridView(vega.getData(), vega);
        }

        private void fillDataGridView(List<Person> listOfPersons, VEGA vega)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("X1", "X1");
            dataGridView1.Columns.Add("X2", "X2");
            dataGridView1.Columns.Add("X1X2", "X1X2");
            dataGridView1.Columns.Add("F1", "F1");
            dataGridView1.Columns.Add("F2", "F2");
            dataGridView1.Columns.Add("F0", "<F>");

            chart1.Series.Add("points");
            int numberS = chart1.Series.Count - 1;
            Series sc = chart1.Series[numberS];
            sc.ChartType = SeriesChartType.Point;
            sc.Color = Color.Green;

            int bestI = -1;
            double bestValue = double.MinValue;

            List<int> pointsId = new List<int>();

            int pointsCount = 0;
            int i = 0;
            foreach (var item in listOfPersons)
            {
                float x1 = item.Chromosome.getNormalValueX1();
                float x2 = item.Chromosome.getNormalValueX2();
                string id = item.Generation + "." + item.ID;
                StringBuilder chromo = new StringBuilder();
                foreach (var itemChromo in item.Chromosome.getFullChromosome())
                {
                    chromo.Append(itemChromo.ToString());
                }

                if (!item.isRemoved && item.FuncionCommonValue > bestValue)
                {
                    bestValue = item.FuncionCommonValue;
                    bestI = i;
                }
                i++;

                dataGridView1.Rows.Add(new string[] {
                    id, 
                    x1.ToString(),
                    x2.ToString(),
                    chromo.ToString(),
                    item.Funcion1Value.ToString(),
                    item.Funcion2Value.ToString(),
                    item.FuncionCommonValue.ToString()});

                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[item.FunctionNumber.Equals(FUNCTION_NUMBER.FIRST) ? 5 : 4].Style.ForeColor = Color.Gray;

                if (item.isRemoved/*x1 > parametersControl1.ParamBoundaries.X1Right || x1 < parametersControl1.ParamBoundaries.X1Left
                    || x2 > parametersControl1.ParamBoundaries.X2Right || x2 < parametersControl1.ParamBoundaries.X2Left*/)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                }

                /*switch (item.Type)
                {
                    case PersonType.SELECTED:
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Olive;
                        break;
                    case PersonType.CROSSING:
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Orange;
                        break;
                    case PersonType.MUTATION:
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Violet;
                        break;
                }*/
                if (vega.selectedId.Contains(id))
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Olive;
                }
                else if (vega.crossedId.Contains(id))
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Orange;
                }
                else if (vega.mutatedId.Contains(id))
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Violet;
                }

                if (!pointsId.Contains(item.ID))
                {
                    sc.Points.AddXY(x1, x2);
                    sc.Points[pointsCount++].Label = id;
                    if (item.isRemoved)
                    {
                        sc.Points[sc.Points.Count - 1].LabelBorderColor = Color.Red;
                    }
                    pointsId.Add(item.ID);
                }
            }

            if (bestI > -1)
            {
                dataGridView1.Rows[bestI].DefaultCellStyle.BackColor = Color.Green;
            }

            formResult.dataGridView1.Rows.Add(new string[] {
                    parametersControl1.N.ToString(), 
                    parametersControl1.PopulationCreation.ToString(),
                    parametersControl1.PopulationChooser.ToString(),
                    parametersControl1.CrossingType.ToString(),
                    parametersControl1.CrossingPoint.ToString(),
                    parametersControl1.MutationType.ToString(),
                    parametersControl1.Mu.ToString(),
                    vega.getGenerations().ToString()});

        }

        private void initForm2()
        {
            formResult.dataGridView1.Rows.Clear();
            formResult.dataGridView1.Columns.Clear();

            formResult.dataGridView1.Columns.Add("N", "N");
            formResult.dataGridView1.Columns.Add("CreatePop", "Create pop.");
            formResult.dataGridView1.Columns.Add("ChoosePop", "Choose pop.");
            formResult.dataGridView1.Columns.Add("CrossType", "Crossing");
            formResult.dataGridView1.Columns.Add("CrossPoint", "Cr. point");
            formResult.dataGridView1.Columns.Add("Mutation", "Mutation");
            formResult.dataGridView1.Columns.Add("Mu", "Mu");
            formResult.dataGridView1.Columns.Add("F", "F");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            formResult.Show();
        }
    }
}
