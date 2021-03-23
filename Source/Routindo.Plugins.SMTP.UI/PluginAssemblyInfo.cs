using Routindo.Contract;
using Routindo.Contract.Attributes;
using Routindo.Plugins.Email.Components.Actions;
using Routindo.Plugins.Email.UI.Views;

[assembly: ComponentConfigurator(typeof(EmailSenderConfigurator), EmailSender.ComponentUniqueId, "Configurator for Email Sender")]