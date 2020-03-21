using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject overPart;
    public Animator mainAnimator;
    public BoxCollider2D col;
    public AudioSource audioSource;
    public AudioClip endSound;
    public float duration = 10.0f;

    public void CreateBarrier()
    {
        mainAnimator.Play("Start");
        col.enabled = true;
        StartCoroutine(DisableAfterTime());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            col.gameObject.SendMessage("SetDead");
        }
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(mainAnimator.GetCurrentAnimatorStateInfo(0).length);

        overPart.SetActive(true);

        yield return new WaitForSeconds(9.30f);

        overPart.SetActive(false);
        col.enabled = false;

        mainAnimator.Play("End");

        audioSource.PlayOneShot(endSound);

        yield return new WaitForSeconds(0.7f);

        gameObject.SetActive(false);
    }
}
