using Bandit.ACS.Configuration;
using Bandit.ACS.Daemon.Configuration;
using Microsoft.AspNetCore.Authentication;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Bandit.ACS.Daemon.Services.Contact
{
    public class TwilioSMSSender : ISMSSender
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly TwilioConfiguration _config;
        public TwilioSMSSender(IAuthenticationService authenticationService, DaemonConfiguration config)
        {
            _authenticationService = authenticationService;
            _config = config.Twilio;
            TwilioClient.Init(config.Twilio.AccountSid, config.Twilio.AuthToken);
        }

        public async Task SendSMS(string number, string content)
        {
            var messageOptions = new CreateMessageOptions(new PhoneNumber(number));
            messageOptions.MessagingServiceSid = _config.MessagingServiceSid;
            messageOptions.Body = content;
            MessageResource.Create(messageOptions);
        }
    }
}
