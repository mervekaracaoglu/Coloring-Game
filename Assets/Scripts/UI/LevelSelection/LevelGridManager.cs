using ColoringApp.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColoringApp.UI.LevelSelection
{
    public class LevelGridManager : MonoBehaviour
    {
        [SerializeField] private LevelDatabaseSO levelDatabase;
        [SerializeField] private SessionDataSO sessionData;
        [SerializeField] private GameObject levelCellPrefab;
        [SerializeField] private Transform gridContainer;

        private void Start()
        {
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            //clear existing children
            foreach (Transform child in gridContainer)
            {
                Destroy(child.gameObject);
            }

            //ceate a cell for each level
            foreach (var level in levelDatabase.levels)
            {
                GameObject cellObj = Instantiate(levelCellPrefab, gridContainer);
                LevelCell cell = cellObj.GetComponent<LevelCell>();
                
                if (cell != null)
                {
                    cell.Setup(level, OnLevelSelected);
                }
            }
        }

        private void OnLevelSelected(LevelDataSO selectedLevel)
        {
            sessionData.currentLevel = selectedLevel;
            SceneManager.LoadScene("ColoringScene");
        }
    }
}
