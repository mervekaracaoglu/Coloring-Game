using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ColoringApp.Core
{
    public static class FloodFillAlgorithm
    {
        public struct FillResult
        {
            public Color32[] ModifiedPixels;
            public int FilledPixelCount;
        }

        //Execute flood fill on a background thread.
        public static async Task<FillResult> FillAsync(
            Color32[] currentPixels, 
            Color32[] originalOutlinePixels, 
            int width, 
            int height, 
            int startX, 
            int startY, 
            Color32 targetColor, 
            Color32 replacementColor, 
            float tolerance)
        {
            // create a copy of the current pixels to modify on the background thread
            Color32[] newPixels = new Color32[currentPixels.Length];
            System.Array.Copy(currentPixels, newPixels, currentPixels.Length);

            return await Task.Run(() => 
            {
                int filledCount = 0;
                
                // if the target color is the same as replacement, no need to fill
                if (ColorsMatch(targetColor, replacementColor, 0f))
                {
                    return new FillResult { ModifiedPixels = newPixels, FilledPixelCount = 0 };
                }

                // if starting pixel is an outline in the original texture, don't fill
                int startIndex = startY * width + startX;
                if (IsOutline(originalOutlinePixels[startIndex]))
                {
                    return new FillResult { ModifiedPixels = newPixels, FilledPixelCount = 0 };
                }

                Queue<int> q = new Queue<int>();
                q.Enqueue(startIndex);

                // using a boolean array for visited checks
                bool[] visited = new bool[width * height];
                visited[startIndex] = true;

                while (q.Count > 0)
                {
                    int currentIndex = q.Dequeue();
                    int x = currentIndex % width;
                    int y = currentIndex / width;

                    newPixels[currentIndex] = replacementColor;
                    filledCount++;

                    //check 4 neighbors
                    CheckNeighbor(x + 1, y, width, height, q, visited, newPixels, originalOutlinePixels, targetColor, tolerance);
                    CheckNeighbor(x - 1, y, width, height, q, visited, newPixels, originalOutlinePixels, targetColor, tolerance);
                    CheckNeighbor(x, y + 1, width, height, q, visited, newPixels, originalOutlinePixels, targetColor, tolerance);
                    CheckNeighbor(x, y - 1, width, height, q, visited, newPixels, originalOutlinePixels, targetColor, tolerance);
                }

                return new FillResult { ModifiedPixels = newPixels, FilledPixelCount = filledCount };
            });
        }

        private static void CheckNeighbor(
            int x, int y, int width, int height, 
            Queue<int> q, bool[] visited, 
            Color32[] currentPixels, Color32[] originalOutlinePixels, 
            Color32 targetColor, float tolerance)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;

            int index = y * width + x;
            if (visited[index]) return;

            // outline boundary check performed against original source texture 
            if (IsOutline(originalOutlinePixels[index]))
            {
                visited[index] = true;
                return;
            }

            // Must match the color we are trying to replace in the current texture
            if (ColorsMatch(currentPixels[index], targetColor, tolerance))
            {
                visited[index] = true;
                q.Enqueue(index);
            }
        }

        private static bool IsOutline(Color32 c)
        {
            // if the pixel is mostly transparent, it is empty space, not an outline.
            if (c.a < 100) return false;

            //if it is opaque, it is an outline if it is dark (black lines).
            float brightness = (c.r + c.g + c.b) / (3f * 255f);
            return brightness < 0.5f;
        }

        private static bool ColorsMatch(Color32 c1, Color32 c2, float tolerance)
        {
            float rDiff = Mathf.Abs(c1.r - c2.r) / 255f;
            float gDiff = Mathf.Abs(c1.g - c2.g) / 255f;
            float bDiff = Mathf.Abs(c1.b - c2.b) / 255f;
            
            // Basic color distance
            return (rDiff + gDiff + bDiff) / 3f <= tolerance;
        }
    }
}
