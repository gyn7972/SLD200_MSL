using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{

    public class MaterialCompansator : PatternMatchingVisionPart
    {

        public enum FunctionID
        {
            Train,
            Search,
        }

        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        public UvwzxyzStage Stage { set; get; }
        public MaterialCompansatorRecipe Recipe { set; get; }
        public IlluminationDataSet IlluminationData { set; get; }
        public XyCoordinateCollection OffsetCoordinates { set; get; }

        public MaterialCompansator(string strName) : base(strName)
        {
            Recipe = new MaterialCompansatorRecipe(this);
            OffsetCoordinates = new XyCoordinateCollection();
        }

        public override int Create()
        {
            int ret = base.Create();

            m_PatternMatchingTool.SubTools.Clear();
            m_PatternMatchingTool.SubTools.Add(m_RoiTrain);

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public int Train()
        {
            int ret = 0;
            
            if((ret = OnTrain(Recipe.TrainRoiStartLocation, Recipe.TrainRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return ret;
            }

            if (Recipe != null)
            {
                if (Recipe.PatternMatchingParameter == null)
                {
                    PatternMatchingParameters newParameter = new PatternMatchingParameters();
                    Recipe.PatternMatchingParameter = newParameter;
                }
                Recipe.PatternMatchingParameter.TrainImage = TrainImage;
                Owner.SaveRecipeData();
            }

            return ret;
        }
        public override void UpdateConfigData() //참고 : 오버라이드,, 파트 콜
        {
            DieUnloader dieUnloader = Owner as DieUnloader;
            if (dieUnloader != null)
            {
                if (Recipe.IlluminationDataSet == null)
                    Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                Recipe.IlluminationDataSet.SetIlluminationChannel(dieUnloader.Config.ListIlluminationChannel);
            }
        }
        public override void UpdateRecipeData()
        {
            DieUnloader dieUnloader = Owner as DieUnloader;
            if (dieUnloader != null)
            {
                if (dieUnloader.Recipe.MaterialCompansatorRecipe != null)
                {
                    this.Recipe = dieUnloader.Recipe.MaterialCompansatorRecipe;
                    if (this.Recipe.IlluminationDataSet != null)
                    {
                        this.Recipe = dieUnloader.Recipe.MaterialCompansatorRecipe;
                        IlluminationData = this.Recipe.IlluminationDataSet;
                    }
                    else
                    {
                        dieUnloader.Recipe.MaterialCompansatorRecipe = this.Recipe;
                        this.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                    }
                    this.Recipe.Init();
                }
            }
        }
        public PatternMatchingResult Search()
        {
            int ret = 0;
            
            if((ret = OnSearch(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return null;
            }

            return m_PatternMatchingTool.Result;
        }

        public override int OnWork()
        {
            int ret = 0;

            XyCoordinateCollection MaterialPos = Recipe.MatrialPos;
            
            DieUnloader dieUnLoader = Owner as DieUnloader;

            //XyztCoordinate ReferencePos = dieUnLoader.GetReferenceCoordinate();
            //XyzztCoordinate ReferencePos = dieUnLoader.GetReferenceCoordinate();
            UvwzxyzCoordinate ReferencePos = dieUnLoader.GetReferenceCoordinate();

            XyCoordinateCollection offsetDatas = new XyCoordinateCollection();

            XyCoordinate movePosition = new XyCoordinate();
            for (int i = 0; i < MaterialPos.Count; i++)
            {
                //movePosition = MaterialPos[i] + (XyCoordinate)ReferencePos;
                movePosition.X = MaterialPos[i].X + ReferencePos.U;
                movePosition.Y = MaterialPos[i].Y + ReferencePos.V;

                PatternMatchingResult result = null;
                XyCoordinate coodinate = new XyCoordinate();
                Stage.MovePosition(movePosition);
                result = Search();
                if (result == null)
                {
                    ret = -1;
                    break;
                }

                FireUpdateResult(result);
                //result.Values mm 변환

                coodinate.X = ((result.Values[0].X - this.Camera.Resolution.Width / 2) * dieUnLoader.Scale.X) - movePosition.X;
                coodinate.Y = ((result.Values[0].Y - this.Camera.Resolution.Height / 2) * dieUnLoader.Scale.Y) - movePosition.Y;

                offsetDatas.Add(coodinate);
            }

            OffsetCoordinates = offsetDatas;

            return ret;
        }

    }
}