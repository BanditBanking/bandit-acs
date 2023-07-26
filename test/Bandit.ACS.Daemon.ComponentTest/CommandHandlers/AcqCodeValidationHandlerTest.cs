using Bandit.ACS.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandit.ACS.ComponentTest.CommandHandlers
{
    [TestClass]
    public class AcqCodeValidationHandlerTest
    {
        private static readonly AcsTcpClient _client = Setup.AcsTcpClient;

        [TestMethod]
        public async Task AcqCodeValidationSuccessfullyHandled()
        {
            _client.CodeValidation("1234");
        }
    }
}
