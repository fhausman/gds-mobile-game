using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stake : MonoBehaviour
{
    public float durability = 1.0f;
    public float damagePerSecond = 0.5f;
    public bool active = false;
    public delegate void OnGameOver();
    public OnGameOver onGameOver;

    private SpriteRenderer sr;
    private Animator anim;
    private Bounds bounds;

    public void ResetDurability()
    {
        durability = 1.0f;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bounds = GetComponent<BoxCollider2D>().bounds;
        anim = GetComponentInChildren<Animator>();
    }

    void CheckForDamage()
    {
        var objs = Physics2D.OverlapAreaAll(
            bounds.min, bounds.max,
            LayerMask.GetMask("Enemies"));
        foreach (var obj in objs)
        {
            if (obj.gameObject.CompareTag("Torch"))
            {
                var torch = obj.gameObject.GetComponent<Torch>();
                if (torch.active)
                {
                    torch.Schorche(0.5f);
                    durability -= damagePerSecond;
                }
                continue;
            }
            else if(obj.gameObject.CompareTag("Enemy"))
            {
                var mob = obj.GetComponent<Mob>();
                if (!mob.DealsDamage)
                {
                    obj.gameObject.SendMessage("StartSettingFire");
                    continue;
                }
            }

            durability -= damagePerSecond * Time.fixedDeltaTime;
            Debug.Log(durability);
        }
    }

    IEnumerator TorchHit(Torch torch)
    {
        yield return null;

    }

    void ShowDamage()
    {
        anim.SetFloat("Durability", durability);
    }

    void Update()
    {
        ShowDamage();
        if (!active)
            return;

        if (durability < 0.0f)
        {
            onGameOver();
        }
    }

    public void Deactivate()
    {
        active = false;
    }

    public void Activate()
    {
        active = true;
    }

    void FixedUpdate()
    {
        CheckForDamage();
    }
}
