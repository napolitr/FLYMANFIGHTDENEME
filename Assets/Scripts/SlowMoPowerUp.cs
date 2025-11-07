using UnityEngine;

public class SlowMoPowerUp : MonoBehaviour
{
    [SerializeField] private float timeScale = 0.35f;
    [SerializeField] private float duration = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.ActivateSlowMo(timeScale, duration);
            }
            Destroy(gameObject);
        }
    }
}


