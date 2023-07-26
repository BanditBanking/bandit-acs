using Bandit.ACS.Daemon.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Bandit.ACS.Daemon.Controllers.v1
{
    [ApiController]
    [Route("challenges")]
    [Produces("application/json")]
    public class ChallengeController : ControllerBase
    {
        /// <summary>
        /// [Debug] Computes the OTP value for the provided challenge code
        /// </summary>
        /// <remarks>
        /// This route should not be used in production
        /// </remarks>
        /// <param name="code">The challenge code</param>
        /// <param name="pin">The card pin</param>
        /// <response code="200">Returns a code that can be used to process a challenge attempt</response>
        [AllowAnonymous]
        [HttpPost("otp/compute/{pin}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> OtpChallengeCompute([FromBody] string code, [FromRoute] int pin)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{code}|{pin}"));
            int digest = BitConverter.ToInt32(hash, 0);
            return Ok(Math.Abs(digest % 100000000).ToString("D8"));
        }
    }
}
