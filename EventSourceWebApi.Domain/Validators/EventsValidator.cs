using EventSourceWebApi.Contracts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Validators
{
    public partial class EventsValidator : AbstractValidator<Event>
    {
        public EventsValidator()
        {
            RuleFor(e => e.Name).NotEmpty();
            RuleFor(e => e.Name).MaximumLength(50);
            RuleFor(e => e.DateRegistered).NotEmpty();
            RuleFor(e => e.Seats).NotEmpty();
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.City).NotEmpty();
            RuleFor(e => e.Category).NotEmpty();
            RuleFor(e => e.Location).NotEmpty();
        }
    }
}
