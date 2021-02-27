using Umator.Contract;
using Umator.Plugins.Email.Components.Actions;
using Umator.Plugins.Email.UI.Views;

[assembly: ComponentConfigurator(typeof(EmailSenderConfigurator), EmailSender.ComponentUniqueId, "Configurator for Email Sender")]