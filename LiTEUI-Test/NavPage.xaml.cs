using LiTEUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                    a = (Color)ColorConverter.ConvertFromString("#FF262626");
                    i = (Color)ColorConverter.ConvertFromString("#FF7F7F7F");
                    b = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
                    break;
                case 1:
                    a = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
                    i = (Color)ColorConverter.ConvertFromString("#FF7F7F7F");
                    b = (Color)ColorConverter.ConvertFromString("#FF000000");
                    break;
                case 2:
                    a = (Color)ColorConverter.ConvertFromString("#FF00FF00");
                    i = (Color)ColorConverter.ConvertFromString("#FFFF0000");
                    b = (Color)ColorConverter.ConvertFromString("#FF000000");
                    break;
                case 3:
                    a = (Color)ColorConverter.ConvertFromString("#FF00FFFF");
                    i = (Color)ColorConverter.ConvertFromString("#FF007F7F");
                    b = (Color)ColorConverter.ConvertFromString("#FF000000");
                    break;
                default:
                    return;
            }

            GetWindow()?.SetColors(a, i, b);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LiTEMessageBox.Show(GetWindow(), "Ciao", MessageBoxImage.Information, MessageBoxButton.YesNo);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<NavPage>(("Title", Title));
        }
    }
}
