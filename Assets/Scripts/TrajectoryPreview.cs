using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPreview : MonoBehaviour
{
    [SerializeField] private int segments = 30;
    [SerializeField] private float timeStep = 0.06f;
    [SerializeField] private Color startColor = new Color(1f,1f,1f,0.8f);
    [SerializeField] private Color endColor = new Color(1f,1f,1f,0.0f);

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.positionCount = 0;
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.widthMultiplier = 0.15f;
    }

    public void Show(Vector3 origin, Vector3 initialVelocity)
    {
        if (lr == null) return;
        lr.positionCount = segments;
        float g = -Physics.gravity.y;
        for (int i = 0; i < segments; i++)
        {
            float t = i * timeStep;
            float x = origin.x;
            float y = origin.y + (initialVelocity.y * t) - (0.5f * g * t * t);
            float z = origin.z + (initialVelocity.z * t);
            lr.SetPosition(i, new Vector3(x, y, z));
        }
    }

    public void Hide()
    {
        if (lr == null) return;
        lr.positionCount = 0;
    }
}


