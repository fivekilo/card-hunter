using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Event//事件类
{
    public int id;
    public string text;//事件文本
    public int DeleteCard;//可以删几张牌
    public int AddCard;//可以加入几张牌
    public int money;//获得/损失金钱
    public int health;//获得/损失生命值
    public List<int> CardsID;//添加指定卡牌
    public int trigger;//0:何时都可以触发 1:去程触发 2:回程触发
    public int equipment;//可以获得的装备编号
    public Event(int id,string text,int DeleteCard,int AddCard,int money,int health,List<int>CardsID,int trigger,int equipment)
    {
        this.id = id;
        this.text = text;
        this.DeleteCard = DeleteCard;
        this.AddCard = AddCard;
        this.money = money;
        this.health = health;
        this.CardsID=CardsID;
        this.trigger=trigger;
        this.equipment=equipment;
    }
}

public class Commission//狩猎委托
{
    public int id;
    public string monster;
    public int difficulty;
    public int place;//1:森林 2:荒漠 3:火山
    public Commission(int id, string monster, int difficulty, int place)
    {
        this.id = id;
        this.monster = monster;
        this.difficulty = difficulty;
        this.place = place;
    }
}
public class RogueMod : MonoBehaviour
{
    private List<Event> events;
    public List<Event> GetEvents()
    {
        return events;
    }
    public List<Commission> ChooseCommission(List<Commission> commissions,int amount)//从给定的数个委托中随机选出amount个
    {
        int ComCount = commissions.Count;
        if (amount > ComCount)
        {
            throw new ArgumentException("选取数量大于可选数");
        }

        HashSet<int>idx = new HashSet<int>();
        System.Random random = new System.Random();
        do
        {
            idx.Add(random.Next(0, ComCount));
        }
        while (idx.Count<ComCount);
        List<Commission> selections = new List<Commission>();
        foreach(int i in  idx)
        {
            selections.Add(commissions[i]);
        }
        return selections;
    }
    public void EventGenerate(List<Event>events,int amount)//同上
    {
        int ComCount = events.Count;
        if (amount > ComCount)
        {
            throw new ArgumentException("选取数量大于可选数");
        }

        HashSet<int> idx = new HashSet<int>();
        System.Random random = new System.Random();
        do
        {
            idx.Add(random.Next(0, ComCount));
        }
        while (idx.Count < ComCount);
        List<Event> selections = new List<Event>();
        foreach (int i in idx)
        {
            selections.Add(events[i]);
        }
        this.events = selections;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
