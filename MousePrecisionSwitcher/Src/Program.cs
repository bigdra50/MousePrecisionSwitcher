using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MousePrecisionSwitcher
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
            RegisterStartup();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new TaskTray();
            var noWait = WaitOpenAppAsync(_targetAppName);
            Application.Run();
        }

        private static async Task WaitOpenAppAsync(string[] processNames, int waitFrequency = 1000)
        {
            await TaskEx.WaitUntil(() => processNames.Any(p => Process.GetProcessesByName(p).Length > 0),
                waitFrequency);
            NativeMethods.ToggleEnhancePointerPrecision(false);
            await TaskEx.WaitUntil(() => processNames.Any(p => Process.GetProcessesByName(p).Length == 0),
                waitFrequency);
            NativeMethods.ToggleEnhancePointerPrecision(true);
            var noWait = WaitOpenAppAsync(processNames);
        }

        private static void RegisterStartup()
        {
            var entryPath = Assembly.GetEntryAssembly()?.Location;
            if (entryPath == null) return;
            var exePath =
                Path.ChangeExtension(Path.Combine(Application.StartupPath, Path.GetFileName(entryPath)), "exe");
            var shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                $"{Path.GetFileNameWithoutExtension(entryPath)}.lnk");


            // wshShellを動的に作成
            var type = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8"));
            dynamic shell = Activator.CreateInstance(type);

            var shortcut = shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = exePath;
            shortcut.WorkingDirectory = Application.StartupPath;
            shortcut.Description = $"場所: {exePath}";
            shortcut.IconLocation = $@"{Application.StartupPath}/Resources/favicon.ico";
            shortcut.Save();

            //後始末
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shortcut);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shell);
        }
    }
}