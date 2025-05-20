using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaflet : MonoBehaviour
{
    public int num;
    public event Action<int> Clicked;
    public void Init(Commission c)
    {
        string StrMons = "ÌÖ·¥" + c.monster;
        string StrMoney = "ÐüÉÍ" + c.money + "Ôª";

        transform.Find("RewardMonster").GetComponent<TextMeshProUGUI>().text = StrMons;
        transform.Find("RewardMoney").GetComponent<TextMeshProUGUI>().text = StrMoney;
        //»»¹ÖÎïÍ¼Æ¬
    }
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Clicked?.Invoke(num);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
