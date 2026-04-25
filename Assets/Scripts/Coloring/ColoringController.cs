using ColoringApp.Core;
using ColoringApp.Data;
using UnityEngine;

namespace ColoringApp.Coloring
{
    public class ColoringController : MonoBehaviour
    {
        [Header("Data References")]
        [SerializeField] private SessionDataSO sessionData;
        [SerializeField] private GameSettingsSO gameSettings;

        [Header("Controller References")]
        [SerializeField] private CanvasController canvasController;
        [SerializeField] private ProgressTracker progressTracker;
        
        [Header("UI References")]
        [SerializeField] private UI.Coloring.PaletteManager paletteManager;
        [SerializeField] private UI.Coloring.CelebrationUI celebrationUI;

        private bool _isLevelComplete;

        private async void Start()
        {
            if (sessionData.currentLevel == null)
            {
                Debug.LogError("No level selected! Please start from MainScene.");
                return;
            }

            _isLevelComplete = SaveManager.IsLevelComplete(sessionData.currentLevel.levelId);
            
            //setup Palette
            paletteManager.Setup(gameSettings.paletteColors);
            
            //initialize Canvas
            Texture2D savedTex = SaveManager.LoadTextureState(sessionData.currentLevel.levelId);
            canvasController.Initialize(sessionData.currentLevel.lineArt, savedTex, gameSettings.fillTolerance);
            
            //extract pixels from Canvas
            Color32[] currentPixels = canvasController.GetCurrentPixels();
            Color32[] originalPixels = canvasController.GetOriginalOutlinePixels();
            
            //initialize ProgressTracker
            await progressTracker.InitializeAsync(originalPixels, currentPixels);

            //subscribe to events
            canvasController.OnFillCompleted += HandleFillCompleted;
            paletteManager.OnColorSelected += HandleColorSelected;
        }

        private void OnDestroy()
        {
            if (canvasController != null)
            {
                canvasController.OnFillCompleted -= HandleFillCompleted;
            }
            if (paletteManager != null)
            {
                paletteManager.OnColorSelected -= HandleColorSelected;
            }
        }

        private void HandleColorSelected(Color color)
        {
            canvasController.SetTargetColor(color);
        }

        private void HandleFillCompleted(int newFilledPixelsCount)
        {
            if (_isLevelComplete) return;

            progressTracker.AddColoredPixels(newFilledPixelsCount);
            
            float percentage = progressTracker.CompletionPercentage;
            Debug.Log($"Level Completion: {percentage * 100f:F1}%");

            if (percentage >= gameSettings.completionThreshold)
            {
                CompleteLevel();
            }
        }

        private void CompleteLevel()
        {
            _isLevelComplete = true;
            Debug.Log("Level Complete!");
            
            SaveManager.SaveLevelComplete(sessionData.currentLevel.levelId);
            canvasController.SaveCurrentState(sessionData.currentLevel.levelId);

            celebrationUI.ShowCelebration();
        }



        public void SaveProgress()
        {
            if (sessionData.currentLevel != null && canvasController != null)
            {
                canvasController.SaveCurrentState(sessionData.currentLevel.levelId);
            }
        }
    }
}
