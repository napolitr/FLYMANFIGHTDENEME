using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    [SerializeField] private int charges = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.ActivateShield(Mathf.Max(1, charges));
            }
            Destroy(gameObject);
        }
    }
}


