using LiTEUI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace LiTEUI_Test
{
    /// <summary>
    /// Logica di interazione per NavPage.xaml
    /// </summary>
    [PageOptions(LaunchMode = PageLaunchMode.Normal)]
    public partial class NavPage : LiTEPage
    {
        public NavPage()
        {
            InitializeComponent();
        }

        protected override void Created(NavigationParams extras)
        {
            base.Created(extras);
            Title = extras.Get<string>("Title", "Default") + "C";
        }

        protected override void Retrieved(NavigationParams extras)
        {
            base.Retrieved(extras);
            Title = extras.Get<string>("Title", "Default") + "R";
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var me = (ListBox)sender;
            Color a;
            Color i;
            Color b;

            switch (me.SelectedIndex)
            {
                case 0:
                    a = Color.FromRgb(0x26, 0x26, 0x26);
                    i = Color.FromRgb(0x7F, 0x7F, 0x7F);
                    b = Color.FromRgb(0xFF, 0xFF, 0xFF);
                    break;
                case 1:
                    a = Color.FromRgb(0xFF, 0xFF, 0xFF);
                    i = Color.FromRgb(0x7F, 0x7F, 0x7F);
                    b = Color.FromRgb(0x00, 0x00, 0x00);
                    break;
                case 2:
                    a = Color.FromRgb(0x00, 0xFF, 0x00);
                    i = Color.FromRgb(0xFF, 0x00, 0x00);
                    b = Color.FromRgb(0x00, 0x00, 0x00);
                    break;
                case 3:
                    a = Color.FromRgb(0x00, 0xFF, 0xFF);
                    i = Color.FromRgb(0x00, 0x7F, 0x7F);
                    b = Color.FromRgb(0x00, 0x00, 0x00);
                    break;
                default:
                    return;
            }

            GetWindow()?.SetColors(a, i, b);
            //LiTEWindow.SetGlobalColors(a, i, b);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LiTEMessageBox.Show(GetWindow(), "Ciao", MessageBoxImage.Information, MessageBoxButton.YesNo);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<NavPage>(("Title", Title));
            Application.Current.Resources["WindowBarHeight"] = 40d;
            Application.Current.Resources["WindowButtonWidth"] = 40d;
            Application.Current.Resources["WindowContentMargin"] = new Thickness(10, 40, 10, 10);
            //Application.Current.Resources["WindowBarTextFontSize"] = 16d;
            //Application.Current.Resources["WindowBarSymbolFontSize"] = 14d;
        }
    }
}
