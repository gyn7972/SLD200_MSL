using QMC.Common;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public partial class StageThetaAgingTestControl : UserControl
    {
        private StageThetaAgingTester m_Owner;
        public StageThetaAgingTestControl(Part part)
        {
            InitializeComponent();

            m_Owner = part as StageThetaAgingTester;
            init();
        }

        private void init()
        {
            this.basePropertyGrid1.SelectedObject = m_Owner.Recipe;
        }
        private void baseButtonRun_Click(object sender, EventArgs e)
        {
            Task<int> task = m_Owner.OnRunAsync();
            ProgressForm progressForm = new ProgressForm("Aging Test", "Aging Test...", task);
            progressForm.StopProcess += ProgressForm_StopProcess;
            progressForm.ShowDialog();

            //Equipment.ApplyRecipeData();
            //Equipment.SaveRecipe();
        }

        private void ProgressForm_StopProcess(object target)
        {
            m_Owner.Stop();
        }
    }
}
