﻿using System;
using AutoMapper;

using MockResponse.Api.Commands.Parameters;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;

using MongoDB.Bson;

namespace MockResponse.Api.Commands
{
    public class PostResponseCommand : IPostResponseCommand
    {
        private readonly IRequestContext _requestContext;
        private readonly INoSqlClient _dbClient;
        private readonly IMapper _mapper;

        public PostResponseCommand(IRequestContext requestContext, INoSqlClient dbClient, IMapper mapper)
        {
            _requestContext = requestContext;
            _dbClient = dbClient;
            _mapper = mapper;
        }

        public Response Execute(ResponsePostParameters request)
        {
            var response = _mapper.Map<Response>(request);
            if(response.Domain == null)
            {
                response.Domain = new Domain { Host = $"{Guid.NewGuid()}.api.mockresponse.net" };
            }

            response.Account = new ObjectId(_requestContext.AccountId);
            _dbClient.InsertOne(response, response.GetType().Name);

            return response;
        }
    }
}
