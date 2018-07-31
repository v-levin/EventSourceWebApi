using EventSourceWebApi.Contracts.Requests;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
    public class IdRequestValidator : AbstractValidator<IdRequest>
    {
        public IdRequestValidator()
        {
            RuleFor(e => e.Id).NotEmpty().WithMessage("Id must be a number.").GreaterThan(0);
        }
    }
}
