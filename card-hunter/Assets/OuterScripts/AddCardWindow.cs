using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddCardWindow : MonoBehaviour
{
    private int ChosedNum=-1;
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
        if (ChosedNum != -1)
        {
            addCard?.Invoke(ChosedNum);
            //销毁当前窗口
            Destroy(this.gameObject);
        }
    }
    private void ClickHandler(int num)
    {
        //选中卡变大效果
        foreach(GameObject card in Cards)
        {
            card.transform.localScale = GameConfig.normalScale;
        }
        Cards[num].transform.localScale = GameConfig.hoverScale;
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
