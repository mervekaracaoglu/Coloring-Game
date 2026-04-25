using UnityEngine;

namespace ColoringApp.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ColoringApp/LevelData")]
    public class LevelDataSO : ScriptableObject
    {        
        public int levelId;
        public string levelName;
        public Sprite thumbnail;
        public Texture2D lineArt;
    }
}
