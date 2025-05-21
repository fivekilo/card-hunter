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
        string StrMons = "�ַ�" + c.monster;
        string StrMoney = "����" + c.money + "Ԫ";

        transform.Find("RewardMonster").GetComponent<TextMeshProUGUI>().text = StrMons;
        transform.Find("RewardMoney").GetComponent<TextMeshProUGUI>().text = StrMoney;
        //������ͼƬ
        Image image = transform.Find("Monster").GetComponent<Image>();
        Sprite loadedSprite = Resources.Load<Sprite>(GameConfig.TransToEnglish[c.monster]);
        image.sprite = loadedSprite;
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
