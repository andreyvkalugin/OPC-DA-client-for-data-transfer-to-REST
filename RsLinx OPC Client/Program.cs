using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RsLinx_OPC_Client
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            (new System.Threading.Thread(delegate () {
                JsonSender.infinityScan().GetAwaiter().GetResult();
            })).Start();
            Application.Run(new Form1());
            
            }
    }
}
