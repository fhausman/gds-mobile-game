using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcLine : MonoBehaviour
{
    private LineRenderer lr;
    private float gravity;
    private int positionCount { get => resolution + 1; }
    private float radianAngle { get => Mathf.Deg2Rad * angle; }

    public float velocity;
    public float angle;
    public int resolution;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics2D.gravity.y);
    }

    void Update()
    {
        Debug.Log(velocity);
        RenderArc();
    }

    void RenderArc()
    {
        if (velocity > 0.0f)
        {
            lr.enabled = true;
            lr.positionCount = positionCount;
            lr.SetPositions(GetArcPoints());
        }
        else
        {
            lr.enabled = false;
        }
    }

    Vector3[] GetArcPoints()
    {
        var points = new Vector3[positionCount];
        var maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle) / gravity);

        for (int i = 0; i < positionCount; ++i)
        {
            var delta = i / (float)resolution;
            points[i] = CalculatePoint(delta, maxDistance*1.5f);
        }

        return points;
    }

    Vector3 CalculatePoint(float delta, float maxDistance)
    {
        var x = delta * maxDistance;
        var y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        
        return new Vector3(x, y);
    }
}
