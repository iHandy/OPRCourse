using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev.PopulationChooseMethods
{
    class RandomMethod : IPopulationChooser
    {
        public string getName()
        {
            return "50/50";
        }

        public override string ToString()
        {
            return getName();
        }

        public List<Person> getPopulationForFunctions(List<Person> nativePopulation, int generation, int N)
        {
            List<Person> nextPopulation = new List<Person>();
            List<int> selectedIds = new List<int>(N);
            int half = N / 2;
            for (int i = 0; i < nativePopulation.Count; i++)
            {

                if (!nativePopulation[i].isRemoved)
                {
                    if (!selectedIds.Contains(nativePopulation[i].ID))
                    {
                        Person newPerson = new Person(generation, nativePopulation[i].ID, nativePopulation[i].Chromosome, i < half ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND);
                        nextPopulation.Add(newPerson);
                        selectedIds.Add(newPerson.ID);
                    }
                }

                if (nextPopulation.Count == N)
                {
                    break;
                }
            }
            return nextPopulation;
        }
    }
}
