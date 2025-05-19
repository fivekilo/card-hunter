using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PlayerInfo : MonoBehaviour
{
    public List<int> deck=new List<int> {1,1,2,3,6,7,8,8,1 };//卡组
    public int money;//金钱
    public int MaxHealth = GameConfig.InitialHealth;
    public int curHealth;
    public int MaxCost = GameConfig.InitialCost;
    public int curCost;
    public int Defence;
    public int Situation;//人物状态 0自由 1连携
    public int curBladeNum = 0; //气刃值
    public int curBladeLevel = 0; //气刃等级
    public Vector2Int PlayerGridPos = new (0 , 0); //玩家位置
    public Vector2Int Direction = new Vector2Int(1, 0);
    


    public TextMeshProUGUI HP;
    public TextMeshProUGUI endurance;
    public TextMeshProUGUI MoveCost;
    // public delegate void CharacterEvent(PlayerInfo Player);
    //public event CharacterEvent OnHealthChanged;
    //public event CharacterEvent OnEnergyChanged;
    //public event CharacterEvent OnBladeNumChanged;
    //public event CharacterEvent OnBladeLevelChanged;

    public Slider BladegasSlot;
   // private BattleManager _BattleManager;

    public void Initialize()
    {
        Situation = 0;
        curHealth = MaxHealth;
        curCost = MaxCost;
        curBladeLevel = 0;
        curBladeNum = 0;
        PlayerGridPos = new(0, 0);
    }
    public void ModifyHealth(int amount)
    {
        curHealth = Mathf.Clamp(amount, 0, MaxHealth);
        HP.text= $"{curHealth}/{MaxHealth}";
    }
    public void ModifyCost(int amount)
    {
        curCost = Mathf.Clamp(amount, 0, 999);
        endurance.text = $"{curCost}/{MaxCost}";
    }
    public void ModifyBladeNum(int amount)
    {
        curBladeNum = Mathf.Clamp(amount, 0, GameConfig.MaxBladeNum);
    }
    public void ModifySituation(int amount)
    {
        Situation = Mathf.Clamp(amount, 0, 1);
        MoveCost.text = $"{Situation + 1}";
    }
    public void ModifyBladeLevel(int amount)
    {
        curBladeLevel = Mathf.Clamp(amount, 0, GameConfig.MaxBladeLevel);
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
    public void ModifyDirection(Vector2Int Dir)
    {
        int[] dx = { 1, 0, -1, -1, 0, 1 };
        int[] dy = { 0, 1, 1, 0, -1, -1 };
        int Dir_id = -1;
        
        for (int i = 0; i < 6; i++)
        {
            if (dx[i] == Dir.x && dy[i] == Dir.y) { Dir_id = i;break; }
        }
        if (Dir_id == -1)
        {
            Debug.Log("PlayerInfo：不存在新方向");
            return;
        }
        Direction = new Vector2Int(dx[Dir_id], dy[Dir_id]);
    }
    public void ModifyDefence(int amount)
    {
        Defence = Mathf.Clamp(amount, 0, 999);
    }
    void Start()
    {
        curHealth = MaxHealth;
        curCost = MaxCost;
        Initialize();
        // 设置初始
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        // 取消订阅以避免内存泄漏
    }
}
