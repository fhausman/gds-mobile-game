using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlybootBounds : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Slyboot"))
        {
            col.gameObject.GetComponent<Slyboot>().ChangeDirection();
        }
    }
}
