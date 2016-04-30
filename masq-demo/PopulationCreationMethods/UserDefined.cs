using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev.PopulationCreationMethods
{
    public class UserDefined : IPopulationCreation
    {
        private List<PersonAsPoint> userDefinedPoints = new List<PersonAsPoint>();

        public override string ToString()
        {
            return getName();
        }

        public string getName()
        {
            return "User defined";
        }

        public List<PersonAsPoint> getPopulation(int N, ParamBoundaries pb)
        {
            return userDefinedPoints;
        }

        public void addPoint(float x1, float x2)
        {
            PersonAsPoint personAsPoint = new PersonAsPoint(x1,x2);
            userDefinedPoints.Add(personAsPoint);
        }
    }
}
