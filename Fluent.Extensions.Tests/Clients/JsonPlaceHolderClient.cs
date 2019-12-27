using Fluent.Extensions.Http;
using Fluent.Extensions.Tests.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fluent.Extensions.Tests.Clients
{
    public class JsonPlaceHolderClient
    {
        private readonly HttpClient _http;
        private readonly ILogger _logger;

        public JsonPlaceHolderClient(ILogger logger)
        {
            _http = new HttpClient();
            _logger = logger;
        }

        public async Task<Todo[]> GetTodos()
        {
            var requestUrl = "https://jsonplaceholder.typicode.com/todos/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl)
                .WithAuthorization("Bearer", "test_auth");

            var response = await _http.SendAsync(request)
                .LogRequestAndResponseAsync(_logger)
                .EnsureSuccessStatusCodeAsync()
                .FromJsonAsync<Todo[]>();

            return response;
        }

        public async Task<Todo> GetTodo(int id)
        {
            var requestUrl = $"https://jsonplaceholder.typicode.com/todos/{id}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl)
                .WithAuthorization("Bearer", "test_auth");

            var response = await _http.SendAsync(request)
                .LogRequestAndResponseAsync(_logger)
                .EnsureSuccessStatusCodeAsync()
                .FromJsonAsync<Todo>();

            return response;
        }
    }
}
