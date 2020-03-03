using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorcher
{
    GameObject obj;
    Material material;

    float scorchTime = 0.3f;

    public Scorcher(GameObject o, Material mat)
    {
        obj = o;
        material = mat;
    }

    public IEnumerator ScorchAndDestroy()
    {
        float time = scorchTime;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            material.SetFloat("_Fade", time / scorchTime);

            yield return null;
        }

        obj.SendMessage("Destroy");
    }

}
