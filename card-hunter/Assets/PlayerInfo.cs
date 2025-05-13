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

    public int curBladeNum = 0; //����ֵ
    public int curBladeLevel = 0; //���еȼ�
    public Vector2Int PlayerGridPos = new (0 , 0); //���λ��
    public Vector2Int Direction = new Vector2Int(1, 0);

    // public delegate void CharacterEvent(PlayerInfo Player);
    //public event CharacterEvent OnHealthChanged;
    //public event CharacterEvent OnEnergyChanged;
    //public event CharacterEvent OnBladeNumChanged;
    //public event CharacterEvent OnBladeLevelChanged;

    public Slider BladegasSlot;
   // private BattleManager _BattleManager;

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
        curHealth = Mathf.Clamp(amount, 0, MaxHealth);
    }
    public void ModifyCost(int amount)
    {
        curCost = Mathf.Clamp(amount, 0, MaxCost);
    }
    public void ModifyBladeNum(int amount)
    {
        curBladeNum = Mathf.Clamp(amount, 0, GameConfig.MaxBladeNum);
    }
    public void ModifyBladeLevel(int amount)
    {
        curBladeLevel = Mathf.Clamp(amount, 0, GameConfig.MaxBladeLevel);
    }
    public void ModifyPos(Vector2Int newPos)
    {
        if(newPos.x < 0 || newPos.x >= GameConfig.size || newPos.y < 0 || newPos.y >= GameConfig.size)
        {
            Debug.Log("������λ�ó�����ͼ");
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
        // ���ó�ʼ
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        // ȡ�������Ա����ڴ�й©
    }
}
