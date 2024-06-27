using ConsysApi.Helpers;

namespace ConsysApi.Exceptions
{
    public class ResultFixerApiException : Exception
    {
        public readonly int Code;

        public ResultFixerApiException(FixerApiHelper.ErrorResultApi errorResult) : base(message: errorResult.Info)
        {
            Code = errorResult.Code;
        }
    }
}
