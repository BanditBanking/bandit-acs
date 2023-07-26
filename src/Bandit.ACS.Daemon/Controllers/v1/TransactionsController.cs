using Bandit.ACS.Daemon.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.Daemon.Models;

namespace Noa.SectorMapper.Docker.Controllers.v1;

[ApiController]
[Route("transactions")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITokenService tokenService, ITransactionService transactionService)
    {
        _tokenService = tokenService;
        _transactionService = transactionService;
    }

    /// <summary>
    /// Make a transaction request
    /// </summary>
    /// <remarks>
    /// Requires a 2FA token
    /// </remarks>
    /// <param name="request">The transaction request details</param>
    /// <returns>Returns an HTTP 200 OK status code if the transaction request was successful, along with the transaction token to be given to the merchant</returns>
    /// <response code="200">Indicates that the transaction request was successful, along with the transaction code</response>
    /// <response code="401">If the provided token isn't a 2FA token or is invalid.</response>
    /// <response code="402">If the balance is insufficient to proceed the transaction</response>
    [Authorize(Roles = "TwoFactorUser,TwoFactorAdmin")]
    [HttpPost("request")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status402PaymentRequired)]
    public async Task<IActionResult> TransactionRequest([FromBody] TransactionRequestDTO request)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var challenger = await _tokenService.GetAccountAsync(token);
        string ip = HttpContext.Connection.RemoteIpAddress.ToString();
        var transactionCode = await _transactionService.RequestTransactionAsync(challenger, request, ip);
        return Ok(transactionCode);
    }

    /// <summary>
    /// Get all the transactions made for the current account
    /// </summary>
    /// <remarks>
    /// Requires a 2FA token
    /// </remarks>
    /// <returns>Returns the list of user's transactions</returns>
    /// <response code="200">The transactions made by the user</response>
    /// <response code="401">If the provided token isn't a 2FA token or is invalid.</response>
    [Authorize(Roles = "TwoFactorUser,TwoFactorAdmin")]
    [HttpGet]
    [ProducesResponseType(typeof(List<Transaction>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTransactions()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var challenger = await _tokenService.GetAccountAsync(token);
        var transactions = await _transactionService.GetTransactionsByAccountId(challenger.Id);
        return Ok(transactions);
    }
}
