using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200.NewStyleForm
{
    public partial class FormNew_QMCSiriusEditorStatus : Form
    {
        public FormNew_QMCSiriusEditorStatus()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // 사용자가 닫기(X 버튼) 누른 경우 → 숨기기만 하고 종료 안 함
                e.Cancel = true;
                this.Hide();
                return;
            }

            // 그 외 종료 (Application.Exit 등) → 정식 해제
            base.OnFormClosing(e);
        }
    }
}
