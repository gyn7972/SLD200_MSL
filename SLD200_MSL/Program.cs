using QMC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            // 전역 Mutex로 다중 실행 방지 (세션 간 포함)
            const string mutexName = @"Global\SLD200_MSL_App_Mutex";
            bool createdNew;

            using (var mutex = new Mutex(initiallyOwned: true, name: mutexName, createdNew: out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("이미 프로그램이 실행 중입니다.", "SLD200_MSL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    Application.SetCompatibleTextRenderingDefault(false);
                    Equipment.CreateInstance("SLD200_MSL");
                    Application.EnableVisualStyles();
                    Equipment.formMain = new FormMain();
                    Application.Run(Equipment.formMain);
                }
                finally
                {
                    Equipment.Close();
                    // using 블록에서 Mutex Dispose 시 자동 해제됩니다.
                }
            }

            //기존
            //Application.SetCompatibleTextRenderingDefault(false);
            //Equipment.CreateInstance("SLD200_MSL");
            //Application.EnableVisualStyles();
            ////   Application.Run(new Form1());
            //Equipment.formMain = new FormMain();
            //Application.Run(Equipment.formMain);
            //Equipment.Close();
        }
    }
}
