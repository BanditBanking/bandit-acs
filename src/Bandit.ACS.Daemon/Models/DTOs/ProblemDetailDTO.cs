using System.Net;

namespace Bandit.ACS.Daemon.Models.DTOs
{
    // This DTO aims to satisfy the RFC 7807 described here : https://www.rfc-editor.org/rfc/rfc7807
    public class ProblemDetailDTO
    {

        public int HttpCode { get; set; }
        public string ErrorCode { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }

        public static ProblemDetailDTO Default = new()
        {
            HttpCode = StatusCodes.Status500InternalServerError,
            ErrorCode = "ghost",
            Title = "An unknown exception occured",
            Detail = "This exception was not handled by the server"
        };

    }
}
