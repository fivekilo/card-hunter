using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    // Start is called before the first frame update
    public int Power = 0;//���� ����Ҫ���� 1
    public int JQ = 0;//���� ���﹥��ʱ���� �»غϼ���2
    public int Buffer = 0;//���� ����ʱ���� 3
    public int BigJu = 0;//��� ����ʱ���� �»غϼ��� 4
    public int Poison = 0;//�ж� �غϽ������� 5
    public int Numbness = 0; //��� �غϿ�ʼ���� 6
    public int CantMove = 0;//��غϲ����ƶ� �غϽ�����1  7
    public int DL = 0; //���� 8
    public int ExCost = 0;//������� �غϿ�ʼʱ���� 9;
    void Start()
    {

    }
    public void ModifyPower(int amount)
    {
        Power = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyJQ(int amount)
    {
        JQ = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyBuffer(int amount)
    {
        Buffer = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyBigJu(int amount)
    {
        BigJu = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyPoison(int amount)
    {
        Poison = Mathf.Clamp(amount, 0, 999);
    }

    public void ModifyNumbness(int amount)
    {
        Numbness = Mathf.Clamp(amount, 0, 999);
    }

    public void ModifyCantMove(int amount)
    {
        CantMove = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyDL(int amount)
    {
        DL = Mathf.Clamp(amount, 0, 999);
    }

    public void ModifyExCost(int amount)
    {
        ExCost = Mathf.Clamp(amount, 0, 999);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
