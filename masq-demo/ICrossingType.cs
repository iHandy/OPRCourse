using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo
{
    public interface ICrossingType
    {
        string getName();

        List<Person> getCrossedChilds(demo.VEGA.Parents parents, int[] crossingPoints, int generation, ref int lastId);
    }
}
