using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.ServiceProcess;

namespace Win10Privacy
{
    public partial class Form1 : Form
    {
        bool telemetry = false;
        bool services = false;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox1.Checked == true )
             telemetry = true;
            else
             telemetry = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox2.Checked == true )
             services = true;
            else
             services = false;
        }

        public void scriptsRun()
        {
            if( telemetry == true )
            {
                RegistryKey telemetry_key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", true);
                if (telemetry_key != null)
                {
                    telemetry_key.SetValue("AllowTelemetry", "0", RegistryValueKind.String);
                    telemetry_key.Close();
                }

            }

            if (services == true)
            {
                RegistryKey service_diag_reg = Registry.LocalMachine.OpenSubKey(@"SYSTEM\\CurrentControlSet\\Services\\DiagTrack", true);

                if (service_diag_reg != null)
                {
                    service_diag_reg.SetValue("Start", 4, RegistryValueKind.DWord); // dword 4 is disabled
                    service_diag_reg.Close();
                }

                try
                {
                    ServiceController service_diag = new ServiceController("Diagnostics Tracking Service");
                    TimeSpan service_timeout = TimeSpan.FromMilliseconds(1000);

                    service_diag.Stop();
                    service_diag.WaitForStatus(ServiceControllerStatus.Stopped, service_timeout);
                }
                catch
                {
                    //
                }


                RegistryKey service_dmwapp_reg = Registry.LocalMachine.OpenSubKey(@"SYSTEM\\CurrentControlSet\\Services\\dmwappushsvc", true);
                if (service_dmwapp_reg != null)
                {
                    service_dmwapp_reg.SetValue("Start", 4, RegistryValueKind.DWord); // dword 4 is disabled
                    service_dmwapp_reg.Close();
                }

                try
                {
                    ServiceController service_dmwapp = new ServiceController("dmwappushsvc");
                    TimeSpan service_timeout = TimeSpan.FromMilliseconds(1000);
                    service_dmwapp.Stop();
                    service_dmwapp.WaitForStatus(ServiceControllerStatus.Stopped, service_timeout); 
                }
                catch
                {
                    //
                }
            }

            MessageBox.Show("Done!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            scriptsRun();
        }

    }
}
