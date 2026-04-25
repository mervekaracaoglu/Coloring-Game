using UnityEngine;

namespace ColoringApp.Data
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ColoringApp/GameSettings")]
    public class GameSettingsSO : ScriptableObject
    {
        public Color[] paletteColors = new Color[6]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan
        };
        
        [Range(0.1f, 1f)]
        public float completionThreshold = 0.9f;
        [Range(0f, 1f)]
        public float fillTolerance = 0.1f;
    }
}
