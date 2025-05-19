using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddCardWindow : MonoBehaviour
{
    private Vector3 hoverScale = new Vector3(2.122892f, 2.54747f, 1f);
    private Vector3 normalScale = new Vector3(1.73865f, 2.08638f, 1f);
    private int ChosedNum;
    private Action<int> addCard;
    private List<GameObject> Cards=new List<GameObject>();
    public void AddCard(Action<int>addCard,List<int>CardsID)//接口函数
    {
        this.addCard = addCard;
        //随机取三张牌
        HashSet<int> Set = new HashSet<int>();
        System.Random rand = new System.Random();
        while (Set.Count < 3)
        {
            Set.Add(rand.Next(1, CardsID.Count));
        }
        List<int> CardNum = Set.ToList();
        for(int i = 1; i < 4; i++)
        {
            Cards.Add(transform.Find("Panel/card"+i).gameObject);
        }
        //链接处理函数和点击事件 初始化卡牌
        for(int i=0;i<3;i++)
        {
            Cards[i].GetComponent<Card>().cardNum = CardsID[CardNum[i]];
            Cards[i].GetComponent<Card>().cardInit();
            Cards[i].GetComponent<CardWin>().Clicked += ClickHandler;
        }
        //链接确认函数
        transform.Find("Panel/ConfirmBtn").gameObject.GetComponent<ConfirmBtn>().Confirm += Confirm;
    }
    private void Confirm()
    {
        addCard?.Invoke(ChosedNum);
        //销毁当前窗口
        Destroy(this.gameObject);
    }
    private void ClickHandler(int num)
    {
        //选中卡变大效果
        foreach(GameObject card in Cards)
        {
            card.transform.localScale = normalScale;
        }
        Cards[num].transform.localScale = hoverScale;
        //记录号码
        ChosedNum = num;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
