using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.IO;
using System.Text;
using Moq;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace MarsRover.Tests
{
    //[TestFixture]

    public class Tests
    {
        StringBuilder _ConsoleOutput;
        Mock<TextReader> _ConsoleInput;

        [SetUp]
        public void Setup()
        {
            _ConsoleOutput = new StringBuilder();
            var consoleOutputWriter = new StringWriter(_ConsoleOutput);
            _ConsoleInput = new Mock<TextReader>();// (MockBehavior.Strict);
            Console.SetOut(consoleOutputWriter);
            Console.SetIn(_ConsoleInput.Object);
        }

        [Test]
        public void Test1()
        {
            SetupUserResponses("5 5", "1 2 N", "LMLMLMLMM");
            var expectedPrompt = "1 3 N";
            var outputLines = RunMainAndGetConsoleOutput();
            Assert.AreEqual(expectedPrompt, outputLines[0]);
        }

        private string[] RunMainAndGetConsoleOutput()
        {
            Program.Main(default);
            return _ConsoleOutput.ToString().Split("\r\n");
        }

        private MockSequence SetupUserResponses(params string[] userResponses)
        {
            var sequence = new MockSequence();
            foreach (var response in userResponses)
                _ConsoleInput.InSequence(sequence).Setup(x => x.ReadLine()).Returns(response);
            return sequence;
        }

    }
}