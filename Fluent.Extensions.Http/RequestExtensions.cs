using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Fluent.Extensions.Http
{
    public static class RequestExtensions
    {
        public static HttpRequestMessage WithAuthorization(this HttpRequestMessage request, string scheme, string value)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, value);
            return request;
        }

        public static HttpRequestMessage WithJsonBody<T>(this HttpRequestMessage request, T body)
        {
            var json = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return request;
        }
    }
}
