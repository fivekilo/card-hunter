using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Choice
{
    public int id;
    public string text;//ѡ������
    public int DeleteCard;//����ɾ������
    public int AddCard;//���Լ��뼸����
    public int money;//���/��ʧ��Ǯ
    public int health;//���/��ʧ����ֵ
    public List<int> CardsID;//���ָ������
    public int equipment;//���Ի�õ�װ�����
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
public class Event//�¼���
{
    public int id;
    public string text;//�¼��ı�

    List<Choice> choices;
    public Event(int id,string text,List<Choice>choices)
    {
        this.id = id;
        this.text = text;
        this.choices = choices;
    }
}

public class Commission//����ί��
{
    public int id;
    public string monster;
    public int difficulty;
    public int place;//1:ɭ�� 2:��Į 3:��ɽ
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
    //��¼·��
    private int CommissionAmount = GameConfig.CommissionAmount;
    private List<List<int>> EventArrangement;//����¼�����,�ڲ��б�洢events���¼�������,����б�洢��Ӧ���г�
    private List<Event> events;
    public List<Event> GetEvents(int tour)//�����г̱�ŷ����г��е��¼�
    {
        List<Event> res = new List<Event>();
        foreach(int i in EventArrangement[tour])
        {
            res.Add(events[i]);
        }
        return res;
    }
    public List<Commission> ChooseCommission(List<Commission> commissions,int amount)//�Ӹ���������ί�������ѡ��amount��
    {
        int ComCount = commissions.Count;
        if (amount > ComCount)
        {
            throw new ArgumentException("ѡȡ�������ڿ�ѡ��");
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
    public void EventGenerate(List<Event>events,int amount)//ͬ��,���մ洢��events
    {
        int ComCount = events.Count;
        if (amount > ComCount)
        {
            throw new ArgumentException("ѡȡ�������ڿ�ѡ��");
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
    public void ArrangeEvent(List<Event>E,Vector2Int bound)//bound��������¼�����������(����������),��E�����ȡ�¼��������г���,E��Ԫ�ظ���Ҫ��������
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
        //����EventArrangement
        EventArrangement=new List<List<int>>();
        for (int i = 0; i < 2*CommissionAmount-1; i++)//�г���=2*ί����-1
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
            if (EventArrangement[idx].Count >= GameConfig.EventPerTour)//ÿ���г�����¼������ɳ���2
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
