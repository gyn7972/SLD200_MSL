using QMC.Common;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SpiralLab.Sirius
{
    public class QMCSiriusEditorForm : SiriusEditorForm
    {
        public QMCSiriusEditorForm() : base()
        {
            // 생성자에서 필요한 초기화 작업 수행
        }
        protected override void Action_OnSelectedEntityChanged(IDocument doc, List<IEntity> list)
        {
            base.Action_OnSelectedEntityChanged(doc, list);
            foreach (var view in this.Document.Views)
            {
                if (this.InvokeRequired)
                {
                    Equipment.formMain.Invoke(new System.Action(() =>
                    {
                        this.Invalidate();
                        this.Refresh();

                    }));


                }
                //view.Render();
            }

        }

        void RenameNewLayer(int Count)
        {

            List<string> list = new List<string>();
            list.Add("Hole1");
            list.Add("Thruhole");
            list.Add("Fiducial");
            list.Add("PreAlign");
            list.Add("Outline");
            list.Add("Marking");
            if (Count > list.Count)
            {
                Count = list.Count;
            }
            var Document = this.Document;
            for (int iter = 0; iter < Count; iter++)
            {
                var l = Document.Layers;
                if (l.Count > iter)
                {
                    l[iter].Name = list[iter];
                }
                else
                {
                    var layer = new Layer();
                    layer.Name = list[iter];
                    l.Add(layer);
                }
            }
        }
        private void AddHoleLayer()
        {
            var Document = this.Document;
            var l = Document.Layers;
            var layer = new Layer();
            int NextNo = l.Where(t => t.Name.Contains("Hole")).Count() + 1;
            layer.Name = "Hole" + NextNo.ToString();
            l.Insert(NextNo - 1, layer);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F7)
            {
                Group();
            }
            if (keyData == Keys.F8)
            {
                UnGroup();
            }
            if (keyData == Keys.Delete)
            {
                var Document = this.Document;
                Document.Action.ActEntityDelete(Document.Action.SelectedEntity);
            }
            switch (keyData)
            {
                case Keys.Alt | Keys.D2:
                    {
                        RenameNewLayer(2);
                    }
                    break;
                case Keys.Alt | Keys.D3:
                    {
                        RenameNewLayer(3);
                    }
                    break;
                case Keys.Alt | Keys.D4:
                    {
                        RenameNewLayer(4);
                    }
                    break;
                case Keys.Alt | Keys.D5:
                    {
                        RenameNewLayer(5);
                    }
                    break;
                case Keys.Alt | Keys.D6:
                    {
                        RenameNewLayer(6);
                    }
                    break;
                case Keys.Control | Keys.Alt | Keys.H:
                    {
                        AddHoleLayer();
                    }
                    break;
                case Keys.Alt | Keys.H:
                    {
                        HoleGroup();
                    }
                    break;
                case Keys.Alt | Keys.T:
                    {
                        ThruholeGroup();

                    }
                    break;
                case Keys.Alt | Keys.F:
                    {
                        MoveToFiducial();
                    }
                    break;
                case Keys.Alt | Keys.P:
                    {
                        MoveToPreAlign();
                    }
                    break;

                case Keys.Control | Keys.Alt | Keys.M:
                    {
                        MoveToMarking();
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MoveToMarking()
        {
            MoveToGroup("Marking");
        }

        private void ThruholeGroup()
        {
            MoveToGroup("Thruhole");
            SortToLayer("Thruhole");


        }

        private void MoveToPreAlign()
        {
            MoveToGroup("PreAlign");
        }

        private void MoveToGroup(string Name)
        {

            try
            {
                var Document = this.Document;
                var l = Document.Layers;
                var layer = l.Where(t => t.Name.Contains(Name)).FirstOrDefault();
                MoveToGroup(Document, layer);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
        private void MoveToFiducial()
        {
            MoveToGroup("Fiducial");
        }
        private void SortToLayer(string Name)
        {
            try
            {
                var Document = this.Document;
                var l = Document.Layers;
                var layer = l.Where(t => t.Name.Contains(Name)).FirstOrDefault();
                if (layer != null)
                {
                    if (Document.Action.SelectedEntity != null)
                    {
                        if (Document.Action.SelectedEntity.Count > 0)
                        {
                            Document.Action.ActEntitySort(Document.Action.SelectedEntity, layer, EntitySort.TopToBottom);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
        private static void MoveToGroup(IDocument Document, Layer layer)
        {
            Document.Action.ActEntityCut(Document.Action.SelectedEntity);
            Document.Action.ActEntityPasteClone(layer);

        }

        private void UnGroup()
        {
            var Document = this.Document;

            Document.Action.ActEntityUngroup(Document.Action.SelectedEntity);
        }

        private void Group()
        {
            var Document = this.Document;
            Document.Action.ActEntityGroup(Document.Action.SelectedEntity);


        }

        private void HoleGroup()
        {
            MoveToGroup("Hole1");
        }

        private void SelectLayer(string strName)
        {
            var Document = this.Document;
            var l = Document.Layers;

            var layer = l.Where(t => t.Name.Contains(strName)).FirstOrDefault();
            if (layer is Layer Lay)
            {
                foreach (var v in l)
                {
                    v.IsSelected = false;
                }
                Lay.IsSelected = true;
            }
        }

    }
}
