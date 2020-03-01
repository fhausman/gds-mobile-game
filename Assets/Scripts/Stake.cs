using UnityEngine;
using UnityEngine.SceneManagement;

public class Stake : MonoBehaviour
{
    public float durability = 1.0f;
    public float damagePerSecond = 0.5f;

    private SpriteRenderer sr;
    private Bounds bounds;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bounds = GetComponent<BoxCollider2D>().bounds;
    }

    void CheckForDamage()
    {
        var objs = Physics2D.OverlapAreaAll(
            bounds.min, bounds.max,
            LayerMask.GetMask("Enemies"));
        foreach (var obj in objs)
        {
            if (obj.gameObject.CompareTag("Torch"))
            {
                var torch = obj.gameObject.GetComponent<Torch>();
                if (!torch.landed)
                    continue;

            }
            else if(obj.gameObject.CompareTag("Enemy"))
            {
                obj.gameObject.SendMessage("Stop");
            }

            durability -= damagePerSecond * Time.fixedDeltaTime;
            Debug.Log(durability);
        }
    }

    void ShowDamage()
    {
        var color = sr.color;
        color.g = durability;
        color.b = durability;
        sr.color = color;
    }

    void Update()
    {
        ShowDamage();
        if (durability < 0.0f)
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void FixedUpdate()
    {
        CheckForDamage();
    }
}
