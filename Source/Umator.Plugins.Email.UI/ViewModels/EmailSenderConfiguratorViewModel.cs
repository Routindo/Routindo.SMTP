using System.Windows.Input;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using Umator.Contract;
using Umator.Contract.UI;
using Umator.Plugins.Email.Components.Actions;

namespace Umator.Plugins.Email.UI.ViewModels
{
    public sealed class EmailSenderConfiguratorViewModel: PluginConfiguratorViewModelBase
    {
        private string _subject;
        private string _body;
        private string _smtpHost= "smtp.gmail.com";
        private string _port = $"{587}";
        private string _username;
        private string _password;
        private bool _enableSsl = true;
        private string _senderDisplayName;
        private string _senderEmail;
        private string _recipientEmail;
        private string _attachedFile;

        public EmailSenderConfiguratorViewModel()
        {
            this.SelectAttachedFileCommand = new ActionCommand(SelectAttachedFile);
        }

        private void SelectAttachedFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {CheckFileExists = false, Title = "Attached File"};
            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                this.AttachedFile = openFileDialog.FileName;
            }
        }

        public ICommand SelectAttachedFileCommand { get; }

        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        public string Body
        {
            get => _body;
            set
            {
                _body = value;
                OnPropertyChanged();
            }
        }

        public string SmtpHost
        {
            get => _smtpHost;
            set
            {
                _smtpHost = value;
                ClearPropertyErrors();
                ValidateNonNullOrEmptyString(SmtpHost);
                OnPropertyChanged();
            }
        }

        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                ClearPropertyErrors();
                ValidateNonNullOrEmptyString(Port);
                ValidatePortNumber(Port);
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                ClearPropertyErrors();
                ValidateNonNullOrEmptyString(Username);
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                ClearPropertyErrors();
                ValidateNonNullOrEmptyString(Password);
                OnPropertyChanged();
            }
        }

        public bool EnableSsl
        {
            get => _enableSsl;
            set
            {
                _enableSsl = value;
                OnPropertyChanged();
            }
        }

        public string SenderDisplayName
        {
            get => _senderDisplayName;
            set
            {
                _senderDisplayName = value;
                OnPropertyChanged();
            }
        }

        public string SenderEmail
        {
            get => _senderEmail;
            set
            {
                _senderEmail = value;
                ClearPropertyErrors();
                ValidateEmail(SenderEmail);
                OnPropertyChanged();
            }
        }

        public string RecipientEmail
        {
            get => _recipientEmail;
            set
            {
                _recipientEmail = value;
                ClearPropertyErrors();
                ValidateNonNullOrEmptyString(RecipientEmail);
                ValidateEmail(RecipientEmail);
                OnPropertyChanged();
            }
        }

        public string AttachedFile
        {
            get => _attachedFile;
            set
            {
                _attachedFile = value;
                OnPropertyChanged();
            }
        }

        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                    .WithArgument(EmailSenderInstanceArgs.Subject, Subject)
                    .WithArgument(EmailSenderInstanceArgs.Body, Body)
                    .WithArgument(EmailSenderInstanceArgs.SmtpHost, SmtpHost)
                    .WithArgument(EmailSenderInstanceArgs.Port, Port)
                    .WithArgument(EmailSenderInstanceArgs.Username, Username)
                    .WithArgument(EmailSenderInstanceArgs.Password, Password)
                    .WithArgument(EmailSenderInstanceArgs.EnableSsl, EnableSsl)
                    .WithArgument(EmailSenderInstanceArgs.SenderDisplayName, SenderDisplayName)
                    .WithArgument(EmailSenderInstanceArgs.SenderEmail, SenderEmail)
                    .WithArgument(EmailSenderInstanceArgs.RecipientEmail, RecipientEmail)
                    .WithArgument(EmailSenderInstanceArgs.AttachedFile, AttachedFile);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments.HasArgument(EmailSenderInstanceArgs.Subject))
                Subject = arguments.GetValue<string>(EmailSenderInstanceArgs.Subject);

            if (arguments.HasArgument(EmailSenderInstanceArgs.Body))
                Body = arguments.GetValue<string>(EmailSenderInstanceArgs.Body);

            if (arguments.HasArgument(EmailSenderInstanceArgs.SmtpHost))
                SmtpHost = arguments.GetValue<string>(EmailSenderInstanceArgs.SmtpHost);

            if (arguments.HasArgument(EmailSenderInstanceArgs.Port))
                Port = arguments.GetValue<string>(EmailSenderInstanceArgs.Port);

            if (arguments.HasArgument(EmailSenderInstanceArgs.Username))
                Username = arguments.GetValue<string>(EmailSenderInstanceArgs.Username);

            if (arguments.HasArgument(EmailSenderInstanceArgs.Password))
                Password = arguments.GetValue<string>(EmailSenderInstanceArgs.Password);

            if (arguments.HasArgument(EmailSenderInstanceArgs.EnableSsl))
                EnableSsl = arguments.GetValue<bool>(EmailSenderInstanceArgs.EnableSsl);

            if (arguments.HasArgument(EmailSenderInstanceArgs.SenderDisplayName))
                SenderDisplayName = arguments.GetValue<string>(EmailSenderInstanceArgs.SenderDisplayName);

            if (arguments.HasArgument(EmailSenderInstanceArgs.SenderEmail))
                SenderEmail = arguments.GetValue<string>(EmailSenderInstanceArgs.SenderEmail);

            if (arguments.HasArgument(EmailSenderInstanceArgs.RecipientEmail))
                RecipientEmail = arguments.GetValue<string>(EmailSenderInstanceArgs.RecipientEmail);

            if (arguments.HasArgument(EmailSenderInstanceArgs.AttachedFile))
                AttachedFile = arguments.GetValue<string>(EmailSenderInstanceArgs.AttachedFile);
        }

        protected override void ValidateProperties()
        {
            // Smtp Host
            ClearPropertyErrors(nameof(SmtpHost));
            ValidateNonNullOrEmptyString(SmtpHost, nameof(SmtpHost));
            OnPropertyChanged(nameof(SmtpHost));

            // Port 
            ClearPropertyErrors(nameof(Port));
            ValidateNonNullOrEmptyString(Port, nameof(Port));
            ValidatePortNumber(Port, nameof(Port));
            OnPropertyChanged(nameof(Port));

            // Username 
            ClearPropertyErrors(nameof(Username));
            ValidateNonNullOrEmptyString(Username, nameof(Username));
            OnPropertyChanged(nameof(Username));

            // Password
            ClearPropertyErrors(nameof(Password));
            ValidateNonNullOrEmptyString(Password, nameof(Password));
            OnPropertyChanged(nameof(Password));

            // Sender Email
            ClearPropertyErrors(nameof(SenderEmail));
            ValidateEmail(SenderEmail, nameof(SenderEmail));
            OnPropertyChanged(nameof(SenderEmail));

            // Recipient email
            ClearPropertyErrors(nameof(RecipientEmail));
            ValidateNonNullOrEmptyString(RecipientEmail, nameof(RecipientEmail));
            ValidateEmail(RecipientEmail, nameof(RecipientEmail));
            OnPropertyChanged(nameof(RecipientEmail));
        }
    }
}
