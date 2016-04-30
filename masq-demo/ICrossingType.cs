using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev
{
    public interface ICrossingType
    {
        string getName();

        List<Person> getCrossedChilds(oprCourseSoloviev.VEGA.Parents parents, int[] crossingPoints, int generation, ref int lastId);
    }
}
