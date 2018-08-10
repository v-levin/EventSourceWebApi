using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcePlaces.Functions
{
    public class PlacesConstants  
    {
        public const  string RouteFunction = "orchestrators/PlacesDurableFunction";

        public const string FunctionName = "PlacesDurableFunction";

        public const string CreatePlace = "CreatePlace";

        public const string UpdatePlace = "UpdatePlace";

        public const string DeletePlace = "DeletePlace";

        public const string GetPlace = "GetPlace";
    }
}
