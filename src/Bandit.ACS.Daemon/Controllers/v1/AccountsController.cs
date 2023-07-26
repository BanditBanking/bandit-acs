using Bandit.ACS.Daemon.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Bandit.ACS.Daemon.Models.DTOs;
using Microsoft.Extensions.Logging.Abstractions;

namespace Noa.SectorMapper.Docker.Controllers.v1;

[ApiController]
[Route("accounts")]
[Produces("application/json")]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;
    private readonly ITokenService _tokenService;
    public NullLogger<AccountsController> Logger;

    public AccountsController(IAccountsService accountsService, ITokenService tokenService)
    {
        _accountsService = accountsService;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get the profile of the connected user.
    /// </summary>
    /// <returns>The profile of the connected user</returns>
    /// <response code="200">Returns a user profile</response>
    /// <response code="401">When the provided token isn't a 2FA token or is invalid</response>
    [Authorize(Roles = "TwoFactorUser,TwoFactorAdmin")]
    [HttpGet("profile")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ProfileDTO), 200)]
    [ProducesResponseType(typeof(ProblemDetailDTO), 401)]
    public async Task<IActionResult> GetProfile()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var challenger = await _tokenService.GetAccountAsync(token);
        var profile = await _accountsService.GetProfile(challenger.Id);
        return Ok(profile);
    }
}
