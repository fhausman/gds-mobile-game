using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] enemies;
    public Vector2 spawnDir = Vector2.right;

    private bool spawnEnemy = true;

    private IEnumerator SpawnEnemy()
    {
        spawnEnemy = false;

        var enemy = Instantiate(enemies[0]);
        enemy.transform.position = transform.position;
        enemy.GetComponent<Enemy>().direction = spawnDir;

        yield return new WaitForSeconds(1.25f);

        spawnEnemy = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnEnemy)
        {
            StartCoroutine("SpawnEnemy");
        }
    }
}
