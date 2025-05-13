using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.Global
{
    public class GlobalHotkeyMessageFilter : IMessageFilter
    {
        public Func<Keys, bool> OnKeyPressed;
        public bool PreFilterMessage(ref Message m)
        {
            const int WM_KEYDOWN = 0x0100;

            if (m.Msg == WM_KEYDOWN)
            {
                Keys key = (Keys)(int)m.WParam | Control.ModifierKeys;

                //Console.WriteLine($"[GlobalHotkey] Pressed: {key}"); // 로그 찍어보기

                if (OnKeyPressed != null)
                    return OnKeyPressed.Invoke(key);  // true: 메시지 처리 완료
            }

            return false;
        }
    }
}
