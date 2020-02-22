using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestBuffArea : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D col)
    {
        var obj = col.gameObject;
        if(obj.tag == "Enemy")
        {
            obj.SendMessage("IncreaseSpeed");
        }
    }
}
