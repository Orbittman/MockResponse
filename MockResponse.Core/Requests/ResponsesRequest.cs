using System;

public class ResponsesRequest : RequestBase
{
	public int Page { get; set; }

	public int PageSize { get; set; }

    public override string Path => "responses";
}