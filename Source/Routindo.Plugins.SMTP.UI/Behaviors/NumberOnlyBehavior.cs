﻿#region using statements

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace Routindo.Plugins.SMTP.UI.Behaviors
{
    /// <summary>
    ///     Exposes attached behaviors that can be
    ///     applied to Control objects.
    /// </summary>
    public static class NumberOnlyBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
                DependencyProperty.RegisterAttached("IsEnabled", typeof(bool),
                typeof(NumberOnlyBehavior), new UIPropertyMetadata(false, OnValueChanged));

        public static bool GetIsEnabled(Control o) { return (bool)o.GetValue(IsEnabledProperty); }

        public static void SetIsEnabled(Control o, bool value) { o.SetValue(IsEnabledProperty, value); }

        private static void OnValueChanged(DependencyObject dependencyObject,
                DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is Control uiElement)) return;
            if (e.NewValue is bool boolean && boolean)
            {
                uiElement.PreviewTextInput += OnTextInput;
                uiElement.PreviewKeyDown += OnPreviewKeyDown;
                DataObject.AddPastingHandler(uiElement, OnPaste);
            }

            else
            {
                uiElement.PreviewTextInput -= OnTextInput;
                uiElement.PreviewKeyDown -= OnPreviewKeyDown;
                DataObject.RemovePastingHandler(uiElement, OnPaste);
            }
        }

        private static void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Any(c => !char.IsDigit(c))) { e.Handled = true; }
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = Convert.ToString(e.DataObject.GetData(DataFormats.Text)).Trim();
                if (text.Any(c => !char.IsDigit(c))) { e.CancelCommand(); }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}