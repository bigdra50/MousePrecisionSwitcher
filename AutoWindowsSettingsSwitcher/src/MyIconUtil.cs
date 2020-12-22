using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoWindowsSettingsSwitcher
{
    static class MyIconUtil
    {
        static class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public extern static bool DestroyIcon(IntPtr handle);
        }

        public static Icon Create16x16Icon(string[] iconDot)
        {
            var bmp = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }

            for (var y = 0; y < 16; y++)
            {
                for (var x = 0; x < 16; x++)
                {
                    if (iconDot[y][x] == '#')
                        bmp.SetPixel(x, y, Color.Black);
                }
            }

            var hicon = bmp.GetHicon();
            return Icon.FromHandle(hicon);
        }

        public static void DestroyIcon(Icon icon)
        {
            NativeMethods.DestroyIcon(icon.Handle);
        }
    }

}