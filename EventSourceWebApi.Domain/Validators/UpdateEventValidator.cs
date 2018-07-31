using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
   public class UpdateEventValidator : AbstractValidator<PutRequest<Event>> 
    {
        public UpdateEventValidator()
        {
            RuleFor(e => e.Id).NotEmpty().WithMessage("Id must be a number.").GreaterThan(0);
            RuleFor(e => e.Payload.Name).NotEmpty();
            RuleFor(e => e.Payload.Name).MaximumLength(50);
            RuleFor(e => e.Payload.DateRegistered).NotEmpty();
            RuleFor(e => e.Payload.Seats).NotEmpty().GreaterThan(0);
            RuleFor(e => e.Payload.Description).NotEmpty().MaximumLength(150);
            RuleFor(e => e.Payload.City).NotEmpty();
            RuleFor(e => e.Payload.Category).NotEmpty();
            RuleFor(e => e.Payload.Location).NotEmpty();
        }
    }
}
