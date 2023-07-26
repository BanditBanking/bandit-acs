using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.Daemon.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bandit.ACS.Daemon.Controllers.v1
{
    [ApiController]
    [Route("admin")]
    [Produces("application/json")]
    public class AdministrationController : ControllerBase
    {

        private readonly ICardsService _cardsService;
        private readonly IAnalyticsService _analyticsService;

        public AdministrationController(ICardsService cardsService, IAnalyticsService analyticsService)
        {
            _cardsService = cardsService;
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// [Admin] Retrieves all cards associated with the specified owner
        /// </summary>
        /// <remarks>
        /// Requires a 2FA admin token
        /// </remarks>
        /// <param name="ownerId">The ID of the owner</param>
        /// <response code="200">Returns the list of cards</response>
        /// <response code="401">If the provided token isn't a 2FA Admin token or is invalid.</response>
        /// <response code="404">If no account could be found with the provided id. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#flash</response>
        [Authorize(Roles = "TwoFactorAdmin")]
        [HttpGet("/cards/owner/{ownerId}")]
        [ProducesResponseType(typeof(SessionTokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCardsByOwner([FromRoute] Guid ownerId)
        {
            return Ok(await _cardsService.GetOwnerCardsAsync(ownerId));
        }

        /// <summary>
        /// [Admin] Generates a new card for the specified owner ID
        /// </summary>
        /// <remarks>
        /// Requires a 2FA admin token
        /// </remarks>
        /// <param name="ownerId">The ID of the card owner</param>
        /// <returns>The newly generated card</returns>
        /// <response code="200">Returns the newly generated card</response>
        /// <response code="401">If the provided token isn't a 2FA Admin token or is invalid.</response>
        /// <response code="404">If no account could be found with the provided id. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#flash</response>
        [Authorize(Roles = "TwoFactorAdmin")]
        [HttpPost("/cards/generate")]
        [ProducesResponseType(typeof(Card), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GenerateCard([FromBody] Guid ownerId)
        {
            return Ok(await _cardsService.GenerateCardAsync(ownerId));
        }

        /// <summary>
        /// [Admin] Sets the balance for the specified card
        /// </summary>
        /// <remarks>
        /// Requires a 2FA admin token
        /// </remarks>
        /// <param name="cardNumber">The card number to update</param>
        /// <param name="balance">The new balance to set</param>
        /// <returns>Returns an HTTP 200 OK status code if the balance was set successfully</returns>
        /// <response code="200">Indicates that the balance was set successfully</response>
        /// <response code="401">If the provided token isn't a 2FA Admin token or is invalid.</response>
        /// <response code="404">If no card could be found with the provided id. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#flash</response>
        [Authorize(Roles = "TwoFactorAdmin")]
        [HttpPatch("/cards/balance/{cardNumber}")]
        [ProducesResponseType(typeof(Card), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetBalance([FromRoute] string cardNumber, [FromBody] double balance)
        {
            return Ok(_cardsService.SetBalanceAsync(cardNumber, balance));
        }


        /// <summary>
        /// [Admin] Manually trigger a synchronisation with the analytics server
        /// </summary>
        /// <remarks>
        /// Requires a 2FA admin token
        /// </remarks>
        /// <returns>Returns an HTTP 200 OK status code if the sync was done without errors</returns>
        /// <response code="200">Indicates that the sync was done without errors</response>
        /// <response code="401">If the provided token isn't a 2FA Admin token or is invalid.</response>
        /// <response code="503">If the analytics server is unreachable. Documentation available at: https://github.com/TristesseLOL/bandit-acs/blob/master/documentation/errors.md#eyeless</response>
        [AllowAnonymous]
        [HttpPost("/analytics/trigger")]
        [ProducesResponseType(typeof(ChallengeAnalyticsResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetailDTO), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> TriggerAnalyticsSync()
        {
            var response = await _analyticsService.SynchronizeChallengesAsync();
            return Ok(response);
        }
    }
}
