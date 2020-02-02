using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoleHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Potato")
        {
            Score.score += 1;
            Destroy(col.gameObject);
            transform.gameObject.SetActive(false);

            var hole = GetComponentInParent<Hole>();
            hole.moveSpeed += 50.0f;
            hole.hitted = true;

            GetComponentInParent<HoleManager>().SpawnHole();
        }
    }
}
