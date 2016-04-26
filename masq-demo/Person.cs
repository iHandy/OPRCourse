using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo
{
    public class Person
    {

        public Person(int generation, int id, Chromosome chromosome)
        {
            this.Generation = generation;
            this.ID = id;
            this.Chromosome = chromosome;
            this.Type = PersonType.NORMAL;
        }

        public Person(int generation, int id, Chromosome chromosome, FUNCTION_NUMBER functionNumber)
        {
            this.Generation = generation;
            this.ID = id;
            this.Chromosome = chromosome;
            this.FunctionNumber = functionNumber;
            this.Type = PersonType.NORMAL;
        }

        public Person()
        {
            this.Type = PersonType.NORMAL;
        }

        public int Generation { get; set; }
        public int ID { get; set; }
        public Chromosome Chromosome { get; set; }
        public FUNCTION_NUMBER FunctionNumber { get; set; }
        public double Funcion1Value { get; set; }
        public double Funcion2Value { get; set; }
        public double FuncionCommonValue { get; set; }
        public PersonType Type { get; set; }
        public bool isRemoved { get; set; }
    }

    public enum PersonType { NORMAL, SELECTED, CROSSING, MUTATION, RESULT }
}
