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
    }
}
