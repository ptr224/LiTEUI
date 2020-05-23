﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LiTEUI
{
    public static class LiTEMessageBox
    {
        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.
        /// </summary>
        /// <param name="owner">A Window that represents the owner window of the message box.</param>
        /// <param name="messageBoxText">A string that specifies the text to display.</param>
        /// <param name="icon">A MessageBoxImage value that specifies the icon to display.</param>
        /// <param name="button">A MessageBoxButton value that specifies which button or buttons to display.</param>
        /// <param name="caption">A string that specifies the title bar caption to display.</param>
        public static MessageBoxResult Show(LiTEWindow owner, string messageBoxText, MessageBoxImage icon = MessageBoxImage.None, MessageBoxButton button = MessageBoxButton.OK, string caption = null)
        {
            var msg = new LiTEMessageBoxWindow(messageBoxText, caption, button, icon);

            //Imposta l'owner e se è una LiTEWindow usa stesso tema
            if (owner != null)
            {
                msg.Owner = owner;
                var oa = (Color)ColorConverter.ConvertFromString(owner.ActiveColor);
                var oi = (Color)ColorConverter.ConvertFromString(owner.InactiveColor);
                var ob = (Color)ColorConverter.ConvertFromString(owner.BackgroundBaseColor);
                msg.SetColors(oa, oi, ob);
                msg.IsTransparent = owner.IsTransparent;
            }

            //Attendi la risposta
            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A string that specifies the text to display.</param>
        /// <param name="icon">A MessageBoxImage value that specifies the icon to display.</param>
        /// <param name="button">A MessageBoxButton value that specifies which button or buttons to display.</param>
        /// <param name="caption">A string that specifies the title bar caption to display.</param>
        public static MessageBoxResult Show(string messageBoxText, MessageBoxImage icon = MessageBoxImage.None, MessageBoxButton button = MessageBoxButton.OK, string caption = null)
        {
            var msg = new LiTEMessageBoxWindow(messageBoxText, caption, button, icon);

            //Attendi la risposta
            msg.ShowDialog();

            return msg.Result;
        }
    }

    /// <summary>
    /// Logica di interazione per LiTEMessageBoxWindow.xaml
    /// </summary>
    internal partial class LiTEMessageBoxWindow : LiTEWindow
    {
        //Classe usata per ricavare il testo dei bottoni
        private class Helper
        {
            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);
            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern IntPtr LoadLibrary(string lpFileName);

            public const uint OK_CAPTION = 800;
            private const uint CANCEL_CAPTION = 801;
            private const uint YES_CAPTION = 805;
            private const uint NO_CAPTION = 806;

            public static string GetOk()
            {
                var sb = new StringBuilder(256);

                IntPtr user32 = LoadLibrary(Environment.SystemDirectory + "\\User32.dll");

                LoadString(user32, OK_CAPTION, sb, sb.Capacity);
                return sb.ToString();
            }

            public static string GetCancel()
            {
                var sb = new StringBuilder(256);

                IntPtr user32 = LoadLibrary(Environment.SystemDirectory + "\\User32.dll");

                LoadString(user32, CANCEL_CAPTION, sb, sb.Capacity);
                return sb.ToString();
            }

            public static string GetYes()
            {
                var sb = new StringBuilder(256);

                IntPtr user32 = LoadLibrary(Environment.SystemDirectory + "\\User32.dll");

                LoadString(user32, YES_CAPTION, sb, sb.Capacity);
                return sb.ToString().Substring(1);
            }

            public static string GetNo()
            {
                var sb = new StringBuilder(256);

                IntPtr user32 = LoadLibrary(Environment.SystemDirectory + "\\User32.dll");

                LoadString(user32, NO_CAPTION, sb, sb.Capacity);
                return sb.ToString().Substring(1);
            }
        }

        internal MessageBoxResult Result { get; private set; } = MessageBoxResult.Cancel;

        internal LiTEMessageBoxWindow(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            InitializeComponent();

            //Imposta testo
            Message.Text = message;
            Title = caption ?? Process.GetCurrentProcess().ProcessName;

            //Imposta bottoni
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    Button_Cancel.Visibility = Visibility.Visible;
                    Button_OK.Content = Helper.GetOk();
                    Button_Cancel.Content = Helper.GetCancel();

                    Button_OK.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    Button_Yes.Visibility = Visibility.Visible;
                    Button_No.Visibility = Visibility.Visible;
                    Button_OK.Visibility = Visibility.Collapsed;
                    Button_Yes.Content = Helper.GetYes();
                    Button_No.Content = Helper.GetNo();

                    Button_Yes.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    Button_Yes.Visibility = Visibility.Visible;
                    Button_No.Visibility = Visibility.Visible;
                    Button_Cancel.Visibility = Visibility.Visible;
                    Button_OK.Visibility = Visibility.Collapsed;
                    Button_Yes.Content = Helper.GetYes();
                    Button_No.Content = Helper.GetNo();
                    Button_Cancel.Content = Helper.GetCancel();

                    Button_Yes.Focus();
                    break;
                case MessageBoxButton.OK:
                    Button_OK.Content = Helper.GetOk();
                    Button_OK.Focus();
                    break;
            }

            //Imposta icona
            switch (icon)
            {
                case MessageBoxImage.Exclamation:       // Enumeration value 48 - also covers "Warning"
                    Image.Content = "\uE7BA";
                    break;
                case MessageBoxImage.Error:             // Enumeration value 16, also covers "Hand" and "Stop"
                    Image.Content = "\uEA39";
                    break;
                case MessageBoxImage.Information:       // Enumeration value 64 - also covers "Asterisk"
                    Image.Content = "\uE946";
                    break;
                case MessageBoxImage.Question:
                    Image.Content = "\uE9CE";
                    break;
                default:
                    return;
            }

            Image.Visibility = Visibility.Visible;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void Button_Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void Button_No_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }
    }
}
