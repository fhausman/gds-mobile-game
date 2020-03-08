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
        mob.Disable();
        mob.StartCoroutine(mob.scorcher.ScorchAndDestroy());
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

    private Rigidbody2D rb;
    private Animator anim;
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
        if(!anim.GetCurrentAnimatorStateInfo(0).IsTag("SettingFire"))
        {
            Stop();
            anim.SetTrigger("StartFire");
        }
    }

    public bool DealsDamage { get => anim.GetCurrentAnimatorStateInfo(0).IsName("Setting Fire"); }

    public void Stop()
    {
        stateMachine.ChangeState(MobStates.Stay);
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;

        scorcher = new Scorcher(gameObject, material);

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
            Score.value += score;
            stateMachine.ChangeState(MobStates.Dead);
        }
        //else
        //{
        //    stateMachine.ChangeState(MobStates.Stay);
        //}
    }
}
