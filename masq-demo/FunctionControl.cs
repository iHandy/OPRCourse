using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NCalc;

namespace oprCourseSoloviev
{
    public enum FUNCTION_NUMBER { FIRST=1, SECOND=2 }

    public partial class FunctionControl : UserControl
    {
        public Expression Function { get; private set; }
        public FUNCTION_NUMBER FunctionNumber { get; set; }

        public FunctionControl()
        {
            InitializeComponent();
        }

        private void textBoxFunction_Leave(object sender, EventArgs e)
        {
            if (textBoxFunction.TextLength > 0)
            {
                Expression expr = null;
                try
                {
                    expr = new Expression(textBoxFunction.Text);
                    evaluateParams(expr, 1, 1);
                    double test = (double)expr.Evaluate();
                }
                catch (InvalidCastException error)
                {
                    textBoxFunctionValid(false);
                    return;
                }
                catch (NCalc.EvaluationException error)
                {
                    textBoxFunctionValid(false);
                    return;
                }
                catch (ArgumentException errror)
                {
                    textBoxFunctionValid(false);
                    return;
                }

                if (expr.HasErrors())
                {
                    textBoxFunctionValid(false);
                    return;
                }

                textBoxFunctionValid(true);
                this.Function = expr;
            }
        }

        private void textBoxFunctionValid(bool v)
        {
            textBoxFunction.BackColor = v ? Color.LightGreen : Color.IndianRed;
        }

       

        static void evaluateParams(Expression expr, double x1, double x2)
        {
            expr.EvaluateParameter += delegate(string name, ParameterArgs args1)
            {
                if (name == "x1")
                {
                    args1.Result = x1;
                }
                if (name == "x2")
                {
                    args1.Result = x2;
                }
            };
        }

        private void textBoxFunction_Enter(object sender, EventArgs e)
        {
            textBoxFunction.BackColor = Color.White;
        }
    }
}
