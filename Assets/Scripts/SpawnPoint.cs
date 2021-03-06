﻿using System.Collections;
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
    public float hordeDelay = 0.0f;

    public int hordeMobCount = 5;
    public int hordeStrongMobCount = 2;

    public int scoreSpeedMultiplier { get => startIncreasingSpeed ? (Score.value - scoreWhenAllTypesOfEnemiesSpawned) / 1000 : 1; }

    private bool active = false;
    private bool spawnBasicMob = false;
    private bool spawnStrongMob = false;
    private bool spawnPriest = false;
    private bool spawnSlyboot = false;
    private bool startIncreasingSpeed = false;
    private int scoreWhenAllTypesOfEnemiesSpawned = 0;

    private int prevHordeCounter = 0;
    private int hordeCounter { get => Score.value / 5000; }
    private bool mobSpawning = false;
    private bool strongMobSpawning = false;
    private bool hordeSpawning { get => mobSpawning && strongMobSpawning; }

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

        startIncreasingSpeed = false;
        mobSpawning = false;
        strongMobSpawning = false;
        scoreWhenAllTypesOfEnemiesSpawned = 0;
        prevHordeCounter = 0;
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
        startIncreasingSpeed = true;
        if(scoreWhenAllTypesOfEnemiesSpawned == 0)
            scoreWhenAllTypesOfEnemiesSpawned = Score.value;
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

    public GameObject SpawnMob(int mobType)
    {
        var enemy = Instantiate(enemies[mobType]);
        SetTransform(enemy.transform, mobType == Enemies.BasicMob);

        var mobComp = enemy.GetComponent<Mob>();
        mobComp.direction = spawnDir;
        mobComp.speed += scoreSpeedMultiplier * additionalSpeed;

        if (mobType == Enemies.StrongMob)
            mobComp.speed = Mathf.Clamp(mobComp.speed, 0.5f, 2.0f);

        return enemy;
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
        if (!active || hordeSpawning)
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

        SpawnHorde();
    }

    private void SpawnHorde()
    {
        if (hordeCounter > prevHordeCounter)
        {
            Debug.Log("Spawn Horde");
            mobSpawning = true;
            strongMobSpawning = true;

            StartCoroutine(SpawnHordeMob());
            StartCoroutine(SpawnStrongHorde());
            prevHordeCounter = hordeCounter;
        }
    }

    private IEnumerator SpawnHordeMob()
    {
        var enemies = new List<GameObject>();

        yield return new WaitForSeconds(4.0f);

        for(int i = 0; i < hordeMobCount; i++)
        {
            enemies.Add(SpawnMob(Enemies.BasicMob));

            yield return new WaitForSeconds(2.0f + Random.Range(-0.5f, 0.5f));
        }

        while(true)
        {
            yield return new WaitForSeconds(0.5f);

            var enemiesExist = false;
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemiesExist = true;
                    break;
                }
            }

            if (!enemiesExist)
                break;
        }

        yield return new WaitForSeconds(3.0f + hordeDelay);

        mobSpawning = false;
    }

    private IEnumerator SpawnStrongHorde()
    {
        var enemies = new List<GameObject>();

        yield return new WaitForSeconds(5.0f);

        for (int i = 0; i < hordeStrongMobCount; i++)
        {
            enemies.Add(SpawnMob(Enemies.StrongMob));

            yield return new WaitForSeconds(3.0f + Random.Range(-0.5f, 0.5f));
        }

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            var enemiesExist = false;
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemiesExist = true;
                    break;
                }
            }

            if (!enemiesExist)
                break;
        }

        yield return new WaitForSeconds(3.0f + hordeDelay);

        strongMobSpawning = false;
    }
}
