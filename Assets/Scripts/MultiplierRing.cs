using UnityEngine;

public class MultiplierRing : MonoBehaviour
{
    public int multiplier;

    private bool firstPlayer;
    private int playerCount;

    private void OnTriggerEnter(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        
        if (root.CompareTag("Player"))
        {
            if (!firstPlayer)
            {
                playerCount = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).Length;
                firstPlayer = true;
            }

            if (!root.GetComponent<PlayerController>().isPassed && playerCount > 0)
            {
                root.GetComponent<PlayerController>().isPassed = true;

                for (int i = 0; i < multiplier - 1; i++)
                {
                    Ring.DuplicatePlayer(root);
                }
                ComboManager.IncrementPositive();
                playerCount--;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject root = other.transform.root.gameObject;

        if (root.CompareTag("Player"))
        {
            if (root.GetComponent<PlayerController>().isPassed)
            {
                root.GetComponent<PlayerController>().isPassed = false;
            }
        }
    }

}
