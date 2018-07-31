using EventSourceWebApi.Contracts;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
    public partial class EventsValidator : AbstractValidator<Event>
    {
        public EventsValidator()
        {
            RuleFor(e => e.Name).NotEmpty();
            RuleFor(e => e.Name).MaximumLength(50);
            RuleFor(e => e.DateRegistered).NotEmpty();
            RuleFor(e => e.Seats).NotEmpty().GreaterThan(0);
            RuleFor(e => e.Description).NotEmpty().MaximumLength(150);
            RuleFor(e => e.City).NotEmpty();
            RuleFor(e => e.Category).NotEmpty();
            RuleFor(e => e.Location).NotEmpty();
        }
    }
}
