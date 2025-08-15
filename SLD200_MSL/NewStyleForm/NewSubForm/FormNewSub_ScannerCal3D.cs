using QMC.Common;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_ScannerCal3D : Form
    {
        // Scanner Calibration
        // 현재 스캐너 보정 파일
        private string m_srcFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
        // 신규로 생성할 스캐너 보정 파일
        private string m_targetFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", $"newfile.ct5");
        // 3x3 (9개) 위치에 대한 보정 테이블 입력용
        private float m_fieldSize = 105;
        private float m_rowInterval = 2; //20;
        private float m_colInterval = 2; //20;
        private int m_row = 5; //3
        private int m_col = 5; //3
        private float m_kfactor = 0; // = (float)Math.Pow(2, 20) / m_fieldSize;

        //
        //   Z = +1 mm  -------------- Z Upper ------------------- 
        //
        //   Z = 0      -------------- Z  =  0-------------------
        //
        //   Z = -1 mm  -------------- Z Lower ------------------- 
        //
        float m_zUpper = 1;
        float m_zLower = -1;

        private Correction3DRtc m_correction3DRtc = null;
        private Correction3DRtcForm m_correction3DRtcForm = null;

        public FormNewSub_ScannerCal3D()
        {
            InitializeComponent();

            if (Equipment.Machine_LaserType_CO2)                                                                                //  CO2 레이저
            {
                // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm -> 50mm
                float fov = 72.5f;          //  MSL-CO2 장비에서 맞춘 데이터
                // k factor (bits/mm) = 2^20 / fov
                float kfactor = (float)Math.Pow(2, 20) / fov; //14461.43448275862

                if (kfactor == 0)
                    kfactor = (float)14461.43448275862; //18830.1889;

                m_fieldSize = fov;
                m_kfactor = kfactor;
            }

            m_correction3DRtc = new Correction3DRtc(m_kfactor, m_row, m_col, m_rowInterval, 
                                                    m_zUpper, m_zLower, m_srcFile, m_targetFile);
            m_correction3DRtcForm = new Correction3DRtcForm(m_correction3DRtc);
            m_correction3DRtcForm.TopLevel = false; // 폼을 최상위 폼이 아니도록 설정
            m_correction3DRtcForm.FormBorderStyle = FormBorderStyle.None; // 폼의 테두리를 제거
            m_correction3DRtcForm.Dock = DockStyle.Fill; // 폼을 패널에 맞게 채움
        }
    }
}
