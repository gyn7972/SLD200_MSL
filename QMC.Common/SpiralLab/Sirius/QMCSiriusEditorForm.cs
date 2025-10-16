using QMC.Common;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Forms;
using static QMC.Common.Equipment;

namespace SpiralLab.Sirius
{
    //public class QMCSiriusEditorForm : SiriusEditorForm
    public class QMCSiriusEditorForm : SiriusEditorForm
    {
        //CustomEditorForm
        public QMCSiriusEditorForm() : base()
        {
            
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
            // 우선순위에 따라 Layer 이름 순서를 동적으로 구성
            List<string> list = new List<string>();

            list.Add("Hole1");

            if (Count >= 4)
                list.Add("Thruhole");

            if (Count >= 5)
                list.Add("Outline");

            if (Count >= 6)
                list.Add("Marking");

            list.Add("Fiducial");
            list.Add("PreAlign");

            // 최종 개수에 맞춰 리스트 잘라내기
            if (Count < list.Count)
                list = list.Take(Count).ToList();

            var Document = this.Document;
            var layers = Document.Layers;

            for (int iter = 0; iter < Count; iter++)
            {
                if (layers.Count > iter)
                {
                    layers[iter].Name = list[iter];
                }
                else
                {
                    var layer = new Layer();
                    layer.Name = list[iter];
                    layers.Add(layer);
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

                //case Keys.Control | Keys.Alt | Keys.M:
                case Keys.Alt | Keys.M:
                    {
                        MoveToMarking();
                    }
                    break;
                case Keys.Alt | Keys.O:
                    {
                        MoveToOutline();
                    }
                    break;
                case Keys.Alt | Keys.D:
                    {
                        AutoDivide();
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
        private void MoveToOutline()
        {
            MoveToGroup("Outline");
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

        private void AutoDivide()
        {
            //AutoDivideBySize(10, 22); // 원하는 mm 단위 셀 크기 설정
            //AutoDivideByLayer("Outline", 10f, 22f); // 원하는 mm 단위 셀 크기 설정

            if(Equipment.m_bDivided)
            {
                // 현재 선택된 레이어 하나 가져오기
                var selectedLayer = doc.Layers.FirstOrDefault(l => l.IsSelected);
                if (selectedLayer == null)
                {
                    MessageBox.Show("선택된 레이어가 없습니다. 레이어를 먼저 선택하세요.", "Auto Divide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //var outlineRecipe = Equipment.stLayerRecipeSet[(int)LayerList.Outline];
                //if (outlineRecipe.Miscellaneous_GroupSplitSize <= 0 ||
                //    outlineRecipe.Miscellaneous_GroupSplitSize_Height <= 0)
                //{
                //    Log.Write("SiriusEditor", "Outline 레이어의 그룹 분할 크기가 유효하지 않습니다.");
                //    return;
                //}

                //float dSplitW = (float)outlineRecipe.Miscellaneous_GroupSplitSize;
                //float dSplitH = (float)outlineRecipe.Miscellaneous_GroupSplitSize_Height;
                float dSplitW = Equipment.m_fDividedX;
                float dSplitH = Equipment.m_fDividedY;
                AutoDivideByLayer(selectedLayer.Name, dSplitW, dSplitH); // 원하는 mm 단위 셀 크기 설정
                UnGroupAllDividedGroupsInSelectedLayer();

                //Group는 우석 막고. 
                //RegroupEntitiesByLastDividedRects(selectedLayer.Name);

            }
        }

        


        private void AutoDivideByLayer(string layerName, float cellWidth, float cellHeight)
        {
            var doc = this.Document;
            if (doc == null || doc.Layers == null)
            {
                MessageBox.Show("문서 또는 레이어 정보가 없습니다.", "Auto Divide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var layer = doc.Layers.FirstOrDefault(l => l.Name.Contains(layerName));
            if (layer == null)
            {
                MessageBox.Show($"Layer '{layerName}' 를 찾을 수 없습니다.", "Auto Divide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const float epsilon = 0.001f; // 겹침 방지용 미세 간격

            var entities = layer
                .Where(e => !(e is Group))
                .ToList();

            Equipment.LastDividedRects.Clear(); // 이전 셀 정보 제거

            foreach (var entity in entities)
            {
                if (entity?.BoundRect == null)
                    continue;

                var bounds = entity.BoundRect;

                // 새로 만든 함수로 Rect 리스트 계산
                var rectList = CalculateDividedRects(bounds, cellWidth, cellHeight);
                // 계산된 Rect들을 LastDividedRects에 추가 저장 (그리기/그룹핑용)
                Equipment.LastDividedRects.AddRange(rectList);

                //var entityCenter = bounds.Center;

                //int cols = (int)Math.Ceiling(bounds.Width / cellWidth);
                //int rows = (int)Math.Ceiling(bounds.Height / cellHeight);

                //float offsetX = entityCenter.X - (cols * cellWidth) / 2f + cellWidth / 2f;
                //float offsetY = entityCenter.Y + (rows * cellHeight) / 2f - cellHeight / 2f;

                //List<BoundRect> rectList = new List<BoundRect>();

                //for (int row = 0; row < rows; row++)
                //{
                //    for (int col = 0; col < cols; col++)
                //    {
                //        float centerX = offsetX + col * cellWidth;
                //        float centerY = offsetY - row * cellHeight;

                //        float left = centerX - cellWidth / 2f;
                //        float right = centerX + cellWidth / 2f;
                //        float bottom = centerY - cellHeight / 2f;
                //        float top = centerY + cellHeight / 2f;

                //        var rect = new BoundRect(left, top, right, bottom);
                //        //rectList.Add(new BoundRect(left, top, right, bottom));
                //        rectList.Add(rect);
                //        Equipment.LastDividedRects.Add(rect);
                //    }
                //}

                try
                {
                    if (rectList.Count > 0)
                    {
                        List<Group> groupList = new List<Group>();
                        //doc.Action.ActEntityDivide(new List<IEntity> { entity }, rectList);
                        doc.Action.ActEntityDivide(new List<IEntity> { entity }, rectList, groupList);
                    }
                        
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Divide Error: {ex.Message}", "Divide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsIntersecting(BoundRect a, BoundRect b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top < b.Bottom || a.Bottom > b.Top);
        }

        private bool IsFullyContained(BoundRect inner, BoundRect outer)
        {
            return inner.Left >= outer.Left &&
                   inner.Right <= outer.Right &&
                   inner.Top <= outer.Top &&
                   inner.Bottom >= outer.Bottom;
        }

        public void UnGroupAllDividedGroupsInSelectedLayer()
        {
            var doc = this.Document;
            var action = doc.Action;

            // 선택된 레이어 가져오기
            var selectedLayer = doc.Layers.FirstOrDefault(l => l.IsSelected);
            if (selectedLayer == null)
            {
                MessageBox.Show("선택된 레이어가 없습니다. 레이어를 먼저 선택하세요.", "UnGroup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<IEntity> toUngroup = new List<IEntity>();

            // 초기 그룹 수집
            foreach (var entity in selectedLayer)
            {
                if (entity is Group)
                    toUngroup.Add(entity);
            }

            // 반복적으로 그룹 해체
            while (toUngroup.Count > 0)
            {
                action.ActEntitySelect(toUngroup);
                action.ActEntityUngroup(toUngroup, selectedLayer);

                // 다시 그룹 찾기
                toUngroup.Clear();
                foreach (var entity in selectedLayer)
                {
                    if (entity is Group)
                        toUngroup.Add(entity);
                }
            }

            //Console.WriteLine($"[완료] 선택된 레이어 '{selectedLayer.Name}' 의 모든 그룹 해체");
        }

        private void RegroupEntitiesByLastDividedRects(string layerName)
        {
            var doc = this.Document;
            if (doc == null || Equipment.LastDividedRects.Count == 0)
                return;

            var layer = doc.Layers.FirstOrDefault(l => l.Name.Contains(layerName));
            if (layer == null) return;

            var allEntities = layer.Where(e => !(e is Group)).ToList();
            HashSet<IEntity> usedEntities = new HashSet<IEntity>();

            var sortedRects = Equipment.LastDividedRects
                .OrderByDescending(r => r.Top)
                .ThenBy(r => r.Left)
                .ToList();

            foreach (var rect in sortedRects)
            {
                var entitiesInRect = allEntities
                                    .Where(e => e?.BoundRect != null &&
                                                !usedEntities.Contains(e) &&
                                                IsFullyContained(e.BoundRect, rect))
                                    .ToList();

                if (entitiesInRect.Count > 0)
                {
                    doc.Action.ActEntityGroup(entitiesInRect, layer);
                    foreach (var e in entitiesInRect)
                        usedEntities.Add(e);
                }
            }
        }

        //객체를 따로 선택해서 영역 분할.
        private void AutoDivideBySize(float cellWidth, float cellHeight)
        {
            var doc = this.Document;
            var selectedEntities = doc.Action.SelectedEntity;

            if (selectedEntities == null || selectedEntities.Count == 0)
            {
                MessageBox.Show("Please select target entity first", "Auto Divide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var entity in selectedEntities)
            {
                if (entity == null || entity.BoundRect == null)
                    continue;

                var bounds = entity.BoundRect;
                float minX = Math.Min(bounds.Left, bounds.Right);
                float maxX = Math.Max(bounds.Left, bounds.Right);
                float minY = Math.Min(bounds.Bottom, bounds.Top);
                float maxY = Math.Max(bounds.Bottom, bounds.Top);

                int cols = Math.Max(1, (int)Math.Ceiling((maxX - minX) / cellWidth));
                int rows = Math.Max(1, (int)Math.Ceiling((maxY - minY) / cellHeight));

                List<BoundRect> rectList = new List<BoundRect>();
                List<Group> groupList = new List<Group>();

                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        float left = minX + col * cellWidth;
                        float right = left + cellWidth;
                        float bottom = minY + row * cellHeight;
                        float top = bottom + cellHeight;

                        BoundRect rect = new BoundRect(left, top, right, bottom);

                        // 셀 영역이 대상 엔티티와 교차되는 경우만 추가
                        if (entity.BoundRect.HitTest(rect, 0))
                        {
                            rectList.Add(rect);
                        }
                    }
                }

                try
                {
                    if (rectList.Count > 0)
                    {
                        //doc.Action.ActEntityDivide(new List<IEntity> { entity }, rectList);
                        doc.Action.ActEntityDivide(new List<IEntity> { entity }, rectList, groupList);
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Divide Error: {ex.Message}", "Divide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        /// <summary>
        /// BoundRect 기준으로 지정된 셀 크기로 격자 분할된 BoundRect 리스트를 계산합니다.
        /// </summary>
        private List<BoundRect> CalculateDividedRects(BoundRect bounds, float cellWidth, float cellHeight)
        {
            List<BoundRect> result = new List<BoundRect>();

            if (bounds == null || cellWidth <= 0 || cellHeight <= 0)
                return result;

            int cols = (int)Math.Ceiling(bounds.Width / cellWidth);
            int rows = (int)Math.Ceiling(bounds.Height / cellHeight);

            float offsetX = bounds.Center.X - (cols * cellWidth) / 2f + cellWidth / 2f;
            float offsetY = bounds.Center.Y + (rows * cellHeight) / 2f - cellHeight / 2f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float centerX = offsetX + col * cellWidth;
                    float centerY = offsetY - row * cellHeight;

                    float left = centerX - cellWidth / 2f;
                    float right = centerX + cellWidth / 2f;
                    float bottom = centerY - cellHeight / 2f;
                    float top = centerY + cellHeight / 2f;

                    result.Add(new BoundRect(left, top, right, bottom));
                }
            }

            return result;
        }

        /// <summary>
        /// 선택된 레이어 기준으로 분할 Rect만 계산하고 LastDividedRects에 저장합니다. Divide는 수행하지 않습니다.
        /// </summary>
        public void RefreshDividedRectsOnly(string layerName, float cellWidth, float cellHeight)
        {
            var doc = this.Document;
            if (doc == null || doc.Layers == null)
                return;

            var layer = doc.Layers.FirstOrDefault(l => l.Name.Contains(layerName));
            if (layer == null)
                return;

            Equipment.LastDividedRects.Clear();

            foreach (var entity in layer)
            {
                if (entity?.BoundRect == null || entity is Group)
                    continue;

                var rectList = CalculateDividedRects(entity.BoundRect, cellWidth, cellHeight);
                Equipment.LastDividedRects.AddRange(rectList);
            }
        }



    }
}
