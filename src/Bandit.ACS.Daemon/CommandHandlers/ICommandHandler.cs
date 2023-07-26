using Bandit.ACS.Clients;
using Bandit.ACS.Commands; 

namespace Bandit.ACS.Handlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(SslClient client, ICommand command, CancellationToken cancellationToken);
    }
}
