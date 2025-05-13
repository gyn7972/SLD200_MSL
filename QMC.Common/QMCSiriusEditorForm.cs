using SpiralLab.Sirius;
using System.Collections.Generic;

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
            this.Invalidate();

        }
    }
}
