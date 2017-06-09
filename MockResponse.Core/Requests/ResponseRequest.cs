using System;

public class ResponseRequest : RequestBase
{
    public string ResponseId { get; set; }

    public override string Path => "responses/{RequestId}";
}