using EventSourceWebApi.Contracts.Requests;
using FluentValidation;

namespace EventSourceWebApi.Domain.Validators
{
    public partial class PagebaleValidator : AbstractValidator<PageableRequest>
    {
        public PagebaleValidator()
        {
            RuleFor(p => p.Limit).LessThan(p => p.MaxLimit);
        }
    }
}
