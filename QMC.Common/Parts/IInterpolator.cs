using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public abstract class BaseInterpolator
    {
        protected List<PositionOffset> m_listMapData;

        protected MotionAxis m_XAxis;
        protected MotionAxis m_YAxis;

        public BaseInterpolator()
        {
            m_listMapData = new List<PositionOffset>();
            m_XAxis = null;
            m_YAxis = null;
        }

        public void SetAxis(MotionAxis x, MotionAxis y)
        {
            m_XAxis = x;
            m_YAxis = y;
        }

        public bool IsInterpolationAxis(MotionAxis axis)
        {
            bool bRet = false;
            if (m_XAxis != null)
            {
                if (m_XAxis.UID == axis.UID)
                {
                    bRet = true;
                }
                else if (m_YAxis.UID == axis.UID)
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        public bool IsXAxis(MotionAxis axis)
        {
            bool bRet = false;

            if (axis.UID == m_XAxis.UID)
            {
                bRet = true;
            }

            return bRet;
        }
        public void Load(string strFilePath)
        {
            StringBuilder builder = FileLoad(strFilePath);
            if (builder != null)
            {
                Load2DMap(m_listMapData, builder);
            }
        }

        private StringBuilder FileLoad(string path)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                builder = new StringBuilder(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                builder = null;
            }

            return builder;
        }
        private void Load2DMap(List<PositionOffset> MapData, StringBuilder builder)
        {
            string[] line = builder.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            double dPositionX = 0D, dPositionY = 0D, dRealPosT = 0D, dOffsetX = 0D, dOffsetY = 0D, dOffsetT = 0D;

            if (MapData == null)
                MapData = new List<PositionOffset>();
            MapData.Clear();

            for (int i = 0; i < line.Length; i++)
            {
                string[] token = line[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if (double.TryParse(token[0], out dPositionX) == false) continue;
                if (double.TryParse(token[1], out dPositionY) == false) continue;
                if (double.TryParse(token[2], out dOffsetX) == false) continue;
                if (double.TryParse(token[3], out dOffsetY) == false) continue;

                PositionOffset positionOffset = new PositionOffset();
                positionOffset.Position = new XyCoordinate(dPositionX, dPositionY);
                positionOffset.Offset = new XyCoordinate(dOffsetX, dOffsetY);

                MapData.Add(positionOffset);
            }

        }

        public void Save(string strFileName)
        {
            if (File.Exists(strFileName))
            {
                File.Delete(strFileName);
            }
            foreach (PositionOffset position in m_listMapData)
            {
                string strSave = position.ToString();

                using (StreamWriter writer = new StreamWriter(strFileName, true))
                {
                    writer.WriteLine(strSave);
                    writer.Close();
                }

            }

        }

        protected PositionOffset GetLeftTop(XyCoordinate source)
        {
            PositionOffset cood = new PositionOffset();
            if (m_listMapData.Count >= 4)
            {
                //leftTop
                var LeftTop = m_listMapData.Where(t => t.Position.X <= source.X && t.Position.Y <= source.Y);
                if (LeftTop != null)
                {
                    if (LeftTop.Count() > 0)
                    {
                        var v = LeftTop.OrderBy(t => Math.Abs(t.Position.X - source.X) + Math.Abs(t.Position.Y - source.Y)).Take(1);
                        return v.ElementAt(0);
                    }
                }
            }
            return cood;
        }
        protected PositionOffset GetLeftBottom(XyCoordinate source)
        {
            PositionOffset cood = new PositionOffset();
            if (m_listMapData.Count >= 4)
            {
                //leftTop
                var LeftTop = m_listMapData.Where(t => t.Position.X <= source.X && t.Position.Y > source.Y);
                if (LeftTop != null)
                {
                    if (LeftTop.Count() > 0)
                    {
                        var v = LeftTop.OrderBy(t => Math.Abs(t.Position.X - source.X) + Math.Abs(t.Position.Y - source.Y)).Take(1);
                        return v.ElementAt(0);
                    }
                }
            }
            return cood;
        }
        protected PositionOffset GetRightTop(XyCoordinate source)
        {
            PositionOffset cood = new PositionOffset();
            if (m_listMapData.Count >= 4)
            {
                //leftTop
                var LeftTop = m_listMapData.Where(t => t.Position.X > source.X && t.Position.Y <= source.Y);
                if (LeftTop != null)
                {
                    if (LeftTop.Count() > 0)
                    {
                        var v = LeftTop.OrderBy(t => Math.Abs(t.Position.X - source.X) + Math.Abs(t.Position.Y - source.Y)).Take(1);
                        return v.ElementAt(0);
                    }
                }
            }
            return cood;
        }
        protected PositionOffset GetRightBottom(XyCoordinate source)
        {
            PositionOffset cood = new PositionOffset();
            if (m_listMapData.Count >= 4)
            {
                //leftTop
                var LeftTop = m_listMapData.Where(t => t.Position.X > source.X && t.Position.Y > source.Y);
                if (LeftTop != null)
                {
                    if (LeftTop.Count() > 0)
                    {
                        var v = LeftTop.OrderBy(t => Math.Abs(t.Position.X - source.X) + Math.Abs(t.Position.Y - source.Y)).Take(1);
                        return v.ElementAt(0);
                    }
                }
            }
            return cood;
        }

        protected PositionOffset GetMeasureLeftTop(XyCoordinate dest)
        {
            PositionOffset cood = new PositionOffset();
            if (m_listMapData.Count >= 4)
            {
                //leftTop
                var LeftTop = m_listMapData.Where(t => t.Position.X + t.Offset.X <= dest.X && t.Position.Y + t.Offset.Y <= dest.Y);
                if (LeftTop != null)
                {
                    if (LeftTop.Count() > 0)
                    {
                        var v = LeftTop.OrderBy(t => Math.Abs(t.Position.X + t.Offset.X - dest.X) + Math.Abs(t.Position.Y + t.Offset.Y - dest.Y)).Take(1);
                        return v.ElementAt(0);
                    }
                }
            }
            return cood;
        }
        protected PositionOffset GetMeasureLeftBottom(XyCoordinate dest)
        {
            PositionOffset cood = new PositionOffset();
            if (m_listMapData.Count >= 4)
            {
                //leftTop
                var LeftTop = m_listMapData.Where(t => t.Position.X + t.Offset.X <= dest.X && t.Position.Y + t.Offset.Y > dest.Y);
                if (LeftTop != null)
                {
                    if (LeftTop.Count() > 0)
                    {
                        var v = LeftTop.OrderBy(t => Math.Abs(t.Position.X + t.Offset.X - dest.X) + Math.Abs(t.Position.Y + t.Offset.Y - dest.Y)).Take(1);
                        return v.ElementAt(0);
                    }
                }
            }
            return cood;
        }
        protected PositionOffset GetMeasureRightTop(XyCoordinate dest)
        {
            PositionOffset cood = new PositionOffset();
            if (m_listMapData.Count >= 4)
            {
                //leftTop
                var LeftTop = m_listMapData.Where(t => t.Position.X + t.Offset.X > dest.X && t.Position.Y + t.Offset.Y <= dest.Y);
                if (LeftTop != null)
                {
                    if (LeftTop.Count() > 0)
                    {
                        var v = LeftTop.OrderBy(t => Math.Abs(t.Position.X + t.Offset.X - dest.X) + Math.Abs(t.Position.Y + t.Offset.Y - dest.Y)).Take(1);
                        return v.ElementAt(0);
                    }
                }
            }
            return cood;
        }
        protected PositionOffset GetMeasureRightBottom(XyCoordinate dest)
        {
            PositionOffset cood = new PositionOffset();
            if (m_listMapData.Count >= 4)
            {
                //leftTop
                var LeftTop = m_listMapData.Where(t => t.Position.X + t.Offset.X > dest.X && t.Position.Y + t.Offset.Y > dest.Y);
                if (LeftTop != null)
                {
                    if (LeftTop.Count() > 0)
                    {
                        var v = LeftTop.OrderBy(t => Math.Abs(t.Position.X + t.Offset.X - dest.X) + Math.Abs(t.Position.Y + t.Offset.Y - dest.Y)).Take(1);
                        return v.ElementAt(0);
                    }
                }
            }
            return cood;
        }

        public abstract int Interpolate(XyCoordinate source, ref XyCoordinate dest);
        public abstract int ReverseInterpolate(XyCoordinate dest, ref XyCoordinate source);

        public int Move(XyCoordinate dest, int nVelPercent)
        {
            int ret = 0;

            if ((m_XAxis.MovePosition(dest.X, nVelPercent)) != 0)
            {
                return ret;
            }

            if ((m_YAxis.MovePosition(dest.Y, nVelPercent)) != 0)
            {
                return ret;
            }


            return ret;
        }

        public int Interpolate(MotionAxis axis, double dPos, ref XyCoordinate dest)
        {
            int ret = 0;
            double dCurrent = 0;

            XyCoordinate source = new XyCoordinate();
            if (m_XAxis.UID == axis.UID)
            {
                source.X = dPos;
                GetInterpolationActualPosition(m_YAxis, ref dCurrent);
                //GetInterpolationCommandPosition(m_YAxis, ref dCurrent);
                source.Y = dCurrent;
            }
            else
            {
                GetInterpolationActualPosition(m_XAxis, ref dCurrent);
                //GetInterpolationCommandPosition(m_XAxis, ref dCurrent);
                source.Y = dPos;
                source.X = dCurrent;
            }

            if ((ret = Interpolate(source, ref dest)) != 0)
            {
                return ret;
            }

            return ret;
        }

        public int GetInterpolationActualPosition(MotionAxis axis, ref double dPos)
        {
            int ret = 0;
            double dCurrent = 0;
            XyCoordinate source = new XyCoordinate();
            XyCoordinate dest = new XyCoordinate();
            if (m_XAxis == null || m_YAxis == null)
            {
                return -1;
            }

            m_XAxis.GetActualPosition(ref dCurrent);
            dest.X = dCurrent;
            m_YAxis.GetActualPosition(ref dCurrent);
            dest.Y = dCurrent;

            if ((ret = ReverseInterpolate(dest, ref source)) != 0)
            {
                return ret;
            }

            if (m_XAxis.UID == axis.UID)
            {
                dPos = source.X;
            }
            else
            {
                dPos = source.Y;
            }

            return ret;
        }

        public int GetInterpolationCommandPosition(MotionAxis axis, ref double dPos)
        {
            int ret = 0;
            double dCurrent = 0;
            XyCoordinate source = new XyCoordinate();
            XyCoordinate dest = new XyCoordinate();
            if (m_XAxis == null || m_YAxis == null)
            {
                return -1;
            }

            m_XAxis.GetCommandPosition(ref dCurrent);
            dest.X = dCurrent;
            m_YAxis.GetCommandPosition(ref dCurrent);
            dest.Y = dCurrent;

            if ((ret = ReverseInterpolate(dest, ref source)) != 0)
            {
                return ret;
            }

            if (m_XAxis.UID == axis.UID)
            {
                dPos = source.X;
            }
            else
            {
                dPos = source.Y;
            }

            return ret;
        }

        public List<PositionOffset> GetPositionOffsetList()
        {
            return this.m_listMapData;
        }

        public void SetPositionOffsetList(List<PositionOffset> positions)
        {
            this.m_listMapData = positions;
        }

        public int GetPositionOffsetCount()
        {
            return this.m_listMapData.Count;
        }
    }
}
