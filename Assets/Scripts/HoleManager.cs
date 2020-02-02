using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public GameObject holePrefab;
    public float speedDelta = 50.0f;
    public Vector3 spawnPoint = new Vector3(11.03f, -3.99f, 0.0f);

    private uint spawnedCount = 0;

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Hole")
        {
            Debug.Log("Hole left the stage");
            Destroy(col.gameObject);

            var newHole = Instantiate(holePrefab);
            newHole.transform.position = spawnPoint;
            newHole.GetComponent<Hole>().MoveSpeed += speedDelta * spawnedCount;

            spawnedCount++;
            Norm.norm -= 10;
        }
    }
}