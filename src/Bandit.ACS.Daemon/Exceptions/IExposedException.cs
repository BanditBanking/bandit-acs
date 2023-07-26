using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Exceptions
{
    public interface IExposedException
    {
        ProblemDetailDTO Expose();
    }
}
