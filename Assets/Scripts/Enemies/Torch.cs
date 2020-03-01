using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Rigidbody2D rb;

    public bool landed { get => rb.velocity.magnitude <= 0.0f + float.Epsilon; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Hit()
    {
        Destroy(gameObject);
    }
}
