﻿using System;
using System.Runtime.InteropServices;

namespace LiTEUI
{
    internal enum AccentFlagsType
    {
        Window = 0,
        Popup,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }

    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public uint GradientColor;
        public int AnimationId;
    }

    internal static class AcrylicHelper
    {
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        internal static void EnableBlur(IntPtr hwnd, AccentFlagsType style = AccentFlagsType.Window)
        {
            var accent = new AccentPolicy();
            var accentStructSize = Marshal.SizeOf(accent);

            switch (SystemInfo.Version.Value)
            {
                case OSVersion.W10_1903:
                    accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
                    break;
                case OSVersion.W10_1809:
                    accent.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;
                    break;
                case OSVersion.W10:
                    accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
                    break;
                case OSVersion.Old:
                    accent.AccentState = AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT;
                    break;
            }

            if (style == AccentFlagsType.Window)
            {
                accent.AccentFlags = 2;
            }
            else
            {
                accent.AccentFlags = 0x20 | 0x40 | 0x80 | 0x100;
            }

            //accent.GradientColor = 0x99FFFFFF;
            accent.GradientColor = 0x00FFFFFF;

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(hwnd, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
    }

    internal enum OSVersion { Old, W10, W10_1809, W10_1903 };

    internal class SystemInfo
    {
        public static Lazy<OSVersion> Version { get; private set; } = new Lazy<OSVersion>(() => GetVersionInfo());

        internal static OSVersion GetVersionInfo()
        {
            var regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\", false);

            if (regkey == null) return default;


            var majorValue = regkey.GetValue("CurrentMajorVersionNumber");
            var minorValue = regkey.GetValue("CurrentMinorVersionNumber");
            var buildValue = (string)regkey.GetValue("CurrentBuild", 7600);
            var canReadBuild = int.TryParse(buildValue, out var build);

            var version = majorValue is int major && minorValue is int minor && canReadBuild
                ? new Version(major, minor, build) : Environment.OSVersion.Version;

            if (version >= new Version(10, 0, 18362))
            {
                return OSVersion.W10_1903;
            }
            else if (version >= new Version(10, 0, 17763))
            {
                return OSVersion.W10_1809;
            }
            else if (version >= new Version(10, 0, 10240))
            {
                return OSVersion.W10;
            }
            else
            {
                return OSVersion.Old;
            }
        }
    }
}
