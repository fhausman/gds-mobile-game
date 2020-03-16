using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public void QuickFlash(float flashDelay = 0.0f)
    {
        StartCoroutine(FlashInternal(flashDelay));
    }

    IEnumerator FlashInternal(float flashDelay)
    {
        yield return new WaitForSeconds(flashDelay);

        var newPos = gameObject.transform.position;
        newPos.z = -newPos.z;
        gameObject.transform.position = newPos;

        yield return new WaitForSeconds(0.1f);

        newPos.z = -newPos.z;
        gameObject.transform.position = newPos;
    }
}
