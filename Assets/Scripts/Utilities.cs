using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpeedable
{
    void IncreaseSpeed();
}

public class Scorcher
{
    GameObject obj;
    Material material;

    public float scorchTime = 0.3f;

    public Scorcher(GameObject o, Material mat)
    {
        obj = o;
        material = mat;
    }

    public IEnumerator ScorchAndDestroy(bool destroy = true)
    {
        float time = scorchTime;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            material.SetFloat("_Fade", time / scorchTime);

            yield return null;
        }

        if(destroy)
            obj.SendMessage("Destroy");
    }

}
