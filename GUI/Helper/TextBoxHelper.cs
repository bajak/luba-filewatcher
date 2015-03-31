using System;
using System.Windows;
using System.Windows.Controls;

namespace Filewatcher.GUI
{
    public class TextBoxHelper
    {
        public static readonly DependencyProperty AlwaysScrollToEndProperty = 
            DependencyProperty.RegisterAttached(
            "AlwaysScrollToEnd", typeof(bool), typeof(TextBoxHelper),
                new PropertyMetadata(false, AlwaysScrollToEndChanged));

        private static void AlwaysScrollToEndChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null)
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property" +
                                                    "can only be applied to TextBox instances.");
                
            var alwaysScrollToEnd = (e.NewValue != null) && (bool) e.NewValue;
            if (alwaysScrollToEnd)
            {
                tb.ScrollToEnd();
                tb.TextChanged += TextChanged;
            }
            else
                tb.TextChanged -= TextChanged;
        }

        public static bool GetAlwaysScrollToEnd(TextBox textBox)
        {
            if (textBox == null) 
                throw new ArgumentNullException("textBox");
            return (bool)textBox.GetValue(AlwaysScrollToEndProperty);
        }

        public static void SetAlwaysScrollToEnd(TextBox textBox, bool alwaysScrollToEnd)
        {
            if (textBox == null)
                throw new ArgumentNullException("textBox");
            textBox.SetValue(AlwaysScrollToEndProperty, alwaysScrollToEnd);
        }

        private static void TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).ScrollToEnd();
        }
    }
}