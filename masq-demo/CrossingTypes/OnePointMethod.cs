using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo.CrossingTypes
{
    public class OnePointMethod : ICrossingType
    {
        public string getName()
        {
            return "One point";
        }

        public override string ToString()
        {
            return getName();
        }

        public List<Person> getCrossedChilds(demo.VEGA.Parents parents, int[] crossingPoints, int generation, ref int lastId)
        {
            List<Person> childs = new List<Person>(2);
            int crossingPoint = crossingPoints[0];

            int[] chrParent1 = parents.parent1.Chromosome.getFullChromosome();
            int[] chrParent2 = parents.parent2.Chromosome.getFullChromosome();

            int[] chromosomeChild1 = new int[chrParent1.Length];
            int[] chromosomeChild2 = new int[chrParent1.Length];

            for (int i = 0; i < chromosomeChild1.Length; i++)
            {
                chromosomeChild1[i] = i < crossingPoint ? chrParent1[i] : chrParent2[i];
                chromosomeChild2[i] = i < crossingPoint ? chrParent2[i] : chrParent1[i];
            }

            Person child1 = new Person(generation, ++lastId,
                new Chromosome(parents.parent1.Chromosome.getIntCount1(), parents.parent1.Chromosome.getFracCount1(),
                parents.parent1.Chromosome.getIntCount2(), parents.parent1.Chromosome.getFracCount2(), chromosomeChild1));
            child1.Type = PersonType.CROSSING;
            childs.Add(child1);

            Person child2 = new Person(generation, ++lastId,
                new Chromosome(parents.parent1.Chromosome.getIntCount1(), parents.parent1.Chromosome.getFracCount1(),
                parents.parent1.Chromosome.getIntCount2(), parents.parent1.Chromosome.getFracCount2(), chromosomeChild2));
            child2.Type = PersonType.CROSSING;
            childs.Add(child2);

            return childs;
        }
    }
}
