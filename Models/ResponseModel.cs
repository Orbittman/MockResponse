public class ResponseModel{
    public int ResponseId { get; set; }

    public string ContentType { get; set; }

    public int StatusCode { get; set; }
    
    public string ContentEncoding { get; set; }

    public string Server { get; set; }

    public string Vary { get; set; }

    public string CacheControl { get; set; } 

    public string Content { get; set; }

    public string Path { get; set; }
}