using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool shouldDestroy = false;

    private Animation anim;
    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    private void Update()
    {
        if (shouldDestroy)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        anim["Explode"].speed = 6.0f;
        anim.Play("Explode");

        var objs = Physics2D.OverlapCircleAll(transform.position, 1.0f, LayerMask.GetMask("Enemies"));
        foreach(var obj in objs)
        {
            obj.SendMessage("Hit");
        }
    }
}
