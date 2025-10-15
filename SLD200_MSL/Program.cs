using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;

namespace SLD200_MSL
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(exceptionDump);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            Application.SetCompatibleTextRenderingDefault(false);
            Equipment.CreateInstance("SLD200_MSL");
            Application.EnableVisualStyles();
            //   Application.Run(new Form1());
            Equipment.formMain = new FormMain();
            Application.Run(Equipment.formMain);
            Equipment.Close();
        }

        static void exceptionDump(object sender, System.Threading.ThreadExceptionEventArgs args)
        {
            MinidumpHelp.Minidump.install_self_mini_dump();
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MinidumpHelp.Minidump.install_self_mini_dump();
        }
    }
}
