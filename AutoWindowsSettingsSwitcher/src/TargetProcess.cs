using System.Diagnostics;

namespace AutoWindowsSettingsSwitcher
{
    public class TargetProcess: Process
    {
        public void Stop()
        {
            CloseMainWindow();
            Close();
            OnExited();
        }
    }
}