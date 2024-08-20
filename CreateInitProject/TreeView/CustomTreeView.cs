using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


namespace PKW
{

    internal class CustomTreeView : TreeViewWithTreeModel<TreeElement>
    {
        public event Action<IList<string>> OnSelectionChanged;


        public CustomTreeView(TreeViewState state, TreeModel<TreeElement> model)
            : base(state, model)
        {
            //경계선을 표시한다
            showBorder = true;

            Reload();
        }

        public override void OnGUI(Rect rect)
        {
            // Background
            // 다시 그리기 이벤트일시
            if (Event.current.type == EventType.Repaint)
                DefaultStyles.backgroundOdd.Draw(rect, false, false, false, false);

            // TreeView
            base.OnGUI(rect);
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            if (selectedIds.Count == 1)
            {
                var selectedItem = treeModel.GetAncestorNames(selectedIds[0]);
                OnSelectionChanged?.Invoke(selectedItem);
            }
        }
    }
}
