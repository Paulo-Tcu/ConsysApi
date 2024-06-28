namespace ConsysApi.Helpers
{
    public class ApiResult
    {
        public bool Success { get; private set; }

        public string Message { get; private set; }

        public dynamic Data { get; private set; }

        public ApiResult(bool success, dynamic data, string message)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public ApiResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ApiResult(dynamic data)
        {
            Success = true;
            Data = data;
        }
    }
}
