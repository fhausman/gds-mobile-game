using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MobStates
{
    public const int
        Running = 0,
        Stay = 1,
        Dead = 2;
}

public class MobRunning : IState
{
    public Mob mob;

    public void Exit()
    {
    }

    public void Init()
    {
    }

    public void Update()
    {
        mob.SetVelocity(mob.speedMultiplier * mob.speed * mob.direction);
    }
}

public class MobStay : IState
{
    public Mob mob;

    public void Exit()
    {
    }

    public void Init()
    {
        mob.SetVelocity(Vector2.zero);
    }

    public void Update()
    {
    }
}

public class MobDead : IState
{
    public Mob mob;

    public void Exit()
    {
    }

    public void Init()
    {
        mob.Destroy();
    }

    public void Update()
    {
    }
}

public class Mob : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float speedMultiplier = 1.0f;
    public int numberOfLives;
    public StateMachine stateMachine { get; } = new StateMachine();

    private Rigidbody2D rb;

    public void SetVelocity(Vector2 v)
    {
        rb.velocity = v;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Stop()
    {
        stateMachine.ChangeState(MobStates.Stay);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine.AddState(MobStates.Running, new MobRunning { mob = this });
        stateMachine.AddState(MobStates.Stay, new MobStay { mob = this });
        stateMachine.AddState(MobStates.Dead, new MobDead { mob = this });
        stateMachine.ChangeState(MobStates.Running);
    }

    void Update()
    {
        stateMachine.Update();
    }

    void Hit()
    {
        --numberOfLives;
        if (numberOfLives == 0)
        {
            Score.value += 1;
            stateMachine.ChangeState(MobStates.Dead);
        }
    }

    void IncreaseSpeed()
    {
        speedMultiplier = 2.0f;
    }
}
