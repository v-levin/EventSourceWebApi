using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Validators
{
   public class UpdateEventValidator<T> : AbstractValidator<PutRequest<Event>> 
    {
    }
}
