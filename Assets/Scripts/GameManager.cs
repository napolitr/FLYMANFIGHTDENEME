using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public bool isPlayerEntered;
    [HideInInspector] public bool isGameStarted;
    public bool CanSmoke = true;

    [SerializeField] private GameObject GameSuccessPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject tapToThrow;
    [SerializeField] private TextMeshProUGUI levelText;
    public Colors[] ColorArray;

    private EndPanelController successPanelController;
    private EndPanelController failurePanelController;

    private readonly EndPanelController.PanelStyle successPanelStyle = new EndPanelController.PanelStyle
    {
        Header = "LEVEL {LEVEL} TAMAMLANDI",
        Subtitle = "{ENEMIES} düşman geride kaldı, hız kesme!",
        PrimaryButtonLabel = "Devam Et",
        BackgroundColor = new Color(0.09f, 0.16f, 0.22f, 0.78f),
        AccentColor = new Color(0.96f, 0.85f, 0.36f, 1f),
        BodyTextColor = Color.white,
        ButtonColor = new Color(0.15f, 0.62f, 0.58f, 1f),
        ButtonTextColor = Color.white,
        FadeDuration = 0.35f
    };

    private readonly EndPanelController.PanelStyle failurePanelStyle = new EndPanelController.PanelStyle
    {
        Header = "TEKRAR DENE",
        Subtitle = "Bitirmen için {ENEMIES} rakip daha var. Hadi başaralım!",
        PrimaryButtonLabel = "Tekrar Başlat",
        BackgroundColor = new Color(0.08f, 0.05f, 0.11f, 0.82f),
        AccentColor = new Color(0.94f, 0.36f, 0.36f, 1f),
        BodyTextColor = Color.white,
        ButtonColor = new Color(0.75f, 0.23f, 0.28f, 1f),
        ButtonTextColor = Color.white,
        FadeDuration = 0.4f
    };

    [Serializable]
    public class Colors{
        public Color RingColor;
        public Color RingTransColor;
        public Color PlatformColor;
    }
    public int currentLevel;
    private void Awake()
    {
        if(Instance == null)Instance = this;
        Application.targetFrameRate = 60;
        ColorArray = LevelDatabase.GetAllThemes();
        successPanelController = EnsureEndPanelController(GameSuccessPanel);
        failurePanelController = EnsureEndPanelController(GameOverPanel);
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level",0);

        if (ColorArray == null || ColorArray.Length == 0)
        {
            ColorArray = LevelDatabase.GetAllThemes();
        }

        if (levelText != null)
        {
            levelText.text = "LEVEL " + (currentLevel+1);

            var theme = LevelDatabase.GetLevel(currentLevel)?.Theme;
            if (theme != null)
            {
                levelText.color = theme.RingColor;
            }
        }
    }

    private void Update()
    {
        if (Instance.isPlayerEntered)
        {
            if (PlayerController.players.Count == 0 && Spawner.enemies.Count >= 0)
            {
                foreach (var item in Spawner.enemies)
                {
                    item.GetComponent<Animator>().SetBool("Win", true);
                    Destroy(item.GetComponent<Rigidbody>());
                }
                GameOver();
                isPlayerEntered = false;

            }
            else if (Spawner.enemies.Count == 0 && PlayerController.players.Count > 0)
            {
                foreach (var item in PlayerController.players)
                {
                    item.GetComponent<Animator>().SetBool("Win", true);
                    Destroy(item.GetComponent<Rigidbody>());                 
                }
                GameSuccess();
                isPlayerEntered = false;
            }
        }
    }

    public void CloseTapText()
    {
        tapToThrow.SetActive(false);
    }

    public void GameOver()
    {
        PlayerController.players = null;
        var levelDefinition = LevelDatabase.GetLevel(currentLevel);
        var theme = GetThemeForLevel(currentLevel, levelDefinition);
        failurePanelController?.ApplyStyle(failurePanelStyle, theme, levelDefinition, currentLevel + 1, false);
        GameOverPanel.SetActive(true);
        CameraShaker.Shake(0.35f, 0.35f);
    }

    public void GameSuccess()
    {
        PlayerController.players = null;
        var levelDefinition = LevelDatabase.GetLevel(currentLevel);
        var theme = GetThemeForLevel(currentLevel, levelDefinition);
        successPanelController?.ApplyStyle(successPanelStyle, theme, levelDefinition, currentLevel + 1, true);
        GameSuccessPanel.SetActive(true);
        CameraShaker.Shake(0.25f, 0.25f);
    }

    public void LoadNextLevel()
    {
        currentLevel+=1;
        PlayerPrefs.SetInt("level",currentLevel);
        SceneManager.LoadScene((currentLevel%(SceneManager.sceneCountInBuildSettings-1))+1);
    }

    public void LoadAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private EndPanelController EnsureEndPanelController(GameObject panel)
    {
        if (panel == null)
        {
            return null;
        }

        var controller = panel.GetComponent<EndPanelController>();
        if (controller == null)
        {
            controller = panel.AddComponent<EndPanelController>();
        }

        var canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            panel.AddComponent<CanvasGroup>();
        }

        return controller;
    }

    private Colors GetThemeForLevel(int levelIndex, LevelDatabase.LevelDefinition definition)
    {
        if (definition != null && definition.Theme != null)
        {
            return definition.Theme;
        }

        if (ColorArray == null || ColorArray.Length == 0)
        {
            return null;
        }

        int index = Mathf.Abs(levelIndex) % ColorArray.Length;
        return ColorArray[index];
    }
}
