using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{

    
    public class UpdatePlaceValidator : AbstractValidator<PutRequest<Place>>
    {
        IPlacesRepository _placesRepository;

        public UpdatePlaceValidator(IPlacesRepository placesRepository)
        {
            _placesRepository = placesRepository;

            RuleFor(p => p.Id).Must(id => CheckIfPlaceExists(id)).WithMessage("Place Not Found").NotEmpty().GreaterThan(0);
            RuleFor(p => p.Payload.Name).NotEmpty().MaximumLength(50);
            RuleFor(p => p.Payload.Capacity).NotEmpty().GreaterThan(0);
            RuleFor(p => p.Payload.Description).NotEmpty().MaximumLength(150);
            RuleFor(p => p.Payload.Location).NotEmpty();
            RuleFor(p => p.Payload.DateRegistered).NotEmpty();
            RuleFor(p => p.Payload.City).NotEmpty();
        }
        private bool CheckIfPlaceExists(int id)
        {
            var placeResponse = _placesRepository.GetPlace(new PlaceIdRequest { Id = id });
            return placeResponse.Result && placeResponse.Place != null;
        }
    }
}
