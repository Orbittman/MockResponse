using System.Collections.Generic;

public class ResponseModel{
    public int ResponseId { get; set; }

    public int StatusCode { get; set; }

    public string Content { get; set; }

    public string Path { get; set; }

    public List<HeaderModel> Headers { get; set; }
}

public class HeaderModel {
    public string Name { get; set; }

    public string Value { get; set; }
}