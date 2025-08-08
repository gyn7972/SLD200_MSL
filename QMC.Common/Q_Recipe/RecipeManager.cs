using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Equipment;

namespace QMC.Common.Q_Recipe
{
    public class RecipeManager
    {
        private static RecipeManager _instance;
        public static RecipeManager Instance => _instance ?? (_instance = new RecipeManager());

        public string CurrentRecipePath { get; private set; }

        private RecipeManager() { }

        /// <summary>
        /// Recipe 파일을 열고 장비 데이터에 반영
        /// </summary>
        public void OpenRecipe(string recipePath)
        {
            if (string.IsNullOrEmpty(recipePath) || !File.Exists(recipePath))
                throw new FileNotFoundException("Recipe 파일을 찾을 수 없습니다.", recipePath);

            // Recipe 로드
            Recipe_Data_Load_Refactory(recipePath);

            Equipment.Current_Recipe = recipePath;
            Equipment.Current_DrawingFileName = Path.GetFileName(recipePath);
            CurrentRecipePath = recipePath;
        }

        /// <summary>
        /// Recipe Layer 데이터를 새로고침
        /// </summary>
        public void RefreshRecipeData(string layerName)
        {
            if (string.IsNullOrEmpty(layerName))
                throw new ArgumentException("Layer 이름이 잘못되었습니다.", nameof(layerName));

            // Equipment 데이터는 GetLayerRecipeData로 접근
            var data = GetLayerRecipeData(layerName);
            if (data.Equals(default(stLayerRecipeParameter)))
                Log.Write("SLD-200", Equipment.User_Name, $"Layer [{layerName}] 데이터 없음");
        }

        private bool Recipe_Data_Load_Refactory(string strRecipeFile)
        {
            // 기존 FormNew_Recipe에 있던 Recipe_Data_Load_Refactory 내용을 이곳으로 이동
            // 파일 파싱 후 Equipment.stLayerRecipeSet, WorkStage 데이터 세팅
            if (string.IsNullOrWhiteSpace(strRecipeFile) || !File.Exists(strRecipeFile))
                return false;

            var iniLines = File.ReadAllLines(strRecipeFile);
            string currentSection = "";
            var sectionData = new Dictionary<string, Dictionary<string, string>>();

            foreach (var rawLine in iniLines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Substring(1, line.Length - 2);
                    if (!sectionData.ContainsKey(currentSection))
                        sectionData[currentSection] = new Dictionary<string, string>();
                }
                else if (currentSection != "")
                {
                    var kvp = line.Split(new[] { '=' }, 2);
                    if (kvp.Length == 2)
                    {
                        sectionData[currentSection][kvp[0].Trim()] = kvp[1].Trim();
                    }
                }
            }

            int layerCount = (int)System.Enum.GetValues(typeof(LayerList)).Length;
            for (int i = 0; i < layerCount; i++)
            {
                string section = $"Layer_{i}";

                if (!sectionData.ContainsKey(section))
                    continue;

                var data = sectionData[section];

                Equipment.stLayerRecipeSet[i].DrawingFile = ReadValue(data, "Drawing_File_Name", "");

                Equipment.stLayerRecipeSet[i].LaserParam_PulseWidth = ReadDouble(data, "Pulse_Width", 0);
                Equipment.stLayerRecipeSet[i].LaserParam_PulsePeriod = ReadDouble(data, "Pulse_Period", 0);
                Equipment.stLayerRecipeSet[i].LaserParam_Frequency = ReadInt(data, "Frequency", 0);
                Equipment.stLayerRecipeSet[i].LaserParam_DutyCycle = ReadDouble(data, "Duty_Cycle", 0.0);

                Equipment.stLayerRecipeSet[i].LaserParam_TriggerMode_External = ReadBool(data, "Trigger_Mode_External", false);
                Equipment.stLayerRecipeSet[i].ProcessPriority_P2P = ReadBool(data, "P2P", false);

                Equipment.stLayerRecipeSet[i].Miscellaneous_ReferenceLayer = ReadValue(data, "Reference_Layer", "");
                Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance = ReadDouble(data, "Defocusing_Distance", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing = ReadDouble(data, "Resizing", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision = ReadInt(data, "HoleDrilling_StartPosDivision", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize = ReadDouble(data, "GroupSplitSize", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize_Height = ReadDouble(data, "GroupSplitSize_Height", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed = ReadDouble(data, "ScannerDrillingSpeed", 10.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerJumpSpeed = ReadDouble(data, "ScannerJumpSpeed", 100.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOnDelay = ReadInt(data, "LaserOnDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOffDelay = ReadInt(data, "LaserOffDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_MarkDelay = ReadInt(data, "MarkDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_JumpDelay = ReadInt(data, "JumpDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_PolygonDelay = ReadInt(data, "PolygonDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Drilling_Power = ReadDouble(data, "DrillingPower", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_P2PDistance = ReadDouble(data, "P2PDistance", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetition = (short)ReadInt(data, "DrillingRepetation", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle = (short)ReadInt(data, "DrillingRepetitionBundle", 100);
                Equipment.stLayerRecipeSet[i].Miscellaneous_RotationAngleArc = ReadDouble(data, "RotationAngleArc", 360.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_CircleStartAngleCircle1time = ReadDouble(data, "RotationStartAngle_Circle1time", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex = ReadInt(data, "MaskIndex", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex = ReadInt(data, "BETPositionIndex", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType = ReadInt(data, "HoleProcessingType", 0);
                //Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialAlignType = ReadInt(data, "FiducialAlignType", 0);
                //Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialMarkType = ReadInt(data, "FiducialMarkType", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortByDistance_Use = ReadBool(data, "HoleSortByDistance_Use", true);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortingDistance = ReadDouble(data, "HoleDataSortingDistance", 0.5);

                Equipment.stLayerRecipeSet[i].ProcessOption_SocketAlign_Use = ReadBool(data, "Socket_Align_Use", false);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use = ReadBool(data, "Socket_HeightCheck_Use", false);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetX = ReadDouble(data, "Socket_HeightCheckPos_OffsetX", 0.0);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetY = ReadDouble(data, "Socket_HeightCheckPos_OffsetY", 0.0);

                Equipment.stLayerRecipeSet[i].ProcessOption_GoldPowderAlign_Use = ReadBool(data, "GoldPowder_Use", false);

                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Width = ReadDouble(data, "Module_Width", 125.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Height = ReadDouble(data, "Module_Height", 120.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness = ReadDouble(data, "Module_SiliconThickness", 0.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Thickness = ReadDouble(data, "Module_GoldPowderThickness", 0.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Percent = ReadDouble(data, "Module_GoldPowderPercent", 0.0);

                Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter = ReadDouble(data, "Spiral_OuterDiameter", 0.0);
                Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter = ReadDouble(data, "Spiral_InnerDiameter", 0.0);
                Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions = ReadInt(data, "Spiral_Revolutions", 10);
                Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor = ReadDouble(data, "Spiral_AngleFactor", 10.0);

                Equipment.stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel = ReadDouble(data, "EPRO_ModuleAbsorptionLevel", -40.0);

                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Ignore = ReadBool(data, "MAlignerVacuumUse_Ignore", false);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center = ReadBool(data, "MAlignerVacuumUse_Center", true);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner = ReadBool(data, "MAlignerVacuumUse_Inner", false);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer = ReadBool(data, "MAlignerVacuumUse_Outer", false);

                Equipment.stLayerRecipeSet[i].DustCollectorRemoteMode_Use = ReadBool(data, "DustCollector_RemoteMode_Use", false);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Upper = ReadDouble(data, "DustCollector_Frequency_Upper", 20.0);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Lower = ReadDouble(data, "DustCollector_Frequency_Lower", 20.0);
                Equipment.stLayerRecipeSet[i].DustCollectorLower_Disable = ReadBool(data, "DustCollector_Lower_Disable", false);
                Equipment.stLayerRecipeSet[i].CalfileOffsetZAxismm = ReadDouble(data, "ZCalFile_OffsetZ", 0.0);
                Equipment.stLayerRecipeSet[i].ChuckMSL_Enable = ReadBool(data, "ChuckMSL_Use", false);
                Equipment.stLayerRecipeSet[i].Align3Point_Enable = ReadBool(data, "Align3Point_Enable", false);

                //  Marking Template
                Equipment.stLayerRecipeSet[i].MarkingData_SiriusTemplate_Use = ReadBool(data, "MarkingData_SiriusTemplate_Use", false);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_DataType = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_DataType", 0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Width = ReadDouble(data, "MarkingData_SiriusTemplate_EntityData_Width", 5.0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Height = ReadDouble(data, "MarkingData_SiriusTemplate_EntityData_Height", 5.0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_TextType = ReadBool(data, "MarkingData_SiriusTemplate_EntityData_TextType", true);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_PrefixData = ReadValue(data, "MarkingData_SiriusTemplate_EntityData_PrefixData", "");
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_StartNumber = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_StartNumber", 1);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Digits = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_Digits", 3);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_IncreaseStep = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_IncreaseStep", 1);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SuffixData = ReadValue(data, "MarkingData_SiriusTemplate_EntityData_SuffixData", "");
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Use = ReadBool(data, "MarkingData_SiriusTemplate_Hatch_Use", false);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Spacing = ReadDouble(data, "MarkingData_SiriusTemplate_Hatch_Spacing", 0.1);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SerialNumberIncreaseType = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_SerialNumberType_IncreaseType", 0);
            }

            return true;
        }

        public LayerRecipeResult GetLayerRecipeData(string layerName)
        {
            // 기존 FormNew_Recipe의 Recipe_Data_Refresh에서 UI 의존 부분 제거 후 이곳으로 이동
            // Equipment 데이터만 세팅
            //  해당 Layer 의 데이터로 변경하여 표시
            int nIndex = -1;
            string layerPrefix = layerName.Length > 4 ? layerName.Substring(0, 4) : layerName;

            if (layerPrefix == "Hole")
            {
                string holeNumber = layerName.Substring(4);
                if (IsNumeric(holeNumber))
                {
                    int idx = Equipment.ToInt(holeNumber);
                    nIndex = (idx >= 1 && idx <= 50) ? idx - 1 : (int)LayerList.Hole1;
                }
            }
            else if (layerName == "Rect") nIndex = (int)LayerList.Rect;
            else if (layerName == "Outline") nIndex = (int)LayerList.Outline;
            else if (layerName == "Marking") nIndex = (int)LayerList.Marking;
            else if (layerName == "Fiducial") nIndex = (int)LayerList.Fiducial;
            else if (layerName == "Thruhole") nIndex = (int)LayerList.Thruhole;
            else if (layerName == "PreAlign") nIndex = (int)LayerList.PreAlign;

            if (nIndex < 0)
            {
                Log.Write("SLD-200", Equipment.User_Name, "Recipe_Data_Refresh - Fail.");
                return new LayerRecipeResult { Index = -1, LayerData = default, CommonData = default };
            }

            return new LayerRecipeResult
            {
                Index = nIndex,
                LayerData = Equipment.stLayerRecipeSet[nIndex],
                CommonData = Equipment.stLayerRecipeSet[(int)LayerList.Hole1] // 공통 데이터는 0번 Hole
            };
        }

        /// <summary>
        /// 자동 전환 시 Recipe 오픈 및 UI 새로고침
        /// </summary>
        public void AutoChangeRecipe(string recipePath)
        {
            OpenRecipe(recipePath);
            RefreshRecipeData("Hole1"); // 기본 첫 레이어
        }

        public bool IsNumeric(string input)
        {
            return double.TryParse(input, out _);
        }
        private string ReadValue(Dictionary<string, string> data, string key, string defaultValue)
            => data.TryGetValue(key, out var value) ? value : defaultValue;

        private int ReadInt(Dictionary<string, string> data, string key, int defaultValue)
            => int.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;

        private double ReadDouble(Dictionary<string, string> data, string key, double defaultValue)
            => double.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;

        private bool ReadBool(Dictionary<string, string> data, string key, bool defaultValue)
            => bool.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;

    }
}
