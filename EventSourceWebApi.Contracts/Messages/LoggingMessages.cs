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

        public static string NullData = "The Event has no data";

        public static string GetEventSuccessfully = "The Event has successfully taken.";

        public static string IdsNotMatch = "Ids did not match.";

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
    }
}
