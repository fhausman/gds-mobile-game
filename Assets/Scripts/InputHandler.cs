using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private GameObject clickedObject = null;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                clickedObject = hit.collider.gameObject;
            }
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            if(clickedObject != null)
            {
                Debug.Log(clickedObject);
                clickedObject.GetComponent<Outbreak>().Shrink();
                clickedObject = null;
            }
        }
    }
}
