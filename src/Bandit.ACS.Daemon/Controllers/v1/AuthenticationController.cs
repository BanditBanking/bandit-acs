using Bandit.ACS.Daemon.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bandit.ACS.Daemon.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using Bandit.ACS.Daemon.Models;

namespace Noa.SectorMapper.Docker.Controllers.v1;

[ApiController]
[Route("auth")]
[Produces("application/json")]
public class AuthenticationController : ControllerBase
{
    private readonly IAccountsService _accountsService;
    private readonly IChallengeService _challengeService;
    private readonly ITokenService _tokenService;

    public AuthenticationController(IAccountsService accountsService, IChallengeService challengeService, ITokenService tokenService)
    {
        _accountsService = accountsService;
        _challengeService = challengeService;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Logs in a user and returns an access token.
    /// </summary>
    /// <param name="loginDTO">The user's login credentials</param>
    /// <response code="200">Returns the access token if login was successful</response>
    /// <response code="401">If the login credentials are invalid. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#sparkle</response>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        string ip = HttpContext.Connection.RemoteIpAddress.ToString();
        var token = await _accountsService.Login(loginDTO, ip);
        return Ok(token);
    }

    /// <summary>
    /// Registers a new user and returns an access token.
    /// </summary>
    /// <param name="registerDto">The user's registration details</param>
    /// <returns>An access token if registration was successful</returns>
    /// <response code="200">Returns the access token if registration was successful</response>
    /// <response code="409">If the email provided is already registered. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#glowfish</response>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 409)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        string ip = HttpContext.Connection.RemoteIpAddress.ToString();
        var token = await _accountsService.Register(registerDto, ip);
        return Ok(token);
    }

    #region OTP

    /// <summary>
    /// Endpoint to generate a one-time password challenge for two-factor authentication.
    /// </summary>
    /// <remarks>
    /// Requires a valid single factor access token.
    /// </remarks>
    /// <response code="200">Returns the generated challenge.</response>
    /// <response code="401">When the provided token isn't a single factor token or is invalid.</response>
    [Authorize(Roles = "SingleFactorUser,SingleFactorAdmin")]
    [HttpGet("login/2fa/otp")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    public async Task<IActionResult> OTPChallengeRequest()
    {
        return Ok(await GenerateChallenge(ChallengeType.OTP));
    }

    /// <summary>
    /// Endpoint to attempt a one-time password challenge for two-factor authentication.
    /// </summary>
    /// <remarks>
    /// Requires a valid single factor access token.
    /// </remarks>
    /// <param name="attempt">The challenge attempt DTO.</param>
    /// <response code="200">Returns the result of the challenge attempt.</response>
    /// <response code="401">When the provided token isn't a single factor token or is invalid.</response>
    /// <response code="404">When no challenge could be found with the provided challenge id. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#jellyfish</response>
    /// <response code="410">Challenge already completed. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#bearer</response>
    /// <response code="429">Maximum attempts reached. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#bunny</response>
    [Authorize(Roles = "SingleFactorUser,SingleFactorAdmin")]
    [HttpPost("login/2fa/otp")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 404)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 410)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 429)]
    public async Task<IActionResult> OTPChallengeAttempt([FromBody] ChallengeAttemptDTO attempt)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var challenger = await _tokenService.GetAccountAsync(token);
        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
        var attemptResult = await _challengeService.AttemptAsync(attempt, challenger, ip);
        return Ok(attemptResult);
    }

    #endregion

    #region EID
    [Authorize(Roles = "SingleFactorUser,SingleFactorAdmin")]
    [HttpGet("login/2fa/eid")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    public async Task<IActionResult> EidChallengeRequest()
    {
        return Ok(await GenerateChallenge(ChallengeType.EID));
    }

    [AllowAnonymous]
    [HttpPost("login/2fa/eid")]
    public async Task<IActionResult> EIDChallengeAttempt([FromBody] EidChallengeAttemptDTO attempt)
    {
        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
        await _challengeService.AttemptEIDAsync(attempt, ip);
        return Ok();
    }

    [Authorize(Roles = "SingleFactorUser,SingleFactorAdmin")]
    [HttpGet("login/2fa/eid/{token}")]
    public async Task<IActionResult> CheckEID2FA([FromRoute] Guid token)
    {
        return Ok(await _challengeService.VerifyEIDChallenge(token));
    }
    #endregion

    #region Mail
    [Authorize(Roles = "SingleFactorUser,SingleFactorAdmin")]
    [HttpGet("login/2fa/mail")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    public async Task<IActionResult> MailChallengeRequest()
    {
        return Ok(await GenerateChallenge(ChallengeType.Mail));
    }

    /// <summary>
    /// Endpoint to attempt a mail based one-time password challenge for two-factor authentication.
    /// </summary>
    /// <remarks>
    /// Requires a valid single factor access token.
    /// </remarks>
    /// <param name="attempt">The challenge attempt DTO.</param>
    /// <response code="200">Returns the result of the challenge attempt.</response>
    /// <response code="401">When the provided token isn't a single factor token or is invalid.</response>
    /// <response code="404">When no challenge could be found with the provided challenge id. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#jellyfish</response>
    /// <response code="410">Challenge already completed. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#bearer</response>
    /// <response code="429">Maximum attempts reached. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#bunny</response>
    [Authorize(Roles = "SingleFactorUser,SingleFactorAdmin")]
    [HttpPost("login/2fa/mail")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 404)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 410)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 429)]
    public async Task<IActionResult> MailChallengeAttempt([FromBody] ChallengeAttemptDTO attempt)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var challenger = await _tokenService.GetAccountAsync(token);
        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
        var attemptResult = await _challengeService.AttemptSentCode(attempt, challenger, ip, ChallengeType.Mail);
        return Ok(attemptResult);
    }
    #endregion

    #region SMS
    [Authorize(Roles = "SingleFactorUser,SingleFactorAdmin")]
    [HttpGet("login/2fa/sms")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    public async Task<IActionResult> SMSChallengeRequest()
    {
        return Ok(await GenerateChallenge(ChallengeType.SMS));
    }

    /// <summary>
    /// Endpoint to attempt a mail based one-time password challenge for two-factor authentication.
    /// </summary>
    /// <remarks>
    /// Requires a valid single factor access token.
    /// </remarks>
    /// <param name="attempt">The challenge attempt DTO.</param>
    /// <response code="200">Returns the result of the challenge attempt.</response>
    /// <response code="401">When the provided token isn't a single factor token or is invalid.</response>
    /// <response code="404">When no challenge could be found with the provided challenge id. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#jellyfish</response>
    /// <response code="410">Challenge already completed. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#bearer</response>
    /// <response code="429">Maximum attempts reached. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#bunny</response>
    [Authorize(Roles = "SingleFactorUser,SingleFactorAdmin")]
    [HttpPost("login/2fa/sms")]
    [ProducesResponseType(typeof(SessionTokenDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 404)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 410)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 429)]
    public async Task<IActionResult> SmsChallengeAttempt([FromBody] ChallengeAttemptDTO attempt)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var challenger = await _tokenService.GetAccountAsync(token);
        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
        var attemptResult = await _challengeService.AttemptSentCode(attempt, challenger, ip, ChallengeType.SMS);
        return Ok(attemptResult);
    }
    #endregion

    private async Task<ChallengeDTO> GenerateChallenge(ChallengeType challengeType)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var challenger = await _tokenService.GetAccountAsync(token);
        string ip = HttpContext.Connection.RemoteIpAddress.ToString();
        return await _challengeService.GenerateChallengeAsync(challenger, ip, challengeType);
    }
}
