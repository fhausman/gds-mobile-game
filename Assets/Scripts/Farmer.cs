using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public GameObject potatoPrefab;
    public float throwAngle;

    private float MaxVelocity = 1000.0f;
    private float velocity = 0.0f;
    private Vector2 normalizedDirection { get => new Vector2(Mathf.Cos(Mathf.Deg2Rad * throwAngle), Mathf.Sin(Mathf.Deg2Rad * throwAngle)); }

    private ArcLine arc;

    void Start()
    {
        arc = GetComponentInChildren<ArcLine>();
    }

    // Update is called once per frame
    void Update()
    {
        arc.angle = throwAngle;
        arc.velocity = velocity;

        if (Input.GetMouseButton(0))
        {
            velocity += 10.0f*Time.deltaTime;
            if (velocity > MaxVelocity)
                velocity = MaxVelocity;
        }

        if(Input.GetMouseButtonUp(0))
        {
            var potato = Instantiate(potatoPrefab);
            potato.transform.position = arc.transform.position;
            potato.GetComponent<Rigidbody2D>().AddForce(normalizedDirection * velocity, ForceMode2D.Impulse);
            velocity = 0.0f;
        }
    }
}
