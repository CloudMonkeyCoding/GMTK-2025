using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int maxBags = 10;

    private int activeBags = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterBag()
    {
        if (isGameOver) return;
        activeBags++;
        if (activeBags > maxBags)
        {
            TriggerGameOver();
        }
    }

    public void UnregisterBag()
    {
        activeBags = Mathf.Max(0, activeBags - 1);
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        var canvasGO = new GameObject("GameOverCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        if (UnityEngine.EventSystems.EventSystem.current == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Background panel
        var panelGO = new GameObject("Background");
        panelGO.transform.SetParent(canvasGO.transform, false);
        var panelImage = panelGO.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.5f);
        var panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        // Score text
        var scoreGO = new GameObject("ScoreText");
        scoreGO.transform.SetParent(canvasGO.transform, false);
        var scoreTMP = scoreGO.AddComponent<TextMeshProUGUI>();
        scoreTMP.alignment = TextAlignmentOptions.Center;
        scoreTMP.fontSize = 36f;
        scoreTMP.text = $"Score: {ScoreManager.Instance.Score}";
        var scoreRect = scoreTMP.rectTransform;
        scoreRect.anchorMin = new Vector2(0.5f, 0.6f);
        scoreRect.anchorMax = new Vector2(0.5f, 0.6f);
        scoreRect.anchoredPosition = Vector2.zero;

        // Hi-score text
        var hiGO = new GameObject("HiScoreText");
        hiGO.transform.SetParent(canvasGO.transform, false);
        var hiTMP = hiGO.AddComponent<TextMeshProUGUI>();
        hiTMP.alignment = TextAlignmentOptions.Center;
        hiTMP.fontSize = 36f;
        hiTMP.text = $"Hi-Score: {ScoreManager.Instance.HighScore}";
        var hiRect = hiTMP.rectTransform;
        hiRect.anchorMin = new Vector2(0.5f, 0.5f);
        hiRect.anchorMax = new Vector2(0.5f, 0.5f);
        hiRect.anchoredPosition = new Vector2(0f, -40f);

        // Retry button
        var buttonGO = new GameObject("RetryButton");
        buttonGO.transform.SetParent(canvasGO.transform, false);
        var buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.color = Color.white;
        var button = buttonGO.AddComponent<Button>();
        var buttonRect = buttonGO.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(160f, 40f);
        buttonRect.anchorMin = new Vector2(0.5f, 0.4f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.4f);
        buttonRect.anchoredPosition = new Vector2(0f, -80f);

        var buttonTextGO = new GameObject("Text");
        buttonTextGO.transform.SetParent(buttonGO.transform, false);
        var buttonText = buttonTextGO.AddComponent<TextMeshProUGUI>();
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.fontSize = 24f;
        buttonText.text = "Try Again";
        buttonText.color = Color.black;
        var textRect = buttonText.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        button.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        activeBags = 0;
        isGameOver = false;
        ScoreManager.Instance?.ResetScore();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
