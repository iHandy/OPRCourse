using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev.PopulationChooseMethods
{
    public class RankMethod : IPopulationChooser
    {
        public string getName()
        {
            return "Rank";
        }

        public override string ToString()
        {
            return getName();
        }

        public List<int> RankList { get; set; }

        public List<Person> getPopulationForFunctions(List<Person> nativePopulation, int generation, int N)
        {
            List<Person> nextPopulation = new List<Person>();
            List<int> selectedIds = new List<int>(N);
            int half = N / 2;

            nativePopulation.Sort(new FuncComparer());

            for (int i = 0; i < RankList.Count; i++)
            {
                int copies = RankList[i] / N;

                for (int j = 0; j < copies; j++)
                {
                    if (!nativePopulation[i].isRemoved)
                    {
                        bool even = (selectedIds.Count % 2) == 0;
                        Person newPerson = new Person(generation, nativePopulation[i].ID, nativePopulation[i].Chromosome, even ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND);
                        nextPopulation.Add(newPerson);
                        selectedIds.Add(newPerson.ID);
                        if (nextPopulation.Count == N)
                        {
                            break;
                        }
                    }
                }
                if (nextPopulation.Count == N)
                {
                    break;
                }
            }

            bool changes = false;
            while (selectedIds.Count < N)
            {
                changes = false;
                foreach (var item in nativePopulation)
                {
                    if (!item.isRemoved && !selectedIds.Contains(item.ID))
                    {
                        changes = true;

                        bool even = (selectedIds.Count % 2) == 0;
                        Person newPerson = new Person(generation, item.ID, item.Chromosome, even ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND);
                        nextPopulation.Add(newPerson);
                        selectedIds.Add(newPerson.ID);

                        if (nextPopulation.Count == N)
                        {
                            break;
                        }
                    }
                }

                if (nextPopulation.Count == N || !changes)
                {
                    break;
                }
            }

            return nextPopulation;
        }

        class FuncComparer : IComparer<Person>
        {
            public int Compare(Person x, Person y)
            {
                return x.FuncionCommonValue.CompareTo(y.FuncionCommonValue);
            }
        }
    }
}
