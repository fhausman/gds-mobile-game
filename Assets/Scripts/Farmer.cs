using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public GameObject potatoPrefab;

    private float MaxVelocity = 1000.0f;
    private float velocity = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            velocity += 10.0f;
            if (velocity > MaxVelocity)
                velocity = MaxVelocity;
        }

        if(Input.GetMouseButtonUp(0))
        {
            var potato = Instantiate(potatoPrefab);
            potato.transform.position = transform.position + new Vector3(0.0f, 3.0f);
            potato.GetComponent<Rigidbody2D>().AddForce(new Vector2(1* velocity, 0));

            velocity = 0.0f;
        }

        Debug.Log(velocity);
    }
}
