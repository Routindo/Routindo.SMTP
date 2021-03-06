using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Umator.Contract;
using Umator.Contract.Services;

namespace Umator.Plugins.Email.Components.Actions
{
    [PluginItemInfo(ComponentUniqueId, "Email Sender",
         "Send and email with custom subject / body and attachment file"),
     ExecutionArgumentsClass(typeof(EmailSenderExecutionArgs))]
    public class EmailSender : IAction
    {
        public const string ComponentUniqueId = "1984D188-3A59-4711-B601-4C98061FECD2";

        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        [Argument(EmailSenderInstanceArgs.SmtpHost, true)] public string SmtpHost { get; set; }

        [Argument(EmailSenderInstanceArgs.Port, true)] public int Port { get; set; }

        [Argument(EmailSenderInstanceArgs.Username, true)] public string Username { get; set; }

        [Argument(EmailSenderInstanceArgs.Password, true)] public string Password { get; set; }

        [Argument(EmailSenderInstanceArgs.EnableSsl, false)] public bool EnableSsl { get; set; }

        [Argument(EmailSenderInstanceArgs.SenderDisplayName, false)] public string SenderDisplayName { get; set; }

        [Argument(EmailSenderInstanceArgs.SenderEmail, true)] public string SenderEmail { get; set; }

        [Argument(EmailSenderInstanceArgs.RecipientEmail, true)] public string RecipientEmail { get; set; }

        [Argument(EmailSenderInstanceArgs.Subject, false)] public string Subject { get; set; }

        [Argument(EmailSenderInstanceArgs.Body, false)] public string Body { get; set; }
        [Argument(EmailSenderInstanceArgs.AttachedFile, false)] public string AttachedFile { get; set; }

        public ActionResult Execute(ArgumentCollection arguments)
        {
            try
            {
                if (!IsValid())
                    return new ActionResult(false, new ArgumentCollection(
                        ("ERROR", "MANDATORY_PROPERTY_MISSING")
                    ));
                if (arguments.HasArgument(EmailSenderExecutionArgs.Subject))
                {
                    this.Subject = arguments[EmailSenderExecutionArgs.Subject].ToString();
                }

                if (arguments.HasArgument(EmailSenderExecutionArgs.Body))
                {
                    this.Body = arguments[EmailSenderExecutionArgs.Body].ToString();
                }

                List<string> attachments = new List<string>();
                if (arguments.HasArgument(EmailSenderExecutionArgs.AttachedFile))
                {
                    LoggingService.Debug($"Arguments contains argument ({nameof(EmailSenderExecutionArgs.AttachedFile)})");
                    var attachmentsArgValue = arguments[EmailSenderExecutionArgs.AttachedFile];
                    if (attachmentsArgValue is string)
                    {
                        LoggingService.Debug($"Argument ({nameof(EmailSenderExecutionArgs.AttachedFile)}) is a string");
                        this.AttachedFile = attachmentsArgValue.ToString();
                    }
                    else if (attachmentsArgValue is IEnumerable<string> value)
                    {
                        LoggingService.Debug($"Argument ({nameof(EmailSenderExecutionArgs.AttachedFile)}) is a IEnumerable<string>");
                        attachments.AddRange(value);
                    }
                    else
                    {
                        LoggingService.Error($"({nameof(EmailSenderExecutionArgs.AttachedFile)}) is in type ({attachmentsArgValue.GetType()})");
                    }
                    // this.AttachedFile = arguments[EmailSenderExecutionArgs.AttachedFile].ToString();
                }


                using (SmtpClient smtpClient = new SmtpClient(this.SmtpHost))
                {
                    // Smtp 
                    smtpClient.Port = Port;
                    smtpClient.Credentials = new NetworkCredential(Username, Password);
                    smtpClient.EnableSsl = EnableSsl;

                    List<Stream> attachmentsStreams = new List<Stream>();
                    // Email Message
                    using (var mail = new MailMessage()
                    {
                        From = new MailAddress(SenderEmail, SenderDisplayName),
                        Subject = this.Subject,
                        Body = this.Body
                    })
                    {

                        mail.To.Add(RecipientEmail);

                        // Attachment 

                        if (!attachments.Any() && !string.IsNullOrWhiteSpace(AttachedFile))
                        {
                            attachments.Add(AttachedFile);
                        }

                        if (attachments.Any())
                        {
                            foreach (var attachment in attachments)
                            {
                                Stream attachmentStream = null;
                                if (!string.IsNullOrWhiteSpace(attachment) && File.Exists(attachment))
                                {
                                    string fileName = Path.GetFileName(attachment);
                                    attachmentStream = File.Open(attachment, FileMode.Open, FileAccess.Read);
                                    mail.Attachments.Add(new Attachment(attachmentStream, fileName));
                                }

                                if (attachmentStream != null)
                                    attachmentsStreams.Add(attachmentStream);
                            }
                        }

                        // Send
                        var sendingTask = smtpClient.SendMailAsync(mail);
                        sendingTask.GetAwaiter().GetResult();
                    }
                    if (attachmentsStreams.Any())
                    {
                        attachmentsStreams.ForEach(a => a?.Close());
                    }

                }
                return ActionResult.Succeeded();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return ActionResult.Failed();
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(this.SmtpHost))
                return false;

            if (string.IsNullOrWhiteSpace(this.Password))
                return false;

            if (string.IsNullOrWhiteSpace(this.RecipientEmail))
                return false;

            if (string.IsNullOrWhiteSpace(this.SenderEmail))
                return false;

            if (string.IsNullOrWhiteSpace(this.Username))
                return false;

            if (this.Port <= 0 || this.Port > 65535)
                return false;

            return true;
        }


    }
}
