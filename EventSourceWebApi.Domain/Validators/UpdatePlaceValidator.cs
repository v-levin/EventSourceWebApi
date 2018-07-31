using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Validators
{

    
    public class UpdatePlaceValidator : AbstractValidator<PutRequest<Place>>
    { 
        public UpdatePlaceValidator()
        {
            RuleFor(p => p.Payload.Name).NotEmpty().MaximumLength(50);
            RuleFor(p => p.Payload.Capacity).NotEmpty().GreaterThan(0);
            RuleFor(p => p.Payload.Description).NotEmpty().MaximumLength(150);
            RuleFor(p => p.Payload.Location).NotEmpty();
            RuleFor(p => p.Payload.DateRegistered).NotEmpty();
            RuleFor(p => p.Payload.City).NotEmpty();
            RuleFor(p => p.Id).NotEmpty().WithMessage("Id must be a number.").GreaterThan(0);
        }
    }
}
