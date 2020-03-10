using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MobStates
{
    public const int
        Running = 0,
        SetFire = 1,
        Damaged = 2,
        Dead = 3,
        Idle = 4;
}

public class MobRunning : IState
{
    public Mob mob;

    public void Exit()
    {
    }

    public void Init()
    {
        mob.anim.SetTrigger("Run");
    }

    public void Update()
    {
        mob.SetVelocity(mob.speedMultiplier * Mathf.Clamp(mob.speed, 0.0f, 3.0f) * mob.direction);
    }
}

public class MobSetFire : IState
{
    public Mob mob;

    public void Exit()
    {
    }

    public void Init()
    {
        mob.SetVelocity(Vector2.zero);
        mob.anim.SetTrigger("SetFire");
    }

    public void Update()
    {
    }
}

public class MobDamaged : IState
{
    public Mob mob;
    private int previousState;

    public void Exit()
    {
    }

    public void Init()
    {
        mob.SetVelocity(Vector2.zero);
        mob.anim.SetTrigger("Damage");
        previousState = mob.stateMachine.currentStateId;
    }

    public void Update()
    {
        if(mob.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            mob.stateMachine.ChangeState(previousState);
        }
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
        mob.Disable();
        mob.StartCoroutine(mob.scorcher.ScorchAndDestroy());
    }

    public void Update()
    {
    }
}

public class MobIdle : IState
{
    public Mob mob;

    public void Exit()
    {
    }

    public void Init()
    {
        mob.anim.SetTrigger("Idle");
        mob.SetVelocity(Vector2.zero);
    }

    public void Update()
    {
    }
}

public class Mob : MonoBehaviour, ISpeedable
{
    public Vector2 direction;
    public float speed;
    public float speedMultiplier = 1.0f;
    public int score = 100;
    public int numberOfLives;
    public StateMachine stateMachine { get; } = new StateMachine();
    public Scorcher scorcher;
    public bool DealsDamage { get => anim.GetCurrentAnimatorStateInfo(0).IsName("Setting Fire"); }

    private Rigidbody2D rb;
    public Animator anim { get; private set; }
    private Material material;

    public void SetVelocity(Vector2 v)
    {
        if(rb != null) 
            rb.velocity = v;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void StartSettingFire()
    {
        if(stateMachine.currentStateId != MobStates.SetFire && stateMachine.currentStateId != MobStates.Idle)
        {
            stateMachine.ChangeState(MobStates.SetFire);
        }
    }

    public void SetDamage()
    {
        stateMachine.ChangeState(MobStates.Damaged);
    }

    public void Disable()
    {
        Destroy(rb);
    }

    public void Scorch(float scorchLevel)
    {
        material.SetFloat("_Fade", scorchLevel);
    }

    public void IncreaseSpeed()
    {
        speedMultiplier = 2.0f;
    }

    public void SetDead()
    {
        if (stateMachine.currentStateId == MobStates.Dead)
            return;
        
        Score.value += score;
        stateMachine.ChangeState(MobStates.Dead);
    }

    public void SetIdle()
    {
        stateMachine.ChangeState(MobStates.Idle);
    }

    public void RestorePreviousState()
    {
        stateMachine.ChangeState(MobStates.Running);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;

        scorcher = new Scorcher(gameObject, material);

        stateMachine.AddState(MobStates.Running, new MobRunning { mob = this });
        stateMachine.AddState(MobStates.SetFire, new MobSetFire { mob = this });
        stateMachine.AddState(MobStates.Damaged, new MobDamaged { mob = this });
        stateMachine.AddState(MobStates.Dead, new MobDead { mob = this });
        stateMachine.AddState(MobStates.Idle, new MobIdle { mob = this });
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
            SetDead();
        }
        else
        {
            SetDamage();
        }
    }
}
