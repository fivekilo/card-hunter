using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    //���ػ�������
    protected BattleManager _battleManager;
    public Vector2Int _currentGridPos { get; set; }
    public MapManager _mapManager;
    [Header("��������")]
    [SerializeField] protected int _maxHealth = 100;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int moveRange=3;//ÿ�غ�����ƶ�����
    [SerializeField] protected int detectionRange=6;//�����ҵ����Χ

    void Start()
    {
        _battleManager = FindObjectOfType<BattleManager>();
        _currentGridPos= Vector2Int.zero;
        _mapManager = FindObjectOfType<MapManager>();
    }

    public virtual void TakeTurn()//ִ�лغϣ�����д��
    {
    
    }

    //Ѱ·�㷨������д��
    //private List<Vector2Int> CalculatePath()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
