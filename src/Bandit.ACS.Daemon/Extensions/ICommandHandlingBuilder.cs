using Bandit.ACS.Commands;
using Bandit.ACS.Handlers;

namespace Bandit.ACS.Daemon.Extensions
{
    public interface ICommandHandlingBuilder
    {
        void AddHandler<T>(Predicate<ICommand> predicate) where T : class, ICommandHandler;
    }
}
