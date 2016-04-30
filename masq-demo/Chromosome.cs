using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev
{
    public class Chromosome
    {
        private bool sign1IsPositive;
        private bool sign2IsPositive;
        private int[] integerPart1;
        private int[] fractionalPart1;
        private int[] integerPart2;
        private int[] fractionalPart2;

        private static char[] splitter = new char[] { '.', ',' };

        public Chromosome(int x1IntCount, int x1FracCount, int x2IntCount, int x2FracCount, float x1, float x2)
        {
            integerPart1 = new int[x1IntCount];
            fractionalPart1 = new int[x1FracCount];
            integerPart2 = new int[x2IntCount];
            fractionalPart2 = new int[x2FracCount];

            string[] x1s = x1.ToString().Split(splitter);
            string[] x2s = x2.ToString().Split(splitter);

            sign1IsPositive = !x1s[0].StartsWith("-");
            int x1Int = x1s.Length > 0 ? Math.Abs(int.Parse(x1s[0])) : 0;
            int x1Frac = x1s.Length > 1 ? int.Parse(x1s[1]) : 0;
            String x1IntS = Convert.ToString(x1Int, 2);
            String x1FracS = Convert.ToString(x1Frac, 2);
            //integerPart1[0] = sign1IsPositive ? 1 : 0;
            for (int i = x1IntCount, j = x1IntS.Length; i > 0; i--, j--)
            {
                integerPart1[i - 1] = j <= 0 ? 0 : int.Parse(x1IntS[j - 1].ToString());
            }
            if (x1FracCount > 0)
            {
                for (int i = x1FracCount, j = x1FracS.Length; i > 0; i--, j--)
                {
                    fractionalPart1[i - 1] = j <= 0 ? 0 : int.Parse(x1FracS[j - 1].ToString());
                }
            }

            sign2IsPositive = !x2s[0].StartsWith("-");
            int x2Int = x2s.Length > 0 ? Math.Abs(int.Parse(x2s[0])) : 0;
            int x2Frac = x2s.Length > 1 ? int.Parse(x2s[1]) : 0;
            String x2IntS = Convert.ToString(x2Int, 2);
            String x2FracS = Convert.ToString(x2Frac, 2);
            //integerPart2[0] = sign2IsPositive ? 1 : 0;
            for (int i = x2IntCount, j = x2IntS.Length; i > 0; i--, j--)
            {
                integerPart2[i - 1] = j <= 0 ? 0 : int.Parse(x2IntS[j - 1].ToString());
            }
            if (x2FracCount > 0)
            {
                for (int i = x2FracCount, j = x2FracS.Length; i > 0; i--, j--)
                {
                    fractionalPart2[i - 1] = j <= 0 ? 0 : int.Parse(x2FracS[j - 1].ToString());
                }
            }
        }

        public Chromosome(int x1IntCount, int x1FracCount, int x2IntCount, int x2FracCount, int[] chromosome)
        {
            integerPart1 = new int[x1IntCount];
            fractionalPart1 = new int[x1FracCount];
            integerPart2 = new int[x2IntCount];
            fractionalPart2 = new int[x2FracCount];

            int i = 0;
            sign1IsPositive = chromosome[i] == 1;
            i++;

            for (int j = 0; j < x1IntCount; j++)
            {
                integerPart1[j] = chromosome[i];
                i++;
            }

            for (int j = 0; j < x1FracCount; j++)
            {
                fractionalPart1[j] = chromosome[i];
                i++;
            }

            sign2IsPositive = chromosome[i] == 1;
            i++;

            for (int j = 0; j < x2IntCount; j++)
            {
                integerPart2[j] = chromosome[i];
                i++;
            }

            for (int j = 0; j < x2FracCount; j++)
            {
                fractionalPart2[j] = chromosome[i];
                i++;
            }
        }

        public float getNormalValueX1()
        {
            StringBuilder sbValue = new StringBuilder();
            sbValue.Append(sign1IsPositive ? "" : "-");
            StringBuilder sbIntX1 = new StringBuilder();
            foreach (var item in integerPart1)
            {
                sbIntX1.Append(item.ToString());
            }
            sbValue.Append(Convert.ToInt32(sbIntX1.ToString(), 2));

            sbValue.Append(",");

            StringBuilder sbFracX1 = new StringBuilder();
            foreach (var item in fractionalPart1)
            {
                sbFracX1.Append(item.ToString());
            }
            sbValue.Append(Convert.ToInt32(sbFracX1.ToString(), 2));

            string valueS = sbValue.ToString();
            return float.Parse(valueS);
        }

        public float getNormalValueX2()
        {
            StringBuilder sbValue = new StringBuilder();
            sbValue.Append(sign2IsPositive ? "" : "-");
            StringBuilder sbIntX2 = new StringBuilder();
            foreach (var item in integerPart2)
            {
                sbIntX2.Append(item.ToString());

            }
            sbValue.Append(Convert.ToInt32(sbIntX2.ToString(), 2));

            sbValue.Append(",");

            StringBuilder sbFracX2 = new StringBuilder();
            foreach (var item in fractionalPart2)
            {
                sbFracX2.Append(item.ToString());

            }
            sbValue.Append(Convert.ToInt32(sbFracX2.ToString(), 2));

            string valueS = sbValue.ToString();
            return float.Parse(valueS);
        }

        public int[] getFullChromosome()
        {
            int i = 0;
            int[] fullChromosome = new int[1 + integerPart1.Length + fractionalPart1.Length + 1 + integerPart2.Length + fractionalPart2.Length];

            fullChromosome[i] = sign1IsPositive ? 1 : 0;
            i++;

            foreach (var item in integerPart1)
            {
                fullChromosome[i] = item;
                i++;
            }

            foreach (var item in fractionalPart1)
            {
                fullChromosome[i] = item;
                i++;
            }

            fullChromosome[i] = sign2IsPositive ? 1 : 0;
            i++;

            foreach (var item in integerPart2)
            {
                fullChromosome[i] = item;
                i++;
            }

            foreach (var item in fractionalPart2)
            {
                fullChromosome[i] = item;
            }

            return fullChromosome;
        }

        public int getIntCount1()
        {
            return integerPart1.Length;
        }

        public int getFracCount1()
        {
            return fractionalPart1.Length;
        }

        public int getIntCount2()
        {
            return integerPart2.Length;
        }

        public int getFracCount2()
        {
            return fractionalPart2.Length;
        }
    }
}
