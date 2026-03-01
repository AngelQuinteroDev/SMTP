public class EmailResult
{
    public bool Success;
    public int StatusCode;
    public string Message;

    public EmailResult(bool success, int statusCode, string message)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
    }
}