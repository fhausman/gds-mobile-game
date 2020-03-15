using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
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

    public float additionalSpeed = 0.10f;
    public int scoreSpeedMultiplier { get => startIncreasingSpeed ? (Score.value - scoreWhenAllTypesOfEnemiesSpawned) / 1000 : 1; }

    private bool active = false;
    private bool spawnBasicMob = false;
    private bool spawnStrongMob = false;
    private bool spawnPriest = false;
    private bool spawnSlyboot = false;
    private bool startIncreasingSpeed = false;
    private int scoreWhenAllTypesOfEnemiesSpawned = 0;

    private GameObject slybootInstance = null;
    private GameObject priestInstance = null;

    public void Activate()
    {
        active = true;

        StartCoroutine("BasicMobSpawnDelay");
        StartCoroutine("StrongMobSpawnDelay");
        StartCoroutine("PriestSpawnDelay");
        StartCoroutine("SlybootSpawnDelay");
    }

    public void Deactivate()
    {
        StopAllCoroutines();

        active = false;
        spawnBasicMob = false;
        spawnStrongMob = false;
        spawnPriest = false;
        spawnSlyboot = false;
}

    private float spawnNoise { get => Random.Range(-0.5f, 0.5f); }

    private void SetTransform(Transform t, bool addYNoise = false)
    {
        var newPosition = transform.position;
        if (addYNoise)
        {
            newPosition.y += Random.Range(-0.5f, 0);
        }
        newPosition.z = newPosition.y;
        t.position = newPosition;
        
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

        var inst = GameObject.FindGameObjectWithTag("Priest");
        while (inst != null)
        {
            yield return null;
        }

        spawnPriest = true;
    }

    private IEnumerator SlybootSpawnDelay()
    {
        yield return new WaitForSeconds(slybootSpawnDelay);

        var inst = GameObject.FindGameObjectWithTag("Slyboot");
        while (inst != null)
        {
            yield return null;
        }

        spawnSlyboot = true;
        startIncreasingSpeed = true;
        scoreWhenAllTypesOfEnemiesSpawned = Score.value;
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
    private IEnumerator SpawnSlybootInternal()
    {
        spawnSlyboot = false;

        SpawnSlyboot();

        while (slybootInstance != null)
        {
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine("SlybootSpawnDelay");
    }

    public void SpawnSlyboot()
    {
        slybootInstance = Instantiate(enemies[Enemies.Slyboot]);
        SetTransform(slybootInstance.transform);

        var slybootComp = slybootInstance.GetComponent<Slyboot>();
        slybootComp.direction = spawnDir;
        slybootComp.speed = 1.0f + scoreSpeedMultiplier * additionalSpeed;
    }

    public void SpawnMob(int mobType)
    {
        var enemy = Instantiate(enemies[mobType]);
        SetTransform(enemy.transform, mobType == Enemies.BasicMob);
        
        enemy.GetComponent<Mob>().direction = spawnDir;
        enemy.GetComponent<Mob>().speed += scoreSpeedMultiplier * additionalSpeed;
    }

    private IEnumerator SpawnPriestInternal()
    {
        spawnPriest = false;

        SpawnPriest();

        while (priestInstance != null)
        {
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine("PriestSpawnDelay");
    }

    public void SpawnPriest()
    {
        priestInstance = Instantiate(enemies[Enemies.Priest]);
        SetTransform(priestInstance.transform);

        var priestComp = priestInstance.GetComponent<Priest>();
        priestComp.direction = spawnDir;
        priestComp.target = priestTarget.position;
        priestComp.speed = 1.0f + scoreSpeedMultiplier * additionalSpeed;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        if (spawnBasicMob)
        {
            StartCoroutine("SpawnBasicMob");
        }

        if (spawnStrongMob)
        {
            StartCoroutine("SpawnStrongMob");
        }

        if (spawnPriest)
        {
            StartCoroutine("SpawnPriestInternal");
        }

        if (spawnSlyboot)
        {
            StartCoroutine("SpawnSlybootInternal");
        }
    }
}
