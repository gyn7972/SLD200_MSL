using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Equipment;

namespace QMC.Common.Q_Recipe
{
    public class RecipeManager
    {
        //사용방법
        // 열기
        //RecipeManager.Instance.OpenRecipe(@"D:\Recipe\MyRecipe.ini");

        //// 저장(동일 경로 덮어쓰기)
        //RecipeManager.Instance.SaveRecipe();

        //// 다른 경로로 저장
        //RecipeManager.Instance.SaveRecipe(@"D:\Recipe\MyRecipe_copy.ini");

        //// 특정 레이어 데이터만 확인/갱신
        //var r = RecipeManager.Instance.GetLayerRecipeData("Hole1");
        //var p = r.LayerData;
        //// p 값 조정 후 SaveRecipe() 호출하면 INI 반영됨

        private static RecipeManager _instance;
        public static RecipeManager Instance => _instance ?? (_instance = new RecipeManager());

        public string CurrentRecipePath { get; private set; }
        private readonly object _sync = new object();

        private RecipeManager() { }

        /// <summary>
        /// Recipe 파일을 열고 장비 데이터에 반영
        /// </summary>
        public void OpenRecipe(string recipePath)
        {
            if (string.IsNullOrEmpty(recipePath) || !File.Exists(recipePath))
                throw new FileNotFoundException("Recipe 파일을 찾을 수 없습니다.", recipePath);

            lock (_sync)
            {
                // Recipe 로드
                Recipe_Data_Load_Refactory(recipePath); // 기존 로더 유지  :contentReference[oaicite:2]{index=2}

                Equipment.Current_Recipe = recipePath;
                Equipment.Current_DrawingFileName = Path.GetFileName(recipePath);
                CurrentRecipePath = recipePath;
            }
        }

        /// <summary>
        /// 현재 Equipment.stLayerRecipeSet 내용을 INI로 저장 (경로 생략 시 현재 경로에 덮어쓰기)
        /// </summary>
        public void SaveRecipe(string recipePath = null)
        {
            lock (_sync)
            {
                var path = string.IsNullOrWhiteSpace(recipePath) ? CurrentRecipePath : recipePath;
                if (string.IsNullOrWhiteSpace(path))
                    throw new InvalidOperationException("저장 경로가 지정되지 않았습니다.");

                var dir = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentException("유효하지 않은 경로입니다.", nameof(recipePath));
                Directory.CreateDirectory(dir);

                var bak = path + ".bak";
                var tmp = path + ".tmp";

                // INI 내용 작성
                var sb = new StringBuilder(4096);
                var inv = CultureInfo.InvariantCulture;
                int layerCount = (int)Enum.GetValues(typeof(Equipment.LayerList)).Length; // :contentReference[oaicite:3]{index=3}

                for (int i = 0; i < layerCount; i++)
                {
                    var p = Equipment.stLayerRecipeSet[i]; // 저장 원본  :contentReference[oaicite:4]{index=4}
                    sb.AppendLine($"[Layer_{i}]");

                    // === Drawing & Laser ===
                    sb.AppendLine($"Drawing_File_Name={p.DrawingFile}");
                    sb.AppendLine($"Pulse_Width={p.LaserParam_PulseWidth.ToString(inv)}");
                    sb.AppendLine($"Pulse_Period={p.LaserParam_PulsePeriod.ToString(inv)}");
                    sb.AppendLine($"Frequency={p.LaserParam_Frequency.ToString(inv)}");
                    sb.AppendLine($"Duty_Cycle={p.LaserParam_DutyCycle.ToString(inv)}");
                    sb.AppendLine($"Trigger_Mode_External={p.LaserParam_TriggerMode_External.ToString(inv)}");
                    sb.AppendLine($"P2P={p.ProcessPriority_P2P.ToString(inv)}");

                    // === Miscellaneous (가공 공통) ===
                    sb.AppendLine($"Reference_Layer={p.Miscellaneous_ReferenceLayer}");
                    sb.AppendLine($"Defocusing_Distance={p.Miscellaneous_DefocusingDistance.ToString(inv)}");
                    sb.AppendLine($"Resizing={p.Miscellaneous_Resizing.ToString(inv)}");
                    sb.AppendLine($"HoleDrilling_StartPosDivision={p.Miscellaneous_HoleDrilling_StartPosDivision.ToString(inv)}");
                    sb.AppendLine($"GroupSplitSize={p.Miscellaneous_GroupSplitSize.ToString(inv)}");
                    sb.AppendLine($"GroupSplitSize_Height={p.Miscellaneous_GroupSplitSize_Height.ToString(inv)}");
                    sb.AppendLine($"ScannerDrillingSpeed={p.Miscellaneous_ScannerDrillingSpeed.ToString(inv)}");
                    sb.AppendLine($"ScannerJumpSpeed={p.Miscellaneous_ScannerJumpSpeed.ToString(inv)}");
                    sb.AppendLine($"LaserOnDelay={p.Miscellaneous_LaserOnDelay.ToString(inv)}");
                    sb.AppendLine($"LaserOffDelay={p.Miscellaneous_LaserOffDelay.ToString(inv)}");
                    sb.AppendLine($"MarkDelay={p.Miscellaneous_MarkDelay.ToString(inv)}");
                    sb.AppendLine($"JumpDelay={p.Miscellaneous_JumpDelay.ToString(inv)}");
                    sb.AppendLine($"PolygonDelay={p.Miscellaneous_PolygonDelay.ToString(inv)}");
                    sb.AppendLine($"DrillingPower={p.Miscellaneous_Drilling_Power.ToString(inv)}");
                    sb.AppendLine($"P2PDistance={p.Miscellaneous_P2PDistance.ToString(inv)}");
                    // 주의: 로더가 "DrillingRepetation" 철자 사용중  :contentReference[oaicite:5]{index=5}
                    sb.AppendLine($"DrillingRepetation={p.Miscellaneous_DrillingRepetition.ToString(inv)}");
                    sb.AppendLine($"DrillingRepetitionBundle={p.Miscellaneous_DrillingRepetitionBundle.ToString(inv)}");
                    sb.AppendLine($"RotationAngleArc={p.Miscellaneous_RotationAngleArc.ToString(inv)}");
                    sb.AppendLine($"RotationStartAngle_Circle1time={p.Miscellaneous_CircleStartAngleCircle1time.ToString(inv)}");
                    sb.AppendLine($"MaskIndex={p.Miscellaneous_MaskIndex.ToString(inv)}");
                    sb.AppendLine($"BETPositionIndex={p.Miscellaneous_BETPositionIndex.ToString(inv)}");
                    sb.AppendLine($"HoleProcessingType={p.Miscellaneous_HoleProcessingType.ToString(inv)}");
                    sb.AppendLine($"HoleSortByDistance_Use={p.Miscellaneous_HoleSortByDistance_Use.ToString(inv)}");
                    sb.AppendLine($"HoleDataSortingDistance={p.Miscellaneous_HoleSortingDistance.ToString(inv)}");

                    // === Process Option ===
                    sb.AppendLine($"Socket_Align_Use={p.ProcessOption_SocketAlign_Use.ToString(inv)}");
                    sb.AppendLine($"Socket_HeightCheck_Use={p.ProcessOption_SocketHeightCheck_Use.ToString(inv)}");
                    sb.AppendLine($"Socket_HeightCheckPos_OffsetX={p.ProcessOption_SocketHeightCheckPos_OffsetX.ToString(inv)}");
                    sb.AppendLine($"Socket_HeightCheckPos_OffsetY={p.ProcessOption_SocketHeightCheckPos_OffsetY.ToString(inv)}");
                    sb.AppendLine($"GoldPowder_Use={p.ProcessOption_GoldPowderAlign_Use.ToString(inv)}");

                    // === Module Info ===
                    sb.AppendLine($"Module_Width={p.ModuleInformation_Module_Width.ToString(inv)}");
                    sb.AppendLine($"Module_Height={p.ModuleInformation_Module_Height.ToString(inv)}");
                    sb.AppendLine($"Module_SiliconThickness={p.ModuleInformation_Silicon_Thickness.ToString(inv)}");
                    sb.AppendLine($"Module_GoldPowderThickness={p.ModuleInformation_GoldPowder_Thickness.ToString(inv)}");
                    sb.AppendLine($"Module_GoldPowderPercent={p.ModuleInformation_GoldPowder_Percent.ToString(inv)}");

                    // === Spiral ===
                    sb.AppendLine($"Spiral_OuterDiameter={p.SpiralParam_OuterDiameter.ToString(inv)}");
                    sb.AppendLine($"Spiral_InnerDiameter={p.SpiralParam_InnerDiameter.ToString(inv)}");
                    sb.AppendLine($"Spiral_Revolutions={p.SpiralParam_Revolutions.ToString(inv)}");
                    sb.AppendLine($"Spiral_AngleFactor={p.SpiralParam_AngleFactor.ToString(inv)}");

                    // === EPRO ===
                    sb.AppendLine($"EPRO_ModuleAbsorptionLevel={p.EPRO_ModuleAbsorptionLevel.ToString(inv)}");

                    // === M-Aligner Vacuum ===
                    sb.AppendLine($"MAlignerVacuumUse_Ignore={p.MAligner_VacuumPos_Ignore.ToString(inv)}");
                    sb.AppendLine($"MAlignerVacuumUse_Center={p.MAligner_VacuumPos_Center.ToString(inv)}");
                    sb.AppendLine($"MAlignerVacuumUse_Inner={p.MAligner_VacuumPos_Inner.ToString(inv)}");
                    sb.AppendLine($"MAlignerVacuumUse_Outer={p.MAligner_VacuumPos_Outer.ToString(inv)}");

                    // === Dust Collector ===
                    sb.AppendLine($"DustCollector_RemoteMode_Use={p.DustCollectorRemoteMode_Use.ToString(inv)}");
                    sb.AppendLine($"DustCollector_Frequency_Upper={p.DustCollectorFreq_Upper.ToString(inv)}");
                    sb.AppendLine($"DustCollector_Frequency_Lower={p.DustCollectorFreq_Lower.ToString(inv)}");
                    sb.AppendLine($"DustCollector_Lower_Disable={p.DustCollectorLower_Disable.ToString(inv)}");

                    // === Cal / Options ===
                    sb.AppendLine($"ZCalFile_OffsetZ={p.CalfileOffsetZAxismm.ToString(inv)}");
                    sb.AppendLine($"ChuckMSL_Use={p.ChuckMSL_Enable.ToString(inv)}");
                    sb.AppendLine($"Align3Point_Enable={p.Align3Point_Enable.ToString(inv)}");

                    // === Marking Template ===
                    sb.AppendLine($"MarkingData_SiriusTemplate_Use={p.MarkingData_SiriusTemplate_Use.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_DataType={p.MarkingTemplate_EntityData_DataType.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_Width={p.MarkingTemplate_EntityData_Width.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_Height={p.MarkingTemplate_EntityData_Height.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_TextType={p.MarkingTemplate_EntityData_TextType.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_PrefixData={p.MarkingTemplate_EntityData_PrefixData}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_StartNumber={p.MarkingTemplate_EntityData_StartNumber.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_Digits={p.MarkingTemplate_EntityData_Digits.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_IncreaseStep={p.MarkingTemplate_EntityData_IncreaseStep.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_SuffixData={p.MarkingTemplate_EntityData_SuffixData}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_Hatch_Use={p.MarkingTemplate_EntityData_Hatch_Use.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_Hatch_Spacing={p.MarkingTemplate_EntityData_Hatch_Spacing.ToString(inv)}");
                    sb.AppendLine($"MarkingData_SiriusTemplate_EntityData_SerialNumberType_IncreaseType={p.MarkingTemplate_EntityData_SerialNumberIncreaseType.ToString(inv)}");

                    sb.AppendLine(); // 섹션 간 공백
                }

                // 원자적 저장(임시 → 백업 → 본파일 교체)
                File.WriteAllText(tmp, sb.ToString(), Encoding.UTF8);

                if (File.Exists(path))
                {
                    // 기존을 .bak로 보존
                    File.Copy(path, bak, overwrite: true);
                }

                // 교체
                File.Copy(tmp, path, overwrite: true);
                File.Delete(tmp);

                // 상태 업데이트
                Equipment.Current_Recipe = path;
                CurrentRecipePath = path;
            }
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
            if (data.Index < 0)
                Log.Write("SLD-200", Equipment.User_Name, $"Layer [{layerName}] 데이터 없음");

            //if (data.Equals(default(stLayerRecipeParameter)))
            //    Log.Write("SLD-200", Equipment.User_Name, $"Layer [{layerName}] 데이터 없음");
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
