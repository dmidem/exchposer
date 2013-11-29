using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Exchposer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string appSettingsFileName = (args.Length > 1 ? args[0] : null);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            //ExchposerApplicationContext exchposerApplicationContext = new ExchposerApplicationContext();
            //Application.ApplicationExit += new EventHandler(exchposerApplicationContext.OnApplicationExit);
            Application.Run(new ExchposerApplicationContext(appSettingsFileName));
        }
    }

}
