using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo.PopulationCreationMethods
{
    class RandomMethod : IPopulationCreation
    {
        public string getName()
        {
            return "Random";
        }

        public override string ToString()
        {
            return getName();
        }

        private static Random random = new Random();

        public List<PersonAsPoint> getPopulation(int N, ParamBoundaries pb)
        {
            string[] stepS = pb.Step.ToString().Split(new char[] { ',', '.' });
            int fracCount = stepS.Length > 1 ? stepS[1].Length : 0;
            List<PersonAsPoint> resultList = new List<PersonAsPoint>(N);
            for (int i = 0; i < N; i++)
            {
                float randomStep = 0;
                float x1 = 0;
                do
                {
                    randomStep = (float)random.NextDouble();
                    x1 = (float)Math.Round((randomStep * (pb.X1Right - pb.X1Left) + pb.X1Left), fracCount);
                } while (x1 % pb.Step == 0);

                randomStep = 0;
                float x2 = 0;
                do
                {
                    randomStep = (float)random.NextDouble();
                    x2 = (float)Math.Round((randomStep * (pb.X2Right - pb.X2Left) + pb.X2Left), fracCount);
                } while (x2 % pb.Step == 0);

                resultList.Add(new PersonAsPoint(x1, x2));
            }
            return resultList;
        }
    }
}
