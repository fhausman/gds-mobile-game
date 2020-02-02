using UnityEngine;

public class Hole : MonoBehaviour
{
    public float moveSpeed = 0f;
    public bool hitted = false;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime / 100.0f;
    }
}
