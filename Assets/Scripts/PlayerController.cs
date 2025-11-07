using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static List<PlayerController> players;

    [SerializeField] private float maxLaunchSpeed = 60f;
    [SerializeField] private float movementSpeed = 100f;
    [SerializeField] private float mobileSpeed = 10f;
    
    [SerializeField] private Transform capsule;
    [SerializeField] private FixedJoint joint;
    
    [HideInInspector] public bool isPassed;
    [HideInInspector] public Rigidbody[] bodies;

    private float xValue;
    private Vector3 initialPos;
    private float time;
    private GameObject SelfHips;

    private void Awake()
    {
        if (players == null || players.Count == 0)
        {
            players = new List<PlayerController>();
        }
    }

    void Start()
    {
        SelfHips = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        bodies = GetComponentsInChildren<Rigidbody>();
        initialPos = capsule.position;
        
        players.Add(this);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (GameManager.Instance.isGameStarted)
            {
                xValue = Input.GetAxis("Mouse X");

                float speedScale = GetMoveSpeed();
                foreach (Rigidbody rb in bodies)
                {
                    if (rb != null && !rb.isKinematic)
                    {
                        rb.linearVelocity += new Vector3(xValue, 0, 0) * Time.deltaTime * speedScale;
                    }
                }
            }
            else
            {
                GameManager.Instance.CloseTapText();
                GameManager.Instance.isGameStarted = true;
            }
        }

#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            if (GameManager.Instance.isGameStarted)
            {
                Touch touch = Input.GetTouch(0);
                TouchPhase phase = touch.phase;

                if (phase == TouchPhase.Moved)
                {
                    xValue = touch.deltaPosition.x;

                    float speedScale = GetMoveSpeed();
                    foreach (Rigidbody rb in bodies)
                    {
                        if (rb != null && !rb.isKinematic)
                        {
                            rb.linearVelocity += new Vector3(xValue, 0, 0) * Time.deltaTime * speedScale;
                        }
                    }
                }
            }
            else
            {
                GameManager.Instance.CloseTapText();
                GameManager.Instance.isGameStarted = true;
            }
        }
#endif

        CheckForBoundaries();
    }

    private void CheckForBoundaries()
    {
        float xPos = SelfHips.transform.position.x;

        if (xPos >= 20f || xPos <= -20f)
        {
            float newX = Mathf.Sign(xPos) * -3f;
            var hipsRb = SelfHips.GetComponent<Rigidbody>();
            if (hipsRb != null && !hipsRb.isKinematic)
            {
                hipsRb.linearVelocity = new Vector3(newX, hipsRb.linearVelocity.y, hipsRb.linearVelocity.z);
            }
        }
    }

    public IEnumerator ApplyLaunchForce(float factor)
    {
        Vector3 targetPos = initialPos + new Vector3(0f, -1f, -4f) * factor;

        while (time <= 0.8f)
        {
            capsule.position = Vector3.Lerp(initialPos, targetPos, time / 0.8f);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;

        while (time <= 0.1f)
        {
            capsule.position = Vector3.Lerp(targetPos, initialPos, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(joint);

        Vector3 forceVector = new Vector3(0, factor, factor * 2f) * maxLaunchSpeed;

        foreach (Rigidbody rb in bodies)
        {
            if (rb == null) continue;
            rb.isKinematic = false;
            rb.linearVelocity = forceVector;
        }

        if (factor > 0.1f)
        {
            var lead = bodies != null && bodies.Length > 0 ? bodies[0] : null;
            var launchVel = lead != null ? lead.linearVelocity : forceVector;
            Spawner.Instance.SpawnObjects(launchVel);
        }
    }
    
    private float GetMoveSpeed()
    {
        return Application.isMobilePlatform ? mobileSpeed : movementSpeed;
    }

}
