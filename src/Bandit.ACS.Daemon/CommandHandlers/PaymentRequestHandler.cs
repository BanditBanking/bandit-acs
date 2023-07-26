using Bandit.ACS.Clients;
using Bandit.ACS.Commands;
using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.Daemon.Services;
using Bandit.ACS.Handlers;

namespace Bandit.ACS.Daemon.CommandHandlers
{
    public class PaymentRequestHandler : ICommandHandler
    {
        private readonly ITransactionService _transactionService;

        public static Predicate<ICommand> Predicate => command => command.Type == nameof(PaymentRequestCommand);

        public PaymentRequestHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task HandleAsync(SslClient client, ICommand command, CancellationToken cancellationToken)
        {
            var parsedCommand = (PaymentRequestCommand) command;
            try
            {
                var transaction = await _transactionService.RequestPaymentAsync(parsedCommand.BankId, parsedCommand.CardNumber, parsedCommand.PaymentToken);
                await client.SendAsync(new PaymentRequestResultDTO { IsSuccess = true, Transaction = transaction });
            } catch (InvalidPaymentTokenException)
            {
                await client.SendAsync(new PaymentRequestResultDTO { IsSuccess = false });
            }
        }
    }
}
