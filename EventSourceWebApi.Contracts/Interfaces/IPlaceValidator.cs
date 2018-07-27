using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlaceValidator
    {
        ValdatePlaceResponse ValidPlace(Place place);

    }
}
