namespace Routindo.Plugins.Email.Components.Actions
{
    public static class EmailSenderInstanceArgs
    {
        public const string Subject = nameof(Subject);
        public const string Body = nameof(Body);
        public const string SmtpHost = nameof(SmtpHost);
        public const string Port = nameof(Port);
        public const string Username = nameof(Username);
        public const string Password = nameof(Password);
        public const string EnableSsl = nameof(EnableSsl);
        public const string SenderDisplayName = nameof(SenderDisplayName);
        public const string SenderEmail = nameof(SenderEmail);
        public const string RecipientEmail = nameof(RecipientEmail);
        public const string AttachedFile = nameof(AttachedFile);
    }
}