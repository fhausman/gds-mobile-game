using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static bool isPriestOnBoard = false;

    public static class Enemies
    {
        public const int
            BasicMob = 0,
            StrongMob = 1,
            Priest = 2,
            Slyboot = 3;
    }

    public GameObject[] enemies;
    public Vector2 spawnDir = Vector2.right;
    public Transform priestTarget;

    public float basicMobSpawnDelay = 1.0f;
    public float basicMobSpawnRate = 1.25f;
    public float strongMobSpawnDelay = 5.0f;
    public float strongMobSpawnRate = 3.0f;
    public float priestSpawnDelay = 10.0f;
    public float slybootSpawnDelay = 30.0f;
    public float slybootSpawnRate = 15.0f;

    private bool spawnBasicMob = false;
    private bool spawnStrongMob = false;
    private bool spawnPriest = false;
    private bool spawnSlyboot = false;

    private GameObject slybootInstance = null;

    private float spawnNoise { get => Random.Range(-0.5f, 0.5f); }

    private void SetTransform(Transform t)
    {
        t.position = transform.position;
        
        var newScale = t.localScale;
        newScale.x *= spawnDir.x;
        t.localScale = newScale;
    }

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

    private IEnumerator PriestSpawnDelay()
    {
        yield return new WaitForSeconds(priestSpawnDelay);

        spawnPriest = true;
    }

    private IEnumerator SlybootSpawnDelay()
    {
        yield return new WaitForSeconds(slybootSpawnDelay);

        spawnSlyboot = true;
    }

    private IEnumerator SpawnBasicMob()
    {
        spawnBasicMob = false;
        SpawnMob(Enemies.BasicMob);

        yield return new WaitForSeconds(basicMobSpawnRate + spawnNoise);

        spawnBasicMob = true;
    }
    private IEnumerator SpawnStrongMob()
    {
        spawnStrongMob = false;
        SpawnMob(Enemies.StrongMob);

        yield return new WaitForSeconds(strongMobSpawnRate + spawnNoise);

        spawnStrongMob = true;
    }
    private IEnumerator SpawnSlyboot()
    {
        spawnSlyboot = false;

        slybootInstance = Instantiate(enemies[Enemies.Slyboot]);
        SetTransform(slybootInstance.transform);

        var slybootComp = slybootInstance.GetComponent<Slyboot>();
        slybootComp.direction = spawnDir;
        slybootComp.speed = 1.0f;

        while (slybootInstance != null)
        {
            yield return new WaitForEndOfFrame();
        }

        spawnSlyboot = true;
    }

    private void SpawnMob(int mobType)
    {
        var enemy = Instantiate(enemies[mobType]);
        SetTransform(enemy.transform);
        
        enemy.GetComponent<Mob>().direction = spawnDir;
    }

    private IEnumerator SpawnPriest()
    {
        spawnPriest = false;

        slybootInstance = Instantiate(enemies[Enemies.Priest]);
        SetTransform(slybootInstance.transform);

        var priestComp = slybootInstance.GetComponent<Priest>();
        priestComp.direction = spawnDir;
        priestComp.target = priestTarget.position;
        priestComp.speed = 1.0f;
        isPriestOnBoard = true;

        while (slybootInstance != null)
        {
            yield return new WaitForEndOfFrame();
        }

        isPriestOnBoard = false;
        StartCoroutine("PriestSpawnDelay");
    }

    void Start()
    {
        StartCoroutine("BasicMobSpawnDelay");
        StartCoroutine("StrongMobSpawnDelay");
        StartCoroutine("PriestSpawnDelay");
        StartCoroutine("SlybootSpawnDelay");
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

        if (spawnPriest && !isPriestOnBoard)
        {
            StartCoroutine("SpawnPriest");
        }

        if (spawnSlyboot)
        {
            StartCoroutine("SpawnSlyboot");
        }
    }
}
