using Bandit.ACS.Configuration;
using Bandit.ACS.Daemon.Configuration;
using Bandit.ACS.Daemon.Models;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Bandit.ACS.MgdbRepository.Repositories;
using Amazon.Runtime.Credentials.Internal;
using System.Security.Authentication;
using Bandit.ACS.Daemon.Mappers;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JWTConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly SigningCredentials _credentials;
        private readonly IAccountsRepository _accountsRepository;

        public static int ResourceTokenLifeSpanMinutes => 15;

        public JwtTokenService(DaemonConfiguration config, IAccountsRepository accountsRepository)
        {
            _config = config.JWT;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            _credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            _accountsRepository = accountsRepository;
        }

        public SessionTokenDTO GenerateToken(Guid userId, string mail, Role role)
        {
            IList<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, $"{userId}"),
                new Claim(ClaimTypes.Email,mail)
            };
            var expiration = DateTime.Now.AddMinutes(_config.LifeSpan);

            return new SessionTokenDTO
            {
                UserId = userId,
                Mail = mail,
                Expiration = expiration,
                Role = role.ToString(),
                Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(_config.Issuer, _config.Audience, claims, expires: expiration, signingCredentials: _credentials))
            };
        }

        public async Task<Account> GetAccountAsync(string token)
        {
            var parsedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var idClaim = parsedToken.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            if(idClaim == null)
                throw new InvalidCredentialException();
            var account = await _accountsRepository.GetByIdAsync(new Guid(idClaim.Value));
            return account.ToContract();
        }
    }
}
