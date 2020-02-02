using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public GameObject holePrefab;
    public static float startingSpeed = 200.0f;
    public static float speedDelta = 50.0f;
    public Vector3 spawnPoint = new Vector3(11.03f, -3.99f, 0.0f);

    public static float currentSpeed { get => startingSpeed + speedDelta * spawnedCount; }
    private static uint spawnedCount = 0;

    void Start()
    {
        spawnedCount = 0;
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Hole").Length == 0)
        {
            SpawnHole();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Hole")
        {
            Destroy(col.gameObject);
            if (!col.GetComponent<Hole>().hitted)
            {
                Norm.norm -= 10;
            }
        }
    }

    public void SpawnHole()
    {
        var newHole = Instantiate(holePrefab, transform);
        newHole.transform.position = spawnPoint;
        newHole.GetComponent<Hole>().moveSpeed = currentSpeed;
        spawnedCount++;
    }
}