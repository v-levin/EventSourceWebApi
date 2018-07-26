﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsService
    {
        IEnumerable<Event> GetAll();
    }
}
