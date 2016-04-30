using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev
{
    public interface IPopulationChooser
    {
        string getName();

        List<Person> getPopulationForFunctions(List<Person> nativePopulation, int generation);
    }
}
