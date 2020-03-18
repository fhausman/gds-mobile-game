using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject overPart;
    public Animator mainAnimator;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(string.Format("{0} : {1}", col.gameObject.layer, LayerMask.NameToLayer("Enemies")));
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            col.gameObject.SendMessage("SetDead");
        }
    }
}
