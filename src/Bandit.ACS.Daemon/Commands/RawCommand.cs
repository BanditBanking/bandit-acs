namespace Bandit.ACS.Commands
{
    public class RawCommand : ICommand
    {
        public string Type { get; set; } = nameof(RawCommand);
    }
}
