namespace Bandit.ACS.Daemon.Services.Contact
{
    public record EmailData
    {
        public string ReplyToFirstName { get; set; }
        public string From { get; set; }
        public string ReplyToLastName { get; set; }
        public string ReplyToEmail { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
