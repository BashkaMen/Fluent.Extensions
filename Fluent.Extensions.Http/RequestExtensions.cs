using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Fluent.Extensions.Http
{
    public static class RequestExtensions
    {
        public static HttpRequestMessage WithAuthorization(this HttpRequestMessage request, string scheme, string value)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, value);
            return request;
        }

        public static HttpRequestMessage WithBody<T>(this HttpRequestMessage request, T body)
        {
            var json = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(json);

            return request;
        }
    }
}
