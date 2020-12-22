using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoWindowsSettingsSwitcher;

namespace TogglePointerPrecisionWhileFPS
{
    class Program
    {
        private static string[] _targetAppName = {"r5apex", "Titanfall2"};

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Initialize();
            WaitOpenApp(_targetAppName);
            Application.Run();
        }

        private static string[] iconDot = new string[]
        {
            "................",
            ".###...##...##..",
            "..#...#..#.#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#....#..#.",
            "..#...#..#.#..#.",
            ".###...##...##..",
            "................",
        };


        private static void Initialize()
        {
            var trayIcon = new NotifyIcon();
            //var tmpIcon = MyIconUtil.Create16x16Icon(iconDot);
            trayIcon.Icon = new Icon(@"./Resources/うんちアイコン2.ico");
            trayIcon.Visible = true;

            trayIcon.Text = "Mouse Acceleration Disabler";
            var menu = new ContextMenuStrip();

            menu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("&Open", null, (s, e) => { }, "Open"),
                new ToolStripMenuItem("E&xit", null, (s, e) => { Exit(); }, "Exit")
            });

            trayIcon.DoubleClick += (s, e) => { };
            trayIcon.ContextMenuStrip = menu;
        }

        private static void Exit()
        {
            var e = new CancelEventArgs();
            Application.Exit(e);
            if (e.Cancel)
            {
                Console.WriteLine("Application.Exit is canceled");
            }
        }

        private static async Task WaitOpenApp(string[] processNames)
        {
            //var process = Process.GetProcessesByName("r5apex");
            await TaskEx.WaitUntil(() => processNames.Any(p => Process.GetProcessesByName(p).Length > 0), 1000);
            NativeMethods.ToggleEnhancePointerPrecision(false);
            await TaskEx.WaitUntil(() => processNames.Any(p => Process.GetProcessesByName(p).Length == 0), 1000);
            NativeMethods.ToggleEnhancePointerPrecision(true);
            WaitOpenApp(processNames);
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
            public static extern bool SystemParametersInfoGet(uint action, uint param, IntPtr vparam, SPIF fWinIni);

            public const UInt32 SPI_GETMOUSE = 0x0003;

            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
            public static extern bool SystemParametersInfoSet(uint action, uint param, IntPtr vparam, SPIF fWinIni);

            public const UInt32 SPI_SETMOUSE = 0x0004;

            public static bool ToggleEnhancePointerPrecision(bool b)
            {
                int[] mouseParams = new int[3];
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
}