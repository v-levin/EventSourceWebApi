using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
    public class EventIdRequestValidator : AbstractValidator<EventIdRequest>
    {
        IEventsRepository _eventsRepository;

        public EventIdRequestValidator(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;

            RuleFor(e => e.Id).Must(id => CheckIfEventExists(id)).WithMessage("Event Not Found").GreaterThan(0);
        }

        private bool CheckIfEventExists(int id)
        {
            var eventResponse = _eventsRepository.GetEvent(new EventIdRequest { Id = id });
            return eventResponse.Result && eventResponse.Event != null;
        }
    }
}
