using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev.PopulationChooseMethods
{
    class RouletteWheelMethodWithoutDupl : IPopulationChooser
    {
        Random mRandomizer = new Random();

        public override string ToString()
        {
            return getName();
        }

        public string getName()
        {
            return "Roulette w/o duplicates";
        }

        public List<Person> getPopulationForFunctions(List<Person> nativePopulation, int generation, int N)
        {
            List<Person> nextPopulation = new List<Person>();
            List<int> selectedIds = new List<int>(N);

            double functionsSum = 0;

            foreach (var item in nativePopulation)
            {
                if (!item.isRemoved && !selectedIds.Contains(item.ID))
                {
                    functionsSum += Math.Abs(item.FuncionCommonValue);
                }
            }

            double oneDeg = functionsSum <= 360 ? 360 / functionsSum : 1;
            double oneVal = functionsSum > 360 ? functionsSum / 360 : 1;

            for (int i = 0; i < N; i++)
            {
                bool even = (i % 2) == 0;
                int targetAng = mRandomizer.Next(0, 361);

                double angleSum = 0;
                foreach (var item in nativePopulation)
                {
                    if (!item.isRemoved && !selectedIds.Contains(item.ID))
                    {
                        angleSum += Math.Abs(item.FuncionCommonValue) * oneDeg / oneVal;

                        if (angleSum > 361)
                        {
                            throw new Exception("Angle more 360 deg!!!");
                        }

                        if (targetAng < angleSum)
                        {
                            Person newPerson = new Person(generation, item.ID, item.Chromosome, even ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND);
                            nextPopulation.Add(newPerson);
                            selectedIds.Add(newPerson.ID);
                            break;
                        }
                    }
                }
            }
            return nextPopulation;
        }
    }
}
