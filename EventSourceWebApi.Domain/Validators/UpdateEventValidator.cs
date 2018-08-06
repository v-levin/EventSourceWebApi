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
            RuleFor(e => e.Payload.Name).NotEmpty();
            RuleFor(e => e.Payload.Name).MaximumLength(50);
            RuleFor(e => e.Payload.DateRegistered).NotEmpty();
            RuleFor(e => e.Payload.Seats).NotEmpty().GreaterThan(0);
            RuleFor(e => e.Payload.Description).NotEmpty().MaximumLength(150);
            RuleFor(e => e.Payload.City).NotEmpty();
            RuleFor(e => e.Payload.Category).NotEmpty();
            RuleFor(e => e.Payload.Location).NotEmpty();
        }

        private bool CheckIfEventExists(int id)
        {
            var eventResponse = _eventsRepository.GetEvent(new EventIdRequest { Id = id });
            return eventResponse.Result && eventResponse.Event != null;
        }
    }
}
