using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputStates
{
    public const int
        Idle = 0,
        Charging = 1,
        Released = 2;
}

public class Idle : IState
{
    public Witch obj;

    public void Exit()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        obj.arc.direction = mousePos.x < 0 ? Vector2.left : Vector2.right;
    }

    public void Init()
    {
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))// && Input.touchCount == 1)
        {
            obj.stateMachine.ChangeState(InputStates.Charging);
        }
    }
}

public class Charging : IState
{
    public Witch obj;

    public void Exit()
    {
    }

    public void Init()
    {
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))// && Input.touchCount == 1)
        {
            obj.arc.range += obj.chargeSpeed * Time.deltaTime;
        }
        else
        {
            obj.stateMachine.ChangeState(InputStates.Released);
        }
    }
}

public class Released : IState
{
    public Witch obj;

    private float time = 0.0f;

    public void Exit()
    {
    }

    public void Init()
    {
        obj.InstatiateProjectile();
        obj.ResetArcRange();
        obj.StartCoroutine(InputDelay());
        obj.spriteRenderer.color = Color.yellow;

        time = 0.0f;
    }

    public void Update()
    {
        time += Time.deltaTime / obj.inputDelay;
        var newColor = Color.Lerp(Color.yellow, Color.white, time);
        obj.spriteRenderer.color = newColor;
    }

    IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(obj.inputDelay);

        obj.stateMachine.ChangeState(InputStates.Idle);
    }
}

public class Witch : MonoBehaviour
{
    public GameObject projectile;
    public float chargeSpeed = 10.0f;
    public float inputDelay = 0.25f;
    
    [HideInInspector]
    public ArcLine arc;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public StateMachine stateMachine { get; private set; } = new StateMachine();

    void Start()
    {
        arc = GetComponentInChildren<ArcLine>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stateMachine.AddState(InputStates.Idle, new Idle { obj = this });
        stateMachine.AddState(InputStates.Charging, new Charging { obj = this });
        stateMachine.AddState(InputStates.Released, new Released { obj = this });
        stateMachine.ChangeState(InputStates.Idle);
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void InstatiateProjectile()
    {
        var proj = Instantiate(projectile);
        proj.transform.position = transform.position;
        proj.GetComponent<Rigidbody2D>().AddForce(arc.direction * Arc.CalcLaunchSpeed(arc.range, transform.position.y + 5.0f, Physics2D.gravity.magnitude, 0), ForceMode2D.Impulse);
    }

    public void ResetArcRange()
    {
        arc.range = ArcLine.baseRange;
    }

    private IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.025f);
        stateMachine.ChangeState(InputStates.Idle);
    }
}
