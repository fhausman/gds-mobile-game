using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    public GameObject projectile;
    public float chargeSpeed = 10.0f;

    private ArcLine arc;

    void Start()
    {
        arc = GetComponentInChildren<ArcLine>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            arc.direction = mousePos.x < 0 ? Vector2.left : Vector2.right;
        }

        if (Input.GetMouseButton(0))
        {
            arc.range += chargeSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var proj = Instantiate(projectile);
            proj.transform.position = transform.position;
            proj.GetComponent<Rigidbody2D>().AddForce(arc.direction * LaunchSpeed(arc.range, transform.position.y + 5.0f, Physics2D.gravity.magnitude, 0), ForceMode2D.Impulse);

            arc.range = 0.0f;
        }
    }

    private float LaunchSpeed(float distance, float yOffset, float gravity, float angle)
    {
        float speed = (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));

        return speed;
    }
}
