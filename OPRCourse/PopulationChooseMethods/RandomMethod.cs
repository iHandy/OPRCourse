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
            //int count = nativePopulation.Count;
            int half = N / 2;
            //List<Person> toRemove = new List<Person>();
            for (int i = 0; i < nativePopulation.Count; i++)
            {

                if (nativePopulation[i].isRemoved)
                {
                    //toRemove.Add(nativePopulation[i]);
                }
                else
                {
                    if (!selectedIds.Contains(nativePopulation[i].ID))
                    {
                        Person newPerson = new Person(generation, nativePopulation[i].ID, nativePopulation[i].Chromosome, i < half ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND);
                        //nativePopulation[i].FunctionNumber = i <= half ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND;
                        //nativePopulation[i].Generation = generation;
                        nextPopulation.Add(newPerson);
                        selectedIds.Add(newPerson.ID);
                    }
                }

                if (nextPopulation.Count == N)
                {
                    break;
                }
            }
            /*foreach (var item in toRemove)
            {
                nativePopulation.Remove(item);
            }*/
            return nextPopulation;
        }
    }
}
