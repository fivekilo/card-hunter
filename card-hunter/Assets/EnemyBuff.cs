using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuff : MonoBehaviour
{
    // Start is called before the first frame update
    public int Poison = 0;//中毒 回合结束结算 1
    public int Numbness = 0; //麻痹 回合开始结算 2
    public int Wound = 0; //伤口 造成伤害时结算 3
    public int Dizzy = 0; //眩晕 回合开始结算 4
    public int TurntoPlayer =0;//转向 玩家移动时结算 5

    void Start()
    {
        
    }
    
    public void ModifyPoison(int amount)
    {
        Poison = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyNumbness(int amount)
    {
        Numbness = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyWound(int amount)
    {
        Wound = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyDizzy(int amount)
    {
        Dizzy = Mathf.Clamp(amount, 0, 999);
    }
    public void ModifyTurntoPlayer(int amount)
    {
        TurntoPlayer = Mathf.Clamp(amount, 0, 999);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
