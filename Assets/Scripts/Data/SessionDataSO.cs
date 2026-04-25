using UnityEngine;

namespace ColoringApp.Data
{
    //acts as a data bridge between scenes
    [CreateAssetMenu(fileName = "SessionData", menuName = "ColoringApp/SessionData")]
    public class SessionDataSO : ScriptableObject
    {
        [HideInInspector]
        public LevelDataSO currentLevel;
    }
}
