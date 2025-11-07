using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;

    private Transform camTransform;
    private Vector3 originalPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        camTransform = Camera.main != null ? Camera.main.transform : null;
        if (camTransform != null) originalPos = camTransform.localPosition;
    }

    public static void Shake(float duration = 0.25f, float magnitude = 0.25f)
    {
        if (Instance == null || Instance.camTransform == null) return;
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(Instance.ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;
        if (camTransform == null) yield break;
        originalPos = camTransform.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float x = (Random.value * 2 - 1f) * magnitude;
            float y = (Random.value * 2 - 1f) * magnitude * 0.6f;
            camTransform.localPosition = originalPos + new Vector3(x, y, 0);
            yield return null;
        }

        camTransform.localPosition = originalPos;
    }
}


