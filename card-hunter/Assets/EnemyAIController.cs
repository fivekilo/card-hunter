using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    //加载基本事项
    protected BattleManager _battleManager;
    public Vector2Int _currentGridPos { get; set; }
    public MapManager _mapManager;
    private PlayerInfo _player;
    [Header("基础属性")]
    [SerializeField] protected int _maxHealth = 100;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int moveRange=3;//每回合最大移动距离
    [SerializeField] protected int detectionRange=6;//检测玩家的最大范围

    void Start()
    {
        _battleManager = FindObjectOfType<BattleManager>();
        _currentGridPos= Vector2Int.FloorToInt(transform.position); // 初始位置需对齐网格
        _mapManager = FindObjectOfType<MapManager>();
        _player= FindObjectOfType<PlayerInfo>();
    }

    public virtual void TakeTurn()//执行回合
    {
        //先出上一回合结束的招式(待编写)
        //再判断移动
        //if(ShouldMoveToPlayer())
        //{
        //    List<Vector2Int> path = CalculatePath();
        //}
        //出下一招（待编写）
    }

    //寻路算法（待编写）
    private List<Vector2Int> CalculatePath()
    {
        // BFS寻路算法
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> camefrom = new Dictionary<Vector2Int, Vector2Int>();
        queue.Enqueue(_currentGridPos);
        camefrom[_currentGridPos] = Vector2Int.zero;
        bool foundplayer = false;
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == _player.PlayerGridPos)
            {
                foundplayer = true;
                break;
            }

                foreach (Vector2Int neighbor in _battleManager.GetAdjacent(new List<int> { 0, 1, 2, 3, 4, 5 }))
            {
                if(!camefrom.ContainsKey(neighbor)&& _battleManager.CheckPosIsValid(neighbor)&& !_mapManager.IsPositionOccupied(neighbor))
                {
                    queue.Enqueue(neighbor);
                    camefrom[neighbor] = current;
                }
            }
        }

        return foundplayer ? ReconstructPath(camefrom) : new List<Vector2Int>();
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom)//辅助生成路径
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = _player.PlayerGridPos;
        while(current !=_currentGridPos)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }

    public Vector2Int GetCurrentGridPos() // 公共方法供MapManager调用
    {
        return _currentGridPos;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
