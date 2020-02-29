﻿using UnityEngine;

public static class Arc
{
    static public float CalcLaunchSpeed(float distance, float yOffset, float gravity, float angle)
    {
        float speed = (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));

        return speed;
    }
}

[RequireComponent(typeof(LineRenderer))]
public class ArcLine : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 startPosition;
    private Vector3 endPosition = new Vector3(0.0f, 5.0f);
    private float a { get => (-c / (range * range)); }
    private float c { get => startPosition.y + endPosition.y; }

    public int resolution;
    public float range = baseRange;
    public Vector2 direction = Vector2.right;

    public static readonly float baseRange = 0.6f;

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
        if (range > baseRange + float.Epsilon)
        {
            lr.enabled = true;
            lr.positionCount = resolution;
            lr.SetPositions(GetArcPoints());
        }
        else
        {
            lr.positionCount = 1;
            lr.enabled = false;
        }
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
        var x = delta*direction.x;
        var y = a*x*x + startPosition.y;
        
        return new Vector3(x, y);
    }
}
