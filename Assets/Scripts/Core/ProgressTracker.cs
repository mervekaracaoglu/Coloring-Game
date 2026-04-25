using System.Threading.Tasks;
using UnityEngine;

namespace ColoringApp.Core
{
    public class ProgressTracker : MonoBehaviour
    {
        private int _totalColorablePixels = 0;
        private int _currentColoredPixels = 0;

        public float CompletionPercentage => _totalColorablePixels == 0 ? 0 : (float)_currentColoredPixels / _totalColorablePixels;

        //analyzes the original outline texture to count how many pixels are not outline.
        //calculates the initial colored pixels if a saved texture is loaded.
        public async Task InitializeAsync(Color32[] originalPixels, Color32[] currentPixels)
        {
            _totalColorablePixels = 0;
            _currentColoredPixels = 0;

            await Task.Run(() =>
            {
                for (int i = 0; i < originalPixels.Length; i++)
                {
                    if (!IsOutline(originalPixels[i]))
                    {
                        _totalColorablePixels++;
                        //if current pixel doesn't match original, it means it's colored
                        if (!ColorsExactlyMatch(originalPixels[i], currentPixels[i]))
                        {
                            _currentColoredPixels++;
                        }
                    }
                }
            });
            
            Debug.Log($"Progress initialized: {_currentColoredPixels}/{_totalColorablePixels} colored.");
        }

        public void AddColoredPixels(int count)
        {
            _currentColoredPixels += count;
        }

        private bool IsOutline(Color32 c)
        {
            if (c.a < 100) return false;
            float brightness = (c.r + c.g + c.b) / (3f * 255f);
            return brightness < 0.5f;
        }

        private bool ColorsExactlyMatch(Color32 c1, Color32 c2)
        {
            return c1.r == c2.r && c1.g == c2.g && c1.b == c2.b && c1.a == c2.a;
        }
    }
}
