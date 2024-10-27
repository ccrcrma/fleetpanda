namespace fleetpanda.common;

public class Response<T>
{
    public Response()
    {
    }
    public Response(bool success, string? message)
    {
        Success = success;
        Message = message;
    }

    public Response(bool success, string? message, T? data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}

public class Response : Response<object>
{
    public Response()
    {
    }

    public Response(bool success, string? message)
    {
        Success = success;
        Message = message;
    }

    public Response(bool success, string? message, object? data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}


