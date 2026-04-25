using ColoringApp.Core;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ColoringApp.Coloring
{
    [RequireComponent(typeof(RawImage))]
    public class CanvasController : MonoBehaviour, IPointerClickHandler
    {
        public event Action<int> OnFillCompleted; //when fill finishes send the number of pixels filled

        [SerializeField] private RawImage canvasImage;
        
        private Texture2D _currentTexture; //editable version
        private Texture2D _originalOutlineTexture; //original black outline
        private Color32[] _currentPixels; //what the user modifies
        private Color32[] _originalOutlinePixels; //used as boundaries
    
        private Color32 _selectedColor = Color.red;
        private float _fillTolerance; //how similar color must be to fill
        private bool _isFilling = false;

        public void Initialize(Texture2D originalLineArt, Texture2D savedState, float fillTolerance)
        {
            _fillTolerance = fillTolerance;
            _originalOutlineTexture = originalLineArt;

            //save original pixels for boundary checking
            _originalOutlinePixels = _originalOutlineTexture.GetPixels32();

            //create a writable texture based on saved state
            _currentTexture = new Texture2D(originalLineArt.width, originalLineArt.height, TextureFormat.RGBA32, false);
            _currentTexture.filterMode = FilterMode.Point; //sharp pixels
            
            //if there is aved state load it else start fresh
            if (savedState != null)
            {
                _currentTexture.SetPixels32(savedState.GetPixels32());
            }
            else
            {
                _currentTexture.SetPixels32(_originalOutlinePixels);
            }
            _currentTexture.Apply();

            _currentPixels = _currentTexture.GetPixels32();
            canvasImage.texture = _currentTexture;
        }

        public void SetTargetColor(Color color)
        {
            _selectedColor = color;
        }

        public Color32[] GetCurrentPixels() => _currentPixels;
        public Color32[] GetOriginalOutlinePixels() => _originalOutlinePixels;

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (_isFilling) return; //prevent multiple fills at once
            if (_currentTexture == null) return;

            //converts screen click to pixel position in texture
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasImage.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            Rect rect = canvasImage.rectTransform.rect;
            float px = Mathf.Clamp01((localCursor.x - rect.x) / rect.width);
            float py = Mathf.Clamp01((localCursor.y - rect.y) / rect.height);

            int texX = Mathf.RoundToInt(px * _currentTexture.width);
            int texY = Mathf.RoundToInt(py * _currentTexture.height);

            int startIndex = texY * _currentTexture.width + texX;
            if (startIndex < 0 || startIndex >= _currentPixels.Length) return;

            Color32 targetColor = _currentPixels[startIndex];

            _isFilling = true;

            //execute flood fill on background thread (asynchronously)
            var result = await FloodFillAlgorithm.FillAsync(
                _currentPixels,
                _originalOutlinePixels,
                _currentTexture.width,
                _currentTexture.height,
                texX,
                texY,
                targetColor,
                _selectedColor,
                _fillTolerance
            );

            if (result.FilledPixelCount > 0)
            {
                _currentPixels = result.ModifiedPixels;
                _currentTexture.SetPixels32(_currentPixels);
                _currentTexture.Apply();
                
                OnFillCompleted?.Invoke(result.FilledPixelCount);
            }

            _isFilling = false;
        }
        public void SaveCurrentState(int levelId)
        {
            if (_currentTexture != null)
            {
                SaveManager.SaveTextureState(levelId, _currentTexture);
            }
        }
    }
}
