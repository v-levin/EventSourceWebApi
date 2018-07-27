using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Messages
{
    public class LoggingMessages
    {
        public static string GettingAllEvents = "Getting all events...";

        public static string GettingEventById(int id)
        {
            return $"Getting Event by Id: {id}";
        }
        public static string GettingAllPlaces = "Getting all places...";

        public static string GettingPlaceById(int id)
        {
            return $"Getting Place by Id: {id}";
        }
    }
}
