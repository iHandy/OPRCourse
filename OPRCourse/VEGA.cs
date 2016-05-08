using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCalc;
using System.Diagnostics;

namespace oprCourseSoloviev
{
    public class VEGA
    {
        private List<Person> startPopulation;
        private List<Person> currentPopulation = new List<Person>();
        private int generation = 0;
        private int lastPersonId = 0;
        private FunctionControl f1, f2;
        private ParametersControl p;

        public List<string> selectedId = new List<string>();
        public List<string> crossedId = new List<string>();
        public List<string> mutatedId = new List<string>();

        Random rnd = new Random();

        private struct BestResult
        {
            public int f1ID;
            public int f2ID;
        }

        public struct Parents
        {
            public Person parent1;
            public Person parent2;
        }

        public VEGA(FunctionControl f1, FunctionControl f2, ParametersControl p)
        {
            this.f1 = f1;
            this.f2 = f2;
            this.p = p;
        }

        public void startSolution()
        {
            for (int i = 0; i < p.EOCC; i++)
            {
                generation++;
                Debug.WriteLine("=================== GENERATION " + generation + " ===================");

                //Generation start N population
                if (i == 0)
                {
                    IPopulationCreation populationCreator = p.PopulationCreation;
                    startPopulation = getPopulationFromPoints(populationCreator.getPopulation(p.N, p.ParamBoundaries));
                    Debug.WriteLine("i == 0. StartPopulation count = " + startPopulation.Count);
                }
                else
                {
                    //mixing
                    startPopulation = new List<Person>(currentPopulation);
                    startPopulation = startPopulation.OrderBy(x => rnd.Next()).ToList();

                    Debug.WriteLine("Mixing. StartPopulation count = " + startPopulation.Count);
                }

                foreach (var item in startPopulation)
                {
                    calculateFunctions(item);
                }

                //Divide population by Function groups (Choose/select)
                IPopulationChooser populationChooser = p.PopulationChooser;
                List<Person> nextPopulation = populationChooser.getPopulationForFunctions(startPopulation, generation, p.N);

                Debug.WriteLine("Next population count = " + nextPopulation.Count);

                if (nextPopulation.Count == 0)
                {
                    return;
                }

                currentPopulation.AddRange(nextPopulation);

                //Calculate F1 and F2 values, find best in F1 and best in F2.
                BestResult br = calculateFunctionAndFindBest(nextPopulation);

                if (br.f1ID == -1 || br.f2ID == -1)
                {
                    return;
                }

                //Generate parents pairs
                Parents parents = generateParents(br, nextPopulation);
                selectedId.Add(parents.parent1.Generation + "." + parents.parent1.ID);
                selectedId.Add(parents.parent2.Generation + "." + parents.parent2.ID);

                //Do crossing
                ICrossingType crossingType = p.CrossingType;
                currentPopulation.AddRange(crossingType.getCrossedChilds(parents, p.CrossingPoint, generation, ref lastPersonId));
                calculateFunctions(currentPopulation[currentPopulation.Count - 2]);
                calculateFunctions(currentPopulation[currentPopulation.Count - 1]);
                crossedId.Add(currentPopulation[currentPopulation.Count - 2].Generation + "." + currentPopulation[currentPopulation.Count - 2].ID);
                crossedId.Add(currentPopulation[currentPopulation.Count - 1].Generation + "." + currentPopulation[currentPopulation.Count - 1].ID);

                //Do mutation
                MutationProcessor mp = new MutationProcessor();
                switch (p.MutationType)
                {
                    case MutationTypes.PARENT:
                        currentPopulation.AddRange(mp.getMutationChilds(parents, p.Mu, generation, ref lastPersonId));
                        break;
                    case MutationTypes.CHILD:
                        Parents childs = new Parents();
                        childs.parent1 = currentPopulation[currentPopulation.Count - 2];
                        childs.parent2 = currentPopulation[currentPopulation.Count - 1];
                        currentPopulation.AddRange(mp.getMutationChilds(childs, p.Mu, generation, ref lastPersonId));
                        break;
                }
                calculateFunctions(currentPopulation[currentPopulation.Count - 2]);
                calculateFunctions(currentPopulation[currentPopulation.Count - 1]);
                mutatedId.Add(currentPopulation[currentPopulation.Count - 2].Generation + "." + currentPopulation[currentPopulation.Count - 2].ID);
                mutatedId.Add(currentPopulation[currentPopulation.Count - 1].Generation + "." + currentPopulation[currentPopulation.Count - 1].ID);
            }
        }

        private List<Person> getPopulationFromPoints(List<PersonAsPoint> list)
        {
            List<Person> resultList = new List<Person>(list.Count);

            foreach (var item in list)
            {
                Chromosome chromosome = new Chromosome(p.x1IntLength, p.x1FracLength, p.x2IntLength, p.x2FracLength, item.X1, item.X2);
                Person newPerson = new Person(generation, ++lastPersonId, chromosome);

                if (item.X1 > p.ParamBoundaries.X1Right || item.X1 < p.ParamBoundaries.X1Left
                    || item.X2 > p.ParamBoundaries.X2Right || item.X2 < p.ParamBoundaries.X2Left)
                {
                    newPerson.isRemoved = true;
                }
                resultList.Add(newPerson);
            }
            return resultList;
        }

        private BestResult calculateFunctionAndFindBest(List<Person> population)
        {
            int bestIDF1 = -1, bestIDF2 = -1;
            double yBest1 = (double.MinValue + 1), yBest2 = (double.MinValue + 1);
            foreach (var item in population)
            {
                float x1 = item.Chromosome.getNormalValueX1();
                float x2 = item.Chromosome.getNormalValueX2();
                double y;
                switch (item.FunctionNumber)
                {
                    case FUNCTION_NUMBER.FIRST:
                        evaluateParams(f1.Function, x1, x2);
                        y = (double)f1.Function.Evaluate();
                        item.Funcion1Value = y;

                        if (x1 > this.p.ParamBoundaries.X1Right || x1 < this.p.ParamBoundaries.X1Left
                               || x2 > this.p.ParamBoundaries.X2Right || x2 < this.p.ParamBoundaries.X2Left)
                        {
                            item.isRemoved = true;
                        }
                        else
                        {
                            yBest1 = y > yBest1 ? y : yBest1;
                            bestIDF1 = yBest1 == y ? item.ID : bestIDF1;
                        }
                        break;
                    case FUNCTION_NUMBER.SECOND:
                        evaluateParams(f2.Function, x1, x2);
                        y = (double)f2.Function.Evaluate();
                        item.Funcion2Value = y;

                        if (x1 > this.p.ParamBoundaries.X1Right || x1 < this.p.ParamBoundaries.X1Left
                               || x2 > this.p.ParamBoundaries.X2Right || x2 < this.p.ParamBoundaries.X2Left)
                        {
                            item.isRemoved = true;
                        }
                        else
                        {
                            yBest2 = y > yBest2 ? y : yBest2;
                            bestIDF2 = yBest2 == y ? item.ID : bestIDF2;
                        }
                        break;
                    default:
                        break;
                }
            }
            BestResult br = new BestResult();
            br.f1ID = bestIDF1;
            br.f2ID = bestIDF2;
            return br;
        }

        private Parents generateParents(BestResult br, List<Person> population)
        {
            Person parent1 = null;
            Person parent2 = null;
            foreach (var item in population)
            {
                if (item.ID == br.f1ID)
                {
                    item.Type = PersonType.SELECTED;
                    parent1 = item;// new Person(generation, ++lastPersonId, item.Chromosome, item.FunctionNumber);
                }
                /*else */
                if (item.ID == br.f2ID)
                {
                    item.Type = PersonType.SELECTED;
                    parent2 = item;// new Person(generation, ++lastPersonId, item.Chromosome, item.FunctionNumber);
                }
            }

            if (parent1 == null || parent2 == null)
            {
                throw new SystemException("Parents is NULL!");
            }

            Debug.WriteLine("parent1 id = " + parent1.ID + "; parent2 id = " + parent2.ID);

            parent1 = calculateFunctions(parent1);
            parent2 = calculateFunctions(parent2);
            //currentPopulation.Add(parent1);
            //currentPopulation.Add(parent2);
            Parents parents = new Parents();
            parents.parent1 = parent1;
            parents.parent2 = parent2;
            return parents;
        }

        private Person calculateFunctions(Person p)
        {
            if (p.FuncionCommonValue == 0)
            {
                evaluateParams(f1.Function, p.Chromosome.getNormalValueX1(), p.Chromosome.getNormalValueX2());
                p.Funcion1Value = (double)f1.Function.Evaluate();

                evaluateParams(f2.Function, p.Chromosome.getNormalValueX1(), p.Chromosome.getNormalValueX2());
                p.Funcion2Value = (double)f2.Function.Evaluate();

                p.FuncionCommonValue = p.Funcion1Value * 0.5 + p.Funcion2Value * 0.5;

                float x1 = p.Chromosome.getNormalValueX1();
                float x2 = p.Chromosome.getNormalValueX2();
                if (x1 > this.p.ParamBoundaries.X1Right || x1 < this.p.ParamBoundaries.X1Left
                       || x2 > this.p.ParamBoundaries.X2Right || x2 < this.p.ParamBoundaries.X2Left)
                {
                    p.isRemoved = true;
                }
            }

            return p;
        }

        static void evaluateParams(Expression expr, double x1, double x2)
        {
            expr.EvaluateParameter += delegate(string name, ParameterArgs args1)
            {
                if (name == "x1")
                {
                    args1.Result = x1;
                }
                if (name == "x2")
                {
                    args1.Result = x2;
                }
            };
        }

        internal List<Person> getData()
        {
            return currentPopulation;
        }

        internal int getGenerations()
        {
            return generation;
        }
    }
}
