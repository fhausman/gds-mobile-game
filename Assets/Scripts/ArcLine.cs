using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcLine : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 startPosition;
    private Vector3 endPosition = new Vector3(0.0f, 5.0f);
    private float a { get => (-c / (range * range)); }
    private float c { get => startPosition.y + endPosition.y; }

    public int resolution;
    public float range = 0.0f;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        startPosition = GetComponentInParent<Transform>().position;
    }

    void Update()
    {
        RenderArc();
    }

    void RenderArc()
    {
        if (range > 0.0f)
        {
            lr.enabled = true;
            lr.positionCount = resolution;
            lr.SetPositions(GetArcPoints());
        }
        //else
        //{
        //    lr.enabled = false;
        //}
    }

    Vector3[] GetArcPoints()
    {
        var points = new Vector3[resolution];
        var delta = range / (float)resolution;

        for (int i = 0; i < resolution; ++i)
        {
            points[i] = CalculatePoint(delta*i);
        }

        return points;
    }

    Vector3 CalculatePoint(float delta)
    {
        var x = delta;
        var y = a*x*x + startPosition.y;
        Debug.Log(a);
        
        return new Vector3(x, y);
    }
}
