using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using System;

public class ColumnWin : MonoBehaviour
{
    public column c;
    public int num;
    public event Action<column,int> Clicked;
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            Clicked?.Invoke(c,num);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
