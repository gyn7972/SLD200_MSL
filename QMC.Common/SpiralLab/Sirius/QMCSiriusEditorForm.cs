using QMC.Common;
using SpiralLab.Sirius;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpiralLab.Sirius
{
    public class QMCSiriusEditorForm : SiriusEditorForm
    {
        public QMCSiriusEditorForm() : base()
        {
            // 생성자에서 필요한 초기화 작업 수행
        }
        protected override void Action_OnSelectedEntityChanged(IDocument doc, List<IEntity> list)
        {
            base.Action_OnSelectedEntityChanged(doc, list);
            foreach(var view in this.Document.Views)
            {
                if(this.InvokeRequired)
                {
                    Equipment.formMain.Invoke(new System.Action(() =>
                    {
                        this.Refresh();
                    }));
                    

                }
                //view.Render();
            }

        }
    }
}
