using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool shouldDestroy = false;

    public Animator anim { get; private set; }
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    private Rigidbody2D rb;
    private CircleCollider2D coll;
    private SpriteRenderer sr;
    private GameObject ps;
    private int sortingLayer;
    private int sortingOrder;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        sortingLayer = sr.sortingLayerID;
        sortingOrder = sr.sortingOrder;
        ps = transform.GetChild(0).gameObject;

        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("End") || Mathf.Abs(transform.position.x) > 11.5f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(rb);
        Destroy(coll);
        ps.SetActive(false);
        audioSource.clip = audioClips[1];
        audioSource.Play();

        anim.Play("Explode");
        var objs = Physics2D.OverlapCircleAll(transform.position, 0.75f, LayerMask.GetMask("Enemies"));
        foreach (var obj in objs)
        {
            obj.SendMessage("Hit");
        }
    }

    public void Hide()
    {
        sr.sortingLayerID = 0;
        sr.sortingOrder = 0;
        sr.enabled = false;
    }

    public void Show()
    {
        sr.sortingLayerID = sortingLayer;
        sr.sortingOrder = sortingOrder;
        sr.enabled = true;
    }

    public void EnableSpriteRenderer()
    {
        sr.enabled = true;
    }

    public void SetFlip(bool flip)
    {
        sr.flipX = flip;
    }

    public void Turn()
    {
        sr.flipX = !sr.flipX;
        anim.Play("Turn");
    }

    public void SetIdle()
    {
        ps.SetActive(false);
        //anim.Play("Idle");
    }

    public void SetCharge()
    {
        anim.Play("Charge");
    }

    public void Throw(float range)
    {
        ps.SetActive(true);
        anim.Play(range > 3.0f ? "Throw" : "Fly");
    }
}
