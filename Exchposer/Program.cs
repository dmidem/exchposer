using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Exchposer
{
    class Program
    {
        private static Mutex mutex;

        [STAThread]
        static void Main(string[] args)
        {
            string appSettingsFileName = (args.Length > 1 ? args[0] : null);

            /*
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] instances = Process.GetProcessesByName(processName);

            if (instances.Length > 1)
            {
                MessageBox.Show("Application \"" + processName + "\" is already running", "Error");
                return;
            }
            */

            bool running;
            mutex = new Mutex(false, "Local\\Exchposer{E6ABE50E-8E14-4887-8AA7-69E26F253574}", out running);
            if (!running)
            {
                MessageBox.Show("Application is already running", "Error");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            //ExchposerApplicationContext exchposerApplicationContext = new ExchposerApplicationContext();
            //Application.ApplicationExit += new EventHandler(exchposerApplicationContext.OnApplicationExit);

            try
            {
                ExchposerApplicationContext exchposerApplicationContext = new ExchposerApplicationContext(appSettingsFileName);
                if (exchposerApplicationContext.Initialized)
                    Application.Run(exchposerApplicationContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Fatal application error: {0}", ex.Message));
                return;
            }
        }
    }

}
