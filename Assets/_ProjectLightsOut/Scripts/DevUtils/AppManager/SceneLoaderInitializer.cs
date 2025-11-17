using UnityEngine;
using UnityEngine.UI;

namespace ProjectLightsOut.DevUtils
{
    public class SceneLoaderInitializer : MonoBehaviour
    {
        [Header("Fade Settings")]
        [SerializeField] private SceneLoader.FadeSettings fadeSettings = new SceneLoader.FadeSettings();
        
        [Header("UI References")]
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private LoadingScreen loadingScreen;
        
        [Header("Debug")]
        [SerializeField] private bool initializeOnStart = true;
        [SerializeField] private bool createDefaultFadeCanvas = false;

        private void Start()
        {
            if (initializeOnStart)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            // Create default fade canvas if needed
            if (fadeCanvasGroup == null && createDefaultFadeCanvas)
            {
                CreateDefaultFadeCanvas();
            }
            
            // Initialize the SceneLoader with settings
            SceneLoader.Initialize(fadeSettings, fadeCanvasGroup);
            
            Debug.Log($"SceneLoader initialized with fade in: {fadeSettings.fadeInDuration}s, " +
                     $"fade out: {fadeSettings.fadeOutDuration}s, " +
                     $"min loading time: {fadeSettings.minLoadingTime}s");
        }

        private void CreateDefaultFadeCanvas()
        {
            // Create a new GameObject for fade canvas
            GameObject fadeObj = new GameObject("FadeCanvas");
            Canvas canvas = fadeObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000; // High sorting order to be on top
            
            CanvasGroup canvasGroup = fadeObj.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            
            // Create a black image for the fade
            GameObject blackImageObj = new GameObject("BlackImage");
            blackImageObj.transform.SetParent(fadeObj.transform, false);
            
            Image blackImage = blackImageObj.AddComponent<Image>();
            blackImage.color = Color.black;
            
            // Stretch to fill screen
            RectTransform rectTransform = blackImageObj.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            
            fadeCanvasGroup = canvasGroup;
            
            Debug.Log("Created default fade canvas");
        }

        // Helper method to test the fade system
        [ContextMenu("Test Fade In")]
        public void TestFadeIn()
        {
            if (fadeCanvasGroup != null)
            {
                StartCoroutine(TestFadeInCoroutine());
            }
        }

        private System.Collections.IEnumerator TestFadeInCoroutine()
        {
            yield return SceneLoader.FadeToBlack(fadeSettings.fadeInDuration);
            yield return new WaitForSeconds(1f);
            yield return SceneLoader.FadeFromBlack(fadeSettings.fadeOutDuration);
        }
    }
}