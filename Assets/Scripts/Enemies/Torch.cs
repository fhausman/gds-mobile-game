using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Scorcher scorcher;
    private Scorcher subScorcher;
    private Rigidbody2D rb;

    public bool active { get; private set; } = true;

    public bool landed { get => rb.velocity.magnitude <= 0.0f + float.Epsilon; }

    void Start()
    {
        scorcher = new Scorcher(gameObject, GetComponent<SpriteRenderer>().material);
        scorcher.scorchTime = 0.5f;

        var fire = transform.GetChild(0).gameObject;
        subScorcher = new Scorcher(fire, fire.GetComponent<SpriteRenderer>().material);
        subScorcher.scorchTime = 0.5f;

        rb = GetComponent<Rigidbody2D>();
    }

    public void Hit()
    {
        active = false;
        StartCoroutine(subScorcher.ScorchAndDestroy(false));
        StartCoroutine(scorcher.ScorchAndDestroy());
    }
    
    void Destroy()
    {
        Destroy(gameObject);
    }
}
