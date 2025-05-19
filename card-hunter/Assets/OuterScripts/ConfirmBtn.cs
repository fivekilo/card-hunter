using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ConfirmBtn : MonoBehaviour
{
    public event Action Confirm;
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            Confirm?.Invoke();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
