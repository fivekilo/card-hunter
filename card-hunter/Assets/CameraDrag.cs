using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 pos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0)
        {
            GetComponent<Camera>().orthographicSize -= 0.2f;
        }
        else if(scrollInput<0)
        {
            GetComponent<Camera>().orthographicSize += 0.2f;
        }
        if (Input.GetMouseButtonDown(2))
        {
            isDragging = true;
            pos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }
        if (isDragging)
        {
            Vector3 delta= GetComponent<Camera>().orthographicSize * (Input.mousePosition - pos)/200;
            transform.position = transform.position - delta;
            pos = Input.mousePosition;
        }
    }
}
