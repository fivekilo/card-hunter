using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    // Start is called before the first frame update
    public int Power = 0;//力量 不需要结算 1
    public int JQ = 0;//见切 怪物攻击时结算 下回合减少2
    public int Buffer = 0;//缓冲 被打时结算 3
    public int BigJu = 0;//大居 被打时结算 下回合减少 4
    public int Poison = 0;//中毒 回合结束结算 5
    public int Numbness = 0; //麻痹 回合开始结算 6
    public int CantMove = 0;//这回合不能移动 回合结束减1  7
    public int DL = 0; //登龙 8
    public int ExCost = 0;//额外费用 回合开始时结算 9;
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
