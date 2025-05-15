using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Event//�¼���
{
    public int id;
}

public class Commission//����ί��
{
    public int id;
    public string monster;
    public int difficulty;
    public Commission(int id, string monster, int difficulty)
    {
        this.id = id;
        this.monster = monster;
        this.difficulty = difficulty;
    }
}
public class RogueMod : MonoBehaviour
{
    private List<Event> events;
    private Commission commission;

    public void ChooseCommission(List<Commission> commissions,int amount)//�Ӹ���������ί�������ѡ��amount��
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
    }
    public void RouteGenerate()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
