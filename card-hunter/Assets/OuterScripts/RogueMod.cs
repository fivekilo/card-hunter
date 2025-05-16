using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Event//�¼���
{
    public int id;
    public string text;//�¼��ı�
    public int DeleteCard;//����ɾ������
    public int AddCard;//���Լ��뼸����
    public int money;//���/��ʧ��Ǯ
    public int health;//���/��ʧ����ֵ
    public List<int> CardsID;//���ָ������
    public int trigger;//0:��ʱ�����Դ��� 1:ȥ�̴��� 2:�س̴���
    public int equipment;//���Ի�õ�װ�����
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
    private List<Event> events;
    public List<Event> GetEvents()
    {
        return events;
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
    public void EventGenerate(List<Event>events,int amount)//ͬ��
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
