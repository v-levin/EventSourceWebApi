using System;

namespace EventSourceWebApi.Contracts.Messages
{
    public class LoggingMessages
    {

        public static string EventNotFound(int id)
        {
            return $"The Event with Id: {id} was not found.";
        }

        public static string GettingPlaceById(int id)
        {
            return $"Getting Place by Id: {id}";
        }

        public static string PlaceSucessfullyCreated(int placeId)
        {
            return $"Place with id: {placeId} is sucessfully created";
        }

    }
}
