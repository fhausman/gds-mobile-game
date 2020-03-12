using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestBuffArea : MonoBehaviour
{
    public delegate void OnBuffBehaviour(ISpeedable obj);
    public OnBuffBehaviour onBuffBehaviour;

    private bool buffActive = false;

    public void Active(bool val)
    {
        buffActive = val;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!buffActive)
            return;

        var obj = col.gameObject;
        if (obj.tag == "Enemy")
        {
            onBuffBehaviour(obj.GetComponent<ISpeedable>());
        }
    }
}
