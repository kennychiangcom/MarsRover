using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.IO;
using System.Text;
using Moq;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace MarsRover.Tests
{
    public class Tests
    {
        StringBuilder _ConsoleOutput;
        Mock<TextReader> _ConsoleInput;

        [SetUp]
        public void Setup()
        {
            _ConsoleOutput = new StringBuilder();
            var consoleOutputWriter = new StringWriter(_ConsoleOutput);
            _ConsoleInput = new Mock<TextReader>();
            Console.SetOut(consoleOutputWriter);
            Console.SetIn(_ConsoleInput.Object);
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

        [Test]
        public void Test_Console()
        {
            SetupUserResponses("5 5", "1 2 N", "LMLMLMLMM");
            var expectedPrompt = "1 3 N";
            var outputLines = RunMainAndGetConsoleOutput();
            Assert.That(outputLines[2], Is.EqualTo(expectedPrompt));
        }

        [Test]
        public void Test_Set_Plateau()
        {
            var _NavSys = new NavSys();
            _NavSys.SetBoundry(5, 5);
            int[] testboundary = _NavSys.GetBoundry();
            int[] expectedBoundary = new int[] { 0, 0, 5, 5 };
            Assert.That(testboundary, Is.EqualTo(expectedBoundary));
        }

        [Test]
        public void Test_Deploy_Rover()
        {
            var _MarsRover = new MarsRover();
            _MarsRover.Deploy(1, 2, 'N');
            object[] testrover = _MarsRover.Detect();
            object[] expectedRover = new object[] { 1, 2, 'N'};
            Assert.That(testrover, Is.EqualTo(expectedRover));
        }

        [Test]
        public void Test_Move_Rover()
        {
            var _MarsRover = new MarsRover();
            _MarsRover.Deploy(1, 2, 'N');
            _MarsRover.Move("LMLMLMLMM");
            object[] testrover = _MarsRover.Detect();
            object[] expectedRover = new object[] { 1, 3, 'N' };
            Assert.That(testrover, Is.EqualTo(expectedRover));
        }
    }
}