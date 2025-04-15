using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class QMCFindLenzCenter
    {
        List<SLDMeasureData> m_sldData = new List<SLDMeasureData>();
        List<SLDMeasureData> m_sldResultX = new List<SLDMeasureData>();
        List<SLDMeasureData> m_sldResultY = new List<SLDMeasureData>();

        public QMCFindLenzCenter()
        {

        }

        public void GetLineABC(double x1, double y1, double x2, double y2, ref double a, ref double b, ref double c)
        {
            a = y2 - y1;
            b = x1 - x2;
            c = a * x1 + b * y1;
        }
        public PointF GetCrossPoint(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            PointF pt = new PointF();

            double determinant = a1 * b2 - a2 * b1;


            pt.X = (float)((b2 * c1 - b1 * c2) / determinant);
            pt.Y = (float)((a1 * c2 - a2 * c1) / determinant);
            return pt;
        }

        public void AddSLDMeasureData(SLDMeasureData sLDMD)
        {
            m_sldData.Add(sLDMD);
        }
        public void Clear()
        {
            m_sldData.Clear();
        }
        public void SaveData(string strPath = "")
        {

            MoveToCenter();
            FindLenzCenter();
            DateTime now = DateTime.Now;
            string timeString = now.ToString("yyyy-MM-dd HH_mm_ss");
            string filePath = strPath + "./ScanerCalData" + timeString;
            string strData = "";
            //File.WriteAllText(filePath + ".txt", strData);
            foreach (var v in m_sldData)
            {
                strData += v.m_nIndexX.ToString();
                strData += "," + v.m_nindexY.ToString();
                strData += ":" + v.m_dX.ToString();
                strData += "," + v.m_dY.ToString();
                strData += "," + v.m_dMeasureX.ToString();
                strData += "," + v.m_dMeasureY.ToString();
                strData += "\n";
            }
            File.AppendAllText(filePath + ".txt", strData);

            filePath = strPath + "./CenterPoint" + timeString;
            strData = "";
            File.WriteAllText(filePath + ".csv", strData);
            List<PointF> pt = GetResult();
            foreach (var v in pt)
            {
                strData += v.X.ToString();
                strData += "," + v.Y.ToString();
                strData += "\n";
            }
            File.AppendAllText(filePath + ".csv", strData);

        }
        public List<PointF> GetResult()
        {
            List<PointF> list = new List<PointF>();
            var varKey = m_sldData.GroupBy(s => Math.Abs(s.m_dX)).OrderBy(t => t.Key);
            foreach (var key in varKey)
            {
                var pt1 = m_sldResultX.Where(t => Math.Abs(t.m_dX) == key.Key).ToList();
                var pt2 = m_sldResultY.Where(t => Math.Abs(t.m_dY) == key.Key).ToList();
                if (pt1.Count == pt2.Count && pt1.Count == 2)
                {
                    double a1 = 0;
                    double b1 = 0;
                    double c1 = 0;
                    double a2 = 0;
                    double b2 = 0;
                    double c2 = 0;
                    GetLineABC(pt1[0].m_dMeasureX, pt1[0].m_dMeasureY, pt1[1].m_dMeasureX, pt1[1].m_dMeasureY, ref a1, ref b1, ref c1);
                    GetLineABC(pt2[0].m_dMeasureX, pt2[0].m_dMeasureY, pt2[1].m_dMeasureX, pt2[1].m_dMeasureY, ref a2, ref b2, ref c2);

                    list.Add(GetCrossPoint(a1, b1, c1, a2, b2, c2));
                }
            }
            int nCount = 0;
            return list;
        }
        public void MoveToCenter()
        {
            try
            {
                double dXOffset = m_sldData.Where(t => t.m_dX == 0 && t.m_dY == 0).Average(t => t.m_dMeasureX);
                double dYOffset = m_sldData.Where(t => t.m_dX == 0 && t.m_dY == 0).Average(t => t.m_dMeasureY);
                foreach (var v in m_sldData)
                {
                    v.m_dMeasureX -= dXOffset;
                    v.m_dMeasureY -= dYOffset;
                }
            }catch(Exception ex)
            {

            }
            
        }
        public void FindLenzCenter()
        {
            m_sldResultX.Clear();
            m_sldResultY.Clear();
            MoveToCenter();
            var varX = m_sldData.GroupBy(s => s.m_dX);
            var varY = m_sldData.GroupBy(s => s.m_dY);

            foreach (var vx in varX)
            {
                double[] x = m_sldData.Where(t => t.m_dY == vx.Key).Select(t => t.m_dMeasureX).ToArray();
                double[] y = m_sldData.Where(t => t.m_dY == vx.Key).Select(t => t.m_dMeasureY).ToArray();

                int degree = 2;

                // 매개변수 벡터
                double[] a = new double[degree + 1];

                // 최소 자승법으로 매개변수 구하기
                LeastSquares(x, y, degree, a);
                double da = a[2];
                double db = a[1];
                double dc = a[0];

                double dVertexX = -db / (2 * da);
                double dVertexY = -(db * db - 4 * da * dc) / (4 * da);
                Console.WriteLine("{0} , {1}", dVertexX, dVertexY);
                SLDMeasureData data = new SLDMeasureData();
                data.m_dY = vx.Key;
                data.m_dMeasureX = dVertexX;
                data.m_dMeasureY = dVertexY;
                m_sldResultY.Add(data);

            }

            Console.WriteLine("Y \n");
            foreach (var vx in varX)
            {
                double[] x = m_sldData.Where(t => t.m_dX == vx.Key).Select(t => t.m_dMeasureY).ToArray();
                double[] y = m_sldData.Where(t => t.m_dX == vx.Key).Select(t => t.m_dMeasureX).ToArray();

                int degree = 2;

                // 매개변수 벡터
                double[] a = new double[degree + 1];

                // 최소 자승법으로 매개변수 구하기
                LeastSquares(x, y, degree, a);
                double da = a[2];
                double db = a[1];
                double dc = a[0];

                double dVertexX = -db / (2 * da);
                double dVertexY = -(db * db - 4 * da * dc) / (4 * da);
                //Console.WriteLine("y = {0} + {1}x + {2}x^2 , 꼭지점 x : {3},y : {4}", a[0], a[1], a[2] , dVertexX, dVertexY);
                Console.WriteLine("{0} , {1}", dVertexY, dVertexX);

                SLDMeasureData data = new SLDMeasureData();

                data.m_dX = vx.Key;
                data.m_dMeasureX = dVertexY;
                data.m_dMeasureY = dVertexX;
                m_sldResultX.Add(data);

            }
        }
        static void LeastSquares(double[] x, double[] y, int degree, double[] a)
        {
            // 행렬의 크기
            int n = degree + 1;

            // 정규 방정식의 계수 행렬
            double[,] A = new double[n, n];

            // 정규 방정식의 상수 벡터
            double[] b = new double[n];

            // 정규 방정식의 계수 행렬 채우기
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = SumOfPowers(x, i + j);
                }
                b[i] = SumOfProducts(y, x, i);
            }

            // 가우스 소거법으로 정규 방정식 풀기
            GaussElimination(A, b);

            // 매개변수 벡터에 값 복사하기
            for (int i = 0; i < n; i++)
            {
                a[i] = b[i];
            }
        }

        // 배열의 거듭제곱 합 구하기
        static double SumOfPowers(double[] x, int p)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += Math.Pow(x[i], p);
            }
            return sum;
        }

        // 배열의 곱셈 합 구하기
        static double SumOfProducts(double[] x, double[] y, int p)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i] * Math.Pow(y[i], p);
            }
            return sum;
        }

        // 가우스 소거법 함수
        static void GaussElimination(double[,] A, double[] b)
        {
            int n = b.Length;

            // 전진 소거 과정
            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    double m = A[i, k] / A[k, k];
                    for (int j = k + 1; j < n; j++)
                    {
                        A[i, j] -= m * A[k, j];
                    }
                    b[i] -= m * b[k];
                }
            }
            for (int k = n - 1; k >= 0; k--)
            {
                b[k] /= A[k, k];
                for (int i = k - 1; i >= 0; i--)
                {
                    b[i] -= A[i, k] * b[k];
                }
            }
        }

    }
    public class SLDMeasureData
    {
        public double m_dX;
        public double m_dY;
        public double m_dMeasureX;
        public double m_dMeasureY;
        public Double m_dOffsetX;
        public Double m_dOffsetY;
        public double m_dError;
        public int m_nIndexX;
        public int m_nindexY;

        public SLDMeasureData()
        {

        }

        public SLDMeasureData(int x, int y, double dX, double dY, double dMeasureX, double dMeasureY)
        {
            m_nIndexX = x;
            m_nindexY = y;
            m_dX = dX;
            m_dY = dY;
            m_dMeasureX = dMeasureX;
            m_dMeasureY = dMeasureY;
            m_dOffsetX = m_dMeasureX - m_dX;
            m_dOffsetY = m_dMeasureY - m_dY;

        }
    }
}
