using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo
{
    public interface IPopulationCreation
    {

        string getName();

        List<PersonAsPoint> getPopulation(int N, ParamBoundaries pb); 
    }
}
