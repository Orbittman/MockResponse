﻿using MockResponse.Api.Queries.Parameters;
using MockResponse.Core.Data.Models;

namespace MockResponse.Api.Queries
{
    public interface IResponseQuery : IQuery<ResponseParameters, Response>
    {
    }
}