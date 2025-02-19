using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;

namespace SLD200_MSL
{
    public partial class FormCameraConfig : FormSubContentBase
    {
        #region Field
        protected Camera m_Owner;
        protected IlluminatorChannelControl m_IlluminatorChannelControl;
        protected AutoFocusConfigControl m_AutoFocuserConfigControl;

        //  for Parameter Load
        public Timer timer_UpperParamReload;
        public Timer timer_LowerParamReload;
        #endregion

        #region Constructor
        public FormCameraConfig(Part part)
            : base(FormType.Content.ToString(), part.Name)
        {
            InitializeComponent();

            m_Owner = part as Camera;

            this.BackColor = Configuration.PanelBackColor;

            if (part.Name == "Upper Camera")                                                                                         //  Upper Camera
            {
                this.panelContent.Location = new Point(Configuration.ContentLocation.X, Configuration.PanelbuttonSize.Height);
                this.panelContent.Hide();

                this.buttonLoad.Size = Configuration.ButtonSize;
                this.buttonLoad.Location = new Point(Configuration.PanelSize.Width - Configuration.ControlsLocation.Width - Configuration.ButtonSize.Width * 2 - Configuration.ContentLocation.X, 3);
                this.buttonSave.Location = new Point(Configuration.PanelSize.Width - Configuration.ButtonSize.Width - Configuration.ControlsLocation.Width, 3);
                this.buttonSave.Size = Configuration.ButtonSize;
                //this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);

                this.CameraPropertyGridConfig_Upper.Size = new Size((Configuration.ContentSize.Width - 25) / 2 - Configuration.ButtonSize.Width * 2, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 7);
                this.CameraPropertyGridConfig_Upper.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 2 + Configuration.ButtonSize.Height);

                this.baseLabelCameraConfig_Upper.Size = new Size(this.CameraPropertyGridConfig_Upper.Width, Configuration.ButtonSize.Height);
                this.baseLabelCameraConfig_Upper.Location = new Point(this.CameraPropertyGridConfig_Upper.Location.X, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height);
                this.baseLabelCameraConfig_Upper.Text = "Camera";
                this.baseLabelCameraConfig_Upper.TextAlign = ContentAlignment.MiddleCenter;

                this.m_IlluminatorChannelControl = new IlluminatorChannelControl();
                this.m_IlluminatorChannelControl.Location = new Point(this.CameraPropertyGridConfig_Upper.Location.X + this.CameraPropertyGridConfig_Upper.Width + Configuration.ControlGap, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 2 + Configuration.ButtonSize.Height);

                Module module = m_Owner.Owner;

                if (module is WorkStage)
                {
                    this.m_IlluminatorChannelControl.listIlluminationChannel = ((WorkStage)module).Config.ListIlluminationChannel;

                    m_AutoFocuserConfigControl = new AutoFocusConfigControl(((WorkStage)module).Config.AutoFocuserConfig_HighRes);
                    m_AutoFocuserConfigControl.Location = new Point(m_IlluminatorChannelControl.Location.X + m_IlluminatorChannelControl.Width + Configuration.ControlGap - 3, m_IlluminatorChannelControl.Location.Y);
                    this.Controls.Add(m_AutoFocuserConfigControl);
                }

                this.Controls.Add(this.m_IlluminatorChannelControl);
                this.Controls.Add(this.baseLabelCameraConfig_Upper);
                this.Controls.Add(this.buttonLoad);
                this.Controls.Add(this.buttonSave);
                this.Controls.Add(this.CameraPropertyGridConfig_Upper);

                if (m_Owner is GrabLinkMultiCamCamera)
                {
                    CameraPropertyGridConfig_Upper.SelectedObject = ((GrabLinkMultiCamCamera)m_Owner).Config;
                }
                else if (m_Owner is HIKGigECamera)
                {
                    CameraPropertyGridConfig_Upper.SelectedObject = ((HIKGigECamera)m_Owner).Config;
                }
                else
                {
                    CameraPropertyGridConfig_Upper.SelectedObject = m_Owner.Config;
                }

                //  for Parameter Reload Timer 
                Equipment.m_bRedraw_FormUpperCameraConfig = false;

                timer_UpperParamReload = new Timer();
                timer_UpperParamReload.Interval = 500;
                timer_UpperParamReload.Tick += new System.EventHandler(Timer_UpperParamReloadFunc);
                timer_UpperParamReload.Enabled = true;
            }
            else                                                                                                                    //  Low - Resolution Camera
            {
                this.panelContent.Location = new Point(Configuration.ContentLocation.X, Configuration.PanelbuttonSize.Height);
                this.panelContent.Hide();

                this.buttonLoad.Size = Configuration.ButtonSize;
                this.buttonLoad.Location = new Point(Configuration.PanelSize.Width - Configuration.ControlsLocation.Width - Configuration.ButtonSize.Width * 2 - Configuration.ContentLocation.X, 3);
                this.buttonSave.Location = new Point(Configuration.PanelSize.Width - Configuration.ButtonSize.Width - Configuration.ControlsLocation.Width, 3);
                this.buttonSave.Size = Configuration.ButtonSize;
                //this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);

                this.CameraPropertyGridConfig_Lower.Size = new Size((Configuration.ContentSize.Width - 25) / 2 - Configuration.ButtonSize.Width * 2, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 7);
                this.CameraPropertyGridConfig_Lower.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 2 + Configuration.ButtonSize.Height);

                this.baseLabelCameraConfig_Lower.Size = new Size(this.CameraPropertyGridConfig_Lower.Width, Configuration.ButtonSize.Height);
                this.baseLabelCameraConfig_Lower.Location = new Point(this.CameraPropertyGridConfig_Lower.Location.X, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height);
                this.baseLabelCameraConfig_Lower.Text = "Camera";
                this.baseLabelCameraConfig_Lower.TextAlign = ContentAlignment.MiddleCenter;

                this.m_IlluminatorChannelControl = new IlluminatorChannelControl();
                this.m_IlluminatorChannelControl.Location = new Point(this.CameraPropertyGridConfig_Lower.Location.X + this.CameraPropertyGridConfig_Lower.Width + Configuration.ControlGap, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 2 + Configuration.ButtonSize.Height);

                Module module = m_Owner.Owner;

                if (module is WorkStage)
                {
                    this.m_IlluminatorChannelControl.listIlluminationChannel = ((WorkStage)module).Config.ListIlluminationChannel;

                    m_AutoFocuserConfigControl = new AutoFocusConfigControl(((WorkStage)module).Config.AutoFocuserConfig_LowRes);
                    m_AutoFocuserConfigControl.Location = new Point(m_IlluminatorChannelControl.Location.X + m_IlluminatorChannelControl.Width + Configuration.ControlGap - 3, m_IlluminatorChannelControl.Location.Y);
                    this.Controls.Add(m_AutoFocuserConfigControl);
                }

                this.Controls.Add(this.m_IlluminatorChannelControl);
                this.Controls.Add(this.baseLabelCameraConfig_Lower);
                this.Controls.Add(this.buttonLoad);
                this.Controls.Add(this.buttonSave);
                this.Controls.Add(this.CameraPropertyGridConfig_Lower);

                if (m_Owner is GrabLinkMultiCamCamera)
                {
                    CameraPropertyGridConfig_Lower.SelectedObject = ((GrabLinkMultiCamCamera)m_Owner).Config;
                }
                else if (m_Owner is HIKGigECamera)
                {
                    CameraPropertyGridConfig_Lower.SelectedObject = ((HIKGigECamera)m_Owner).Config;
                }
                else
                {
                    CameraPropertyGridConfig_Lower.SelectedObject = m_Owner.Config;
                }

                //  for Parameter Reload Timer 
                Equipment.m_bRedraw_FormLowerCameraConfig = false;

                timer_LowerParamReload = new Timer();
                timer_LowerParamReload.Interval = 500;
                timer_LowerParamReload.Tick += new System.EventHandler(Timer_LowerParamReloadFunc);
                timer_LowerParamReload.Enabled = true;
            }
        }
        #endregion

        void Timer_UpperParamReloadFunc(object sender, EventArgs e)
        {
            if (Equipment.m_bRedraw_FormUpperCameraConfig)
            {
                Equipment.m_bRedraw_FormUpperCameraConfig = false;

                if (m_Owner is GrabLinkMultiCamCamera)
                {
                    CameraPropertyGridConfig_Upper.SelectedObject = ((GrabLinkMultiCamCamera)m_Owner).Config;
                }
                else if (m_Owner is HIKGigECamera)
                {
                    CameraPropertyGridConfig_Upper.SelectedObject = ((HIKGigECamera)m_Owner).Config;
                }
                else
                {
                    CameraPropertyGridConfig_Upper.SelectedObject = m_Owner.Config;
                }
            }
        }

        void Timer_LowerParamReloadFunc(object sender, EventArgs e)
        {
            if (Equipment.m_bRedraw_FormLowerCameraConfig)
            {
                Equipment.m_bRedraw_FormLowerCameraConfig = false;

                if (m_Owner is GrabLinkMultiCamCamera)
                {
                    CameraPropertyGridConfig_Lower.SelectedObject = ((GrabLinkMultiCamCamera)m_Owner).Config;
                }
                else if (m_Owner is HIKGigECamera)
                {
                    CameraPropertyGridConfig_Lower.SelectedObject = ((HIKGigECamera)m_Owner).Config;
                }
                else
                {
                    CameraPropertyGridConfig_Lower.SelectedObject = m_Owner.Config;
                }
            }
        }

        #region Event Handler
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Module module = m_Owner.Owner;
            //if (module != null)
            //{
            //    Equipment.LoadConfig(); // 참고 : param load
            //    DataManager.Instance.ApplyConfigData(module);
            //}

            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            if (m_recipeInfo != null)
            {
                m_strRecipe = m_recipeInfo.Name;

                //Equipment.LoadConfig(); // 참고 : param load                                //  2022. 06. 30.  SCH : 원래 이건데...
                Equipment.LoadConfig(m_strRecipe); // 참고 : param load                       //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                DataManager.Instance.ApplyConfigData(module);
            }
            else
            {
                Equipment.LoadConfig(); // 참고 : param load                                //  2022. 06. 30.  SCH : 원래 이건데...
                //Equipment.LoadConfig(m_strRecipe); // 참고 : param load                       //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                DataManager.Instance.ApplyConfigData(module);
            }

            WorkStage workStage = module as WorkStage;

            ////  요걸 해 줘야 화면의 데이터가 바뀐다.
            //workStage.workStageParameter.LaserPositionGrid.DataSource = workStage.Config.Positions;
            //LaserParameterGrid.SelectedObject = workStage.workStageParameter.Config;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Module module = m_Owner.Owner;
            //if (module != null)
            //{
            //    DataManager.Instance.UpdateConfigData(module); // 참고 : param save
            //    Equipment.SaveConfig();
            //}

            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            if (m_recipeInfo != null)
            {
                m_strRecipe = m_recipeInfo.Name;

                DataManager.Instance.UpdateConfigData(module); // 참고 : param save
                                                                 //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
            }
            else
            {
                DataManager.Instance.UpdateConfigData(module); // 참고 : param save
                Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            }
        }
        #endregion
    }
}
