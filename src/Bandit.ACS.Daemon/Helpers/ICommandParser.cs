using Bandit.ACS.Commands;

namespace Bandit.ACS.Daemon.Helpers
{
    public interface ICommandParser
    {
        ICommand Parse(string rawCommand);
    }
}
