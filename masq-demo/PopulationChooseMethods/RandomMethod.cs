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

        public List<Person> getPopulationForFunctions(List<Person> nativePopulation, int generation)
        {
            List<Person> nextPopulation = new List<Person>();
            int count = nativePopulation.Count;
            int half = count / 2;
            //List<Person> toRemove = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                if (nativePopulation[i].isRemoved)
                {
                    //toRemove.Add(nativePopulation[i]);
                }
                else
                {
                    Person newPerson = new Person(generation, nativePopulation[i].ID, nativePopulation[i].Chromosome, i <= half ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND);
                    //nativePopulation[i].FunctionNumber = i <= half ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND;
                    //nativePopulation[i].Generation = generation;
                    nextPopulation.Add(newPerson);
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
