using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Scorcher scorcher;
    private Scorcher subScorcher;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public bool active { get; private set; } = true;

    public bool landed { get => rb.velocity.magnitude <= 0.0f + float.Epsilon; }

    void Start()
    {
        scorcher = new Scorcher(gameObject, GetComponent<SpriteRenderer>().material);

        var fire = transform.GetChild(0).gameObject;
        subScorcher = new Scorcher(fire, fire.GetComponent<SpriteRenderer>().material);

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Hit()
    {
        Schorche(0.05f);
    }

    public void Schorche(float time)
    {
        scorcher.scorchTime = time;
        subScorcher.scorchTime = time;
        sr.color = new Color(0xFF, 0xAE, 0x00);

        active = false;
        StartCoroutine(subScorcher.ScorchAndDestroy(false));
        StartCoroutine(scorcher.ScorchAndDestroy());
    }
    
    void Destroy()
    {
        Destroy(gameObject);
    }
}
