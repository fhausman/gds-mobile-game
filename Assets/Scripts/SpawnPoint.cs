using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static class Enemies
    {
        public const int
            BasicMob = 0,
            StrongMob = 1;
    }

    public GameObject[] enemies;
    public Vector2 spawnDir = Vector2.right;
    public float basicMobSpawnDelay = 1.0f;
    public float basicMobSpawnRate = 1.25f;
    public float strongMobSpawnDelay = 5.0f;
    public float strongMobSpawnRate = 3.0f;

    private bool spawnBasicMob = false;
    private bool spawnStrongMob = false;

    private IEnumerator BasicMobSpawnDelay()
    {
        yield return new WaitForSeconds(basicMobSpawnDelay);

        spawnBasicMob = true;
    }
    private IEnumerator StrongMobSpawnDelay()
    {
        yield return new WaitForSeconds(strongMobSpawnDelay);

        spawnStrongMob = true;
    }

    private IEnumerator SpawnBasicMob()
    {
        spawnBasicMob = false;

        var enemy = Instantiate(enemies[Enemies.BasicMob]);
        enemy.transform.position = transform.position;
        enemy.GetComponent<Mob>().direction = spawnDir;

        yield return new WaitForSeconds(basicMobSpawnRate);

        spawnBasicMob = true;
    }
    private IEnumerator SpawnStrongMob()
    {
        spawnStrongMob = false;

        var enemy = Instantiate(enemies[Enemies.StrongMob]);
        enemy.transform.position = transform.position;
        enemy.GetComponent<Mob>().direction = spawnDir;

        yield return new WaitForSeconds(strongMobSpawnRate);

        spawnStrongMob = true;
    }

    void Start()
    {
        StartCoroutine("BasicMobSpawnDelay");
        StartCoroutine("StrongMobSpawnDelay");
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnBasicMob)
        {
            StartCoroutine("SpawnBasicMob");
        }

        if (spawnStrongMob)
        {
            StartCoroutine("SpawnStrongMob");
        }
    }
}
