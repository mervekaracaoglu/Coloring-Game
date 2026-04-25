using ColoringApp.Coloring;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ColoringApp.UI.Coloring
{
    public class NavigationUI : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private ColoringController coloringController;

        private void Start()
        {
            backButton.onClick.AddListener(HandleBack);
        }

        private void HandleBack()
        {
            // Save progress before leaving
            coloringController.SaveProgress();
            
            // Load MainScene
            SceneManager.LoadScene("MainScene");
        }
    }
}
