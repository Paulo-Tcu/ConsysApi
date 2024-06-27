using ConsysApi.Exceptions;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ConsysApi.Helpers
{
    public class FixerApiHelper
    {
        private readonly string Uri;
        private readonly string AccessKey;
        private ResultApi _result;

        public string BaseCoin => _result.Base;

        public struct ErrorResultApi
        {
            public int Code { get; set; }
            public string Info { get; set; }
        }

        public struct ResultApi
        {
            public bool Success { get; set; }
            public ErrorResultApi Error { get; set; }
            public string Base { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }
        }

        public FixerApiHelper(IConfiguration config)
        {
            Uri = config.GetSection("FixerApiHelper").GetValue<string>("Uri");
            AccessKey = config.GetSection("FixerApiHelper").GetValue<string>("AccessKey");
        }

        public async Task<Dictionary<string, decimal>> GetRates()
        {
            if(_result.Rates == null)
                await SetRates();

            return _result.Rates;
        }

        public async Task SetRates()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{Uri}?access_key={AccessKey}");

                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    _result = JsonConvert.DeserializeObject<ResultApi>(resultString);

                    if (!_result.Success)
                        throw new ResultFixerApiException(_result.Error);

                }
                else
                {
                    throw new BadHttpRequestException("Erro ao tentar consultar Fixer Api", (int)response.StatusCode);
                }
            }
        }
    }
}
