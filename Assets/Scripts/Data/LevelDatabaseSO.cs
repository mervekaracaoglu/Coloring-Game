using System.Collections.Generic;
using UnityEngine;

namespace ColoringApp.Data
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "ColoringApp/LevelDatabase")]
    public class LevelDatabaseSO : ScriptableObject
    {
        public List<LevelDataSO> levels = new List<LevelDataSO>();
    }
}
