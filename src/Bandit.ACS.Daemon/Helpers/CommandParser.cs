using Bandit.ACS.Commands;
using Bandit.ACS.Exceptions;
using System.Text.Json;

namespace Bandit.ACS.Daemon.Helpers
{
    public class CommandParser : ICommandParser
    {

        public ICommand Parse(string rawCommand)
        {
            var commandType = JsonSerializer.Deserialize<RawCommand>(rawCommand)?.Type;
            ICommand? command = commandType switch
            {
                nameof(PaymentRequestCommand) => JsonSerializer.Deserialize<PaymentRequestCommand>(rawCommand),
                _ => throw new UnknownCommandException($"Command \"{rawCommand}\" not recognized")
            };
            return command;
        }
    }
}
