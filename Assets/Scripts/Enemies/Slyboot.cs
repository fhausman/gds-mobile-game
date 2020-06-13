using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlybootStates
{
    public const int
        Running = 0,
        Throwing = 1,
        Dead = 2,
        Idle = 3;
}

public class SlybootRunning : IState
{
    public Slyboot slyboot;

    public void Exit()
    {
    }

    public void Init()
    {
        slyboot.anim.SetTrigger("Run");
        slyboot.StartCoroutine(ThrowDelay());
    }

    public void Update()
    {
        slyboot.SetVelocity(slyboot.speedMultiplier * Mathf.Clamp(slyboot.speed, 0.5f, 2.5f) * slyboot.direction);
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
        slyboot.anim.SetTrigger("Throw");
        yield return new WaitForSeconds(0.4f);

        slyboot.Throw();
        yield return new WaitForSeconds(0.85f);

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
        slyboot.audioSource.PlayOneShot(slyboot.babaDead);
        //slyboot.StartCoroutine(slyboot.scorcher.ScorchAndDestroy());
        slyboot.anim.Play("Death");
    }

    public void Update()
    {
    }
}

public class SlybootIdle : IState
{
    public Slyboot slyboot;

    public void Exit()
    {
    }

    public void Init()
    {
        slyboot.anim.SetTrigger("Idle");
        slyboot.SetVelocity(Vector2.zero);
    }

    public void Update()
    {
    }
}

public class Slyboot : MonoBehaviour, ISpeedable
{
    public Vector2 direction;
    public bool movingForward = true;
    public float speed;
    public float speedMultiplier = 1.0f;
    public float throwAngle = 80.0f;
    public GameObject torch;
    public Transform torchSpawnPosition;
    public StateMachine stateMachine { get; } = new StateMachine();
    public Scorcher scorcher;
    public Animator anim;
    public SpriteRenderer sr;
    public RuntimeAnimatorController ForwardsRuntimeController;
    public AnimatorOverrideController BackwardsRuntimeController;
    public AudioSource audioSource;
    public AudioClip babaDead;
    public AudioClip babaThrow;

    private Rigidbody2D rb;
    private Transform stakeTransform;
    private Material material;
    private Vector2 facingDirection;
    private Vector2 throwVector { get => (Quaternion.Euler(0, 0, throwAngle * facingDirection.x) * facingDirection); }

    public void SetVelocity(Vector2 v)
    {
        if(rb != null)
            rb.velocity = v;
    }

    public void ChangeDirection()
    {
        movingForward = !movingForward;
        direction = -direction;

        anim.runtimeAnimatorController = movingForward ? ForwardsRuntimeController : BackwardsRuntimeController;
    }

    public void Throw()
    {
        audioSource.PlayOneShot(babaThrow);

        var torchInstance = Instantiate(torch);
        torchInstance.transform.position = torchSpawnPosition.position;

        var trb = torchInstance.GetComponent<Rigidbody2D>();
        trb.AddForce(
            Arc.CalcLaunchSpeed(Mathf.Abs(transform.position.x-stakeTransform.position.x), 0, Physics2D.gravity.magnitude, Mathf.Deg2Rad * throwAngle) * throwVector,
            ForceMode2D.Impulse);
        trb.AddTorque(0.2f, ForceMode2D.Impulse);
    }

    public void Destroy()
    {
        StopAllCoroutines();
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        sr.enabled = false;

        while (audioSource.isPlaying)
            yield return null;

        Destroy(gameObject);
    }

    public void Disable()
    {
        StopAllCoroutines();
        Destroy(rb);
    }

    public void IncreaseSpeed()
    {
        speedMultiplier = 2.0f;
    }

    public void SetDead()
    {
        if (stateMachine.currentStateId == SlybootStates.Dead)
            return;

        Score.value += 300;
        stateMachine.ChangeState(SlybootStates.Dead);
    }

    public void SetIdle()
    {
        stateMachine.ChangeState(SlybootStates.Idle);
    }

    public void RestorePreviousState()
    {
        stateMachine.ChangeState(SlybootStates.Running);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        material = sr.material;
        stakeTransform = GameObject.Find("Stake").transform;

        scorcher = new Scorcher(gameObject, material);

        stateMachine.AddState(SlybootStates.Running, new SlybootRunning { slyboot = this });
        stateMachine.AddState(SlybootStates.Throwing, new SlybootThrowing { slyboot = this });
        stateMachine.AddState(SlybootStates.Dead, new SlybootDead { slyboot = this });
        stateMachine.AddState(SlybootStates.Idle, new SlybootIdle { slyboot = this });
        stateMachine.ChangeState(SlybootStates.Running);

        facingDirection = direction;
    }

    void FixedUpdate()
    {
        stateMachine.Update();
    }

    void Hit()
    {
        SetDead();
    }
}
