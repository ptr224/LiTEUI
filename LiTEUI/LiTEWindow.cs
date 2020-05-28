using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace LiTEUI
{
    public class LiTEWindow : Window
    {
        public static readonly DependencyProperty ActiveColorProperty = DependencyProperty.Register(nameof(ActiveColor),
            typeof(string), typeof(LiTEWindow), new FrameworkPropertyMetadata("#FF262626", FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty InactiveColorProperty = DependencyProperty.Register(nameof(InactiveColor),
            typeof(string), typeof(LiTEWindow), new FrameworkPropertyMetadata("#FF7F7F7F", FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(nameof(BackgroundColor),
            typeof(string), typeof(LiTEWindow), new FrameworkPropertyMetadata("#FFFFFFFF", FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty HighlightedColorProperty = DependencyProperty.Register(nameof(HighlightedColor),
            typeof(string), typeof(LiTEWindow), new FrameworkPropertyMetadata("#90FFFFFF", FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty WindowBaseColorProperty = DependencyProperty.Register(nameof(WindowBaseColor),
            typeof(string), typeof(LiTEWindow), new FrameworkPropertyMetadata("#FFD3D3D3", FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty WindowColorProperty = DependencyProperty.Register(nameof(WindowColor),
            typeof(string), typeof(LiTEWindow), new FrameworkPropertyMetadata("#C7D3D3D3", FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty IsToolWindowProperty = DependencyProperty.Register(nameof(IsToolWindow),
            typeof(bool), typeof(LiTEWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty IsResizableProperty = DependencyProperty.Register(nameof(IsResizable),
            typeof(bool), typeof(LiTEWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty IsFullscreenProperty = DependencyProperty.Register(nameof(IsFullscreen),
            typeof(bool), typeof(LiTEWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, FullscreenChanged));
        public static readonly DependencyProperty IsTransparentProperty = DependencyProperty.Register(nameof(IsTransparent),
            typeof(bool), typeof(LiTEWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public string ActiveColor
        {
            get => (string)GetValue(ActiveColorProperty);
            private set => SetValue(ActiveColorProperty, value);
        }

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public string InactiveColor
        {
            get => (string)GetValue(InactiveColorProperty);
            private set => SetValue(InactiveColorProperty, value);
        }

        internal string BackgroundBaseColor { get; private set; } = "#FFFFFFFF";

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public string BackgroundColor
        {
            get => (string)GetValue(BackgroundColorProperty);
            private set => SetValue(BackgroundColorProperty, value);
        }

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public string HighlightedColor
        {
            get => (string)GetValue(HighlightedColorProperty);
            private set => SetValue(HighlightedColorProperty, value);
        }

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public string WindowBaseColor
        {
            get => (string)GetValue(WindowBaseColorProperty);
            private set => SetValue(WindowBaseColorProperty, value);
        }

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public string WindowColor
        {
            get => (string)GetValue(WindowColorProperty);
            private set => SetValue(WindowColorProperty, value);
        }

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public bool IsToolWindow
        {
            get => (bool)GetValue(IsToolWindowProperty);
            set => SetValue(IsToolWindowProperty, value);
        }

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public bool IsResizable
        {
            get => (bool)GetValue(IsResizableProperty);
            set => SetValue(IsResizableProperty, value);
        }

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public bool IsFullscreen
        {
            get => (bool)GetValue(IsFullscreenProperty);
            set => SetValue(IsFullscreenProperty, value);
        }

        [Bindable(true)]
        [Category(nameof(LiTEWindow))]
        public bool IsTransparent
        {
            get => (bool)GetValue(IsTransparentProperty);
            set => SetValue(IsTransparentProperty, value);
        }

        private static void FullscreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (LiTEWindow)d;
            window.MaxHeight = (bool)e.NewValue ? double.PositiveInfinity : SystemParameters.WorkArea.Height + 8;

            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Minimized;
                window.WindowState = WindowState.Maximized;
            }
        }

        public void SetColors(Color active, Color inactive, Color background)
        {
            ActiveColor = active.ToString();
            InactiveColor = inactive.ToString();
            BackgroundBaseColor = background.ToString();

            // Sottrai da background il 20% di active
            int sr = (int)(active.R * 0.2 + background.R * 0.8);
            int sg = (int)(active.G * 0.2 + background.G * 0.8);
            int sb = (int)(active.B * 0.2 + background.B * 0.8);

            string special = $"{sr:X2}{sg:X2}{sb:X2}";

            // Usa il colore più brillante per evidenziare e il meno per lo sfondo
            var ac = System.Drawing.Color.FromArgb(255, active.R, active.G, active.B);
            var bc = System.Drawing.Color.FromArgb(255, background.R, background.G, background.B);

            if (ac.GetBrightness() > bc.GetBrightness())
            {
                BackgroundColor = "#FF" + special;
                HighlightedColor = "#90" + special;
                WindowBaseColor = "#FF" + BackgroundBaseColor.Substring(3);
                WindowColor = "#C7" + BackgroundBaseColor.Substring(3);
            }
            else
            {
                BackgroundColor = "#FF" + BackgroundBaseColor.Substring(3);
                HighlightedColor = "#90" + BackgroundBaseColor.Substring(3);
                WindowBaseColor = "#FF" + special;
                WindowColor = "#C7" + special;
            }
        }

        public LiTEWindow()
        {
            DataContext = this;
            Style = (Style)FindResource(typeof(LiTEWindow));

            // Forza aggiornamento dell'altezza massima
            IsFullscreen = false;
        }

        public override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Gestisci pressione bottoni barra
            ((Button)GetTemplateChild("maximize")).Click += (_, __) =>
                WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

            ((Button)GetTemplateChild("close")).Click += (_, __) =>
                Close();

            ((Button)GetTemplateChild("minimize")).Click += (_, __) =>
                WindowState = WindowState.Minimized;

            // Attiva l'effetto blur (su thread UI per non rallentare)
            await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                var windowHelper = new WindowInteropHelper(this);
                AcrylicHelper.EnableBlur(windowHelper.Handle);
                ContentRendered += OnContentRendered;

                void OnContentRendered(object sender, EventArgs e)
                {
                    if (SizeToContent != SizeToContent.Manual)
                    {
                        InvalidateMeasure();
                    }

                    ContentRendered -= OnContentRendered;
                }
            }));
        }
    }
}
