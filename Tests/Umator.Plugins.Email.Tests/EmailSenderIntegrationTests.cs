using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using Umator.Contract;
using Umator.Plugins.Email.Components.Actions;

namespace Umator.Plugins.Email.Tests
{
    [TestClass]
    public class EmailSenderIntegrationTests
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private string _attachmentPath;

        // ReSharper disable once UnusedMember.Local
        private const string EmailAppPassword = "";

        [TestInitialize]
        public void TestInitialize()
        {
            _attachmentPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".txt");
            TestCleanUp();
            File.WriteAllText(_attachmentPath, $" [{DateTime.Now:F}] This is a sample file");
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            try
            {
                if (File.Exists(_attachmentPath))
                    File.Delete(_attachmentPath);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        [TestMethod]
        public void SendEmailSuccessfulTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "Younes.Cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
                AttachedFile = _attachmentPath
            };

            var result = emailSenderAction.Execute(new ArgumentCollection());
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public void SendEmailSuccessfulAttachedListFilesArgumentsTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "Younes.Cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
            };

            var result = emailSenderAction.Execute(new ArgumentCollection()
            {
                new Argument(EmailSenderExecutionArgs.AttachedFile, new List<string>
                {
                    _attachmentPath
                })
            });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }


        [TestMethod]
        public void SendEmailSuccessfulAttachedSingleFileArgumentsTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "Younes.Cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
            };

            var result = emailSenderAction.Execute(new ArgumentCollection()
            {
                new Argument(EmailSenderExecutionArgs.AttachedFile, _attachmentPath)
            });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public void SendEmailFailsOnMissingSmtpHostTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                // SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "younes.cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
                AttachedFile = _attachmentPath
            };

            var result = emailSenderAction.Execute(new ArgumentCollection());
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
            Assert.IsNotNull(result.AdditionalInformation);
            Assert.IsTrue(result.AdditionalInformation.HasArgument("ERROR"));
        }

        [TestMethod]
        public void SendEmailFailsOnMissingPortTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                // Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "younes.cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
                AttachedFile = _attachmentPath
            };

            var result = emailSenderAction.Execute(new ArgumentCollection());
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
            Assert.IsNotNull(result.AdditionalInformation);
            Assert.IsTrue(result.AdditionalInformation.HasArgument("ERROR"));
        }

        [TestMethod]
        public void SendEmailFailsOnMissingPasswordTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                // Password = EMAIL_APP_PASSWORD,
                SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "younes.cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
                AttachedFile = _attachmentPath
            };

            var result = emailSenderAction.Execute(new ArgumentCollection());
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
            Assert.IsNotNull(result.AdditionalInformation);
            Assert.IsTrue(result.AdditionalInformation.HasArgument("ERROR"));
        }

        [TestMethod]
        public void SendEmailFailsOnMissingReceiptEmailTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                SenderEmail = "umator.software@gmail.com",
                // RecipientEmail = "younes.cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
                AttachedFile = _attachmentPath
            };

            var result = emailSenderAction.Execute(new ArgumentCollection());
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
            Assert.IsNotNull(result.AdditionalInformation);
            Assert.IsTrue(result.AdditionalInformation.HasArgument("ERROR"));
        }

        [TestMethod]
        public void SendEmailFailsOnMissingUsernameTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "younes.cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                // Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
                AttachedFile = _attachmentPath
            };

            var result = emailSenderAction.Execute(new ArgumentCollection());
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
            Assert.IsNotNull(result.AdditionalInformation);
            Assert.IsTrue(result.AdditionalInformation.HasArgument("ERROR"));
        }

        [TestMethod]
        public void SendEmailFailsOnMissingSenderEmailTest()
        {

            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                // SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "younes.cheikh@gmail.com",
                SenderDisplayName = "Umator Software",
                Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                Body = "This is a message sent from Integration testing",
                AttachedFile = _attachmentPath
            };

            var result = emailSenderAction.Execute(new ArgumentCollection());
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
            Assert.IsNotNull(result.AdditionalInformation);
            Assert.IsTrue(result.AdditionalInformation.HasArgument("ERROR"));
        }

        [TestMethod]
        public void SendEmailSucceedOnMissingOptionPropertiesTest()
        {
            _logger.Debug("Sending without optional emails");
            IAction emailSenderAction = new EmailSender()
            {
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Password = EmailAppPassword,
                SenderEmail = "umator.software@gmail.com",
                RecipientEmail = "younes.cheikh@gmail.com",
                // SenderDisplayName = "Umator Software",
                // Subject = "Hello world!",
                Username = "umator.software@gmail.com",
                // Body = "This is a message sent from Integration testing",
                // AttachedFile = _attachmentPath
            };

            var result = emailSenderAction.Execute(new ArgumentCollection());
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }


    }
}
