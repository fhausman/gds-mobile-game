using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Norm : MonoBehaviour
{
    public static int norm = 200;

    private Text uiNorm;

    void Start()
    {
        norm = 200;
        uiNorm = GetComponent<Text>();
    }

    void Update()
    {
        if (norm < 100)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        uiNorm.text = norm.ToString() + "%";
    }
}