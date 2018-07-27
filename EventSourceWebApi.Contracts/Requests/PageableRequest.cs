﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Requests
{
    public class PageableRequest
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

    }
}
