using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.VisionPart;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Optics.Leesos;
using SLD200_MSL;

namespace SLD200_MSL
{
    public static class FormManager
    {
        private static Dictionary<Part, Form> m_dicConfigForm = new Dictionary<Part, Form>();
        private static Dictionary<Part, Form> m_dicMaintForm = new Dictionary<Part, Form>();
        private static Dictionary<Part, Form> m_dicRecipeForm = new Dictionary<Part, Form>();
        private static Dictionary<Module, Form> m_dicIOForm = new Dictionary<Module, Form>();

        public delegate void UpdateRecipeEvent();
        public static event UpdateRecipeEvent UpdateRecipe;

        public static Form GetMaintForm(Part part)                  //  2022. 03. 30.  SCH : Maint 버튼을 누르면 여기로 오는군...
        {
            Form form = null;
            if (m_dicMaintForm.ContainsKey(part))
            {
                form = m_dicMaintForm[part];
            }
            else
            {
                if (part is NeedleBlock)
                {
                    form = new FormNeedleBlockMaint(part);
                }
                else if (part is WorkStage)                     //  Maint -> Module -> Laser Drilling
                {
                    form = new FormWorkStageParameterMaint((Module)part);
                }
                else if (part is Loader)
                {
                    form = new FormLoaderParameterMaint((Module)part);
                }
                else if (part is Unloader)
                {
                    form = new FormUnloaderParameterMaint((Module)part);
                }
                else if(part is TwoPointAligner)
                {
                    form = new FormTwoPointAlignerMaint(part);
                }
                else if (part is VisionCalibrator)
                {
                    form = new FormVisionCalibratorMaint(part);
                }
                else if (part is Revision)
                {
                    form = new FormRevisionMaint(part);
                } 
                else if(part is VisionCompensator)
                {
                    form = new FormVisionCompensatorMaint(part);
                }
                else if (part is ScannerCompensator)
                {
                    form = new FormScannerCompensatorMaint(part);
                }
                else if(part is JigAligner)
                {
                    form = new FormJigAlignerMaint(part);
                }
                //else if(part is CommonModule)
                //{
                //    form = new FormCommonModuleConfig((Module)part);
                //}
                //else if(part is LaserPitchMoveShotter)
                //{
                //    form = new FormLaserPitchMoveShotterMaint(part);
                //}
                else
                {
                    return form;
                }

                m_dicMaintForm.Add(part, form);
            }


            return form;

        }

        public static Form GetConfigurationForm(Part part)              //  2022. 03. 30.  SCH : Config 버튼을 누르면 여기로 오는군...
        {
            //  2022. 03. 25.  SCH : Config --> Module --> "StageLoader" 제외한 나머지 항목에 데이터 넣어야 함. (현재 null 로 리턴됨)

            Form form = null;
            if(m_dicConfigForm.ContainsKey(part))
            {
                form = m_dicConfigForm[part];
            }
            else
            {
                if (part is NeedleBlock)
                {
                    form = new FormNeedleBlockConfig((NeedleBlock)part);
                }
                else if (part is Camera) // Module
                {
                    form = new FormCameraConfig(part);
                }
                else if (part is WorkStage)                         //  Config -> Module -> Laser Drilling
                {
                    form = new FormWorkStageParameterConfig((Module)part);
                }
                else if (part is Vision)                            //  Config -> Module -> Vision
                {
                    form = new FormVisionParameterConfig((Module)part);
                }
                else if (part is Loader)
                {
                    form = new FormLoaderParameterConfig((Module)part);
                }
                else if (part is Unloader)
                {
                    form = new FormUnloaderParameterConfig((Module)part);
                }
                else if (part is Bds)
                {
                    form = new FormBdsParameterConfig((Module)part);
                }
                else if (part is CommonModule)
                {
                    form = new FormCommonModuleConfig((Module)part);
                }
                else if (part is VisionCalibrator)
                {
                    form = new FormVisionCalibratorConfig(part);
                }
                else if (part is VisionCompensator)
                {
                    form = new FormVisionCompensatorConfig(part);
                }
                else if (part is ScannerCompensator)
                {
                    form = new FormScannerCompensatorConfig(part);
                }
                else if (part is JigAligner)
                {
                    //form = new 
                    form = new FormJigAlignerMaint(part);
                }
                //else if(part is CommonModule)
                //{
                //    form = new FormCommonModuleConfig((Module)part);
                //}
                else
                {
                    return form;// = new Form();
                }

                m_dicConfigForm.Add(part, form);
            }

            return form;
        }

        public static Form GetRecipeForm(Part part)              //  2022. 04. 01.  SCH : Recipe 버튼을 누르면 여기로 오는군...
        {
            FormSubContentBase form = null;

            if (m_dicRecipeForm.ContainsKey(part))
            {
                form = m_dicRecipeForm[part] as FormSubContentBase;
            }
            else
            {
                if (part is WorkStage)                      //  Recipe -> Module -> Laser Drilling
                {
                    form = new FormWorkStageParameterRecipe((WorkStage)part);
                }
                else if(part is JigAligner)
                {
                    form = new FormJigAlignerRecipe((JigAligner)part);
                }
                else if (part is Loader)
                {
                    form = new FormLoaderParameterRecipe((Loader)part);
                }
                else if (part is Unloader)
                {
                    form = new FormUnloaderParameterRecipe((Unloader)part);
                }
                else if (part is ScannerCompensator)
                {
                    form = new FormScannerCompensatorRecipe((ScannerCompensator)part);
                }
                else
                {
                    return form;
                }

                UpdateRecipe += form.OnUpdateRecipe;
                m_dicRecipeForm.Add(part, form);
            }


            return form;
        }

        private static void form()
        {
            throw new NotImplementedException();
        }

        //public static Form GetIOForm(Module module)
        //{
        //    FormSubContentBase form = null;

        //    if (m_dicIOForm.ContainsKey(module))
        //    {
        //        form = m_dicIOForm[module] as FormSubContentBase;
        //    }
        //    else
        //    {
        //        if (module != null)
        //        {
        //            form = new FormModuleIO(module);
        //        }
        //        else
        //        {
        //            return form;
        //        }
        //        m_dicIOForm.Add(module, form);
        //    }


        //    return form;
        //}

        public static void FireUpdateRecipeEvent()
        {
            if (UpdateRecipe != null)
                UpdateRecipe();
        }

        public static Form GetOperationForm(Module module)
        {
            Form form = null;
            //? 모듈네임으로 비교 바꿔야할수도
            if (module.Name == OperationModuleName.SourceLoader.ToString())
            {
                form = new FormOperationSourceStage(module);
            }
            else if (module.Name == OperationModuleName.WorkStage.ToString())
            {
                form = new FormOperationWorkStage(module);
            }

            return form;
        }
    }
}
