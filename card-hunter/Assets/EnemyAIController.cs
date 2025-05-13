using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    //加载基本事项
    protected BattleManager _battleManager;
    public Vector2Int _currentGridPos { get; set; }
    public MapManager _mapManager;
    [Header("基础属性")]
    [SerializeField] protected int _maxHealth = 100;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int moveRange=3;//每回合最大移动距离
    [SerializeField] protected int detectionRange=6;//检测玩家的最大范围

    void Start()
    {
        _battleManager = FindObjectOfType<BattleManager>();
        _currentGridPos= Vector2Int.zero;
        _mapManager = FindObjectOfType<MapManager>();
    }

    public virtual void TakeTurn()//执行回合（待编写）
    {
    
    }

    //寻路算法（待编写）
    //private List<Vector2Int> CalculatePath()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
