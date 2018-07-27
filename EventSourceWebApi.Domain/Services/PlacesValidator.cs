using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Responses;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Services
{
    public class PlacesValidator : AbstractValidator<Place>, IPlaceValidator
    {
        public PlacesValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Capacity).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.Location).NotEmpty();
            RuleFor(p => p.DateRegistered).NotEmpty();
            RuleFor(p => p.City).NotEmpty();
        }

        public ValdatePlaceResponse ValidPlace(Place place)
        {
            PlacesValidator validator = new PlacesValidator();
            ValidationResult result = validator.Validate(place);
            IList<ValidationFailure> failures = result.Errors;
            var errors = new List<string>();

            foreach(var e in failures)
            {
                errors.Add(result.Errors[0].ErrorMessage);
            }
            var response = new ValdatePlaceResponse()
            {
                Result = result.IsValid,
                Errors = errors,
            };
            return response;
        }
    }

}
