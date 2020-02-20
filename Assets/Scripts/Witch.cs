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
        if (Input.GetMouseButton(0) && Input.touchCount == 1)
        {
            obj.inputState.ChangeState(InputStates.Charging);
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
        if (Input.GetMouseButton(0) && Input.touchCount == 1)
        {
            obj.arc.range += obj.chargeSpeed * Time.deltaTime;
        }
        else
        {
            obj.inputState.ChangeState(InputStates.Released);
        }
    }
}

public class Released : IState
{
    public Witch obj;

    public void Exit()
    {
    }

    public void Init()
    {
        obj.InstatiateProjectile();
        obj.ResetArcRange();
    }

    public void Update()
    {
        obj.StartCoroutine("InputDelay");
    }
}

public class Witch : MonoBehaviour
{
    public GameObject projectile;
    public float chargeSpeed = 10.0f;
    
    [HideInInspector]
    public ArcLine arc;

    public StateMachine inputState { get; private set; } = new StateMachine();

    void Start()
    {
        arc = GetComponentInChildren<ArcLine>();

        inputState.AddState(InputStates.Idle, new Idle { obj = this });
        inputState.AddState(InputStates.Charging, new Charging { obj = this });
        inputState.AddState(InputStates.Released, new Released { obj = this });
        inputState.ChangeState(InputStates.Idle);
    }

    void Update()
    {
        inputState.Update();
    }

    public void InstatiateProjectile()
    {
        var proj = Instantiate(projectile);
        proj.transform.position = transform.position;
        proj.GetComponent<Rigidbody2D>().AddForce(arc.direction * LaunchSpeed(arc.range, transform.position.y + 5.0f, Physics2D.gravity.magnitude, 0), ForceMode2D.Impulse);
    }

    public void ResetArcRange()
    {
        arc.range = ArcLine.baseRange;
    }

    private float LaunchSpeed(float distance, float yOffset, float gravity, float angle)
    {
        float speed = (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));

        return speed;
    }

    private IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.025f);
        inputState.ChangeState(InputStates.Idle);
    }
}
