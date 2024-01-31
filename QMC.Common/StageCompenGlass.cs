using QMC.Core;
using System;
using System.Windows;

namespace QMC.Common
{
    //public partial class Stage
    public class Stage
    {
        #region << variable >>
        private int _compenCnt = 0;
        private int tagetMsg = 0;
        //private CompenMemory _compen = new CompenMemory();
        public CompenMemory _compenStage = new CompenMemory();

        //-- test temp
        public double[,] xErr = new double[63, 63];//new double[43, 43];
        public double[,] yErr = new double[63, 63];//new double[43, 43];

        //-- LCI Module 용 맵핑 데이터
        public double[,] LCI_xErr = new double[63, 63];//new double[43, 43];
        public double[,] LCI_yErr = new double[63, 63];//new double[43, 43];

        #endregion

        #region << public method >>
        //public bool StartGlassCompen(bool clear = false)
        //{
        //    if (Fsm.IsBusy())
        //        return false;

        //    if (clear)
        //        _compen.Clear();

        //    InitGlassCompen();
        //    Fsm.Set(Cmd.GlassCompen_Start, 0);
        //    return true;
        //}

        public bool StartGlassStageCompen(bool clear = false)
        {
            //if (Fsm.IsBusy())
            //    return false;

            if (clear)
            {
                _compenStage.Clear();
                //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] _compenStage.Clear()");
            }

            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] InitGlassStageCompen()");
            InitGlassStageCompen();
            //Fsm.Set(Cmd.GlassCompensation_Start);
            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Fsm.Set(Cmd.GlassCompensation_Start)");
            return true;
        }


        //public void ClearGlassCompen()
        //{
        //    _compen.Clear();
        //}
        //public void SaveGlassCompen()
        //{
        //    _compen.Save(Def.GlassCompenFile);
        //}
        //public void LoadGlassCompen()
        //{
        //    _compen = CompenMemory.Load(Def.GlassCompenFile);
        //}
        public void SaveGlassStageCompen()
        {
            _compenStage.Save(Def.GlassStageCompenFile);
        }

        public bool LoadGlassStageCompen()
        {
            _compenStage = CompenMemory.Load(Def.GlassStageCompenFile);

            if ((_compenStage.MaxCnt.Row == 0) || (_compenStage.MaxCnt.Col == 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region => cycle method
        private void GlassCompensationCycle()
        {
            //if (!Fsm.IsBusy(Cmd.GlassCompensation_Start, Cmd.GlassCompensation_End))
            //    return;
            //if (Fsm.IsTimeOverError(90))
            //{
            //    g.Error.Save(ErrorEnum.GlassCompenCycleTimeOver);
            //    Fsm.Set(Cmd.Error);
            //    return;
            //}

            //if (!IsAxesReady())
            //    return;

            //switch (Fsm.Get())
            //{
            //    case Cmd.GlassCompensation_Start:
            //        {
            //            if (Fsm.IsOnce)
            //            {
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Cmd.GlassCompensation_Start");

            //                _compenCnt = 0;
            //                Vector Pos = new Vector();
            //                //Pos.X = g.Dm[DmEnum.HighVisionCenterX];
            //                //Pos.Y = g.Dm[DmEnum.HighVisionCenterY];
            //                Pos.X = g.Dm[DmEnum.GlassStageCompenInitPosX];
            //                Pos.Y = g.Dm[DmEnum.GlassStageCompenInitPosY];
            //                g.AxesAcs[StageX].PMove((int)Position_StageXY.Teaching, Pos.X, 50);
            //                g.AxesAcs[StageY].PMove((int)Position_StageXY.Teaching, Pos.Y, 50);

            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] PMove({Pos.X}, {Pos.Y})");
            //            }
            //            else
            //            {
            //                if (!Fsm.IsElapsed(0.3))
            //                    break;
            //                Fsm.Set(Cmd.GlassCompensation_Move);
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Fsm.Set(Cmd.GlassCompensation_Move)");
            //            }
            //        }
            //        break;

            //    case Cmd.GlassCompensation_Move:
            //        {
            //            if (Fsm.IsOnce)
            //            {
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Cmd.GlassCompensation_Move");

            //                if (Fsm.IsCanceled(Cmd.Idle))
            //                    break;

            //                //StageCompen__Modify
            //                //var cnt = new RowColCnt((_compenCnt / _compenStage.MaxCnt.Row), (_compenCnt % _compenStage.MaxCnt.Row));
            //                //var cnt = new RowColCnt((_compenCnt % _compenStage.MaxCnt.Row), (_compenCnt / _compenStage.MaxCnt.Row));
            //                var cnt = new RowColCnt((_compenCnt / _compenStage.MaxCnt.Row), (_compenCnt % _compenStage.MaxCnt.Col));
            //                var pos = new Vector(_compenStage.Pos[cnt.Row, cnt.Col].X, _compenStage.Pos[cnt.Row, cnt.Col].Y);
            //                //var err = _compenStage.GetCompen(pos);

            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] _compenCnt : {_compenCnt}, _compenStage.MaxCnt.Row : {_compenStage.MaxCnt.Row}, _compenStage.MaxCnt.Col : {_compenStage.MaxCnt.Col}");
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] cnt({cnt.Row}, {cnt.Col})");

            //                g.AxesAcs[StageX].PMove((int)Position_StageXY.Teaching, (pos.X /*+ err.X*/), 50);
            //                g.AxesAcs[StageY].PMove((int)Position_StageXY.Teaching, (pos.Y /*+ err.Y*/), 50);

            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] PMove({_compenStage.Pos[cnt.Row, cnt.Col].X}, {_compenStage.Pos[cnt.Row, cnt.Col].Y})");
            //            }
            //            else
            //            {
            //                if (!Fsm.IsElapsed(0.5))
            //                    break;

            //                Fsm.Set(Cmd.GlassCompensation_VisionInspect);
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Fsm.Set(Cmd.GlassCompensation_VisionInspect)");
            //            }
            //        }
            //        break;

            //    case Cmd.GlassCompensation_VisionInspect:
            //        {
            //            if (Fsm.IsElapsed(30))
            //            {
            //                g.Error.Save(ErrorEnum.GlassInspectTimeOver);
            //                Fsm.Set(Cmd.Error);
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] ErrorEnum.GlassInspectTimeOver");
            //                break;
            //            }

            //            if (Fsm.IsOnce)
            //            {
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Cmd.GlassCompensation_VisionInspect");

            //                if (!g.Pgm.ServiceVision.Inspect(Shared.VisionProcess.HighVision_StageCal))
            //                {
            //                    g.Error.Save(ErrorEnum.FailGlassInspectectStart);
            //                    Fsm.Set(Cmd.Error);
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] ErrorEnum.FailGlassInspectectStart");
            //                }
            //            }
            //            else
            //            {

            //                if (g.Pgm.ServiceVision.IsError(Shared.VisionPart.High))
            //                {
            //                    g.Error.Save(ErrorEnum.FailGlassInspect);
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] ErrorEnum.FailGlassInspect");

            //                    //Fsm.Set(Cmd.Error);
            //                    //break;
            //                    var count = new RowColCnt((_compenCnt / _compenStage.MaxCnt.Row), (_compenCnt % _compenStage.MaxCnt.Row));
            //                    _compenStage.ViResult[count.Row, count.Col].X = 0;
            //                    _compenStage.ViResult[count.Row, count.Col].Y = 0;

            //                    _compenStage.Measure[count.Row, count.Col].X += 0;
            //                    _compenStage.Measure[count.Row, count.Col].Y += 0;

            //                    xErr[count.Row, count.Col] += 0 * -1.0;
            //                    yErr[count.Row, count.Col] += 0 * -1.0;

            //                    _compenCnt++;

            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] _compenCnt : {_compenCnt}, _compenStage.MaxCnt.Row : {_compenStage.MaxCnt.Row}, _compenStage.MaxCnt.Col : {_compenStage.MaxCnt.Col}");
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] cnt({count.Row}, {count.Col})");
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] ViResult({_compenStage.ViResult[count.Row, count.Col].X}, {_compenStage.ViResult[count.Row, count.Col].Y})");
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Measure({_compenStage.Measure[count.Row, count.Col].X}, {_compenStage.Measure[count.Row, count.Col].Y})");
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Err({xErr[count.Row, count.Col]}, {yErr[count.Row, count.Col]})");

            //                    int MaxCount = _compenStage.MaxCnt.Row * _compenStage.MaxCnt.Col;
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] MaxCount : {MaxCount}, _compenCnt : {_compenCnt}");
            //                    if (MaxCount <= _compenCnt)
            //                    {
            //                        Fsm.Set(Cmd.GlassCompensation_End);
            //                        Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Cmd.GlassCompensation_End");
            //                    }
            //                    else
            //                    {
            //                        Fsm.Set(Cmd.GlassCompensation_Move);
            //                        Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Cmd.GlassCompensation_Move");
            //                    }
            //                }

            //                if (!g.Pgm.ServiceVision.IsInspDone(Shared.VisionPart.High))
            //                    break;

            //                if (g.Pgm.ServiceVision.IsInspError(Shared.VisionPart.High, 5.0))
            //                {
            //                    g.Error.Save(ErrorEnum.GlassInspectResultErr);
            //                    Fsm.Set(Cmd.Error);
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] ErrorEnum.GlassInspectResultErr");
            //                    break;
            //                }

            //                var viResult = g.Pgm.ServiceVision.GetInspResult(Shared.VisionPart.High);
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] viResult : {viResult}");

            //                var cnt = new RowColCnt((_compenCnt / _compenStage.MaxCnt.Row), (_compenCnt % _compenStage.MaxCnt.Row));

            //                _compenStage.ViResult[cnt.Row, cnt.Col].X = Math.Round(viResult.X, 3) * -1.0;
            //                _compenStage.ViResult[cnt.Row, cnt.Col].Y = Math.Round(viResult.Y, 3) * -1.0;

            //                _compenStage.Measure[cnt.Row, cnt.Col].X += Math.Round(viResult.X, 3) * -1.0;
            //                _compenStage.Measure[cnt.Row, cnt.Col].Y += Math.Round(viResult.Y, 3) * -1.0;

            //                xErr[cnt.Row, cnt.Col] += Math.Round(viResult.X, 3) * -1.0;
            //                yErr[cnt.Row, cnt.Col] += Math.Round(viResult.Y, 3) * -1.0;

            //                _compenCnt++;

            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] _compenCnt : {_compenCnt}, _compenStage.MaxCnt.Row : {_compenStage.MaxCnt.Row}, _compenStage.MaxCnt.Col : {_compenStage.MaxCnt.Col}");
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] cnt({cnt.Row}, {cnt.Col})");
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] ViResult({_compenStage.ViResult[cnt.Row, cnt.Col].X}, {_compenStage.ViResult[cnt.Row, cnt.Col].Y})");
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Measure({_compenStage.Measure[cnt.Row, cnt.Col].X}, {_compenStage.Measure[cnt.Row, cnt.Col].Y})");
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Err({xErr[cnt.Row, cnt.Col]}, {yErr[cnt.Row, cnt.Col]})");

            //                int maxCnt = _compenStage.MaxCnt.Row * _compenStage.MaxCnt.Col;
            //                Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] maxCnt : {maxCnt}, _compenCnt : {_compenCnt}");

            //                if (maxCnt <= _compenCnt)
            //                {
            //                    Fsm.Set(Cmd.GlassCompensation_End);
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Fsm.Set(Cmd.GlassCompensation_End)");
            //                }
            //                else
            //                {
            //                    Fsm.Set(Cmd.GlassCompensation_Move);
            //                    Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Fsm.Set(Cmd.GlassCompensation_Move)");
            //                }
            //            }
            //        }
            //        break;

            //    case Cmd.GlassCompensation_End:
            //        {
            //            Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] Cmd.GlassCompensation_End");

            //            //_compenStage.Save(Def.GlassStageCompenFile);
            //            SaveGlassStageCompen();
            //            Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] SaveGlassStageCompen()");

            //            g.AxesAcs.SetStageGlassCal(xErr, yErr, (double)g.Dm[DmEnum.GlassStageCompenInitPosX],
            //                (double)g.Dm[DmEnum.GlassStageCompenInitPosY], (int)g.Dm[DmEnum.GlassStageCompenGridOffsetX], (int)g.Dm[DmEnum.GlassStageCompenGridOffsetY]);
            //            Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] g.AxesAcs.SetStageGlassCal({xErr}, {yErr}, {(double)g.Dm[DmEnum.GlassStageCompenInitPosX]}, {(double)g.Dm[DmEnum.GlassStageCompenInitPosY]}, {(double)g.Dm[DmEnum.GlassStageCompenGridOffsetX]}, {(double)g.Dm[DmEnum.GlassStageCompenGridOffsetY]})");

            //            var mb = new MessageBoxOk();
            //            mb.ShowDialog("End", $@"GlassCompensation_End", 5);

            //            Fsm.Set(Cmd.Idle);
            //        }
            //        break;
            //}
        }
        #endregion


        #region << private method >>
        /// <summary>
        /// Initalize parameter, create json file
        /// </summary>
        //private void InitGlassCompen()
        //{
        //    _compenCnt = 0;

        //    var maxcnt = new RowColCnt(g.Dm.ToInt(DmEnum.GlassCompenGridCntRow),
        //                               g.Dm.ToInt(DmEnum.GlassCompenGridCntCol));

        //    var initpos = new Vector(g.Dm[DmEnum.GlassCompenInitPosX], g.Dm[DmEnum.GlassCompenInitPosY]);
        //    var offset = new Vector(g.Dm[DmEnum.GlassCompenGridOffsetX], g.Dm[DmEnum.GlassCompenGridOffsetY]);

        //    _compen.Init(maxcnt, initpos, offset);
        //    _compen.CreatePosTable();
        //}

        private void InitGlassStageCompen()
        {
            //_compenCnt = 0;

            //var maxcnt = new RowColCnt(g.Dm.ToInt(DmEnum.GlassStageCompenGridCntRow),
            //                           g.Dm.ToInt(DmEnum.GlassStageCompenGridCntCol));

            //var initpos = new Vector(g.Dm[DmEnum.GlassStageCompenInitPosX], g.Dm[DmEnum.GlassStageCompenInitPosY]);
            //var offset = new Vector(g.Dm[DmEnum.GlassStageCompenGridOffsetX], g.Dm[DmEnum.GlassStageCompenGridOffsetY]);

            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] maxcnt({maxcnt.Row}, {maxcnt.Col})");
            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] initpos({initpos.X}, {initpos.Y})");
            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] offset({offset.X}, {offset.Y})");

            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] _compenStage.Init(maxcnt, initpos, offset)");
            //_compenStage.Init(maxcnt, initpos, offset);
            //Logger.Log(Logger.Module.Global, Logger.Type.Info, $@"[StageCompensator] _compenStage.CreatePosTable()");
            //_compenStage.CreatePosTable();
        }
        #endregion
    }
}
