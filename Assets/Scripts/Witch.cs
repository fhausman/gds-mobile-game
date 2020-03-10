using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class InputStates
{
    public const int
        Idle = 0,
        Charging = 1,
        Released = 2,
        Inactive = 3;
}

public class Idle : IState
{
    public Witch obj;

    public void Exit()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isLeftSideOfScreenClicked = mousePos.x < 0;

        obj.arc.direction = isLeftSideOfScreenClicked ? Vector2.left : Vector2.right;
        obj.spriteRenderer.flipX = isLeftSideOfScreenClicked;
    }

    public void Init()
    {
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))// && Input.touchCount == 1)
        {
            if(EventSystem.current.IsPointerOverGameObject(/*Input.GetTouch(0).fingerId*/))
            {
                return;
            }

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
        obj.InstatiateProjectile();
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
        obj.ReleaseProjectile();
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

public class Inactive : IState
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
    }
}

public class Witch : MonoBehaviour
{
    public GameObject projectile;
    public float chargeSpeed = 10.0f;
    public float inputDelay = 0.25f;
    private GameObject projectileInstance;
    
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
        stateMachine.AddState(InputStates.Inactive, new Inactive { obj = this });
        stateMachine.ChangeState(InputStates.Inactive);
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void InstatiateProjectile()
    {
        projectileInstance = Instantiate(projectile);
        projectileInstance.transform.position = arc.transform.position;
        projectileInstance.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void ReleaseProjectile()
    {
        projectileInstance.GetComponent<Rigidbody2D>().isKinematic = false;
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(arc.direction * Arc.CalcLaunchSpeed(arc.range, transform.position.y + 5.0f, Physics2D.gravity.magnitude, 0), ForceMode2D.Impulse);
    }

    public void ResetArcRange()
    {
        arc.range = ArcLine.baseRange;
    }

    public void SetInactive()
    {
        stateMachine.ChangeState(InputStates.Inactive);
    }

    public void SetActive()
    {
        stateMachine.ChangeState(InputStates.Idle);
    }

    private IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.025f);
        stateMachine.ChangeState(InputStates.Idle);
    }
}
