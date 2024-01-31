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
    public partial class ProductionControl : UserControl
    {
        #region Constructor
        public ProductionControl()
        {
            InitializeComponent();
            TactTime = 0.0;
        }
        #endregion

        #region Property
        public double TactTime { get; set; }
        public ProductionData Data { get; set; }
        #endregion

        #region Method
        public virtual void UpdateData()
        {
            if(Data != null)
            {
                this.baseLabelCount.Text = Data.TotalCount.ToString();
                this.baseLabelOK.Text = Data.OKCount.ToString();
                this.baseLabelNG.Text = Data.NGCount.ToString();
                this.baseLabelTactTime.Text = Data.TactTime.ToString();
            }
        }
        protected void ResetData()
        {
            Data.Reset();
        }

        public void SetTecTimeLabel(string strName)
        {
            baseLabelTitleTactTime.Text = strName;
        }

        public void SetEnable(bool enable)
        {
            this.Enabled = enable;
        }
        #endregion

        #region Event Handler
        private void baseButtonResetProductionInformation_Click(object sender, EventArgs e)
        {
            MessageBoxYesNo mb = new MessageBoxYesNo();
            
            if(mb.ShowDialog("Production Data", "Production Data를 초기화 하시겠습니까?")== DialogResult.Yes)
            {
                ResetData();
                UpdateData();
            }
        }
        #endregion
    }
}
