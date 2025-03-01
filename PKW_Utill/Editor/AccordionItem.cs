using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PKW_Tool
{
    [RequireComponent(typeof(LayoutElement))]
    public class AccordionItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _header;
        [SerializeField] private Button _toggleButton;

        private LayoutElement _layoutElement;
        private AccordionPanel _accordionPanel;

        private bool _isExpanded = false;

        private float _contentPreferredHeight = 0f;
        private float _headerHeight = 0f;

        public void Initialize(AccordionPanel parent)
        {
            _accordionPanel = parent;

            _layoutElement = GetComponent<LayoutElement>();
            if (_content == null)
            {
                Debug.LogError($"{name}: _content is not assigned!");
            }
            if (_toggleButton == null)
            {
                Debug.LogError($"{name}: _toggleButton is not assigned!");
            }
            if (_header == null)
            {
                Debug.LogError($"{name}: _header is not assigned!");
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);

            _contentPreferredHeight = LayoutUtility.GetPreferredHeight(_content);

            _headerHeight = _header.sizeDelta.y;

            _toggleButton.onClick.AddListener(Toggle);
            CollapseImmediate();  // 초기에 content들을 모두 닫아둔다.
        }

        private void Toggle()
        {
            if (_isExpanded)
            {
                Collapse();
            }
            else
            {
                Expand();
            }
        }

        public void Expand()
        {
#if DOTWEEN_EXISTS
            _content.DOScaleY(1, 0.3f).SetEase(Ease.OutCubic);

            float targetHeight = _contentPreferredHeight + _headerHeight;
            _layoutElement.DOPreferredSize(new Vector2(_layoutElement.preferredWidth, targetHeight), _accordionPanel.EaseDuration).SetEase(_accordionPanel.EaseOut);
#else
            _content.localScale = new Vector3(1, 1, 1);
            _layoutElement.preferredHeight = _contentPreferredHeight + _headerHeight;
#endif
            _isExpanded = true;
        }

        public void Collapse()
        {
#if DOTWEEN_EXISTS
            _content.DOScaleY(0, 0.3f).SetEase(Ease.InCubic);

            float targetHeight = _headerHeight;
            _layoutElement.DOPreferredSize(new Vector2(_layoutElement.preferredWidth, targetHeight), _accordionPanel.EaseDuration).SetEase(_accordionPanel.EaseIn);
#else
            _content.localScale = new Vector3(1, 0, 1);
            _layoutElement.preferredHeight = _headerHeight;
#endif
            _isExpanded = false;
        }

        public void CollapseImmediate()
        {
            _content.localScale = new Vector3(1, 0, 1);
            _layoutElement.preferredHeight = _headerHeight;
            _isExpanded = false;
        }

#if UNITY_EDITOR
        public void SetHeader(RectTransform header)
        {
            _header = header;
        }
        public void SetContent(RectTransform content)
        {
            _content = content;
        }

        public void SetToggleButton(Button toggleButton)
        {
            _toggleButton = toggleButton;
        }
#endif
    }
}