using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Exceptions
{
    [Serializable]
    public class AnalyticsServerUnreachableException : Exception, IExposedException
    {
        private string _address;
        private int _port;

        public AnalyticsServerUnreachableException() { }

        public AnalyticsServerUnreachableException(string address, int port) : base($"Analytics server unreachable ({address}:{port})") 
        {
            _address = address;
            _port = port;
        }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status503ServiceUnavailable,
            ErrorCode = "eyeless",
            Title = "Analytics server unreachable",
            Detail = $"Can't reach analytics server ({_address}:{_port})"
        };
    }
}
