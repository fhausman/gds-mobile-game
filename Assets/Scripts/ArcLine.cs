using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcLine : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 startPosition;
    private Vector3 endPosition = new Vector3(0.0f, 5.0f);

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
        var delta = (endPosition.y + startPosition.y) / (float)resolution;

        for (int i = 0; i < resolution; ++i)
        {
            points[i] = CalculatePoint(delta*i);
        }

        return points;
    }

    Vector3 CalculatePoint(float delta)
    {
        var y = delta;
        var x = Mathf.Sqrt(y) * range;
        
        return new Vector3(x, -y + startPosition.y);
    }
}
