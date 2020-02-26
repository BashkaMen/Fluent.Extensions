using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fluent.Extensions.Http
{
    public static class ResponseExtensions
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; }
        public static Func<HttpRequestMessage, Task<string>> RequestLogFormatter { get; set; }
        public static Func<HttpResponseMessage, Task<string>> ResponseLogFormatter { get; set; }

        static ResponseExtensions()
        {
            RequestLogFormatter = async (req) =>
            {
                var method = req.Method;
                var url = req.RequestUri.ToString();
                var body = "";

                if (req.Content != null)
                {
                    body = await req.Content.ReadAsStringAsync();
                }

                return $"HTTP Request {method} {url}\n{body}";
            };

            ResponseLogFormatter = async (res) =>
            {
                var code = res.StatusCode;
                var body = await res.Content.ReadAsStringAsync();

                return $"HTTP Response {code}\n{body}";
            };
        }

        public static async Task<HttpResponseMessage> EnsureSuccessResponseAsync(this Task<HttpResponseMessage> httpResponse, Func<HttpResponseMessage, Task<bool>> validator)
        {
            var response = await httpResponse;

            if (!await validator(response))
                throw new HttpRequestException("Response is not success");

            return response;
        }

        public static async Task<HttpResponseMessage> EnsureSuccessStatusCodeAsync(this Task<HttpResponseMessage> httpResponse)
        {
            var response = await httpResponse;

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Bad response code {response.StatusCode}:\n{body}");
            }
            

            return response;
        }

        public static async Task<T> FromJsonAsync<T>(this Task<HttpResponseMessage> httpResponse)
        {
            var response = await httpResponse;
            var body = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(body, JsonSerializerSettings);
        }

        public static async Task<HttpResponseMessage> LogRequestAsync(this Task<HttpResponseMessage> httpResponse, ILogger logger)
        {
            var response = await httpResponse;

            var log = await RequestLogFormatter(response.RequestMessage);
            logger.LogTrace(log);

            return response;
        }

        public static async Task<HttpResponseMessage> LogResponseAsync(this Task<HttpResponseMessage> responseMessage, ILogger logger)
        {
            var res = await responseMessage;

            var log = await ResponseLogFormatter(res);

            logger.LogTrace(log);

            return res;
        }

        public static async Task<HttpResponseMessage> LogRequestAndResponseAsync(this Task<HttpResponseMessage> responseMessage, ILogger logger)
        {
            return await responseMessage.LogRequestAsync(logger).LogResponseAsync(logger);
        }
    }
}
