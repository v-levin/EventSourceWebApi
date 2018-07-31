using System.Collections.Generic;

namespace EventSourceWebApi.Contracts.Responses
{
    public class Response
    {
        public Response()
        {
            Result = true;
            Errors = new List<ResponseError>();
        }

        public bool Result { get; set; }

        public List<ResponseError> Errors { get; set; }

        public override string ToString()
        {
            var text = "";
            foreach (var error in Errors)
            {
                text += error;
            }
                return text;
        }
    }
}
