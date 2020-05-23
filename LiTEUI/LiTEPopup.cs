using System;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace LiTEUI
{
    public class LiTEPopup : Popup
    {
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            //Attiva l'effetto blur (su thread UI per non rallentare)
            _ = Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                var hwnd = (HwndSource)System.Windows.PresentationSource.FromVisual(Child);
                AcrylicHelper.EnableBlur(hwnd.Handle, AccentFlagsType.Popup);
            }));
        }
    }
}
