using Fluent.Extensions.Tests.Clients;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fluent.Extensions.Tests
{
    public class Examples
    {
        private Logger _logger;
        private JsonPlaceHolderClient _client;

        [SetUp]
        public void Setup()
        {
            _logger = new Logger();
            _client = new JsonPlaceHolderClient(_logger);
        }


        [Test]
        public async Task Example1()
        {
            var result = await _client.GetTodos();
            Assert.IsNotNull(result);
        }
    }
}
