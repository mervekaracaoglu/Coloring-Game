using ColoringApp.Core;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ColoringApp.UI.Coloring
{
    public class SwatchUI : MonoBehaviour
    {
        [SerializeField] private Image colorImage;
        [SerializeField] private GameObject selectionHighlight;
        [SerializeField] private Button swatchButton;

        private Color _swatchColor;
        private int _index;
        private Action<int, Color> _onSelected;

        public void Setup(Color color, int index, Action<int, Color> onSelected)
        {
            _swatchColor = color;
            _index = index;
            _onSelected = onSelected;

            if (colorImage != null)
            {
                colorImage.color = color;
            }

            swatchButton.onClick.RemoveAllListeners();
            swatchButton.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            _onSelected?.Invoke(_index, _swatchColor);
        }

        public void SetSelected(bool isSelected, bool animate)
        {
            if (selectionHighlight != null)
            {
                selectionHighlight.SetActive(isSelected);
            }

            if (animate && isSelected)
            {
                // Small scale bounce using DOTween
                transform.DOKill();
                transform.localScale = Vector3.one;
                transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1f);
            }
        }
    }
}
