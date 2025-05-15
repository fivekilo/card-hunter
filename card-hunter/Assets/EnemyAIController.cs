using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    //加载基本事项
    public BattleManager _battleManager;
    public Vector2Int _currentGridPos { get; set; }
    public MapManager _mapManager;
    public PlayerInfo _player;
    public TextMeshProUGUI text;
    [Header("基础属性")]
    [SerializeField] protected int _maxHealth = 100;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int moveRange = 2;//每回合最大移动距离
    [SerializeField] protected int detectionRange = 4;//检测玩家的最大范围
    [SerializeField] private float moveInterval = 0.3f; // 移动动画间隔
    
    void Start()
    {
        _battleManager = GetComponentInParent<BattleManager>();
        _mapManager = _battleManager.mapmanager;
        _currentGridPos = new(GameConfig.size/2,GameConfig.size/2); 
        Vector3 InitialPos = _mapManager.GetVector3(_currentGridPos);
        InitialPos.z = -5;
        transform.position = InitialPos;
        _player= FindObjectOfType<PlayerInfo>();
        _currentHealth = _maxHealth;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
   
    public IEnumerator TakeTurn()//执行回合
    {

        //先出上一回合结束的招式(待编写)
        //再判断移动
        if (ShouldMoveToPlayer())
        {
            List<Vector2Int> path = CalculatePath();
            if(path.Count==0 )//没生成路径相当于没找到玩家
            {
                yield return WanderRandomly();
            }
            else
                yield return MoveAlongPath(path);
        }
        else
        {
            yield return WanderRandomly();
        }
        //出下一招（待编写）
    }

    bool ShouldMoveToPlayer()
    {
        if (_player == null) return false;
        int distance = (int)Vector2Int.Distance(_currentGridPos, _player.PlayerGridPos);
        return distance <= detectionRange;
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
            if (foundplayer == true)
                break;

            foreach (Vector2Int neighbor in _mapManager.GetNearby(current))
            {
                if (neighbor == _player.PlayerGridPos)
                {
                    camefrom[neighbor] = current;
                    foundplayer = true;
                    break;
                }
                if (!camefrom.ContainsKey(neighbor)&& _battleManager.CheckPosIsValid(neighbor)&& !_mapManager.IsPositionOccupied(neighbor))
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
            // 防止无限循环
            if (path.Count > 100) break;
        }
        path.Reverse();
        return TrimPathToMoveRange(path);
    }
    //行走范围不能超过最大移动距离
    List<Vector2Int> TrimPathToMoveRange(List<Vector2Int> path)
    {
        List<Vector2Int> trimmedPath = new List<Vector2Int>();
        int stepsTaken = 0;

        foreach (var pos in path)
        {
            if (stepsTaken >= moveRange) break;
            trimmedPath.Add(pos);
            stepsTaken++; 
        }
        return trimmedPath;
    }

    //沿着路径走或随机游荡
    IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        foreach (var pos in path)
        {
            if (!_battleManager.CheckPosIsValid(pos)) break;
            if (pos == _player.PlayerGridPos) break;//如果要走到玩家那一格了就刹车
            UpdatePosition(pos);
            yield return new WaitForSeconds(moveInterval);//等待一定时长
        }
    }
    IEnumerator WanderRandomly()
    {

        //先找出周围可以移动的点
        List<Vector2Int> possibleMoves = _mapManager.GetNearby(_currentGridPos)
            .FindAll(pos => !_mapManager.IsPositionOccupied(pos));
        if (possibleMoves.Count > 0)
        {
            Vector2Int target = possibleMoves[Random.Range(0, possibleMoves.Count)];
            UpdatePosition(target);
            yield return new WaitForSeconds(moveInterval);
        }
    }

    //更新坐标位置
    void UpdatePosition(Vector2Int newPos)
    {
        _currentGridPos = newPos;
        Vector3 newPos3 = _mapManager.GetVector3(_currentGridPos);
        newPos3.z = -5;
        transform.position = newPos3;
    }
    
    //加减血量
   /* public void AddHealth(int num)
    {
        _currentHealth += num;
    }*/
    public void ReduceHealth(int num)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - num, 0, _maxHealth);
        text.text = $"{_currentHealth}/{_maxHealth}";
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
