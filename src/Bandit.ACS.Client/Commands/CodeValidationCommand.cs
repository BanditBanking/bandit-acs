using Bandit.ACS.Client.Commands;

namespace Bandit.ACS.Commands
{
    public class CodeValidationCommand : ICommand
    {
        public string Type { get; set; } = nameof(CodeValidationCommand);
        public string? Code { get; set; }
    }
}
