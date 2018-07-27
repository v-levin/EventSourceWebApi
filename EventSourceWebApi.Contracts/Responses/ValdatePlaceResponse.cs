using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Responses
{
   public class ValdatePlaceResponse
    {
        public bool Result  { get; set; }

        public IList<string> Errors { get; set; }
       
        public string Property  { get; set; }

    }
}
