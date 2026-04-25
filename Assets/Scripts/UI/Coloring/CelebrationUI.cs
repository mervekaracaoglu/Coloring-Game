using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ColoringApp.UI.Coloring
{
    public class CelebrationUI : MonoBehaviour
    {
        [SerializeField] private GameObject celebrationPanel;
        [SerializeField] private RectTransform panelRect;
        [SerializeField] private Button backToLevelsButton;
        [SerializeField] private GameObject inputBlocker;
        [SerializeField] private ParticleSystem confettiParticles;

        private void Start()
        {
            celebrationPanel.SetActive(false);
            if (inputBlocker != null) inputBlocker.SetActive(false);

            backToLevelsButton.onClick.AddListener(ReturnToMainScene);
        }

        public void ShowCelebration()
        {
            if (inputBlocker != null) inputBlocker.SetActive(true);
            
            celebrationPanel.SetActive(true);
            
            // Start scale small
            panelRect.localScale = Vector3.zero;
            
            // DOTween bounce animation
            panelRect.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack);

            // Play Confetti
            if (confettiParticles != null)
            {
                confettiParticles.Play();
            }
        }

        private void ReturnToMainScene()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
