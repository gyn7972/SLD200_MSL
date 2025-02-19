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
            Equipment.CreateInstance("SLD200_MSL");
            Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
            //   Application.Run(new Form1());
            Application.Run(new FormMain());
            Equipment.Close();
        }
    }
}
