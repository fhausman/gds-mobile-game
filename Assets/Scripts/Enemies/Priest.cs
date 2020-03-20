using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PriestStates
{
    public const int
        Walking = 0,
        Buffing = 1,
        Dead = 2,
        Idle = 3,
        DeadByLightning = 4;
}

public class PriestWalking : IState
{
    public Priest priest;

    public void Exit()
    {
    }

    public void Init()
    {
    }

    public void Update()
    {
        float step = priest.speed * Time.deltaTime;
        priest.transform.position = Vector2.MoveTowards(priest.transform.position, priest.target, step);

        if(Mathf.Abs(priest.transform.position.x - priest.target.x) <= 0 + 0.1f)
        {
            priest.stateMachine.ChangeState(PriestStates.Buffing);
        }
    }
}

public class PriestBuffing : IState
{
    public Priest priest;

    public void Exit()
    {
    }

    public void Init()
    {
        priest.ActivateBuffArea();
        priest.anim.SetTrigger("Idle");
    }

    public void Update()
    {
    }
}

public class PriestDead : IState
{
    public Priest priest;

    public void Exit()
    {
    }

    public void Init()
    {
        priest.Disable();
        priest.StartCoroutine(priest.scorcher.ScorchAndDestroy());
    }

    public void Update()
    {
    }
}

public class PriestDeadByLightning : IState
{
    public Priest priest;

    public void Exit()
    {
    }

    public void Init()
    {
        priest.Disable();
        priest.anim.Play("DeadByLightning");
    }

    public void Update()
    {
        if (!priest.anim.GetCurrentAnimatorStateInfo(0).IsName("DeadByLightning"))
            priest.Destroy();
    }
}

public class PriestIdle : IState
{
    public Priest priest;

    public void Exit()
    {
    }

    public void Init()
    {
        priest.anim.SetTrigger("Idle");
    }

    public void Update()
    {
    }
}

public class Priest : MonoBehaviour
{
    public Vector2 direction;
    public Vector2 target;
    public float speed;
    public int score = 50;
    private bool isCurrentlyBuffing { get => anim.GetCurrentAnimatorStateInfo(0).IsName("Cast_1"); }

    public StateMachine stateMachine { get; } = new StateMachine();
    public Scorcher scorcher;
    public Animator anim;

    private Rigidbody2D rb;
    private PriestBuffArea buffArea;
    private Material material;
    private SoundManager sm;

    public void ActivateBuffArea()
    {
        Debug.Log("Buff area activated!");
        buffArea.Active(true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Disable()
    {
        buffArea.Active(false);
        Destroy(rb);
    }

    public void SetDead()
    {
        if (stateMachine.currentStateId == PriestStates.Dead)
            return;

        Score.value += score;
        stateMachine.ChangeState(PriestStates.Dead);
    }

    public void DeadByLightning()
    {
        Score.value += score;
        stateMachine.ChangeState(PriestStates.DeadByLightning);
    }

    public void SetIdle()
    {
        stateMachine.ChangeState(PriestStates.Idle);
    }

    public void RestorePreviousState()
    {
        stateMachine.ChangeState(PriestStates.Walking);
    }

    public void PlayBuffAnimation(ISpeedable obj)
    {
        if(!isCurrentlyBuffing)
            anim.SetTrigger("Cast");

        StartCoroutine(SpeedMobUp(obj));
    }

    IEnumerator SpeedMobUp(ISpeedable obj)
    {
        yield return null;

        while(isCurrentlyBuffing)
        {
            yield return null;
        }

        sm.PriestBellSound();
        obj.IncreaseSpeed();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        buffArea = transform.GetChild(0).gameObject.GetComponent<PriestBuffArea>();
        buffArea.onBuffBehaviour += PlayBuffAnimation;
        material = GetComponent<SpriteRenderer>().material;
        sm = GameObject.Find("Ambient Sounds").GetComponent<SoundManager>();

        scorcher = new Scorcher(gameObject, material);

        stateMachine.AddState(PriestStates.Walking, new PriestWalking { priest = this });
        stateMachine.AddState(PriestStates.Buffing, new PriestBuffing { priest = this });
        stateMachine.AddState(PriestStates.Dead, new PriestDead { priest = this });
        stateMachine.AddState(PriestStates.Idle, new PriestIdle { priest = this });
        stateMachine.AddState(PriestStates.DeadByLightning, new PriestDeadByLightning { priest = this });
        stateMachine.ChangeState(PriestStates.Walking);
    }

    void Update()
    {
        stateMachine.Update();
    }

    void Hit()
    {
        SetDead();
    }
}
