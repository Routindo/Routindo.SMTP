using Routindo.Contract;
using Routindo.Contract.Attributes;
using Routindo.Plugins.SMTP.Components.Actions;
using Routindo.Plugins.SMTP.UI.Views;

[assembly: ComponentConfigurator(typeof(EmailSenderConfigurator), EmailSender.ComponentUniqueId, "Configurator for Email Sender")]