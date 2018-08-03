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

            RuleFor(e => e.Id).Must(id => CheckIfPlaceExists(id)).WithMessage("Place Not Found").GreaterThan(0);
        }

        private bool CheckIfPlaceExists(int id)
        {
            var eventResponse = _placesRepository.GetPlace(new PlaceIdRequest { Id = id });
            return eventResponse.Result && eventResponse.Place != null;
        }
    }
}
