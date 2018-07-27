using EventSourceWebApi.Contracts;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
    public partial class PlacesValidator : AbstractValidator<Place>
    {
        public PlacesValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Name).MaximumLength(50);
            RuleFor(p => p.Capacity).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.Location).NotEmpty();
            RuleFor(p => p.DateRegistered).NotEmpty();
            RuleFor(p => p.City).NotEmpty();
        }
    }
}
