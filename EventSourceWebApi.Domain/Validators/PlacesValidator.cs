using System;
using System.Text.RegularExpressions;
using EventSourceWebApi.Contracts;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
    public partial class PlacesValidator : AbstractValidator<Place>
    {
        public PlacesValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
            RuleFor(p => p.Capacity).NotEmpty().GreaterThan(0);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(150);
            RuleFor(p => p.Location).NotEmpty();
            RuleFor(p => p.DateRegistered).NotEmpty();
            RuleFor(p => p.City).NotEmpty();
        }
    }
}
