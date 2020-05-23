using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LiTEUI
{
    /// <summary>
    /// Una speciale <see cref="LiTEWindow"/> che contiene e naviga tra delle <see cref="LiTEPage"/>.
    /// </summary>
    public class LiTENavigationWindow : LiTEWindow
    {
        public static readonly DependencyProperty NavigationButtonProperty = DependencyProperty.Register(nameof(NavigationButton),
            typeof(bool), typeof(LiTENavigationWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty StartupPageProperty = DependencyProperty.Register(nameof(StartupPage),
            typeof(Type), typeof(LiTENavigationWindow), new FrameworkPropertyMetadata(null));

        internal Button Back;
        internal Label PageTitle;

        public NavigationService NavigationService { get; }

        [Bindable(true)]
        [Category(nameof(LiTENavigationWindow))]
        public bool NavigationButton
        {
            get => (bool)GetValue(NavigationButtonProperty);
            set => SetValue(NavigationButtonProperty, value);
        }

        [Bindable(false)]
        [Category(nameof(LiTENavigationWindow))]
        public Type StartupPage
        {
            get => (Type)GetValue(StartupPageProperty);
            set => SetValue(StartupPageProperty, value);
        }

        public LiTENavigationWindow()
        {
            NavigationService = new NavigationService(this);
            Style = (Style)FindResource(typeof(LiTENavigationWindow));

            Closing += LiTENavigationWindow_Closing;
            Closed += LiTENavigationWindow_Closed;
        }

        private void LiTENavigationWindow_Closing(object sender, CancelEventArgs e)
        {
            // Se la pagina corrente non va chiusa non chiuderti
            e.Cancel = NavigationService.CancelClosing();
        }

        private void LiTENavigationWindow_Closed(object sender, EventArgs e)
        {
            // Disponi le pagine ancora caricate
            NavigationService.Dispose();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Salva riferimenti e gestisci pressione indietro
            Back = (Button)GetTemplateChild("back");
            PageTitle = (Label)GetTemplateChild("title");

            Back.Click += BackButton_Click;

            if (StartupPage != null)
                NavigationService.Navigate(StartupPage);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
