using NUnit.Framework.Internal;

namespace Fluent.Extensions.Tests
{
    public class TestBody
    {
        public string Text { get; set; }
        public int Number { get; set; }

        public TestBody()
        {
            Text = "Text";
            Number = 100;
        }

    }
}
