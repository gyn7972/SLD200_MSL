using QMC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_CalcCPK
{
    public class QMCCPKCalculator
    {
        private double m_dUSL;
        private double m_dLSL;
        private bool m_bUseStandardDeviationOverall;
        private Queue<double> m_listDataX;
        private Queue<double> m_listDataRs;
        private Queue<double> m_listDataXPower;
        private const double m_d2 = 1.128;
        private double m_dSumX;
        private double m_dSumRs;
        private double m_dSumXPower;
        private int m_nMaxCount;
        public QMCCPKCalculator()
        {
            m_dUSL = 1;
            m_dLSL = 0;
            m_bUseStandardDeviationOverall = true;
            m_listDataX = new Queue<double>();
            m_listDataRs = new Queue<double>(); ;
            m_listDataXPower = new Queue<double>(); ;
            m_nMaxCount = 1000;
            m_dSumX = 0;
            m_dSumRs = 0;
            m_dSumXPower = 0;

        }

        public Queue<double> ListDataX
        {
            get { return m_listDataX; }
        }

        public Queue<double> ListDataRs
        {
            get { return m_listDataRs; }
        }

        public double USL
        {
            get { return m_dUSL; }
            set { m_dUSL = value; }
        }
        public double LSL
        {
            get { return m_dLSL; }
            set { m_dLSL = value; }
        }
        public bool BUseStandardDeviationOverall
        {
            get { return m_bUseStandardDeviationOverall; }
            set { m_bUseStandardDeviationOverall = value; }
        }
        public void MaxCount(int nMaxCount)
        {
            m_nMaxCount = nMaxCount;
        }
        private void PopQueue()
        {
            if (m_listDataRs.Count > m_nMaxCount - 1)
            {
                m_dSumRs -= m_listDataRs.Dequeue();
            }

            if (m_listDataX.Count > m_nMaxCount)
            {
                m_dSumX -= m_listDataX.Dequeue();
            }

            if (m_listDataXPower.Count > m_nMaxCount)
            {
                m_dSumXPower -= m_listDataXPower.Dequeue();
            }
        }
        public void AddData(double dX)
        {

            if (m_listDataX.Count > 0)
            {
                double dLast = m_listDataX.Last();
                double dRs = Math.Abs(dX - dLast);
                m_listDataRs.Enqueue(dRs);
                m_dSumRs += dRs;

            }
            m_listDataX.Enqueue(dX);
            m_dSumX += dX;
            double dXPower = dX * dX;
            m_listDataXPower.Enqueue(dXPower);
            m_dSumXPower += dXPower;
            PopQueue();

        }
        public double GetK()
        {
            double k = 0;
            double dSpec = (m_dUSL - m_dLSL) / 2.0;
            double dM = (m_dUSL + m_dLSL) / 2.0;
            double dAvg = GetAverageX();
            //k = Math.Abs((dSpec - dAvg) / dSpec);
            k = Math.Abs((dM - dAvg) / dSpec);               //  Median 값에서 Average 값을 빼 주어야 하는데.... 그래서 바꿔봄.
            return k;
        }
        public double GetCpk()
        {
            double dCpk = 1.67;
            double dK = GetK();
            double dCP = GetCp();
            dCpk = (1 - dK) * dCP;
            return dCpk;
        }

        public double GetCp()
        {
            double dCp = 1.67;
            double s = GetStdev();
            double dSpecRange = (m_dUSL - m_dLSL);
            dCp = (dSpecRange) / (s * 6);
            return dCp;
        }
        public double GetAverageX()
        {
            double dAvg = 0;
            int nCount = m_listDataX.Count;
            if (nCount > 0)
            {
                dAvg = m_dSumX / nCount;
            }
            return dAvg;
        }

        public double GetAverageRs()
        {
            double dAvg = 0;
            int nCount = m_listDataRs.Count;
            if (nCount > 0)
            {
                dAvg = m_dSumRs / nCount;
            }
            return dAvg;
        }

        public double GetStdev()
        {
            double s = 0;
            int nCount = m_listDataX.Count;
            if (m_bUseStandardDeviationOverall)
            {
                if (nCount > 0)
                {
                    double dAvgX = GetAverageX();
                    double XPowerBar = Math.Abs(m_dSumXPower / ((double)nCount));
                    s = Math.Sqrt(XPowerBar - dAvgX * dAvgX);
                }
            }
            else
            {
                if (nCount > 1)
                {
                    double dRBar = m_dSumRs / (nCount - 1);

                    s = dRBar / m_d2;
                }

            }
            return s;
        }

        public RangeD GetUCLLCL()
        {
            double s = 0.0;
            double avg = 0.0;
            double ucl = 0.0;
            double lcl = 0.0;

            s = GetStdev();
            avg = this.GetAverageX();

            ucl = avg + 3 * s;
            lcl = avg - 3 * s;

            return new RangeD(lcl, ucl);
        }

        public double GetRsUCL()
        {
            double s = 0.0;
            double avg = 0.0;
            double ucl = 0.0;

            s = GetStdev();
            avg = this.GetAverageRs();

            ucl = avg + 3 * s;

            return ucl;
        }
    }
}
