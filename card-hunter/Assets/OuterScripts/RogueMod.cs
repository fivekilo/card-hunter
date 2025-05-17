using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Choice
{
    public int id;
    public string text;//选项描述
    public int DeleteCard;//可以删几张牌
    public int AddCard;//可以加入几张牌
    public int money;//获得/损失金钱
    public int health;//获得/损失生命值
    public List<int> CardsID;//添加指定卡牌
    public int equipment;//可以获得的装备编号
    public Choice(int id, string text, int deleteCard, int addCard, int money, int health, List<int> cardsID, int equipment)
    {
        this.id = id;
        this.text = text;
        DeleteCard = deleteCard;
        AddCard = addCard;
        this.money = money;
        this.health = health;
        CardsID = cardsID;
        this.equipment = equipment;
    }
}
public class Event//事件类
{
    public int id;
    public string text;//事件文本

    List<Choice> choices;
    public Event(int id,string text,List<Choice>choices)
    {
        this.id = id;
        this.text = text;
        this.choices = choices;
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
    //记录路线
    private int CommissionAmount = GameConfig.CommissionAmount;
    private List<List<int>> EventArrangement;//随机事件编排,内层列表存储events中事件的索引,外层列表存储对应的行程
    private List<Event> events;
    public List<Event> GetEvents(int tour)//输入行程编号返回行程中的事件
    {
        List<Event> res = new List<Event>();
        foreach(int i in EventArrangement[tour])
        {
            res.Add(events[i]);
        }
        return res;
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
    public void EventGenerate(List<Event>events,int amount)//同上,最终存储于events
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
    public void ArrangeEvent(List<Event>E,Vector2Int bound)//bound决定随机事件数量上下限(包含上下限),从E中随机取事件编排在行程中,E的元素个数要大于上限
    {
        System.Random random = new System.Random();
        int amount=random.Next(bound[0], bound[1]+1);
        try
        {
            EventGenerate(E, amount);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return;
        }
        //构建EventArrangement
        EventArrangement=new List<List<int>>();
        for (int i = 0; i < 2*CommissionAmount-1; i++)//行程数=2*委托数-1
        {
            List<int>route = new List<int>();
            EventArrangement.Add(route);
        }
        for(int i=0;i<events.Count;i++)
        {
            List<int>PossibleNum = Enumerable.Range(0, 2*CommissionAmount-1).ToList();
            int ri=random.Next(PossibleNum.Count);
            int idx = PossibleNum[ri];
            EventArrangement[idx].Add(i);
            if (EventArrangement[idx].Count >= GameConfig.EventPerTour)//每个行程随机事件数不可超过2
            {
                PossibleNum.Remove(idx);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
