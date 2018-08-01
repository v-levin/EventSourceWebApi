using EventSourceWebApi.Contracts.Requests;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Validators
{
    public class PlaceSearchValidator : AbstractValidator<PlaceSearchRequest>
    {
        public PlaceSearchValidator()
        {
            RuleFor(e => e.Name).MaximumLength(20);
            RuleFor(e => e.Location).MaximumLength(20);
            RuleFor(e => e.City).MaximumLength(20);
            RuleFor(p => p.Limit).LessThan(100);
        }
    }
}
