using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGroundHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Potato")
        {
            Lives.livesLeft -= 1;
            Destroy(col.gameObject);
        }
    }
}
