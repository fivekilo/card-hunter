using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Camp : MonoBehaviour
{
    public event Action ClickEvent;
    private void OnMouseDown()
    {
        ClickEvent?.Invoke();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
