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
        if (isLeftSideOfScreenClicked != obj.spriteRenderer.flipX)
        {
            obj.spriteRenderer.flipX = isLeftSideOfScreenClicked;
            obj.anim.SetTrigger("Turn");
            obj.projectileInstance.Turn();
            obj.turn = true;
        }
        //var newScale = obj.transform.localScale;
        //newScale.y = Mathf.Abs(newScale.y) * (isLeftSideOfScreenClicked ? -1.0f : 1.0f);
        //obj.transform.localScale = newScale;
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
        obj.turn = false;
    }

    public void Init()
    {
        if (!obj.turn)
        {
            obj.anim.SetTrigger("Charge");
            obj.projectileInstance.SetCharge();
        }
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

    public void Exit()
    {
    }

    public void Init()
    {
        obj.ReleaseProjectile();
        obj.ResetArcRange();
        obj.projectileInstance.Throw();
        obj.anim.SetTrigger("Throw");
        obj.StartCoroutine(InputDelay());
    }

    public void Update()
    {
    }

    IEnumerator InputDelay()
    {
        while (!obj.anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            yield return null;
        }

        while (obj.anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            yield return null;
        }

        obj.InstatiateProjectile();

        while(obj.anim.GetCurrentAnimatorStateInfo(0).IsTag("Unresponsive"))
        {
            yield return null;
        }

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
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    
    [HideInInspector]
    public ArcLine arc;
    [HideInInspector]
    public Projectile projectileInstance;
    [HideInInspector]
    public bool turn = false;

    public StateMachine stateMachine { get; private set; } = new StateMachine();

    void Start()
    {
        arc = GetComponentInChildren<ArcLine>();

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
        projectileInstance = Instantiate(projectile).GetComponent<Projectile>();
        projectileInstance.transform.position = arc.transform.position;
        projectileInstance.GetComponent<Rigidbody2D>().isKinematic = true;
        projectileInstance.SetIdle();
        projectileInstance.SetFlip(spriteRenderer.flipX);
    }

    public void ReleaseProjectile()
    {
        if (projectileInstance)
        {
            projectileInstance.GetComponent<Rigidbody2D>().isKinematic = false;
            projectileInstance.GetComponent<Rigidbody2D>().AddForce(arc.direction * Arc.CalcLaunchSpeed(arc.range, transform.position.y + 5.0f, Physics2D.gravity.magnitude, 0), ForceMode2D.Impulse);
        }
    }

    public void ResetArcRange()
    {
        arc.range = ArcLine.baseRange;
    }

    public void SetInactive()
    {
        ResetArcRange();
        stateMachine.ChangeState(InputStates.Inactive);
    }

    public void SetActive()
    {
        InstatiateProjectile();
        stateMachine.ChangeState(InputStates.Idle);
    }

    private IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.025f);
        stateMachine.ChangeState(InputStates.Idle);
    }
}
