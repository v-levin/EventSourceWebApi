using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
   public class UpdateEventValidator : AbstractValidator<PutRequest<Event>> 
    {
        IEventsRepository _eventsRepository;

        public UpdateEventValidator(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;

            RuleFor(e => e.Id).Must(id => CheckIfEventExists(id)).WithMessage("Event Not Found").NotEmpty().GreaterThan(0);
            RuleFor(e => e.Payload.Name).MaximumLength(50);
            RuleFor(e => e.Payload.Description).MaximumLength(150);
        }

        private bool CheckIfEventExists(int id)
        {
            var eventResponse = _eventsRepository.GetEvent(new EventIdRequest { Id = id });
            return eventResponse.Result && eventResponse.Event != null;
        }
    }
}
