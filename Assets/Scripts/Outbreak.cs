using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outbreak : MonoBehaviour
{
    public Vector3 maximumScale;

    private readonly Vector3 shrinkDelta = new Vector3(0.1f, 0.1f);

    // Update is called once per frame
    void Update()
    {
        Grow();
    }

    void Grow()
    {
        var newScale = Vector3.MoveTowards(transform.localScale, maximumScale, Time.deltaTime / 10.0f);
        transform.localScale = newScale;
    }

    public void Shrink()
    {
        var newScale = transform.localScale - shrinkDelta;
        transform.localScale = newScale;
    }
}
