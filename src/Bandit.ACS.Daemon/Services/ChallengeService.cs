using Bandit.ACS.Configuration;
using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Mappers;
using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.Daemon.Services.Contact;
using Bandit.ACS.Daemon.Services.EidChecking;
using Bandit.ACS.Daemon.Mappers;
using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.Exceptions;
using Bandit.ACS.MgdbRepository.Exceptions;
using Bandit.ACS.MgdbRepository.Models;
using Bandit.ACS.MgdbRepository.Repositories;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using Account = Bandit.ACS.Daemon.Models.Account;
using ChallengeDTO = Bandit.ACS.Daemon.Models.DTOs.ChallengeDTO;
using ChallengeType = Bandit.ACS.Daemon.Models.ChallengeType;

namespace Bandit.ACS.Daemon.Services
{
    public class ChallengeService : IChallengeService
    {
        private DaemonConfiguration _config;
        private IChallengeRepository _challengeRepository;
        private ICardsRepository _cardsRepository;
        private ITokenService _tokenService;
        private IAccountsService _accountsService;
        private IAccountsRepository _accountsRepository;
        private ICertificateChecker _certificateChecker;
        private readonly IMailSender _mailSender;
        private readonly ISMSSender _smsSender;

        public ChallengeService(DaemonConfiguration config, IChallengeRepository challengeRepository, ICardsRepository cardsRepository, 
            ITokenService tokenService, IAccountsService accountsService, IAccountsRepository accountsRepository, ICertificateChecker certificateChecker,
            ISMSSender smsSender, IMailSender mailSender)
        {
            _config = config;
            _challengeRepository = challengeRepository;
            _cardsRepository = cardsRepository;
            _tokenService = tokenService;
            _accountsService = accountsService;
            _accountsRepository = accountsRepository;
            _certificateChecker = certificateChecker;
            _smsSender = smsSender;
            _mailSender = mailSender;
        }

        public async Task<AttemptResult> AttemptAsync(ChallengeAttemptDTO attempt, Account challenger, string ip)
        {
            try
            {
                var responseTime = DateTime.Now;
                var challenge = await _challengeRepository.GetByIdAsync(attempt.Id);
                if (challenge.RemainingAttempts == 0) throw new MaxAttemptsReachedException(ChallengeType.OTP, 3);
                if (challenge.IsSuccess) throw new ChallengeAlreadyCompletedException(challenge.ChallengeId);

                var card = await _cardsRepository.GetByCardNumberAsync(challenge.CardNumber);
                var expectedOTP = GenerateOTP($"{challenge.OTP}|{card.PinCode}");
                challenge.RemainingAttempts -= 1;
                challenge.ResponseTime = responseTime;
                challenge.DecisionTime = DateTime.Now;

                return await UpdateChallenge(challenge, challenger, expectedOTP.Equals(attempt.Code));
            }
            catch (ResourceNotFoundException)
            {
                throw new InvalidChallengeIdException(attempt.Id);
            }
        }
        public async Task<AttemptResult> AttemptSentCode(ChallengeAttemptDTO attempt, Account challenger, string ip, ChallengeType type)
        {
            try
            {
                var responseTime = DateTime.Now;
                var challenge = await _challengeRepository.GetByIdAsync(attempt.Id);
                
                if (challenge.RemainingAttempts == 0) 
                    throw new MaxAttemptsReachedException(type, 3);
                if (challenge.IsSuccess) 
                    throw new ChallengeAlreadyCompletedException(challenge.ChallengeId);

                challenge.RemainingAttempts -= 1;
                challenge.ResponseTime = responseTime;
                challenge.DecisionTime = DateTime.Now;

                return await UpdateChallenge(challenge, challenger, challenge.OTP.Equals(attempt.Code));
            }
            catch (ResourceNotFoundException)
            {
                throw new InvalidChallengeIdException(attempt.Id);
            }
        }

        private async Task<AttemptResult> UpdateChallenge(Challenge challenge, Account challenger, bool success)
        {
            if(!success)
            {
                await _challengeRepository.UpdateChallengeAsync(challenge);
                return new() { Id = challenge.ChallengeId, IsSuccess = false, RemainingAttempts = challenge.RemainingAttempts };
            }
            else
            {
                challenge.IsSuccess = true;
                await _challengeRepository.UpdateChallengeAsync(challenge);
                var token = _tokenService.GenerateToken(challenger.Id, challenger.Mail, challenger.IsAdmin ? Role.TwoFactorAdmin : Role.TwoFactorUser);
                return new() { Id = challenge.ChallengeId, IsSuccess = true, SessionToken = token };
            }
        }

        private (string FirstName, string LastName) ExtractNameFromCertificateCN(string cn)
        {
            string pattern = @"CN=([^\s]+)\s+([^\s]+)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(cn);
            if (match.Success)
            {
                string firstName = match.Groups[1].Value;
                string lastName = match.Groups[2].Value;
                return (firstName, lastName);
            }

            return (string.Empty, string.Empty);
        }

        public async Task AttemptEIDAsync(EidChallengeAttemptDTO attempt, string ip)
        {
            var certBytes = Convert.FromBase64String(attempt.Cert);
            var cert = new X509Certificate2(certBytes);
            var validCertificate = _certificateChecker.CheckCertificateAuthenticity(cert);

            if (!validCertificate)
                throw new InvalidEidCertificateException();

            var algorithm = new EidCheckingFactory().Create(attempt.Hash);

            if (algorithm == null)
                throw new InvalidAlgorithmException();

            var challenge = await _challengeRepository.GetByIdAsync(attempt.Token);
            if (challenge.IsSuccess)
                throw new ResourceNotFoundException();

            var (firstName, lastName) = ExtractNameFromCertificateCN(cert.GetName());
            var account = await _accountsService.GetByFullName(firstName, lastName);

            if (account == null || account.Id != challenge.ChallengerId)
                throw new InvalidEidCertificateException();

            var response = algorithm.Check(cert, attempt.Token.ToString(), attempt.TokenSignature);

            if(!response)
                throw new InvalidEidCertificateException();

            await UpdateChallenge(challenge, account, true);
        }

        public async Task<AttemptResult> VerifyEIDChallenge(Guid challengeId)
        {
            var challenge = await _challengeRepository.GetByIdAsync(challengeId);
            var challenger = await _accountsRepository.GetByIdAsync(challenge.ChallengerId);

            if(challenger != null && 
                challenge.RequestTime.AddMinutes(5) > DateTime.Now && 
                challenge.IsSuccess)
            {
                var token = _tokenService.GenerateToken(challenge.ChallengerId, challenger.Mail, challenger.IsAdmin ? Role.TwoFactorAdmin : Role.TwoFactorUser);
                return new() { Id = challenge.ChallengeId, IsSuccess = true, SessionToken = token };
            }

            return new() { Id = challenge.ChallengeId, IsSuccess = false, RemainingAttempts = 1 };
        }
        public async Task<ChallengeDTO> GenerateChallengeAsync(Account challenger, string challengerIp, ChallengeType type)
        {
            var card = await _cardsRepository.GetCardByOwnerAsync(challenger.Id);
            var time = DateTime.Now;
            var key = $"{challenger.Id}{card.CardNumber}{time}";
            var challenge = new Challenge
            {
                ChallengeId = Guid.NewGuid(),
                ChallengeType = (MgdbRepository.Models.ChallengeType)type,
                BankId = _config.BankIdentifier,
                ClientName = $"{challenger.FirstName} {challenger.LastName}",
                CardNumber = card.CardNumber,
                ChallengerId = challenger.Id,
                ChallengerBirthDay = challenger.BirthDay,
                ChallengerAge = challenger.Age,
                ChallengerGender = challenger.Gender,
                ChallengerIp = challengerIp,
                RequestTime = time,
                OTP = GenerateOTP(key, time),
                ServerIp = "217.182.70.119",
                RemainingAttempts = 3
            };

            await _challengeRepository.AddChallengeAsync(challenge);

            if(type == ChallengeType.SMS)
            {
                await _smsSender.SendSMS(challenger.PhoneNumber, $"Verification code:\r\n{challenge.OTP}");
            }
            else if(type == ChallengeType.Mail)
            {
                await _mailSender.SendMail(challenger.Mail, "Verification code", challenge.OTP);
            }
            return challenge.ToContract();
        }

        private string GenerateOTP(string key, DateTime time)
        {
            string iso8601DateTime = time.ToString("O");
            return GenerateOTP($"{key}|{iso8601DateTime}");
        }

        private string GenerateOTP(string key)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            int digest = BitConverter.ToInt32(hash, 0);
            return Math.Abs(digest % 100000000).ToString("D8");
        }
    }
}
