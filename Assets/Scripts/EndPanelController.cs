using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EndPanelController : MonoBehaviour
{
    [System.Serializable]
    public class PanelStyle
    {
        public string Header;
        public string Subtitle;
        public string PrimaryButtonLabel;
        public Color BackgroundColor = new Color(0f, 0f, 0f, 0.7f);
        public Color AccentColor = Color.white;
        public Color BodyTextColor = Color.white;
        public Color ButtonColor = Color.white;
        public Color ButtonTextColor = Color.black;
        public float FadeDuration = 0.4f;
    }

    private const string HeaderObjectName = "HeaderText";
    private const string SubtitleObjectName = "SubtitleText";

    private bool initialized;
    private Image backgroundImage;
    private Image illustrationImage;
    private Button primaryButton;
    private Image primaryButtonImage;
    private TMP_Text primaryButtonLabel;
    private TMP_Text headerText;
    private TMP_Text subtitleText;
    private CanvasGroup canvasGroup;
    private TMP_FontAsset cachedFontAsset;
    private Material cachedFontMaterial;

    public void ApplyStyle(PanelStyle style, GameManager.Colors theme, LevelDatabase.LevelDefinition definition, int levelNumber, bool isWin)
    {
        if (style == null)
        {
            return;
        }

        CacheReferences();

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        ApplyBackground(style, theme);
        ApplyIllustration(style, theme, isWin);
        ApplyTexts(style, theme, definition, levelNumber);
        ApplyButton(style, theme);

        if (canvasGroup != null)
        {
            StopAllCoroutines();
            canvasGroup.alpha = 0f;
            StartCoroutine(FadeIn(style.FadeDuration));
        }
    }

    private IEnumerator FadeIn(float duration)
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        if (duration <= 0.01f)
        {
            canvasGroup.alpha = 1f;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private void ApplyBackground(PanelStyle style, GameManager.Colors theme)
    {
        if (backgroundImage == null)
        {
            return;
        }

        Color overlay = style.BackgroundColor;
        if (theme != null)
        {
            overlay = Color.Lerp(overlay, theme.RingTransColor, 0.35f);
            overlay.a = Mathf.Max(style.BackgroundColor.a, 0.55f);
        }
        backgroundImage.color = overlay;
    }

    private void ApplyIllustration(PanelStyle style, GameManager.Colors theme, bool isWin)
    {
        if (illustrationImage == null)
        {
            return;
        }

        Color accent = style.AccentColor;
        if (theme != null)
        {
            accent = Color.Lerp(accent, theme.RingColor, 0.6f);
        }

        if (!isWin)
        {
            accent = Color.Lerp(accent, Color.red, 0.35f);
        }

        illustrationImage.color = accent;
    }

    private void ApplyTexts(PanelStyle style, GameManager.Colors theme, LevelDatabase.LevelDefinition definition, int levelNumber)
    {
        if (headerText != null)
        {
            headerText.text = FormatLine(style.Header, definition, levelNumber);
            headerText.color = GetBodyColor(style, theme);
            headerText.gameObject.SetActive(!string.IsNullOrWhiteSpace(headerText.text));
        }

        if (subtitleText != null)
        {
            subtitleText.text = FormatLine(style.Subtitle, definition, levelNumber);
            subtitleText.color = Color.Lerp(GetBodyColor(style, theme), Color.white, 0.2f);
            subtitleText.gameObject.SetActive(!string.IsNullOrWhiteSpace(subtitleText.text));
        }
    }

    private void ApplyButton(PanelStyle style, GameManager.Colors theme)
    {
        if (primaryButton == null)
        {
            return;
        }

        if (primaryButtonLabel != null)
        {
            primaryButtonLabel.text = style.PrimaryButtonLabel;
            primaryButtonLabel.color = style.ButtonTextColor;
        }

        if (primaryButtonImage != null)
        {
            if (primaryButtonImage.sprite == null)
            {
                primaryButtonImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
            }

            Color buttonColor = style.ButtonColor;
            if (theme != null)
            {
                buttonColor = Color.Lerp(buttonColor, theme.PlatformColor, 0.4f);
            }

            primaryButtonImage.color = buttonColor;

            primaryButton.transition = Selectable.Transition.ColorTint;
            var colors = primaryButton.colors;
            colors.normalColor = buttonColor;
            colors.highlightedColor = buttonColor * 1.1f;
            colors.pressedColor = buttonColor * 0.85f;
            colors.disabledColor = new Color(buttonColor.r, buttonColor.g, buttonColor.b, 0.3f);
            colors.colorMultiplier = 1f;
            primaryButton.colors = colors;
        }
    }

    private void CacheReferences()
    {
        if (initialized)
        {
            return;
        }

        backgroundImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        CacheFont();

        illustrationImage = FindChildImage("Image");

        var buttons = GetComponentsInChildren<Button>(true);
        if (buttons.Length > 0)
        {
            primaryButton = buttons[0];
            primaryButtonImage = primaryButton.GetComponent<Image>();
            if (primaryButtonImage == null)
            {
                primaryButtonImage = primaryButton.gameObject.AddComponent<Image>();
            }
            primaryButton.targetGraphic = primaryButtonImage;
            primaryButtonLabel = primaryButton.GetComponentInChildren<TMP_Text>(true);
        }

        headerText = FindOrCreateText(HeaderObjectName, new Vector2(0.5f, 0.78f), new Vector2(0.5f, 0.78f), new Vector2(0f, 0f), new Vector2(920f, 200f), 126f);
        subtitleText = FindOrCreateText(SubtitleObjectName, new Vector2(0.5f, 0.57f), new Vector2(0.5f, 0.57f), new Vector2(0f, 0f), new Vector2(920f, 140f), 56f);

        AdjustButtonLayout();
        AdjustIllustrationLayout();

        initialized = true;
    }

    private void CacheFont()
    {
        if (cachedFontAsset != null)
        {
            return;
        }

        var texts = GetComponentsInChildren<TMP_Text>(true);
        foreach (var tmp in texts)
        {
            if (tmp == null || tmp.font == null)
            {
                continue;
            }

            cachedFontAsset = tmp.font;
            cachedFontMaterial = tmp.fontSharedMaterial;
            break;
        }
    }

    private void AdjustButtonLayout()
    {
        if (primaryButton == null)
        {
            return;
        }

        var rect = primaryButton.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.23f);
        rect.anchorMax = new Vector2(0.5f, 0.23f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(620f, 170f);
    }

    private void AdjustIllustrationLayout()
    {
        if (illustrationImage == null)
        {
            return;
        }

        var rect = illustrationImage.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, 120f);
        rect.sizeDelta = new Vector2(640f, 640f);
    }

    private TMP_Text FindOrCreateText(string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta, float fontSize)
    {
        Transform existing = transform.Find(name);
        TMP_Text textComponent = null;

        if (existing == null)
        {
            var go = new GameObject(name);
            var rect = go.AddComponent<RectTransform>();
            rect.SetParent(transform, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;

            textComponent = go.AddComponent<TextMeshProUGUI>();
            if (cachedFontAsset != null)
            {
                textComponent.font = cachedFontAsset;
            }
            if (cachedFontMaterial != null)
            {
                textComponent.fontSharedMaterial = cachedFontMaterial;
            }
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.textWrappingMode = TextWrappingModes.Normal;
            textComponent.fontSize = fontSize;
            textComponent.text = string.Empty;
        }
        else
        {
            textComponent = existing.GetComponent<TMP_Text>();
            if (textComponent == null)
            {
                textComponent = existing.gameObject.AddComponent<TextMeshProUGUI>();
            }
            var rect = textComponent.rectTransform;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;
            textComponent.fontSize = fontSize;
            if (cachedFontAsset != null)
            {
                textComponent.font = cachedFontAsset;
            }
            if (cachedFontMaterial != null)
            {
                textComponent.fontSharedMaterial = cachedFontMaterial;
            }
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.textWrappingMode = TextWrappingModes.Normal;
        }

        return textComponent;
    }

    private Image FindChildImage(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            var image = child.GetComponent<Image>();
            if (image != null)
            {
                return image;
            }
        }

        var images = GetComponentsInChildren<Image>(true);
        foreach (var image in images)
        {
            if (image != null && image != backgroundImage && image.transform.parent == transform)
            {
                return image;
            }
        }

        return null;
    }

    private string FormatLine(string template, LevelDatabase.LevelDefinition definition, int levelNumber)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            return string.Empty;
        }

        string result = template.Replace("{LEVEL}", Mathf.Max(levelNumber, 1).ToString());
        if (definition != null)
        {
            result = result.Replace("{ENEMIES}", definition.EnemyCount.ToString());
            int ringPairs = definition.Rings != null ? definition.Rings.Length : 0;
            result = result.Replace("{RINGS}", ringPairs.ToString());
        }

        return result;
    }

    private Color GetBodyColor(PanelStyle style, GameManager.Colors theme)
    {
        Color bodyColor = style.BodyTextColor;
        if (theme != null)
        {
            bodyColor = Color.Lerp(bodyColor, theme.PlatformColor, 0.25f);
        }

        return bodyColor;
    }
}

