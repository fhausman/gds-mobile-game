using System.Collections;
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
        slyboot.StartCoroutine(ThrowDelay());
    }

    public void Update()
    {
        slyboot.SetVelocity(slyboot.speedMultiplier * slyboot.speed * slyboot.direction);
    }

    IEnumerator ThrowDelay()
    {
        yield return new WaitForSeconds(5.0f);

        slyboot.stateMachine.ChangeState(SlybootStates.Throwing);
    }
}

public class SlybootThrowing: IState
{
    public Slyboot slyboot;

    public void Exit()
    {
    }

    public void Init()
    {
        slyboot.StartCoroutine(Throwing());
        slyboot.SetVelocity(Vector2.zero);
    }

    public void Update()
    {
    }

    IEnumerator Throwing()
    {
        yield return new WaitForSeconds(1.0f);

        slyboot.Throw();
        slyboot.stateMachine.ChangeState(SlybootStates.Running);
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
        slyboot.Disable();
        slyboot.StartCoroutine(slyboot.scorcher.ScorchAndDestroy());
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
    public float throwAngle = 80.0f;
    public GameObject torch;
    public StateMachine stateMachine { get; } = new StateMachine();
    public Scorcher scorcher;

    private Rigidbody2D rb;
    private Transform stakeTransform;
    private Material material;

    public void SetVelocity(Vector2 v)
    {
        rb.velocity = v;
    }

    public void ChangeDirection()
    {
        direction = -direction;
    }

    public void Throw()
    {
        var torchInstance = Instantiate(torch);
        torchInstance.transform.position = gameObject.transform.position;

        var trb = torchInstance.GetComponent<Rigidbody2D>();
        trb.AddForce(
            Arc.CalcLaunchSpeed(Mathf.Abs(transform.position.x-stakeTransform.position.x), 0, Physics2D.gravity.magnitude, Mathf.Deg2Rad * throwAngle) * (Quaternion.Euler(0, 0, throwAngle) * Vector2.right),
            ForceMode2D.Impulse);
        trb.AddTorque(-1.0f, ForceMode2D.Impulse);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Disable()
    {
        Destroy(rb);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stakeTransform = GameObject.Find("Stake").transform;
        material = GetComponent<SpriteRenderer>().material;

        scorcher = new Scorcher(gameObject, material);

        stateMachine.AddState(SlybootStates.Running, new SlybootRunning { slyboot = this });
        stateMachine.AddState(SlybootStates.Throwing, new SlybootThrowing { slyboot = this });
        stateMachine.AddState(SlybootStates.Dead, new SlybootDead { slyboot = this });
        stateMachine.ChangeState(SlybootStates.Running);
    }

    void FixedUpdate()
    {
        stateMachine.Update();
    }

    void Hit()
    {
        Score.value += 300;
        stateMachine.ChangeState(SlybootStates.Dead);
    }

    void IncreaseSpeed()
    {
        speedMultiplier = 2.0f;
    }
}
