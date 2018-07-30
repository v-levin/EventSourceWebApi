using EventSourceWebApi.Contracts.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

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
