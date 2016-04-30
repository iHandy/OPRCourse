using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev
{
    public class MutationProcessor
    {
        Random random = new Random();

        public List<Person> getMutationChilds(oprCourseSoloviev.VEGA.Parents parents, float mutationFactor, int generation, ref int lastId)
        {
            List<Person> childs = new List<Person>(2);

            int[] chrParent1 = parents.parent1.Chromosome.getFullChromosome();
            int[] chrParent2 = parents.parent2.Chromosome.getFullChromosome();

            int[] chromosomeChild1 = new int[chrParent1.Length];
            int[] chromosomeChild2 = new int[chrParent1.Length];

            for (int i = 0; i < chromosomeChild1.Length; i++)
            {
                float randValue= random.Next(1, 100)/100;
                chromosomeChild1[i] = randValue < mutationFactor ? (chrParent1[i] == 1 ? 0 : 1) : chrParent1[i];

                randValue = random.Next(1, 100) / 100;
                chromosomeChild2[i] = randValue < mutationFactor ? (chrParent2[i] == 1 ? 0 : 1) : chrParent2[i];
            }

            Person child1 = new Person(generation, ++lastId,
                new Chromosome(parents.parent1.Chromosome.getIntCount1(), parents.parent1.Chromosome.getFracCount1(),
                parents.parent1.Chromosome.getIntCount2(), parents.parent1.Chromosome.getFracCount2(), chromosomeChild1));
            child1.Type = PersonType.MUTATION;
            childs.Add(child1);

            Person child2 = new Person(generation, ++lastId,
                new Chromosome(parents.parent1.Chromosome.getIntCount1(), parents.parent1.Chromosome.getFracCount1(),
                parents.parent1.Chromosome.getIntCount2(), parents.parent1.Chromosome.getFracCount2(), chromosomeChild2));
            child2.Type = PersonType.MUTATION;
            childs.Add(child2);

            return childs;
        }
    }
}
