using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MousePrecisionSwitcher
{
    public class TaskTray
    {
        private NotifyIcon _notifyIcon;

        public TaskTray()
        {
            Initialize();
        }

        private void Initialize()
        {
            _notifyIcon = new NotifyIcon {Icon = new Icon(@"./Resources/favicon.ico"), Visible = true, Text = "Mouse Precision Switcher"};

            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("&Exit", null, (_, _) => Exit(), "Exit")
            });
            _notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        private void Exit()
        {
            var e = new CancelEventArgs();
            Application.Exit(e);
            if (e.Cancel)
            {
                Console.WriteLine("Application.Exit is canceled!");
            }
        }
    }
}