using System;
using System.Collections;
using System.Windows.Forms;

namespace MMG.Exec
{
    class ProgramCliente
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Esta linha existe para nao dar erro no Debuger
     //       Control.CheckForIllegalCrossThreadCalls = false;

            Application.EnableVisualStyles();
     //       Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MMGCliente());
        }
    }
}