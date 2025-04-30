using Newtonsoft.Json;
using QMC.Common;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace QMC.Common
{
    [JsonObject]
    public class CompenMemory
    {
        public const int MaxIdx = 220/*75*/;
        public RowColCnt MaxCnt { get; set; }
        public Vector StartPos { get; set; }
        public Vector Offset { get; set; }
        public Vector[,] Measure { get; set; }
        public Vector[,] Pos { get; set; }
        public Vector[,] ViResult { get; set; }

        public CompenMemory()
        {
            this.MaxCnt = new RowColCnt(0, 0);
            this.StartPos = new Vector(0, 0);
            this.Offset = new Vector(0, 0);

            this.Measure = new Vector[MaxIdx, MaxIdx];
            this.Pos = new Vector[MaxIdx, MaxIdx];
            this.ViResult = new Vector[MaxIdx, MaxIdx];

            for (int r = 0; r < MaxIdx; r++)
            {
                for (int c = 0; c < MaxIdx; c++)
                {
                    this.Measure[r, c] = new Vector(0, 0);
                    this.Pos[r, c] = new Vector(0, 0);
                    this.ViResult[r, c] = new Vector(0, 0);
                }
            }
        }

        public void Init(RowColCnt maxCnt, Vector initPos, Vector gridOffset)
        {
            if (MaxIdx < maxCnt.Row) { maxCnt.Row = MaxIdx; }
            if (MaxIdx < maxCnt.Col) { maxCnt.Col = MaxIdx; }

            if (2 > maxCnt.Row) { maxCnt.Row = 2; }
            if (2 > maxCnt.Col) { maxCnt.Col = 2; }

            this.MaxCnt = maxCnt;
            this.StartPos = initPos;
            this.Offset = gridOffset;

            //  2023. 07. 18.  SCH : Logger 가 없으므로 주석처리
            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"MaxIdx : {MaxIdx}");
            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"this.MaxCnt({this.MaxCnt.Row}, {this.MaxCnt.Col})");
            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"this.StartPos({this.StartPos.X}, {this.StartPos.Y})");
            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"this.Offset({this.Offset.X}, {this.Offset.Y})");
        }

        public void CreatePosTable()
        {
            for (int r = 0; r < this.MaxCnt.Row; r++)
            {
                for (int c = 0; c < this.MaxCnt.Col; c++)
                {
                    //StageCompen__Modify
                    this.Pos[r, c].X = this.StartPos.X + (this.Offset.X * c);
                    this.Pos[r, c].Y = this.StartPos.Y + (this.Offset.Y * r);
                    //this.Pos[r, c].X = this.StartPos.X + (this.Offset.X * r);         //  2023. 07. 18.  SCH : 원래 주석
                    //this.Pos[r, c].Y = this.StartPos.Y + (this.Offset.Y * c);         //  2023. 07. 18.  SCH : 원래 주석

                    //  2023. 07. 18.  SCH : Logger 가 없으므로 주석처리
                    //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] CreatePosTable({r}, {c})_[{this.Pos[r, c].X}, {this.Pos[r, c].Y}]");
                }
            }
        }

        public void Clear()
        {
            for (int r = 0; r < MaxIdx; r++)
            {
                for (int c = 0; c < MaxIdx; c++)
                {
                    this.Measure[r, c].X = 0;
                    this.Measure[r, c].Y = 0;
                }
            }
        }

        #region Compensation value
        public Vector GetCompen(Vector pos)
        {
            var compen = new Vector(0, 0);

            if (2 > MaxCnt.Row || 2 > MaxCnt.Col) { return compen; }
            if (1.0 > Offset.X || 1.0 > Offset.Y) { return compen; }

            var idxMin = ToIdx(pos);
            var idxMax = new RowColCnt(idxMin.Row + 1, idxMin.Col + 1);
            if (0 > idxMin.Row || 0 > idxMin.Col) { return compen; }

            var posMin = this.Pos[idxMin.Row, idxMin.Col];
            var posMax = this.Pos[idxMax.Row, idxMax.Col];

            var dev = posMax - posMin;
            if (0.001 > Math.Abs(dev.X) || 0.001 > Math.Abs(dev.Y)) { return compen; }

            var weight = new Vector(0, 0);
            weight.X = (pos.X - posMin.X) / (posMax.X - posMin.X);
            weight.Y = (pos.Y - posMin.Y) / (posMax.Y - posMin.Y);

            var top = this.Measure[idxMax.Row, idxMin.Col] + ((this.Measure[idxMax.Row, idxMax.Col] - this.Measure[idxMax.Row, idxMin.Col]) * weight.X);
            var btm = this.Measure[idxMin.Row, idxMin.Col] + ((this.Measure[idxMin.Row, idxMax.Col] - this.Measure[idxMin.Row, idxMin.Col]) * weight.X);
            compen = btm + ((top - btm) * weight.Y);

            return (compen);
        }

        /// <summary>
        /// position이 해당하는 index값을 찾음..
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private RowColCnt ToIdx(Vector pos)
        {
            var idx = new RowColCnt(-1, -1);

            var minpos = new Vector(this.Pos[0, 0].X, this.Pos[0, 0].Y);
            var maxpos = new Vector(this.Pos[MaxCnt.Row - 1, MaxCnt.Col - 1].X, this.Pos[MaxCnt.Row - 1, MaxCnt.Col - 1].Y);

            if (pos.X < minpos.X) { pos.X = minpos.X; }
            else if (maxpos.X < pos.X) { pos.X = maxpos.X; }

            if (pos.Y < minpos.Y) { pos.Y = minpos.Y; }
            else if (maxpos.Y < pos.Y) { pos.Y = maxpos.Y; }

            double min, max;
            for (int i = 0; i < (MaxCnt.Col - 1); i++)
            {
                min = this.Pos[0, i].X;
                max = this.Pos[0, (i + 1)].X;

                if (min <= pos.X && pos.X <= max)
                {
                    idx.Col = i;
                    break;
                }
            }

            for (int i = 0; i < (MaxCnt.Row - 1); i++)
            {
                min = this.Pos[i, 0].Y;
                max = this.Pos[(i + 1), 0].Y;

                if (min <= pos.Y && pos.Y <= max)
                {
                    idx.Row = i;
                    break;
                }
            }

            return (idx);
        }
        #endregion

        #region File control
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CompenMemory Load(string fileName)
        {
            try
            {
                string filePath = Path.Combine(Def.ConfigPath, fileName);
                filePath = "D:\\CWA-150SA_Parameter\\" + fileName;

                DirectoryInfo di = new DirectoryInfo(Def.ConfigPath);
                if (!di.Exists) { di.Create(); }

                var compen = FileHelper.ReadFromJsonFile<CompenMemory>(filePath);
                if (null == compen)
                {
                    
                    return (new CompenMemory());
                }
                                
                return compen;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("config 경로 위치에서 맵 데이터 파일을 찾을 수 없습니다. \r\n\r\n[config\\gridStagecompen.json]");
                Log.Write(ex);
                MessageBox.Show("config 경로 위치에서 맵 데이터 파일을 찾을 수 없습니다. \r\n\r\n[ D:\\CWA-150SA_Parameter\\gridStagecompen.json ]", "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return new CompenMemory();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Save(string fileName)
        {
            string filePath = Path.Combine(Def.ConfigPath, fileName);
            DirectoryInfo di = new DirectoryInfo(Def.ConfigPath);
            if (!di.Exists) { di.Create(); }

            FileHelper.WriteToJsonFile<CompenMemory>(filePath, this);
        }
        #endregion
    }
}
