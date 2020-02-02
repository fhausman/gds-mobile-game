using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public GameObject potatoPrefab;

    private float MaxVelocity = 1000.0f;
    private float velocity = 0.0f;

    private ArcLine arc;

    void Start()
    {
        arc = GetComponentInChildren<ArcLine>();
    }

    // Update is called once per frame
    void Update()
    {
        arc.velocity = velocity;
        if(Input.GetMouseButtonDown(0))
        {
            velocity = 0.0f;
        }

        if (Input.GetMouseButton(0))
        {
            velocity += 0.1f;
            if (velocity > MaxVelocity)
                velocity = MaxVelocity;
        }

        if(Input.GetMouseButtonUp(0))
        {
            var potato = Instantiate(potatoPrefab);
            potato.transform.position = arc.transform.position;
            potato.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 45.0f), Mathf.Sin(Mathf.Deg2Rad * 45.0f)) * velocity, ForceMode2D.Impulse);
            Debug.Log(velocity);
            //velocity = 0.0f;
        }
    }
}
