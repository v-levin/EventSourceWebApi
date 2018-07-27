using System;

namespace EventSourceWebApi.Contracts.Messages
{
    public class LoggingMessages
    {
        public static string GettingAllEvents = "Getting all events...";

        public static string CreatingEvent = "Creating new Event...";

        public static string GettingEventById(int id)
        {
            return $"Getting Event by Id: {id}.";
        }

        public static string DataNotValid = "The data has not valid format.";

        public static string EventSuccessfullyCreated = "The Event has successfully creted.";
        
        public static string GetEventSuccessfully(int id)
        {
            return $"The Event with Id: {id} has successfully taken.";
        }

        public static string IdsNotMatch = "Ids did not match.";

        public static string GetEventsSuccessfully = "The Events has successfully taken.";

        public static string PassedIdNotMatchWithEventId(int id, int eventId)
        {
            return $"Passed id {id} did not match with Event id {eventId}.";
        }

        public static string EditingEventById(int id)
        {
            return $"Editing Event Id: {id}.";
        }

        public static string EventSuccessfullyModified(int id)
        {
            return $"The Event with id: {id} has been successfully modified.";
        }

        public static string LookingForEventToDelete(int id)
        {
            return $"Looking for Event to delete with id: {id}...";
        }

        public static string EventNotFound(int id)
        {
            return $"The Event with Id: {id} was not found.";
        }

        public static string EventSuccessfullyDeleted(int id)
        {
            return $"The Event with Id: {id} has been successfully deleted.";
        }
        public static string GettingAllPlaces = "Getting all places...";

        public static string GettingPlaceById(int id)
        {
            return $"Getting Place by Id: {id}";
        }
    }
}
