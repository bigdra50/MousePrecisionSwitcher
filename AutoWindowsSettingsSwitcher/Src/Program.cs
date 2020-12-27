using System;
using System.Diagnostics;
using System.Linq;
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
            var taskTray = new TaskTray();
            WaitOpenAppAsync(_targetAppName);
            Application.Run();
        }

        private static async Task WaitOpenAppAsync(string[] processNames)
        {
            await TaskEx.WaitUntil(() => processNames.Any(p => Process.GetProcessesByName(p).Length > 0), 1000);
            NativeMethods.ToggleEnhancePointerPrecision(false);
            await TaskEx.WaitUntil(() => processNames.Any(p => Process.GetProcessesByName(p).Length == 0), 1000);
            NativeMethods.ToggleEnhancePointerPrecision(true);
            WaitOpenAppAsync(processNames);
        }
    }
}