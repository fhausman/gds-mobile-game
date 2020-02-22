using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PriestStates
{
    public const int
        Walking = 0,
        Buffing = 1,
        Dead = 2;
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

        if(Mathf.Abs(priest.transform.position.x - priest.target.x) <= 0 + step)
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
        priest.Destroy();
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

    public StateMachine stateMachine { get; } = new StateMachine();

    private Rigidbody2D rb;
    private GameObject buffArea;

    public void ActivateBuffArea()
    {
        Debug.Log("Buff area activated!");
        buffArea.SetActive(true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        buffArea = transform.GetChild(0).gameObject;

        stateMachine.AddState(PriestStates.Walking, new PriestWalking { priest = this });
        stateMachine.AddState(PriestStates.Buffing, new PriestBuffing { priest = this });
        stateMachine.AddState(PriestStates.Dead, new PriestDead { priest = this });
        stateMachine.ChangeState(PriestStates.Walking);
    }

    void Update()
    {
        stateMachine.Update();
    }

    void Hit()
    {
        Score.value += 1;
        stateMachine.ChangeState(PriestStates.Dead);
    }
}
