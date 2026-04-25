using ColoringApp.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColoringApp.UI.Coloring
{
    public class PaletteManager : MonoBehaviour
    {
        public event Action<Color> OnColorSelected;

        [SerializeField] private GameObject swatchPrefab;
        [SerializeField] private Transform paletteContainer;

        private List<SwatchUI> _swatches = new List<SwatchUI>();
        private int _selectedIdx = -1;

        public void Setup(Color[] paletteColors)
        {
            //clear existing children
            foreach (Transform child in paletteContainer)
            {
                Destroy(child.gameObject);
            }
            _swatches.Clear();

            int lastColorIndex = SaveManager.GetLastColorIndex();
            if (lastColorIndex >= paletteColors.Length) lastColorIndex = 0;

            for (int i = 0; i < paletteColors.Length; i++)
            {
                GameObject swatchObj = Instantiate(swatchPrefab, paletteContainer);
                SwatchUI swatch = swatchObj.GetComponent<SwatchUI>();
                if (swatch != null)
                {
                    swatch.Setup(paletteColors[i], i, HandleSwatchSelected);
                    _swatches.Add(swatch);
                    swatch.SetSelected(false, false);
                }
            }

            // Select last saved color or first one
            if (_swatches.Count > 0)
            {
                HandleSwatchSelected(lastColorIndex, paletteColors[lastColorIndex]);
            }
        }

        private void HandleSwatchSelected(int index, Color color)
        {
            if (_selectedIdx == index) return;

            if (_selectedIdx >= 0 && _selectedIdx < _swatches.Count)
            {
                _swatches[_selectedIdx].SetSelected(false, false);
            }

            _selectedIdx = index;
            _swatches[_selectedIdx].SetSelected(true, true);
            
            SaveManager.SaveLastColorIndex(index);
            OnColorSelected?.Invoke(color);
        }
    }
}
