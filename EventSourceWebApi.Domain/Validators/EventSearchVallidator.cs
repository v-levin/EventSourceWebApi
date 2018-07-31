using EventSourceWebApi.Contracts.Requests;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
    public class EventSearchVallidator : AbstractValidator<EventSearchRequest>
    {
        public EventSearchVallidator()
        {
            RuleFor(e => e.Limit).LessThan(100);
            RuleFor(e => e.Name).MaximumLength(20);
            RuleFor(e => e.City).MaximumLength(20);
            RuleFor(e => e.Category).MaximumLength(20);
            RuleFor(e => e.Location).MaximumLength(20);
        }
    }
}
