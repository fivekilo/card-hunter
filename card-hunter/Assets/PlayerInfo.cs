using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int MaxHealth = GameConfig.InitialHealth;
    public int curHealth;
    public int MaxCost = GameConfig.InitialCost;
    public int curCost;

    public int curBladeNum = 0; //气刃值
    public int curBladeLevel = 0; //气刃等级
    public Vector2Int PlayerGridPos = new (0 , 0); //玩家位置

   // public delegate void CharacterEvent(PlayerInfo Player);
    //public event CharacterEvent OnHealthChanged;
    //public event CharacterEvent OnEnergyChanged;
    //public event CharacterEvent OnBladeNumChanged;
    //public event CharacterEvent OnBladeLevelChanged;

    public void Initialize()
    {
        curHealth = MaxHealth;
        curCost = MaxCost;
        curBladeLevel = 0;
        curBladeNum = 0;
        PlayerGridPos = new(0, 0);
    }
    public void ModifyHealth(int amount)
    {
        Mathf.Clamp(curHealth + amount, 0, MaxHealth);
    }
    public void ModifyCost(int amount)
    {
        Mathf.Clamp(curCost + amount, 0, MaxCost);
    }
    public void ModifyBladeNum(int amount)
    {
        Mathf.Clamp(curBladeNum + amount, 0, GameConfig.MaxBladeNum);
    }
    public void ModifyBladeLevel(int amount)
    {
        Mathf.Clamp(curBladeNum + amount, 0, GameConfig.MaxBladeLevel);
    }
    public void ModifyPos(Vector2Int newPos)
    {
        if(newPos.x < 0 || newPos.x >= GameConfig.size || newPos.y < 0 || newPos.y >= GameConfig.size)
        {
            Debug.Log("错误！新位置超出地图");
            return;
        }
        PlayerGridPos.x = newPos.x;
        PlayerGridPos.y = newPos.y;
    }
    void Start()
    {
        curHealth = MaxHealth;
        curCost = MaxCost;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
