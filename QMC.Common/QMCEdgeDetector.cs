using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common
{
    public class QMCEdgeDetector
    {
        private PointF m_ptTopToCenter;
        private PointF m_ptBottomToCenter;


        public int ThresholdIntensity = 22;
        public int MaskSize = 3;
        public int HighPassFilterBand = 30;
        public int m_nColletSize = 200;
        public int m_nChipThreshold = 220;
        public int m_nAvgCount = 75;
        public int m_nFindColletToleranceX = 5;
        public bool m_bShowInspectImage = false;
        public double m_nHpfOffset = 5;
        public double m_dXDistanceSpec = 0;
        public double m_dYDistanceSpec = 0;

        public double m_dPointWidthSpec = 0;
        public double m_dPointHeightSpec = 0;

        public double m_dChipAngleSpec = 0;

        private byte[] bufferDest = new byte[100 * 100];
        private int m_nWidth = 100;
        private int m_nHeight = 100;





        public QMCEdgeDetector()
        {
            m_ptTopToCenter = new PointF();
            m_ptBottomToCenter = new PointF();
        }
        public int GetTopEdge(Bitmap bmp)
        {
            try
            {


                int top = 99999;
                BitmapData bmpData;

                IntPtr point = IntPtr.Zero;

                //Here create the Bitmap to the know height, width and format


                //Create a BitmapData and Lock all pixels to be written 
                bmpData = bmp.LockBits(
                            new Rectangle(0, 0, bmp.Width, bmp.Height),
                            ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                //Copy the data from the byte array into BitmapData.Scan0
                IntPtr p = bmpData.Scan0;
                byte[] buffer = new byte[bmp.Width * bmp.Height];
                byte[] buffer2 = new byte[bmp.Width * bmp.Height];
                Marshal.Copy(p, buffer2, 0, bmp.Width * (bmp.Height));
                Marshal.Copy(p, buffer, 0, bmp.Width * (bmp.Height));
                for (int x = 3; x < bmp.Width - 3; x++)
                {

                    for (int y = 3; y < bmp.Height - 3; y++)
                    {
                        int v = 0;
                        for (int iter = 0; iter < 7; iter++)
                        {

                            v += buffer2[x - 3 + iter + (y - 3) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 2) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 1) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 0) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 1) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 2) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 3) * bmpData.Stride];
                        }
                        buffer[x + y * bmpData.Stride] = (byte)(v / 49);
                    }
                }

                for (int x = 3; x < bmp.Width - 3; x++)
                {

                    for (int y = 6; y < bmp.Height - 6; y++)
                    {
                        int v = 0;
                        for (int iter = 0; iter < 11; iter++)
                        {

                            v += buffer[x - 6 + iter + (y - 0) * bmpData.Stride];

                        }
                        buffer2[x + y * bmpData.Stride] = (byte)(v / 11);

                    }
                }
                int[] YPos = new int[bmp.Width];
                for (int x = 0; x < bmp.Width; x++)
                {
                    bool bBingo = false;
                    for (int y = 20; y < bmp.Height - 18; y++)
                    {

                        int v = buffer2[x + y * bmpData.Stride];
                        v += buffer2[x + (y + 1) * bmpData.Stride];
                        v += buffer2[x + (y + 2) * bmpData.Stride];

                        v -= buffer2[x + (y + 15) * bmpData.Stride];
                        v -= buffer2[x + (y + 16) * bmpData.Stride];
                        v -= buffer2[x + (y + 17) * bmpData.Stride];

                        if (v > ThresholdIntensity)
                        {
                            if (bBingo == false)
                            {

                                YPos[x] = y;
                            }
                            bBingo = true;
                        }
                    }
                }


                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 20; y < bmp.Height - 18; y++)
                    {
                        if (YPos[x] < y)
                        {
                            buffer[x + y * bmpData.Stride] = 0;
                        }
                        else
                        {
                            buffer[x + y * bmpData.Stride] = 255;
                        }


                    }
                }

                Marshal.Copy(buffer, 0, p, bmp.Width * (bmp.Height));
                bmp.UnlockBits(bmpData);
                return top;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }



        public int GetRightEdge(Bitmap bmp)
        {
            try
            {

                int nRight = 0;

                BitmapData bmpData;

                IntPtr point = IntPtr.Zero;

                //Here create the Bitmap to the know height, width and format


                //Create a BitmapData and Lock all pixels to be written 
                bmpData = bmp.LockBits(
                            new Rectangle(0, 0, bmp.Width, bmp.Height),
                            ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                //Copy the data from the byte array into BitmapData.Scan0
                IntPtr p = bmpData.Scan0;
                byte[] buffer = new byte[bmp.Width * bmp.Height];
                byte[] buffer2 = new byte[bmp.Width * bmp.Height];
                Marshal.Copy(p, buffer2, 0, bmp.Width * (bmp.Height));
                Marshal.Copy(p, buffer, 0, bmp.Width * (bmp.Height));
                for (int y = 3; y < bmp.Height - 3; y++)
                {

                    for (int x = 3; x < bmp.Width - 3; x++)
                    {
                        int v = 0;
                        for (int iter = 0; iter < 7; iter++)
                        {

                            v += buffer2[x - 3 + iter + (y - 3) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 2) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 1) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 0) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 1) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 2) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 3) * bmpData.Stride];
                        }
                        buffer[x + y * bmpData.Stride] = (byte)(v / 49);
                    }
                }

                for (int y = 6; y < bmp.Height - 6; y++)
                {

                    for (int x = 6; x < bmp.Width - 6; x++)
                    {
                        int v = 0;
                        for (int iter = 0; iter < 11; iter++)
                        {

                            v += buffer[x - 0 + (y - iter + 6) * bmpData.Stride];

                        }
                        buffer2[x + y * bmpData.Stride] = (byte)(v / 11);

                    }
                }
                int[] YPos = new int[bmp.Height];
                for (int y = 20; y < bmp.Height - 18; y++)
                {
                    bool bBingo = false;
                    for (int x = bmp.Width - 30; x > 30; x--)
                    {

                        int v = buffer2[x + y * bmpData.Stride];
                        v += buffer2[x + 1 + (y) * bmpData.Stride];
                        v += buffer2[x + 2 + (y) * bmpData.Stride];

                        v -= buffer2[x + 15 + (y) * bmpData.Stride];
                        v -= buffer2[x + 16 + (y) * bmpData.Stride];
                        v -= buffer2[x + 17 + (y) * bmpData.Stride];

                        if (v > this.ThresholdIntensity)
                        {
                            if (bBingo == false)
                            {

                                YPos[y] = x;
                            }
                            bBingo = true;
                        }
                    }
                }
                for (int x = 5; x < bmp.Height - 5; x++)
                {
                    int v = Math.Abs((YPos[x - 5] - YPos[x]) - (YPos[x] - YPos[x + 5]));
                }

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 20; x < bmp.Width - 20; x++)
                    {
                        if (YPos[y] < x)
                        {
                            buffer[x + y * bmpData.Stride] = 0;
                        }
                        else
                        {
                            buffer[x + y * bmpData.Stride] = 255;
                        }


                    }
                }
                Marshal.Copy(buffer, 0, p, bmp.Width * (bmp.Height));
                bmp.UnlockBits(bmpData);
                return nRight;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return 0;
        }
        public bool GetChipCenter(byte[] bufferSource, int w, int h, out PointF ptChip)
        {
            ptChip = new PointF(0, 0);
            int nIndex = 0;
            int nCount = 0;
            List<Point> listChipPoint = new List<Point>();
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (bufferSource[x + y * w] > m_nChipThreshold)
                    {
                        nCount++;
                        listChipPoint.Add(new Point(x, y));
                    }
                    nIndex++;
                }
            }
            if (nCount > 20)
            {
                ptChip.X = (float)(((from t in listChipPoint
                                     orderby t.X
                                     select t).Take(10).Average(t => t.X) +
                                              (from t in listChipPoint
                                               orderby t.X descending
                                               select t).Take(10).Average(t => t.X)) / 2.0);

                ptChip.Y = (float)(((from t in listChipPoint
                                     orderby t.Y
                                     select t).Take(10).Average(t => t.Y) +
                                          (from t in listChipPoint
                                           orderby t.Y descending
                                           select t).Take(10).Average(t => t.Y)) / 2.0);



                return true;
            }
            return false;
        }
        public bool GetGetColletCenter(byte[] bufferSource, int w, int h, out PointF ptCollet, out PointF ptChip)
        {
            bool ret = false;
            ptCollet = new PointF(0, 0);
            if (GetChipCenter(bufferSource, w, h, out ptChip))
            {
                List<Point> listChipPoint = new List<Point>();


                if (m_nWidth != w || m_nHeight != h)
                {
                    bufferDest = new byte[w * h];
                    m_nWidth = w;
                    m_nHeight = h;
                }
                Buffer.BlockCopy(bufferSource, 0, bufferDest, 0, w * h);

                PointF pointF = new PointF(0, 0);
                ret = true;

                int nStartX = Math.Max((int)ptChip.X - m_nColletSize, 0);
                int nStartY = Math.Max((int)ptChip.Y - m_nColletSize, 0);
                int nEndX = Math.Min((int)ptChip.X + m_nColletSize, w);
                int nEndY = Math.Min((int)ptChip.Y + m_nColletSize, h);

                int nStartOffset = 10;
                if (nStartX < nStartOffset)
                {
                    nStartX = nStartOffset;
                }

                if (nStartY < nStartOffset)
                {
                    nStartY = nStartOffset;
                }
                if (nEndX > w - nStartOffset)
                {
                    nEndX = w - nStartOffset;
                }
                if (nEndY > h - nStartOffset)
                {
                    nEndX = h - nStartOffset;
                }


                Queue<int> que = new Queue<int>();
                for (int y = nStartY; y < nEndY; y++)
                {
                    int nSum = 0;
                    que.Clear();
                    for (int x = nStartX; x < nEndX; x++)
                    {

                        if (x == nStartX)
                        {
                            for (int iter = 0; iter < 5; iter++)
                            {
                                int v = 0;
                                v += bufferSource[x - 2 + iter + (y - 2) * w];
                                v += bufferSource[x - 2 + iter + (y - 1) * w];
                                v += bufferSource[x - 2 + iter + (y - 0) * w];
                                v += bufferSource[x - 2 + iter + (y + 1) * w];
                                v += bufferSource[x - 2 + iter + (y + 2) * w];
                                nSum += v;
                                que.Enqueue(v);
                            }

                        }
                        else
                        {
                            int v = 0;
                            v += bufferSource[x + 2 + (y - 2) * w];
                            v += bufferSource[x + 2 + (y - 1) * w];
                            v += bufferSource[x + 2 + (y - 0) * w];
                            v += bufferSource[x + 2 + (y + 1) * w];
                            v += bufferSource[x + 2 + (y + 2) * w];
                            que.Enqueue(v);
                            nSum += v;
                            if (que.Count > 0)
                            {
                                nSum -= que.Dequeue();
                            }
                        }
                        bufferDest[x + y * w] = (byte)(nSum / 25);
                    }
                }

                int[] YPosMin = new int[w];


                double nOffset = m_nHpfOffset;
                for (int x = nEndX; x > nStartX; x--)
                {
                    bool bBingo = false;

                    for (int y = nEndY; y > nStartY; y--)
                    {

                        int v = 0;
                        v = bufferDest[x + y * w];
                        if (v > this.ThresholdIntensity)
                        {
                            if (bBingo == false)
                            {

                                YPosMin[x] = y;
                            }
                            bBingo = true;
                            break;
                        }
                    }
                }

                int[] YPosMax = new int[w];
                for (int x = nEndX; x > nStartX; x--)
                {
                    bool bBingo = false;

                    for (int y = nStartY; y < nEndY; y++)
                    {

                        int v = 0;
                        v = bufferDest[x + y * w];

                        if (v > this.ThresholdIntensity)
                        {
                            if (bBingo == false)
                            {

                                YPosMax[x] = y;
                            }
                            bBingo = true;
                            break;
                        }
                    }
                }
                PointF ptTop = new Point(0, 9999);
                PointF ptBottom = new Point(0, 0);
                float fMax = 0;
                int maxIndex = 0;
                for (int iter = 20; iter < YPosMax.Length - 20; iter++)
                {
                    float nMinSum = 0;
                    float nMaxSum = 0;

                    if (YPosMax[iter] > 0 && YPosMin[iter] > 0)
                    {
                        int AvgCount = m_nAvgCount;
                        int nCount = 0;
                        for (int x = -AvgCount / 2; x < AvgCount / 2; x++)
                        {
                            if (YPosMax[iter + x] > 0 && YPosMin[iter + x] > 0)
                            {
                                nMaxSum += YPosMax[iter + x];
                                nMinSum += YPosMin[iter + x];
                                nCount++;
                            }
                        }
                        if (nCount < AvgCount)
                            continue;
                        float AvgMin = nMinSum / nCount;
                        float AvgMax = nMaxSum / nCount;
                        if (fMax < Math.Abs(AvgMax - AvgMin))
                        {
                            fMax = Math.Abs(AvgMax - AvgMin);
                            maxIndex = iter;
                        }


                        bufferDest[iter + YPosMax[iter] * w] = 255;
                        bufferDest[iter + YPosMin[iter] * w] = 255;
                    }
                }

                int nX = 0;
                int nY = 0;

                if (Math.Abs(ptBottom.X - ptTop.X) < m_nFindColletToleranceX)
                {
                    ptBottom.Y = (float)YPosMin[maxIndex];
                    ptTop.Y = (float)YPosMax[maxIndex];
                    ptBottom.X = maxIndex;
                    ptTop.X = maxIndex;
                    nX = maxIndex;
                    nY = (int)(ptTop.Y + ptBottom.Y) / 2;
                }


                int nLength = 10;
                ptCollet.X = nX;
                ptCollet.Y = nY;
                if (m_bShowInspectImage)
                {
                    for (int iter = -nLength; iter < nLength; iter++)
                    {

                        if (nX > nLength && nY > nLength && nX < w - nLength && nY < h - nLength)
                        {
                            bufferDest[(int)ptTop.X + iter + (int)ptTop.Y * w] = 120;
                            bufferDest[(int)ptTop.X + ((int)ptTop.Y + iter) * w] = 120;

                            bufferDest[nX + iter + nY * w] = 0;
                            bufferDest[nX + (nY + iter) * w] = 0;


                        }

                        if (ptTop.X > nLength && ptTop.Y > nLength && ptTop.X < w - nLength && ptTop.Y < h - nLength)
                        {
                            bufferDest[(int)ptTop.X + iter + (int)ptTop.Y * w] = 120;
                            bufferDest[(int)ptTop.X + ((int)ptTop.Y + iter) * w] = 120;

                        }
                        if (ptBottom.X > nLength && ptBottom.Y > nLength && ptBottom.X < w - nLength && ptBottom.Y < h - nLength)
                        {

                            bufferDest[(int)ptBottom.X + iter + (int)ptBottom.Y * w] = 0;
                            bufferDest[(int)ptBottom.X + ((int)ptBottom.Y + iter) * w] = 0;
                        }
                        if (ptChip.X > nLength && ptChip.Y > nLength && ptChip.X < w - nLength && ptChip.Y < h - nLength)
                        {
                            bufferDest[(int)ptChip.X + iter + (int)ptChip.Y * w] = 128;
                            bufferDest[(int)ptChip.X + ((int)ptChip.Y + iter) * w] = 128;

                        }


                    }

                    Buffer.BlockCopy(bufferDest, 0, bufferSource, 0, w * h);

                }

            }

            return ret;
        }
        public bool GetColletCenterForPatternMatching(byte[] bufferSource, int w, int h, out PointF ptCollet, PointF ptChip)
        {
            bool ret = false;
            ptCollet = new PointF(0, 0);
            //if (GetChipCenter(bufferSource, w, h, out ptChip))
            {
                List<Point> listChipPoint = new List<Point>();


                if (m_nWidth != w || m_nHeight != h)
                {
                    bufferDest = new byte[w * h];
                    m_nWidth = w;
                    m_nHeight = h;
                }
                Buffer.BlockCopy(bufferSource, 0, bufferDest, 0, w * h);

                PointF pointF = new PointF(0, 0);
                ret = true;

                int nStartX = Math.Max((int)ptChip.X - m_nColletSize, 0);
                int nStartY = Math.Max((int)ptChip.Y - m_nColletSize, 0);
                int nEndX = Math.Min((int)ptChip.X + m_nColletSize, w);
                int nEndY = Math.Min((int)ptChip.Y + m_nColletSize, h);

                int nStartOffset = 10;
                if (nStartX < nStartOffset)
                {
                    nStartX = nStartOffset;
                }

                if (nStartY < nStartOffset)
                {
                    nStartY = nStartOffset;
                }
                if (nEndX > w - nStartOffset)
                {
                    nEndX = w - nStartOffset;
                }
                if (nEndY > h - nStartOffset)
                {
                    nEndX = h - nStartOffset;
                }


                Queue<int> que = new Queue<int>();
                for (int y = nStartY; y < nEndY; y++)
                {
                    int nSum = 0;
                    que.Clear();
                    for (int x = nStartX; x < nEndX; x++)
                    {

                        if (x == nStartX)
                        {
                            for (int iter = 0; iter < 5; iter++)
                            {
                                int v = 0;
                                v += bufferSource[x - 2 + iter + (y - 2) * w];
                                v += bufferSource[x - 2 + iter + (y - 1) * w];
                                v += bufferSource[x - 2 + iter + (y - 0) * w];
                                v += bufferSource[x - 2 + iter + (y + 1) * w];
                                v += bufferSource[x - 2 + iter + (y + 2) * w];
                                nSum += v;
                                que.Enqueue(v);
                            }

                        }
                        else
                        {
                            int v = 0;
                            v += bufferSource[x + 2 + (y - 2) * w];
                            v += bufferSource[x + 2 + (y - 1) * w];
                            v += bufferSource[x + 2 + (y - 0) * w];
                            v += bufferSource[x + 2 + (y + 1) * w];
                            v += bufferSource[x + 2 + (y + 2) * w];
                            que.Enqueue(v);
                            nSum += v;
                            if (que.Count > 0)
                            {
                                nSum -= que.Dequeue();
                            }
                        }
                        bufferDest[x + y * w] = (byte)(nSum / 25);
                    }
                }

                int[] YPosMin = new int[w];


                double nOffset = m_nHpfOffset;
                for (int x = nEndX; x > nStartX; x--)
                {
                    bool bBingo = false;

                    for (int y = nEndY; y > nStartY; y--)
                    {

                        int v = 0;
                        v = bufferDest[x + y * w];
                        if (v > this.ThresholdIntensity)
                        {
                            if (bBingo == false)
                            {

                                YPosMin[x] = y;
                            }
                            bBingo = true;
                            break;
                        }
                    }
                }

                int[] YPosMax = new int[w];
                for (int x = nEndX; x > nStartX; x--)
                {
                    bool bBingo = false;

                    for (int y = nStartY; y < nEndY; y++)
                    {

                        int v = 0;
                        v = bufferDest[x + y * w];

                        if (v > this.ThresholdIntensity)
                        {
                            if (bBingo == false)
                            {

                                YPosMax[x] = y;
                            }
                            bBingo = true;
                            break;
                        }
                    }
                }
                PointF ptTop = new Point(0, 9999);
                PointF ptBottom = new Point(0, 0);
                float fMax = 0;
                int maxIndex = 0;
                for (int iter = 20; iter < YPosMax.Length - 20; iter++)
                {
                    float nMinSum = 0;
                    float nMaxSum = 0;

                    if (YPosMax[iter] > 0 && YPosMin[iter] > 0)
                    {
                        int AvgCount = m_nAvgCount;
                        int nCount = 0;
                        for (int x = -AvgCount / 2; x < AvgCount / 2; x++)
                        {
                            if (YPosMax[iter + x] > 0 && YPosMin[iter + x] > 0)
                            {
                                nMaxSum += YPosMax[iter + x];
                                nMinSum += YPosMin[iter + x];
                                nCount++;
                            }
                        }
                        if (nCount < AvgCount)
                            continue;
                        float AvgMin = nMinSum / nCount;
                        float AvgMax = nMaxSum / nCount;
                        if (fMax < Math.Abs(AvgMax - AvgMin))
                        {
                            fMax = Math.Abs(AvgMax - AvgMin);
                            maxIndex = iter;
                        }


                        bufferDest[iter + YPosMax[iter] * w] = 255;
                        bufferDest[iter + YPosMin[iter] * w] = 255;
                    }
                }

                int nX = 0;
                int nY = 0;

                if (Math.Abs(ptBottom.X - ptTop.X) < m_nFindColletToleranceX)
                {
                    ptBottom.Y = (float)YPosMin[maxIndex];
                    ptTop.Y = (float)YPosMax[maxIndex];
                    ptBottom.X = maxIndex;
                    ptTop.X = maxIndex;
                    nX = maxIndex;
                    nY = (int)(ptTop.Y + ptBottom.Y) / 2;
                }


                int nLength = 10;
                ptCollet.X = nX;
                ptCollet.Y = nY;
                if (m_bShowInspectImage)
                {
                    for (int iter = -nLength; iter < nLength; iter++)
                    {

                        if (nX > nLength && nY > nLength && nX < w - nLength && nY < h - nLength)
                        {
                            bufferDest[(int)ptTop.X + iter + (int)ptTop.Y * w] = 120;
                            bufferDest[(int)ptTop.X + ((int)ptTop.Y + iter) * w] = 120;

                            bufferDest[nX + iter + nY * w] = 255;
                            bufferDest[nX + (nY + iter) * w] = 255;


                        }

                        if (ptTop.X > nLength && ptTop.Y > nLength && ptTop.X < w - nLength && ptTop.Y < h - nLength)
                        {
                            bufferDest[(int)ptTop.X + iter + (int)ptTop.Y * w] = 120;
                            bufferDest[(int)ptTop.X + ((int)ptTop.Y + iter) * w] = 120;

                        }
                        if (ptBottom.X > nLength && ptBottom.Y > nLength && ptBottom.X < w - nLength && ptBottom.Y < h - nLength)
                        {

                            bufferDest[(int)ptBottom.X + iter + (int)ptBottom.Y * w] = 0;
                            bufferDest[(int)ptBottom.X + ((int)ptBottom.Y + iter) * w] = 0;
                        }
                        if (ptChip.X > nLength && ptChip.Y > nLength && ptChip.X < w - nLength && ptChip.Y < h - nLength)
                        {
                            bufferDest[(int)ptChip.X + iter + (int)ptChip.Y * w] = 128;
                            bufferDest[(int)ptChip.X + ((int)ptChip.Y + iter) * w] = 128;

                        }


                    }

                    Buffer.BlockCopy(bufferDest, 0, bufferSource, 0, w * h);

                }

            }





            return ret;
        }
        public bool GetGetColletCenter(Bitmap bmp, out PointF ptCollet, out PointF ptChip)
        {
            try
            {



                BitmapData bmpData;

                IntPtr point = IntPtr.Zero;

                //Here create the Bitmap to the know height, width and format


                //Create a BitmapData and Lock all pixels to be written 
                bmpData = bmp.LockBits(
                            new Rectangle(0, 0, bmp.Width, bmp.Height),
                            ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);


                IntPtr p = bmpData.Scan0;
                byte[] buffer = new byte[bmp.Width * bmp.Height];
                Marshal.Copy(p, buffer, 0, bmp.Width * (bmp.Height));
                GetGetColletCenter(buffer, bmp.Width, bmp.Height, out ptCollet, out ptChip);
                Marshal.Copy(buffer, 0, p, bmp.Width * (bmp.Height));
                bmp.UnlockBits(bmpData);
                return true;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            ptCollet = new PointF();
            ptChip = new PointF();
            return false;
        }
        public Point GetGetColletCenter(Bitmap bmp)
        {
            try
            {



                BitmapData bmpData;

                IntPtr point = IntPtr.Zero;

                //Here create the Bitmap to the know height, width and format


                //Create a BitmapData and Lock all pixels to be written 
                bmpData = bmp.LockBits(
                            new Rectangle(0, 0, bmp.Width, bmp.Height),
                            ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                //Copy the data from the byte array into BitmapData.Scan0
                List<Point> listChipPoint = new List<Point>();
                IntPtr p = bmpData.Scan0;
                byte[] buffer = new byte[bmp.Width * bmp.Height];
                byte[] buffer2 = new byte[bmp.Width * bmp.Height];
                Marshal.Copy(p, buffer2, 0, bmp.Width * (bmp.Height));
                Marshal.Copy(p, buffer, 0, bmp.Width * (bmp.Height));
                PointF pointF = new PointF(0, 0);

                for (int y = 200; y < bmp.Height - 200; y++)
                {

                    for (int x = 200; x < bmp.Width - 200; x++)
                    {
                        int v = 0;
                        for (int iter = 0; iter < 7; iter++)
                        {

                            v += buffer2[x - 3 + iter + (y - 3) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 2) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 1) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y - 0) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 1) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 2) * bmpData.Stride];
                            v += buffer2[x - 3 + iter + (y + 3) * bmpData.Stride];
                        }
                        buffer[x + y * bmpData.Stride] = (byte)(v / 49);

                        if (buffer[x + y * bmpData.Stride] > 100)
                        {

                            pointF.X += x;
                            pointF.Y += y;
                            listChipPoint.Add(new Point(x, y));

                        }
                    }
                }
                if (listChipPoint.Count > 20)
                {
                    pointF.X = (float)(((from t in listChipPoint
                                         orderby t.X
                                         select t).Take(10).Average(t => t.X) +
                                              (from t in listChipPoint
                                               orderby t.X descending
                                               select t).Take(10).Average(t => t.X)) / 2.0);

                    pointF.Y = (float)(((from t in listChipPoint
                                         orderby t.Y
                                         select t).Take(10).Average(t => t.Y) +
                                              (from t in listChipPoint
                                               orderby t.Y descending
                                               select t).Take(10).Average(t => t.Y)) / 2.0);


                }
                else
                {
                    pointF.X = 0;
                    pointF.Y = 0;
                }




                int[] YPosMin = new int[bmp.Width];
                for (int x = bmp.Width - 150; x > 150; x--)
                {
                    bool bBingo = false;

                    for (int y = bmp.Height - 100; y > 100; y--)
                    {

                        int v = 0;
                        v -= buffer2[x + y * bmpData.Stride];
                        v -= buffer2[x + (y + 1) * bmpData.Stride];
                        v -= buffer2[x + (y + 2) * bmpData.Stride];

                        v += buffer2[x + (y - 1) * bmpData.Stride];
                        v += buffer2[x + (y - 2) * bmpData.Stride];
                        v += buffer2[x + (y - 3) * bmpData.Stride];

                        if (v > this.ThresholdIntensity)
                        {
                            if (bBingo == false)
                            {

                                YPosMin[x] = y;
                            }
                            bBingo = true;
                            break;
                        }
                    }
                }

                int[] YPosMax = new int[bmp.Width];
                for (int x = bmp.Width - 100; x > 100; x--)
                {
                    bool bBingo = false;

                    for (int y = 100; y < bmp.Height - 100; y++)
                    {

                        int v = 0;
                        v += buffer2[x + y * bmpData.Stride];
                        v += buffer2[x + (y + 1) * bmpData.Stride];
                        v += buffer2[x + (y + 2) * bmpData.Stride];

                        v -= buffer2[x + (y - 1) * bmpData.Stride];
                        v -= buffer2[x + (y - 2) * bmpData.Stride];
                        v -= buffer2[x + (y - 3) * bmpData.Stride];

                        if (v > this.ThresholdIntensity)
                        {
                            if (bBingo == false)
                            {

                                YPosMax[x] = y;
                            }
                            bBingo = true;
                            break;
                        }
                    }
                }
                PointF ptTop = new Point(0, 9999);
                PointF ptBottom = new Point(0, 0);

                for (int iter = 20; iter < YPosMax.Length - 20; iter++)
                {
                    float nMinSum = 0;
                    float nMaxSum = 0;

                    if (YPosMax[iter] > 0 && YPosMin[iter] > 0)
                    {
                        int AvgCount = 50;
                        int nCount = 0;
                        for (int x = -AvgCount / 2; x < AvgCount / 2; x++)
                        {
                            if (YPosMax[iter + x] > 0 && YPosMin[iter + x] > 0)
                            {
                                nMaxSum += YPosMax[iter + x];
                                nMinSum += YPosMin[iter + x];
                                nCount++;
                            }
                        }
                        if (nCount < 30)
                            continue;
                        float AvgMin = nMinSum / nCount;
                        float AvgMax = nMaxSum / nCount;
                        if (ptTop.Y > AvgMax)
                        {
                            ptTop.Y = AvgMax;
                            ptTop.X = iter;
                        }
                        if (ptBottom.Y < AvgMin)
                        {
                            ptBottom.Y = AvgMin;
                            ptBottom.X = iter;
                        }

                        buffer[iter + YPosMax[iter] * bmpData.Stride] = 255;
                        buffer[iter + YPosMin[iter] * bmpData.Stride] = 255;
                    }
                }
                ptBottom.Y = (float)YPosMin[(int)ptBottom.X];
                ptTop.Y = (float)YPosMax[(int)ptTop.X];
                int nX = (int)(ptTop.X + ptBottom.X) / 2;
                int nY = (int)(ptTop.Y + ptBottom.Y) / 2;
                if (pointF.X > 1)
                {
                    if (Math.Abs(ptTop.X - ptBottom.X) > 2 || Math.Abs(ptTop.Y - ptBottom.Y) > 120)
                    {
                        if (Math.Abs(ptTop.Y - ptBottom.Y) > 120)
                        {
                            if (Math.Abs(ptTop.Y - nY) > Math.Abs(ptBottom.Y - nY))
                            {
                                nX = (int)(ptTop.X - m_ptTopToCenter.X);
                                nY = (int)(ptTop.Y - m_ptTopToCenter.Y);
                            }
                            else
                            {
                                nX = (int)(ptBottom.X - m_ptBottomToCenter.X);
                                nY = (int)(ptBottom.Y - m_ptBottomToCenter.Y);
                            }
                        }
                        else
                        {
                            if (Math.Abs(ptTop.Y - nY) >= Math.Abs(ptBottom.Y - nY))
                            {
                                nX = (int)(ptTop.X - m_ptTopToCenter.X);
                                nY = (int)(ptTop.Y - m_ptTopToCenter.Y);
                            }
                            else
                            {
                                nX = (int)(ptBottom.X - m_ptBottomToCenter.X);
                                nY = (int)(ptBottom.Y - m_ptBottomToCenter.Y);
                            }
                        }

                    }

                    else
                    {
                        m_ptTopToCenter.X = ptTop.X - nX;
                        m_ptTopToCenter.Y = ptTop.Y - nY;

                        m_ptBottomToCenter.X = ptBottom.X - nX;
                        m_ptBottomToCenter.Y = ptBottom.Y - nY;
                    }
                }
                for (int iter = -5; iter < 5; iter++)
                {
                    buffer[nX + iter + nY * bmpData.Stride] = 255;
                    buffer[nX + (nY + iter) * bmpData.Stride] = 128;


                    buffer[(int)ptTop.X + iter + (int)ptTop.Y * bmpData.Stride] = 80;
                    buffer[(int)ptTop.X + ((int)ptTop.Y + iter) * bmpData.Stride] = 80;


                    buffer[(int)ptBottom.X + iter + (int)ptBottom.Y * bmpData.Stride] = 128;
                    buffer[(int)ptBottom.X + ((int)ptBottom.Y + iter) * bmpData.Stride] = 255;

                    buffer[(int)pointF.X + iter + (int)pointF.Y * bmpData.Stride] = 255;
                    buffer[(int)pointF.X + ((int)pointF.Y + iter) * bmpData.Stride] = 255;




                }
                Marshal.Copy(buffer, 0, p, bmp.Width * (bmp.Height));
                bmp.UnlockBits(bmpData);
                return new Point();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return new Point();
        }

    }
}
