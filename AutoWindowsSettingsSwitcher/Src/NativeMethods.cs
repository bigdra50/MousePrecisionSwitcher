using System;
using System.Runtime.InteropServices;

namespace MousePrecisionSwitcher
{
    [Flags]
    public enum SPIF
    {
        None = 0x00,

        /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
        SPIF_UPDATEINIFILE = 0x01,

        /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
        SPIF_SENDCHANGE = 0x02,

        /// <summary>Same as SPIF_SENDCHANGE.</summary>
        SPIF_SENDWININICHANGE = 0x02
    }

    public static class NativeMethods
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoGet(uint action, uint param, IntPtr vparam, SPIF fWinIni);

        public const UInt32 SPI_GETMOUSE = 0x0003;

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoSet(uint action, uint param, IntPtr vparam, SPIF fWinIni);

        public const UInt32 SPI_SETMOUSE = 0x0004;

        public static bool ToggleEnhancePointerPrecision(bool b)
        {
            var mouseParams = new int[3];
            // Get the current values.
            SystemParametersInfoGet(SPI_GETMOUSE, 0,
                GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), 0);
            // Modify the acceleration value as directed.
            mouseParams[2] = b ? 1 : 0;
            // Update the system setting.
            return SystemParametersInfoSet(SPI_SETMOUSE, 0,
                GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), SPIF.SPIF_SENDCHANGE);
        }
    }
}