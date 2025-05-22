using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardWin : MonoBehaviour
{
    public int num;
    public event Action<int> Clicked;
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            Clicked?.Invoke(num);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
