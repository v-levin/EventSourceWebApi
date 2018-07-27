using System;

namespace EventSourceWebApi.Contracts.Messages
{
    public class LoggingMessages
    {
        public static string DataNotValid = "The data has not valid format.";

        public static string EventSuccessfullyCreated = "The Event has successfully creted.";

        public static string IdsNotMatch = "Ids did not match.";

        public static string NoDataInDb = "There is no data in the database.";

        public static string PassedIdNotMatchWithEventId(int id, int eventId)
        {
            return $"Passed id {id} did not match with Event id {eventId}.";
        }

        public static string EventNotFound(int id)
        {
            return $"The Event with Id: {id} was not found.";
        }

        public static string GettingAllPlaces = "Getting all places...";

        public static string GettingPlaceById(int id)
        {
            return $"Getting Place by Id: {id}";
        }

        public static string CreatingPlace = "Creating new Place...";

        public static string InvalidInput = "Invalid Input.";

        public static string PlaceSucessfullyCreated(int placeId)
        {
            return $"Place with id: {placeId} is sucessfully created";
        }

    }
}
