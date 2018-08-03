using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
    public class PlaceIdRequestValidator : AbstractValidator<PlaceIdRequest>
    {
        IPlacesRepository _placesRepository;

        public PlaceIdRequestValidator(IPlacesRepository placesRepository)
        {
            _placesRepository = placesRepository;

            RuleFor(e => e.Id).GreaterThan(0);
        }
    }
}
