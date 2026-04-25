using ColoringApp.Core;
using ColoringApp.Data;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace ColoringApp.UI.LevelSelection
{
    public class LevelCell : MonoBehaviour
    {
        [SerializeField] private Image thumbnailImage;
        [SerializeField] private GameObject completionIndicator;
        [SerializeField] private Button cellButton;
        [SerializeField] private TextMeshProUGUI levelText;

        private LevelDataSO _levelData;
        private Action<LevelDataSO> _onLevelClicked;

        public void Setup(LevelDataSO levelData, Action<LevelDataSO> onClicked)
        {
            _levelData = levelData;
            _onLevelClicked = onClicked;

            if (thumbnailImage != null && _levelData.thumbnail != null)
            {
                thumbnailImage.sprite = _levelData.thumbnail;
            }

            if (completionIndicator != null)
            {
                bool isComplete = SaveManager.IsLevelComplete(_levelData.levelId);
                completionIndicator.SetActive(isComplete);
            }

            if (levelText != null)
            {
                levelText.text = "Level " + _levelData.levelId;
            }

            if (cellButton != null)
            {
                cellButton.onClick.RemoveAllListeners();
                cellButton.onClick.AddListener(HandleClick);
            }
        }

        private void HandleClick()
        {
            _onLevelClicked?.Invoke(_levelData);
        }
    }
}
