using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    private int shieldCharges;
    private bool slowMoActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ActivateShield(int charges = 1)
    {
        shieldCharges = Mathf.Max(shieldCharges, 0) + Mathf.Max(1, charges);
    }

    public bool ConsumeShieldIfAvailable()
    {
        if (shieldCharges > 0)
        {
            shieldCharges--;
            return true;
        }
        return false;
    }

    public void ActivateSlowMo(float timeScale = 0.3f, float duration = 1.25f)
    {
        if (slowMoActive) return;
        Instance.StartCoroutine(SlowMoCoroutine(timeScale, duration));
    }

    private IEnumerator SlowMoCoroutine(float timeScale, float duration)
    {
        slowMoActive = true;
        float original = Time.timeScale;
        Time.timeScale = Mathf.Clamp(timeScale, 0.05f, 1f);
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = original;
        slowMoActive = false;
    }
}


