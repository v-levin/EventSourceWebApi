using EventSourceWebApi.Contracts.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Validators
{
    public class IdRequestValidator : AbstractValidator<IdRequest>
    {
        public IdRequestValidator()
        {
            RuleFor(e => e.Id).NotEmpty().GreaterThan(0);
        }
    }
}
