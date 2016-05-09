using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev.PopulationChooseMethods
{
    public class TournamentMethod : IPopulationChooser
    {
        public string getName()
        {
            return "Tournament";
        }

        public override string ToString()
        {
            return getName();
        }

        public int IndividN { get; set; }
        public int IndividForSelectN { get; set; }
        public int SubWay { get; set; }
        public int SelectWay { get; set; }

        public List<Person> getPopulationForFunctions(List<Person> nativePopulation, int generation, int N)
        {
            List<Person> nextPopulation = new List<Person>();
            List<int> selectedIds = new List<int>(N);
            int half = N / 2;

            List<List<Person>> workList = new List<List<Person>>();

            if (SubWay == 1)//range
            {
                nativePopulation.Sort(new FuncComparer());
            }

            List<Person> groupList = new List<Person>(IndividN);
            List<Person> groupListEven = new List<Person>(IndividN);
            int i = 0;
            foreach (var item in nativePopulation)
            {
                if (groupList.Count >= IndividN)
                {
                    workList.Add(groupList);
                    groupList = new List<Person>(IndividN);
                }
                if (groupListEven.Count >= IndividN)
                {
                    workList.Add(groupListEven);
                    groupListEven = new List<Person>(IndividN);
                }

                if (!item.isRemoved)
                {
                    switch (SubWay)
                    {
                        case 0: //random
                            groupList.Add(item);
                            break;
                        case 1: //range
                            groupList.Add(item);
                            break;
                        case 2: //even\odd
                            bool even = (i % 2) == 0;
                            if (even)
                            {
                                groupListEven.Add(item);
                            }
                            else
                            {
                                groupList.Add(item);
                            }
                            break;
                    }
                    i++;
                }
            }
            if (groupList.Count > 0)
            {
                workList.Add(groupList);
            }
            if (groupListEven.Count > 0)
            {
                workList.Add(groupListEven);
            }

            i = 0;
            foreach (var item in workList)
            {
                if (SelectWay == 1) //best
                {
                    item.Sort(new FuncComparer());
                }
                for (int j = 0; j < Math.Min(IndividForSelectN, item.Count); j++)
                {
                    bool even = (i % 2) == 0;
                    switch (SelectWay)
                    {
                        case 0: //random
                        case 1: //best
                            Person newPerson = new Person(generation, item[j].ID, item[j].Chromosome, even ? FUNCTION_NUMBER.FIRST : FUNCTION_NUMBER.SECOND);
                            nextPopulation.Add(newPerson);
                            selectedIds.Add(newPerson.ID);
                            i++;
                            break;
                    }
                    if (nextPopulation.Count == N)
                    {
                        break;
                    }
                }
                if (nextPopulation.Count == N)
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
