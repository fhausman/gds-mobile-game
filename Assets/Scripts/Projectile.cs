﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool shouldDestroy = false;

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D coll;
    private SpriteRenderer sr;
    private GameObject ps;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        ps = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(rb);
        Destroy(coll);
        ps.SetActive(false);

        anim.SetTrigger("Explode");
        var objs = Physics2D.OverlapCircleAll(transform.position, 0.75f, LayerMask.GetMask("Enemies"));
        foreach (var obj in objs)
        {
            obj.SendMessage("Hit");
        }

    }

    public void SetFlip(bool flip)
    {
        sr.flipX = flip;
    }

    public void Turn()
    {
        sr.flipX = !sr.flipX;
        anim.SetTrigger("Turn");
    }

    public void SetIdle()
    {
        ps.SetActive(false);
        anim.SetTrigger("Idle");
    }

    public void SetCharge()
    {
        anim.SetTrigger("Charge");
    }

    public void Throw()
    {
        ps.SetActive(true);
        anim.SetTrigger("Throw");
    }
}
