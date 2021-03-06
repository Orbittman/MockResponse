﻿using MockResponse.Core.Models;

namespace MockResponse.Core.Requests
{
    public class PostResponseRequest : RequestBase
    {
        public PostResponseRequest(ResponseModel response)
        {
            Response = response;
        }

        public string ResponseId { get { return Response.Id; }}

        public ResponseModel Response { get; }

        public override string Path => "responses/{ResponseId}";
    }
}
