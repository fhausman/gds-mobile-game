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
            obj.Turn();
            obj.turn = true;
        }
    }

    public void Init()
    {
    }

    public void Update()
    {
        var animInfo = obj.projectileInstance.anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName("Charging_new") || animInfo.IsName("Intro"))
            return;

        if (GameManager.acceptsPlayerInput && Input.GetMouseButton(0))// && Input.touchCount == 1)
        {
            if(EventSystem.current.IsPointerOverGameObject(
#if !UNITY_EDITOR
                Input.GetTouch(0).fingerId
#endif
                ))
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
        if (!obj.turn)
        {
            obj.anim.Play("Charge");
            obj.projectileInstance.SetCharge();
        }
        obj.turn = false;
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))// && Input.touchCount == 1)
        {
            obj.arc.range += obj.chargeSpeed * Time.deltaTime;
            obj.arc.range = Mathf.Clamp(obj.arc.range, 0.0f, 20.0f);
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
        obj.audioSource.Play();
        obj.projectileInstance.Throw(obj.arc.range);
        obj.anim.Play(obj.arc.range > 3.0f ? "Throw" : "FastThrow");

        obj.ReleaseProjectile();
        obj.ResetArcRange();
        obj.StartCoroutine(InputDelay());
    }

    public void Update()
    {
    }

    IEnumerator InputDelay()
    {
        while (!obj.anim.GetCurrentAnimatorStateInfo(0).IsTag("Throw"))
        {
            yield return null;
        }

        while (obj.anim.GetCurrentAnimatorStateInfo(0).IsTag("Throw"))
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
        obj.anim.Play("Empty");
    }

    public void Update()
    {
    }
}

public class Witch : MonoBehaviour
{
    public GameObject projectile;
    public GameObject death;
    public float chargeSpeed = 10.0f;
    public float inputDelay = 0.25f;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public Flash flash;
    public AudioSource audioSource;
    public Sprite inactiveSprite;

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

    public void InstatiateProjectile(bool playIntroAnimation = false)
    {
        projectileInstance = Instantiate(projectile).GetComponent<Projectile>();
        projectileInstance.transform.position = arc.transform.position;
        projectileInstance.GetComponent<Rigidbody2D>().isKinematic = true;
        projectileInstance.SetIdle();
        projectileInstance.SetFlip(spriteRenderer.flipX);

        if(playIntroAnimation)
            projectileInstance.anim.Play("Intro");
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
        StopAllCoroutines();

        ResetArcRange();
        stateMachine.ChangeState(InputStates.Inactive);
        spriteRenderer.sprite = inactiveSprite;
    }

    public void SetActive()
    {
        death.SetActive(false);

        anim.Play("Intro");
        flash.QuickFlash(2.0f);
        InstatiateProjectile(true);
        stateMachine.ChangeState(InputStates.Idle);
    }

    public void Turn()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        anim.Play("Turn");
        projectileInstance.Turn();
    }

    public void Burn()
    {
        death.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        death.SetActive(true);
    }

    public void HideDeathFire()
    {
        death.SetActive(false);
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
        projectileInstance.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Show()
    {
        spriteRenderer.enabled = true;
        projectileInstance.GetComponent<SpriteRenderer>().enabled = true;
    }

    private IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.025f);
        stateMachine.ChangeState(InputStates.Idle);
    }
}
