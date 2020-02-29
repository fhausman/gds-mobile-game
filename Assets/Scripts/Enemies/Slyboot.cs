﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlybootStates
{
    public const int
        Running = 0,
        Throwing = 1,
        Dead = 2;
}

public class SlybootRunning : IState
{
    public Slyboot slyboot;

    public void Exit()
    {
    }

    public void Init()
    {
    }

    public void Update()
    {
        slyboot.SetVelocity(slyboot.speedMultiplier * slyboot.speed * slyboot.direction * Time.deltaTime);
    }
}

public class SlybootThrowing: IState
{
    public Slyboot slyboot;

    private bool isThrowing = false;

    public void Exit()
    {
    }

    public void Init()
    {
    }

    public void Update()
    {
        if (!isThrowing)
        {
            slyboot.StartCoroutine(Throwing());
        }
    }

    IEnumerator Throwing()
    {
        isThrowing = true;

        Debug.Log("Started throwing");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Thrown");

        isThrowing = false;
    }
}

public class SlybootDead : IState
{
    public Slyboot slyboot;

    public void Exit()
    {
    }

    public void Init()
    {
        slyboot.Destroy();
    }

    public void Update()
    {
    }
}

public class Slyboot : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float speedMultiplier = 1.0f;
    public StateMachine stateMachine { get; } = new StateMachine();

    private Rigidbody2D rb;

    public void SetVelocity(Vector2 v)
    {
        rb.velocity = v;
    }

    public void ChangeDirection()
    {
        direction = -direction;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine.AddState(SlybootStates.Running, new SlybootRunning { slyboot = this });
        stateMachine.AddState(SlybootStates.Throwing, new SlybootThrowing { slyboot = this });
        stateMachine.AddState(SlybootStates.Dead, new SlybootDead { slyboot = this });
        stateMachine.ChangeState(SlybootStates.Running);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    void Hit()
    {
        Score.value += 1;
        stateMachine.ChangeState(SlybootStates.Dead);
    }

    void IncreaseSpeed()
    {
        speedMultiplier = 2.0f;
    }
}