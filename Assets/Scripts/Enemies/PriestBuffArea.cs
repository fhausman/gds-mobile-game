using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestBuffArea : MonoBehaviour
{
    private bool buffActive = false;

    public void Active(bool val)
    {
        buffActive = val;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!buffActive)
            return;

        var obj = col.gameObject;
        if(obj.tag == "Enemy")
        {
            obj.GetComponent<ISpeedable>().IncreaseSpeed();
        }
    }
}
