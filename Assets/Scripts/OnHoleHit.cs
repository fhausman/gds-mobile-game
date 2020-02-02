using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoleHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Potato")
        {
            Destroy(col.gameObject);
        }
    }
}
