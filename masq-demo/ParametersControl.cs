using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using oprCourseSoloviev.PopulationCreationMethods;
using oprCourseSoloviev.PopulationChooseMethods;

namespace oprCourseSoloviev
{
    public partial class ParametersControl : UserControl
    {
        List<IPopulationCreation> populationCreationMethods = new List<IPopulationCreation>();
        List<IPopulationChooser> populationChooseMethods = new List<IPopulationChooser>();
        List<ICrossingType> crossingTypes = new List<ICrossingType>();
        List<MutationTypes> mutationTypes = new List<MutationTypes>();

        public ParametersControl()
        {
            InitializeComponent();

            //Init population creation methods
            populationCreationMethods.Add(new PopulationCreationMethods.RandomMethod());
            populationCreationMethods.Add(new PopulationCreationMethods.UserDefined());
            comboBoxPopulationCreation.DataSource = populationCreationMethods;

            populationChooseMethods.Add(new PopulationChooseMethods.RandomMethod());
            comboBoxPopulationChooser.DataSource = populationChooseMethods;

            crossingTypes.Add(new CrossingTypes.OnePointMethod());
            comboBoxCrossingTypeChooser.DataSource = crossingTypes;

            mutationTypes.Add(MutationTypes.PARENT);
            mutationTypes.Add(MutationTypes.CHILD);
            comboBoxMutationType.DataSource = mutationTypes;
        }

        private static char[] splitter = new char[] { '.', ',' };

        public int x1IntLength { get; private set; }
        public int x1FracLength { get; private set; }
        public int x2IntLength { get; private set; }
        public int x2FracLength { get; private set; }

        public IPopulationCreation PopulationCreation { get { return populationCreationMethods[comboBoxPopulationCreation.SelectedIndex]; } }
        public ParamBoundaries ParamBoundaries { get; private set; }
        public int N { get { return textBoxN.TextLength > 0 ? int.Parse(textBoxN.Text) : 0; } }

        public int EOCC { get { return textBoxEOCC.TextLength > 0 ? int.Parse(textBoxEOCC.Text) : 0; } }

        public IPopulationChooser PopulationChooser { get { return populationChooseMethods[comboBoxPopulationChooser.SelectedIndex]; } }

        public ICrossingType CrossingType { get { return crossingTypes[comboBoxCrossingTypeChooser.SelectedIndex]; } }
        public int CrossingPoint { get { return textBoxCrossingPoint.TextLength > 0 ? int.Parse(textBoxCrossingPoint.Text) : 0; } }

        public MutationTypes MutationType { get { return mutationTypes[comboBoxMutationType.SelectedIndex]; } }
        public float Mu { get { return textBoxMu.TextLength > 0 ? float.Parse(textBoxMu.Text) : 0; } }

        private void textBoxStepAccuracy_Leave(object sender, EventArgs e)
        {
            if (textBoxX1Left.TextLength > 0 && textBoxX1Right.TextLength > 0
                && textBoxX2Left.TextLength > 0 && textBoxX2Right.TextLength > 0
                && textBoxStepAccuracy.TextLength > 0)
            {
                string x1l, x1r, x2l, x2r, step;
                x1l = textBoxX1Left.Text;
                x1r = textBoxX1Right.Text;
                x2l = textBoxX2Left.Text;
                x2r = textBoxX2Right.Text;
                step = textBoxStepAccuracy.Text;

                double dX1L, dX1R, dX2L, dX2R, dStep;
                bool result = true;
                result &= double.TryParse(x1l, out dX1L);
                result &= double.TryParse(x1r, out dX1R);
                result &= double.TryParse(x2l, out dX2L);
                result &= double.TryParse(x2r, out dX2R);
                result &= double.TryParse(step, out dStep);
                if (!result)
                {
                    textBoxsParamsValid(false);
                    return;
                }
                textBoxsParamsValid(true);
                ParamBoundaries pb = new ParamBoundaries(dX1L, dX1R, dX2L, dX2R, dStep);
                this.ParamBoundaries = pb;

                string[] x1lS = x1l.Split(splitter);
                string[] x1rS = x1r.Split(splitter);
                int x1lIntCount = x1lS.Length > 0 ? Convert.ToString(Math.Abs(int.Parse(x1lS[0])), 2).Length : 0;
                int x1lFracCount = x1lS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(x1lS[1])), 2).Length : 0;
                int x1rIntCount = x1rS.Length > 0 ? Convert.ToString(Math.Abs(int.Parse(x1rS[0])), 2).Length : 0;
                int x1rFracCount = x1rS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(x1rS[1])), 2).Length : 0;

                double x1lMinusStep = dX1L - dStep;
                double x1rMinusStep = dX1R - dStep;
                string[] x1lMinusStepS = (x1lMinusStep.ToString()).Split(splitter);
                string[] x1rMinusStepS = (x1rMinusStep.ToString()).Split(splitter);
                int x1lMinusStepFracCount = x1lMinusStepS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(x1lMinusStepS[1])), 2).Length : 0;
                int x1rMinusStepFracCount = x1rMinusStepS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(x1rMinusStepS[1])), 2).Length : 0;

                int x1IntLength = Math.Max(x1lIntCount, x1rIntCount);
                int x1FracLength = Math.Max(x1lFracCount, x1rFracCount);

                bool is1Signed = true;// x1lS[0].StartsWith("-") || x1rS[0].StartsWith("-");

                string[] x2lS = x2l.Split(splitter);
                string[] x2rS = x2r.Split(splitter);
                int x2lIntCount = x2lS.Length > 0 ? Convert.ToString(Math.Abs(int.Parse(x2lS[0])), 2).Length : 0;
                int x2lFracCount = x2lS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(x2lS[1])), 2).Length : 0;
                int x2rIntCount = x2rS.Length > 0 ? Convert.ToString(Math.Abs(int.Parse(x2rS[0])), 2).Length : 0;
                int x2rFracCount = x2rS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(x2rS[1])), 2).Length : 0;

                double x2lMinusStep = dX2L - dStep;
                double x2rMinusStep = dX2R - dStep;
                string[] x2lMinusStepS = (x2lMinusStep.ToString()).Split(splitter);
                string[] x2rMinusStepS = (x2rMinusStep.ToString()).Split(splitter);
                int x2lMinusStepFracCount = x2lMinusStepS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(x2lMinusStepS[1])), 2).Length : 0;
                int x2rMinusStepFracCount = x2rMinusStepS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(x2rMinusStepS[1])), 2).Length : 0;

                int x2IntLength = Math.Max(x2lIntCount, x2rIntCount);
                int x2FracLength = Math.Max(x2lFracCount, x2rFracCount);

                bool is2Signed = true;// x2lS[0].StartsWith("-") || x2rS[0].StartsWith("-");

                string[] stepS = step.Split(splitter);
                int stepInt = stepS.Length > 0 ? Convert.ToString(Math.Abs(int.Parse(stepS[0])), 2).Length : 0;
                int stepFrac = stepS.Length > 1 ? Convert.ToString(Math.Abs(int.Parse(stepS[1])), 2).Length : 0;

                x1IntLength = Math.Max(x1IntLength, stepInt);
                x1FracLength = Math.Max(x1FracLength, stepFrac);
                x1FracLength = Math.Max(x1FracLength, x1lMinusStepFracCount);
                x1FracLength = Math.Max(x1FracLength, x1rMinusStepFracCount);

                x2IntLength = Math.Max(x2IntLength, stepInt);
                x2FracLength = Math.Max(x2FracLength, stepFrac);
                x2FracLength = Math.Max(x2FracLength, x2lMinusStepFracCount);
                x2FracLength = Math.Max(x2FracLength, x2rMinusStepFracCount);

                int maxLength1 = x1IntLength + x1FracLength + (is1Signed ? 1 : 0);
                int maxLength2 = x2IntLength + x2FracLength + (is2Signed ? 1 : 0);

                labelChrL.Text = maxLength1 + "+" + maxLength2;

                this.x1IntLength = x1IntLength;
                this.x1FracLength = x1FracLength;
                this.x2IntLength = x2IntLength;
                this.x2FracLength = x2FracLength;
            }
        }

        private void textBoxsParamsValid(bool v)
        {
            textBoxX1Left.BackColor = v ? Color.LightGreen : Color.IndianRed;
            textBoxX1Right.BackColor = v ? Color.LightGreen : Color.IndianRed;
            textBoxX2Left.BackColor = v ? Color.LightGreen : Color.IndianRed;
            textBoxX2Right.BackColor = v ? Color.LightGreen : Color.IndianRed;
            textBoxStepAccuracy.BackColor = v ? Color.LightGreen : Color.IndianRed;
        }

        private void comboBoxPopulationCreation_Leave(object sender, EventArgs e)
        {
            if (PopulationCreation is UserDefined)
            {
                FormUserDefinePoints form2 = new FormUserDefinePoints((UserDefined)PopulationCreation, N, ParamBoundaries);
                form2.ShowDialog();
            }
        }
    }

    public enum MutationTypes { PARENT = 0, CHILD = 1 }
}
