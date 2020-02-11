using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    public GameObject projectile;
    //public float throwAngle;

    //private float MaxVelocity = 1000.0f;
    private float range = 0.0f;
    //private Vector2 normalizedDirection { get => new Vector2(Mathf.Cos(Mathf.Deg2Rad * throwAngle), Mathf.Sin(Mathf.Deg2Rad * throwAngle)); }

    private ArcLine arc;

    void Start()
    {
        arc = GetComponentInChildren<ArcLine>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            range += 1.0f * Time.deltaTime;
            arc.range = range;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var proj = Instantiate(projectile);
            proj.transform.position = transform.position;
            proj.GetComponent<Rigidbody2D>().velocity = Vector3.right * LaunchSpeed(range, transform.position.y + 5.0f, Physics2D.gravity.magnitude, 0);
            range = 0.0f;
        }
    }

    private float LaunchSpeed(float distance, float yOffset, float gravity, float angle)
    {
        float speed = (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));

        return speed;
    }
}
