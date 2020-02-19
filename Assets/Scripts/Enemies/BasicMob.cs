using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BMStates
{
    public const int
        Running = 0,
        Dead = 1;
}

public class BMRunning : IState
{
    public BasicMob obj;

    public void Exit()
    {
    }

    public void Init()
    {
    }

    public void Update()
    {
        obj.SetVelocity(obj.speed * obj.direction);
    }
}

public class BMDead : IState
{
    public BasicMob obj;

    public void Exit()
    {
    }

    public void Init()
    {
        obj.Destroy();
    }

    public void Update()
    {
    }
}

public class BasicMob : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public StateMachine stateMachine { get; private set; } = new StateMachine();

    private Rigidbody2D rb;

    public void SetVelocity(Vector2 v)
    {
        rb.velocity = v;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine.AddState(BMStates.Running, new BMRunning { obj = this });
        stateMachine.AddState(BMStates.Dead, new BMDead { obj = this });
        stateMachine.ChangeState(BMStates.Running);
    }

    void Update()
    {
        stateMachine.Update();
    }

    void Die()
    {
        Score.value += 1;
        stateMachine.ChangeState(BMStates.Dead);
    }
}
