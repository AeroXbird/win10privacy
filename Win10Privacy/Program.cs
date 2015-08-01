using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecurityIdentifier = System.Security.Principal.SecurityIdentifier;
using WellKnownSidType = System.Security.Principal.WellKnownSidType;

namespace Win10Privacy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Is admin code basically copy+pasted from http://stackoverflow.com/questions/509292/how-can-i-tell-if-my-process-is-running-as-administrator
            // Thanks casperOne!
            var localAdminGroupSid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            bool shouldRun = System.Security.Principal.WindowsIdentity.GetCurrent().Groups.
                    Select(g => (SecurityIdentifier)g.Translate(typeof(SecurityIdentifier))).
                    Any(s => s == localAdminGroupSid);
            if (!shouldRun)
            {
                DialogResult result = MessageBox.Show("You must run this program as an Administrator! This program makes changes to the Windows Registry, "
                    + "which requires Administrator access. You may continue, but it probably won't work!", "Windows 10 Privacy Tool",
                    MessageBoxButtons.AbortRetryIgnore);
                if (result == DialogResult.Abort)
                {
                    // Do nothing, it will exit normally
                } else if (result == DialogResult.Retry)
                {
                    Main();
                } else if (result == DialogResult.Ignore)
                {
                    shouldRun = true;
                }
            }
            if (shouldRun)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
