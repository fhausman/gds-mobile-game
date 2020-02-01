using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public float MoveSpeed = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += Vector3.left * MoveSpeed * Time.deltaTime / 100.0f;
    }
}
