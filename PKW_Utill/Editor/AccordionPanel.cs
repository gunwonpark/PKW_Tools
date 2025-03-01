using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PKW_Tool
{
    [RequireComponent(typeof(LayoutGroup))]
    public class AccordionPanel : MonoBehaviour
    {
        [SerializeField] private List<AccordionItem> items = new();

#if DOTWEEN_EXISTS
        [field: SerializeField] public float EaseDuration { get; private set; } = 0.3f;
        [field: SerializeField] public DG.Tweening.Ease EaseOut { get; private set; } = DG.Tweening.Ease.OutCubic;
        [field: SerializeField] public DG.Tweening.Ease EaseIn { get; private set; } = DG.Tweening.Ease.InCubic;
#endif
        private void Start()
        {
            FindAllItems();

            foreach (var item in items)
            {
                item.Initialize(this);
            }
        }

        private void FindAllItems()
        {
            items.Clear();
            foreach (Transform child in transform)
            {
                var item = child.GetComponent<AccordionItem>();
                if (item != null)
                {
                    items.Add(item);
                }
            }
        }
    }
}
